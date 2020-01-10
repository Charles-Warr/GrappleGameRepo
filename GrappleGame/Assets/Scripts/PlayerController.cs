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
    [SerializeField] float dashDelay;
    [SerializeField] bool dashing;

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
            dashInput = Input.GetButton("Fire3");
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

        //dash time rules
        if (curDashTime < startDashTime)
        {
            curDashTime += Time.deltaTime;
        }
        else if (curDashTime >= startDashTime)
        {
            curDashTime = startDashTime;
        }
        if (curDashTime <= 0)
        {
            curDashTime = 0;
        }
        if(dashing == true && curDashTime == startDashTime)
        {
            dashing = false;
        }

    }

    void FixedUpdate()
    {
        //movement
        curVel = new Vector3(dirInput.x * groundSpeed, curVel.y);

        if (dashInput && curDashTime == startDashTime) 
        {
            curDashTime -= Time.deltaTime;
            Dash();
        }
    }

    void Dash()
    {
        dashing = true;
        curVel = new Vector2(dashSpeed * Time.deltaTime, 0);
    }
}
