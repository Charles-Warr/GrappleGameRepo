using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject feet;

    Vector2 dirInput;
    bool dashInput;
    [SerializeField] Vector2 curVel;

    float gravity = 9.8f;
    [SerializeField] bool grounded;
    [SerializeField] float groundSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float startDashTime;
    [SerializeField] float curDashTime;
    [SerializeField] bool grabbing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curDashTime = startDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        {
            dirInput.x = Input.GetAxis("Horizontal");
            dirInput.y = Input.GetAxis("Vertical");
            dashInput = Input.GetKeyDown(KeyCode.LeftShift);
        }
        //ground check
        {
            grounded = feet.GetComponent<FeetCheck>().grounded;
            if (grounded == true)
            {
                curVel.y = 0;
            }
            else
            {
                curVel.y -= gravity * Time.deltaTime;
            }
        }
        rb.velocity = curVel;



    }

    void FixedUpdate()
    {
        //movement
        curVel = new Vector3(dirInput.x * groundSpeed, curVel.y);

        if (dashInput && curDashTime == startDashTime)
        {
            Dash();
        }
    }

    void Dash()
    {
        curVel = new Vector2(dashSpeed, 0);
    }
}
