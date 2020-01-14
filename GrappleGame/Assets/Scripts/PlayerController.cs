using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //components
    Rigidbody rb;
    [SerializeField] GameObject feet;
    [SerializeField] GameObject grabTrigger;

    public bool cameraTrigger;
    Vector2 dirInput;
    bool dashInput;
    bool jumpInput;
    [SerializeField] float dirInf;
    [SerializeField] Vector2 curVel;
    [SerializeField] Vector2[] powMoveVel;
    [SerializeField] float DIMult;

    //checks
    [SerializeField] bool grounded;
    [SerializeField] bool groundable;
    bool canDash;
    [SerializeField] bool dashing;
    bool canGrab;
    [SerializeField] bool grabbing;
    [SerializeField] bool powMove;
    bool frontAttack;
    bool backAttack;
    bool upAttack;
    bool downAttack;
    [SerializeField] GameObject grabbedObject;
    [SerializeField] bool aiming;
    [SerializeField] GameObject aimReticle;

    //stats
    [SerializeField] float gravityMult;
    [SerializeField] float powFallSpeed;
    [SerializeField] float regFallSpeed;
    float curFallSpeed;
    [SerializeField] float groundSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float curDashTime;
    [SerializeField] float dashDelay;
    [SerializeField] float ungroundedTime;
    [SerializeField] float curUngroundedTime;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curDashTime = dashTime;;
        cameraTrigger = true;
        groundable = true;
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
        if (other.gameObject.GetComponent<Wall>())
        {
            cameraTrigger = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Wall>())
        {
            cameraTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        dirInf = (dirInput.normalized.x * transform.right.x + 1) * DIMult;
        //Input
        {
            dirInput.x = Input.GetAxis("Horizontal");
            dirInput.y = Input.GetAxis("Vertical");
            dashInput = Input.GetButtonDown("Fire3");
            jumpInput = Input.GetButtonDown("Jump");

        }
        //ground check
        {
            if(feet.GetComponent<FeetCheck>().grounded && groundable && !powMove)
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
            if (grounded) 
            {
                curVel.y = 0;
            }
            if(!grounded || powMove)
            {
                curVel.y -= -Physics.gravity.y * gravityMult * Time.deltaTime;
            }

            //check for not grounded timer
            if (curUngroundedTime <= 0)
            {
                curUngroundedTime = 0;
            }
            if (curUngroundedTime >= ungroundedTime)
            {
                curUngroundedTime = ungroundedTime;
            }
            if (curUngroundedTime > 0 && powMove)
            {
                groundable = false;
            }
            else if (curUngroundedTime <= 0)
            {
                groundable = true;
            }

        }

        //dash time rules
        {
            if (canDash && dashInput)
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
        }

        grabbedObject = grabTrigger.GetComponent<GrabCheck>().objectGrabbed;

        if (grabbing)
        {
            if (dashInput)
            {
                grabbing = false;
            }
            else if (jumpInput && transform.right.x * dirInput.x > 0 && !powMove)
            {
                curVel.x = 0;
                curVel.y = 0;
                ungroundedTime = .5f;
                curUngroundedTime = ungroundedTime;
                frontAttack = true;
            }
            else if(jumpInput && transform.right.x * dirInput.x < 0 && !powMove)
            {
                curVel.x = 0;
                curVel.y = 0;
                ungroundedTime = .5f;
                curUngroundedTime = ungroundedTime;
                backAttack = true;
            }
            else if(jumpInput && transform.right.x * dirInput.y > 0 && !powMove)
            {
                curVel.x = 0;
                curVel.y = 0;
                ungroundedTime = .5f;
                curUngroundedTime = ungroundedTime;
                upAttack = true;
            }
            else if(jumpInput && transform.right.x * dirInput.y < 0 && !powMove)
            {
                curVel.x = 0;
                curVel.y = 0;
                ungroundedTime = .7f;
                curUngroundedTime = ungroundedTime;
                downAttack = true;
            }
        }

        if (frontAttack)
        {
            powMove = true;
            canDash = false;
            curUngroundedTime -= Time.deltaTime;
            if (grounded && curUngroundedTime <= 0)
            {
                frontAttack = false;
                grabbing = false;
            }
        }
        else if (backAttack)
        {
            powMove = true;
            canDash = false;
            curUngroundedTime -= Time.deltaTime;
            if (grounded && curUngroundedTime <= 0)
            {
                backAttack = false;
                grabbing = false;
            }
        }
        else if (upAttack)
        {
            powMove = true;
            canDash = false;
            curUngroundedTime -= Time.deltaTime;
            if (grounded && curUngroundedTime <= 0)
            {
                upAttack = false;
                grabbing = false;
            }
        }
        else if (downAttack)
        {
            powMove = true;
            canDash = false;
            curUngroundedTime -= Time.deltaTime;
            if (grounded && curUngroundedTime <= 0)
            {
                downAttack = false;
                grabbing = false;
            }
        }
        else if(!frontAttack && !backAttack && !upAttack && !downAttack)
        {
            powMove = false;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = curVel;

        //movement
        if (!dashing && !grabbing && !powMove)
        {
            curVel = new Vector3(dirInput.x * groundSpeed, curVel.y);
            curFallSpeed = regFallSpeed;
            if (curVel.y < -curFallSpeed)
            {
                curVel.y = -curFallSpeed;
            }
        }
        if (powMove)
        {
            curFallSpeed = powFallSpeed;
            if (curVel.y < -curFallSpeed)
            {
                curVel.y = -curFallSpeed;
            }
            
        }

        //Flip player if moving in opposite direction 
        if (dirInput.x * transform.right.x < 0 && !dashing && !powMove)
        {
            FlipPlayer(dirInput.x > 0);
        }

        //Dash Function
        {
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
        }

        //Grab Function
        {
            if (grabbing)
            {
                canDash = false;
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;
                curVel.x = 0;

            }
            if (frontAttack)
            {
                rb.AddForce(transform.up * powMoveVel[0], ForceMode.Impulse);
                rb.AddForce(transform.right * powMoveVel[0] * dirInf, ForceMode.Impulse);
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;
            }
            else if (backAttack)
            {
                rb.AddForce(transform.up * powMoveVel[1], ForceMode.Impulse);
                rb.AddForce(transform.right * powMoveVel[1] * dirInf, ForceMode.Impulse);
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;
            }
            else if (upAttack)
            {
                rb.AddForce(transform.up * powMoveVel[2], ForceMode.Impulse);
                rb.AddForce(transform.right * powMoveVel[2] * dirInf, ForceMode.Impulse);
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;
            }
            else if (downAttack)
            {
                rb.AddForce(transform.up * powMoveVel[3], ForceMode.Impulse);
                rb.AddForce(transform.right * powMoveVel[3] * dirInput, ForceMode.Impulse);
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;
            }
            else
            {
                grabbedObject.transform.parent = null;
                grabbedObject.GetComponent<Rigidbody>().velocity = grabbedObject.GetComponent<Rigidbody>().velocity;
            }
        }

    }

    void FlipPlayer(bool r)   //Makes player face correct direction
    {
        transform.rotation = Quaternion.LookRotation(r ? Vector3.forward : -Vector3.forward, Vector3.up);
    }

}
