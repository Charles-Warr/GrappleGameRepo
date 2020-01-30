using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UserController : MonoBehaviour
{
    [SerializeField] GameObject feet;

    private Rigidbody player;
    
    [System.Serializable]
    public class Settings
    {
        public float acceleration;
        public float deceleration;
        public float maxVelocity;
        public float dashForce;
        public float dashDistance;
    }

    private bool acceptInput;
    private bool grounded;

    public Settings settings = new Settings();

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<Rigidbody>();
        acceptInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = feet.GetComponent<FeetCheck>().grounded;

        
    }

    private void FixedUpdate()
    {
        move();
        if (!grounded)
            player.velocity += Physics.gravity * Time.deltaTime;
    }

    private void move()
    {
        if (acceptInput)
        {
            if(Input.GetButton("Horizontal"))
            {
                if(Mathf.Abs(player.velocity.x) >= settings.maxVelocity)
                {
                    //player.velocity = new Vector2(settings.maxVelocity*player.velocity.normalized.x, player.velocity.y);
                }
                else
                    player.AddForce(Input.GetAxis("Horizontal")*player.transform.right * settings.acceleration , ForceMode.Acceleration);
            }
            else
            {
                player.velocity = new Vector2(player.velocity.x-(player.velocity.x*Time.deltaTime*settings.deceleration), player.velocity.y);
            }
            //player.velocity += Input.GetAxis("Horizontal") * player.transform.right;
        }
    }
}
