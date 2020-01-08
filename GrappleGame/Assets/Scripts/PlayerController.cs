using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] FeetCheck feet;
    public Vector2 curVel;
    public Vector2 dirInput;

    [SerializeField] bool grounded;
    [SerializeField] float groundSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        feet = GetComponent<FeetCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = curVel;
        grounded = feet.grounded;

        if(grounded == false)
        {
            curVel.y -= Time.deltaTime;
        }
        else
        {
            curVel.y = 0;
        }

        dirInput.x = Input.GetAxis("Horizontal");
        dirInput.y = Input.GetAxis("Vertical");

    }

    void FixedUpdate()
    {
        curVel = new Vector3(dirInput.x * groundSpeed, curVel.y);
    }
}
