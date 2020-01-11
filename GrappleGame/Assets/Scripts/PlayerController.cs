using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject feet;
    [SerializeField] GameObject grabTrigger;

    public bool cameraTrigger;

    Vector2 dirInput;
    bool dashInput;
    bool jumpInput;
    [SerializeField] Vector2 curVel;

    //checks
    [SerializeField] bool grounded;
    [SerializeField] bool canDash;
    [SerializeField] bool dashing;
    [SerializeField] bool canGrab;
    [SerializeField] bool grabbing;
    [SerializeField] GameObject grabbedObject;
    [SerializeField] bool aiming;
    [SerializeField] GameObject aimReticle;

    float gravity = 9.8f;
    [SerializeField] float groundSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float curDashTime;
    [SerializeField] float dashDelay;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curDashTime = dashTime;
        cameraTrigger = true;
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            cameraTrigger = false;
        }
    }
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            cameraTrigger = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Wall")
        {
            cameraTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        {
            dirInput.x = Input.GetAxis("Horizontal");
            dirInput.y = Input.GetAxis("Vertical");
            dashInput = Input.GetButtonDown("Fire3");

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
        if (dashInput && canDash)
        {
            dashing = true;
        }
        if (!dashing && !grabbing)
        {
            if (curDashTime < dashTime)
            {
                curDashTime += Time.deltaTime;
                canDash = false;
            }
            else if (curDashTime >= dashTime)
            {
                curDashTime = dashTime;
                canDash = true;
            }
        }
        if (curDashTime <= 0)
        {
            curDashTime = 0;
            dashing = false;
        }

        grabbedObject = grabTrigger.GetComponent<GrabCheck>().objectGrabbed;
    }

    void FixedUpdate()
    {
        //movement
        if (!dashing && !grabbing)
        {
            curVel = new Vector3(dirInput.x * groundSpeed, curVel.y);

        }
        //Flip player if moving in opposite direction 
        if (dirInput.x * transform.right.x < 0 && !dashing)
        {
            FlipPlayer(dirInput.x > 0);
        }

        //Dash Function
        if (dashing)
        {
            canDash = false;
            canGrab = true;
            curDashTime -= Time.deltaTime;
            curVel = transform.right * dashSpeed;
            if (grabTrigger.GetComponent<GrabCheck>().grabbable)
            {
                grabbing = true;
                dashing = false;
            }
        }
        else
        {
            canGrab = false;
        }


        if (grabbing)
        {
            canDash = false;
            grabbedObject.transform.parent = grabTrigger.transform;
            grabbedObject.transform.position = grabTrigger.transform.position;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            curVel.x = 0;
            if (dashInput)
            {
                grabbedObject.transform.parent = null;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbing = false;
            }
        }

    }

    void FlipPlayer(bool r)   //Makes player face correct direction
    {
        transform.rotation = Quaternion.LookRotation(r ? Vector3.forward : -Vector3.forward, Vector3.up);
    }
}
