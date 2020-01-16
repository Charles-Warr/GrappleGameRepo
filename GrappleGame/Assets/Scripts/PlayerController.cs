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
    public float hitstun = 0;
    public Vector2 knockback;
    public bool vulnerable = true;
    public bool alive = true;
    [SerializeField] GameObject lastCheckpoint;
    [SerializeField] bool grounded;
    [SerializeField] bool groundable;
    [SerializeField] bool canDash;
    [SerializeField] bool dashing;
    [SerializeField] bool canGrab;
    [SerializeField] bool lifting;
    [SerializeField] bool bounce;
    [SerializeField] bool canPow;
    [SerializeField] bool powMove;
    [SerializeField] bool landed;
    bool frontAttack;
    bool backAttack;
    bool upAttack;
    bool downAttack;
    [SerializeField] bool powCancel;
    [SerializeField] GameObject grabbedObject;
    [SerializeField] bool aiming;
    [SerializeField] GameObject aimReticle;

    //stats
    public int maxHealth = 3;
    public int curHealth;
    [SerializeField] float gravityMult;
    [SerializeField] float regFallSpeed = 30;
    [SerializeField] float liftingFallSpeed = 15;
    [SerializeField] float powFallSpeed = 50;
    [SerializeField] float curFallSpeed;
    [SerializeField] float groundSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] Vector2 bounceAmt;
    [SerializeField] float dashSpeed = 50;
    [SerializeField] float dashSpeedMult = 2;
    [SerializeField] float dashTime = .5f;
    [SerializeField] float curDashTime;
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

    // Update is called once per frame
    void Update()
    {
        //Input
        {
            dirInput.x = Input.GetAxis("Horizontal");
            dirInput.y = Input.GetAxis("Vertical");
            dashInput = Input.GetButtonDown("Fire3");
            jumpInput = Input.GetButtonDown("Jump");
            dirInf = (dirInput.normalized.x +1 * Time.deltaTime);
        }
        //ground check
        {
            if(feet.GetComponent<FeetCheck>().grounded && groundable)
                grounded = true;        
            else
                grounded = false;
            
            if (grounded) 
                curVel.y = 0;
            else if(!grounded || powMove)
                curVel.y -= -Physics.gravity.y * gravityMult * Time.deltaTime;

            //ungrounded timer rules
            {
                if (curUngroundedTime <= 0)
                    curUngroundedTime = 0;
                if (curUngroundedTime >= ungroundedTime)
                    curUngroundedTime = ungroundedTime;

                if (curUngroundedTime > 0 && powMove)
                    groundable = false;   
                
                else if (curUngroundedTime <= 0)
                    groundable = true;
            }
        }

        //health
        if (hitstun > 0)
        {
            if (rb != null)
            {
                curVel = (knockback);
            }
            hitstun -= Time.deltaTime;
            if (hitstun < 0)
            {
                hitstun = 0;
            }
        }

        //dash timer rules
        {
            if (canDash && dashInput)
                dashing = true;
            
            if (!dashing && !lifting)
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

        grabbedObject = grabTrigger.GetComponent<GrabCheck>().objectInRange;
        if (lifting)
        {
            if (dashInput)
                lifting = false;

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

        if(powMove && dashInput)
            bounce = true;
        if (powMove && grounded)
            landed = true;
        if (powMove)
        {
            lifting = false;
        }
        if (!frontAttack && !backAttack && !upAttack && !downAttack)
            powMove = false;
        else
            powMove = true;

    }

    //For Pyshics
    void FixedUpdate()
    {
        rb.velocity = curVel;

        //Flip player if moving in opposite direction 
        if (dirInput.x * transform.right.x < 0 && !dashing && !powMove)
            FlipPlayer(dirInput.x > 0);


        //movement
        if (!dashing && !lifting && !powMove)
        {
            curVel = new Vector3(dirInput.x * groundSpeed, curVel.y);
            curFallSpeed = regFallSpeed;
            if (curVel.y < -curFallSpeed)
            {
                curVel.y = -curFallSpeed;
            }
        }

        //Dash Function
        {
            if (dashing)
            {
                canDash = false;
                canGrab = true;
                curDashTime -= Time.deltaTime;
                curVel = transform.right * dashSpeed * curDashTime * dashSpeedMult;

                if (grabTrigger.GetComponent<GrabCheck>().grabbable)
                {
                    lifting = true;
                    dashing = false;
                }
            }
            if(!dashing && !lifting && !powMove)
            {
                canGrab = false;
                grabTrigger.GetComponent<GrabCheck>().objectInRange.transform.parent = null;
            }

        }
        

        //Grab Function
        {
            if (lifting)
            {
                canDash = false;
                grabbedObject = grabTrigger.GetComponent<GrabCheck>().objectInRange;

                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;
                curFallSpeed = liftingFallSpeed;
                curVel.x = 0;            
            }

            if (frontAttack)
            {
                curUngroundedTime -= Time.deltaTime;
                rb.AddForce((powMoveVel[0].x * dirInf), (powMoveVel[0].y + curVel.y), 0, ForceMode.Impulse);
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;

                if (grounded && curUngroundedTime<0)
                {
                    landed = true;
                }
            }
            else if (backAttack)
            {
                curUngroundedTime -= Time.deltaTime;
                rb.AddForce(powMoveVel[1], ForceMode.Impulse);
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;

                if (grounded)
                {
                    backAttack = false;
                }
            }
            else if (upAttack)
            {
                curUngroundedTime -= Time.deltaTime;
                rb.AddForce(powMoveVel[2] * dirInf, ForceMode.Impulse);               
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;

                if (grounded)
                {
                    upAttack = false;
                }
            }
            else if (downAttack)
            {
                curUngroundedTime -= Time.deltaTime;
                rb.AddForce(transform.right * powMoveVel[3] * dirInput, ForceMode.Impulse);
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                grabbedObject.GetComponent<Rigidbody>().velocity = curVel;

                if (grounded)
                {
                    downAttack = false;
                }
            }

            //Cancel
            if (landed)
            {
                if (bounce)
                {
                    frontAttack = false;
                    backAttack = false;
                    upAttack = false;
                    downAttack = false;

                    curVel = new Vector2(0, 0);
                    rb.AddForce(bounceAmt, ForceMode.Impulse);

                    grabTrigger.GetComponent<GrabCheck>().objectInRange.transform.parent = null;
                    grabbedObject.GetComponent<Rigidbody>().velocity = grabbedObject.GetComponent<Rigidbody>().velocity;
                    grabbedObject.GetComponent<Rigidbody>().AddForce(transform.right * 1, ForceMode.Impulse);

                }

            }
        }
    }

    void FlipPlayer(bool r)   //Makes player face correct direction
    {
        transform.rotation = Quaternion.LookRotation(r ? Vector3.forward : -Vector3.forward, Vector3.up);
    }

    public void Hit(int amount, Vector3 kb, float stun)     //Getting hit by something with Damager component
    {
        if (vulnerable)
        {
            curHealth -= amount;
            if (curHealth <= 0 && alive)
            {
                Die();
            }
        }
        hitstun = stun;
        knockback = kb;

    }

    void Die()
    {
        alive = false;
        transform.position = lastCheckpoint.transform.position;
        curHealth = maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Damager>())
        {

            Hit(other.gameObject.GetComponent<Damager>().damage,
                other.gameObject.GetComponent<Damager>().knockback,
                other.gameObject.GetComponent<Damager>().hitstun);
        }
    }
}
