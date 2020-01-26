using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //components
    Rigidbody rb;
    [SerializeField] GameObject feet;
    [SerializeField] GameObject grabTrigger;
    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject healthUI;

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
    [SerializeField] bool lifting;
    [SerializeField] bool canPow;
    [SerializeField] bool powMove;
    [SerializeField] bool landed;
    [SerializeField] float bounceIntensity;
    public float bounceValue = 100f;
    public bool frontAttack;
    public bool backAttack;
    public bool upAttack;
    public bool downAttack;
    [SerializeField] bool bounce;
    [SerializeField] GameObject grabbedObject;  
    [SerializeField] bool aiming;  
    [SerializeField] GameObject aimReticle;

    [SerializeField] float slopeVelocity = 2f;

    [SerializeField] float timer = 0.2f;
    [SerializeField] float timerStart;
    [SerializeField] float maxContinuousGrabs;
    [SerializeField] float currentGrabs;

    [SerializeField] float hitTimer = 2f;
    [SerializeField] float hitTimerStart;

    //stats
    public int maxHealth = 3;
    public int curHealth;
    [SerializeField] float gravityMult = 3f;
    [SerializeField] float regFallSpeed = 30;
    [SerializeField] float liftingFallSpeed = 15;
    [SerializeField] float powFallSpeed = 50;
    [SerializeField] float curFallSpeed;
    [SerializeField] float groundSpeed;
    [SerializeField] float airSpeed;
    [SerializeField] Vector2 bounceAmt = new Vector2(-5f, 10f);
    [SerializeField] float dashSpeed = 50;
    [SerializeField] float dashSpeedMult = 1.61f;
    [SerializeField] float dashTime = .5f;
    [SerializeField] float curDashTime;
    [SerializeField] float ungroundedTime;
    [SerializeField] float curUngroundedTime;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        healthUI = GameObject.FindGameObjectWithTag("Health");
        cameraTrigger = true;
        groundable = true;
        bounce = false;
        curHealth = maxHealth;
        curDashTime = dashTime;
        timerStart = timer;
        hitTimerStart = hitTimer;
        currentGrabs = 0f;
        bounceIntensity = bounceValue;
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

        }

        //Slope check
        {
            RaycastHit slopes;

            //Physics.Raycast(new Vector3(this.transform.position.x-1, transform.position.y,transform.position.z), transform.right, 1f, slopes);

        }

        //ground check
        {
            if (feet.GetComponent<FeetCheck>().grounded && groundable)
                grounded = true;
            else
                grounded = false;

            if (grounded)
                curVel.y = 0;
            else if (!grounded || powMove)
                curVel.y += Physics.gravity.y * gravityMult * Time.deltaTime;

            //ungrounded timer rules
            {
                curUngroundedTime -= Time.deltaTime;
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
        {
            healthUI.GetComponent<TextMeshProUGUI>().text = "Health:" + curHealth;

            if (curHealth <= 0)
            {
                curHealth = 0;

                gameObject.GetComponent<PlayerController>().enabled = false;
            }
                
            if (curHealth >= maxHealth)
                curHealth = maxHealth;

            if (hitstun > 0)
            {
                if (rb != null)
                {
                    curVel = (knockback.normalized);
                }
                hitstun -= Time.deltaTime;
                if (hitstun < 0)
                {
                    hitstun = 0;
                }
            }

            
        }



        //dash timer rules
        {
            if (canDash && dashInput)
                dashing = true;
            
            if (!dashing)
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

        // CHUCK ADDED THIS
        if(grabbedObject == null)
        {
            lifting = false;
        }

        if (lifting)
        {
            if (dashInput && !powMove && grounded)
                lifting = false;
            //Front Pow Input
            else if (jumpInput && transform.right.x * dirInput.x > 0 && !powMove) 
            {
                curVel.x = 0;
                curVel.y = 0;
                powFallSpeed = 50;
                ungroundedTime = .5f;
                curUngroundedTime = ungroundedTime;
                frontAttack = true;
                powMove = true;
            }
            //Back Pow Input
            else if (jumpInput && transform.right.x * dirInput.x < 0 && !powMove) 
            {
                curVel.x = 0;
                curVel.y = 0;
                powFallSpeed = 40;
                ungroundedTime = .5f;
                curUngroundedTime = ungroundedTime;
                backAttack = true;
                powMove = true;
            }
            //Up Pow Input
            else if (jumpInput && dirInput.y > 0 && !powMove) 
            {
                curVel.x = 0;
                curVel.y = 0;
                powFallSpeed = 60;
                ungroundedTime = .5f;
                curUngroundedTime = ungroundedTime;
                upAttack = true;
                powMove = true;
            }
            //Down Pow Input
            else if (jumpInput && dirInput.y < 0 && !powMove) 
            {
                curVel.x = 0;
                curVel.y = 0;
                powFallSpeed = 13;
                ungroundedTime = .7f;
                curUngroundedTime = ungroundedTime;
                downAttack = true;
                powMove = true;
            }
        }
        else
        {
            curVel = new Vector3(curVel.x, curVel.y + Physics.gravity.y * gravityMult * Time.deltaTime);

        }

        if(vulnerable && hitbox.GetComponent<TakeDamage>().hit)
        {
            hitbox.GetComponent<TakeDamage>().setHit(false);
            
            curHealth -= (int)hitbox.GetComponent<TakeDamage>().damage;

            bounce = true;

            //activateHitTimer();

            vulnerable = false;

        }

        if(!vulnerable)
        {
            activateHitTimer();
        }
        
        //Check how many consecutive grabs, if reach max set can grab to false, start Timer
        
        if(!grounded && Input.GetButtonDown("Fire3"))
        {
            lifting = false;
            currentGrabs++;
            if(currentGrabs >= maxContinuousGrabs)
            {
                canDash = false;

            }
            else
            {

            }
        }
        
        if (powMove)
        {
            canDash = false;
            curFallSpeed = powFallSpeed;
            vulnerable = false;
            if (dashInput) //Cancel Input
            {
                bounce = true;
                curVel = new Vector2(0, 0);
                ungroundedTime = 1f;
                curUngroundedTime = ungroundedTime;
                powMove = false;
            }
        }

        if (!frontAttack && !backAttack && !upAttack && !downAttack)
            powMove = false;
    }

    void activateHitTimer()
    {

            hitTimer -= Time.deltaTime;

            if (hitTimer <= 0)
            {
                vulnerable = true;

                hitTimer = hitTimerStart;
            }
        
    }

    //For Pyshics
    void FixedUpdate()
    {
        if (bounce)
        {
            Bounce();
        }
        rb.velocity = curVel;

        //Flip player if moving in opposite direction 
        if (dirInput.normalized.x * transform.right.x < 0 && !dashing && !powMove)
            FlipPlayer(dirInput.x > 0);

        //movement
        if (!dashing && !lifting && !powMove)
        {
            curVel = new Vector3(dirInput.x * groundSpeed, curVel.y);

            Debug.DrawRay(new Vector3(rb.position.x+(.5f*rb.transform.right.x), rb.position.y-.5f, rb.position.z), gameObject.transform.right, Color.red);

            RaycastHit slopes;

            if(Physics.Raycast(new Vector3(rb.position.x + (.5f * rb.transform.right.x), rb.position.y - .5f, rb.position.z), gameObject.transform.right,out slopes, 1f))
            {
                if(slopes.collider.GetComponent<Ground>())
                {
                    curVel = new Vector2(dirInput.x * groundSpeed, curVel.y + slopeVelocity);
                    //curVel += Vector2.up * slopeVelocity;
                }
            }

         

            curFallSpeed = regFallSpeed;
        }
        if (curVel.y < -curFallSpeed)
        {
            curVel.y = -curFallSpeed;
        }

        //Dash Function

        if (dashing)
        {
            canDash = false;
            curDashTime -= Time.deltaTime;
            curVel = transform.right * dashSpeed * curDashTime * dashSpeedMult;

            if (grabTrigger.GetComponent<GrabCheck>().grabbable)
            {
                lifting = true;
                dashing = false;
            }
        }       

        //Lift Function
        if (lifting)
        {
            canDash = false;
            grabbedObject = grabTrigger.GetComponent<GrabCheck>().objectInRange;

            if (grabbedObject != null)
            {
               
                grabbedObject.transform.parent = grabTrigger.transform;
                grabbedObject.transform.position = grabTrigger.transform.position;
                //grabbedObject.GetComponent<Rigidbody>().velocity = curVel;

            }
            rb.velocity += Physics.gravity;

            //curFallSpeed = liftingFallSpeed;
            curVel.x = 0;
        }
        else if (!lifting && !powMove)
        {
            if(grabTrigger.GetComponent<GrabCheck>().objectInRange != null)
            {
                grabTrigger.GetComponent<GrabCheck>().objectInRange.transform.parent = null;
                grabbedObject = null;
            }

        }

        
        dirInf = ((dirInput.x + transform.right.normalized.x * 1.5f));

        if (powMove)
            PowerMove();                     
        
    }

    //Makes player face correct direction
    void FlipPlayer(bool r)   
    {
        transform.rotation = Quaternion.LookRotation(r ? Vector3.forward : -Vector3.forward, Vector3.up);
    }

    // This activates the timer for each power move;
    void activateTimer()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            

            if (grounded || grabbedObject.GetComponentInChildren<FeetCheck>().grounded)
            {
                frontAttack = canDash = backAttack = upAttack = downAttack = powMove = false;
                timer = timerStart;
                applyDamage();
            }
        }
    }

    void applyDamage()
    {
        if(grabbedObject.GetComponent<EnemyHealth>())
        {
            grabbedObject.GetComponent<EnemyHealth>().Hit(1);
            //grabTrigger.GetComponent<GrabCheck>().objectInRange.transform.parent = null;
            //grabbedObject = null;

        }

        lifting = false;
        vulnerable = true;
        bounce = true;
    }

    //Pow Move Function
    void PowerMove()
    {
        canDash = false;
        activateTimer();
        if (frontAttack)
        {
            rb.AddForce((powMoveVel[0].x * dirInf), (powMoveVel[0].y + curVel.y), 0, ForceMode.Impulse);
        }
        else if (backAttack)
        {
            rb.AddForce((powMoveVel[1].x * -transform.right.x * dirInf), (powMoveVel[1].y + curVel.y), 0, ForceMode.Impulse);
        }
        else if (upAttack)
        {
            rb.AddForce((powMoveVel[2].x * dirInf), (powMoveVel[2].y + curVel.y), 0, ForceMode.Impulse);
        }
        else if (downAttack)
        {
            rb.AddForce((powMoveVel[3].x * dirInf), (powMoveVel[3].y + curVel.y), 0, ForceMode.Impulse);
        }

        grabbedObject.transform.parent = grabTrigger.transform;
        grabbedObject.transform.position = grabTrigger.transform.position;
        grabbedObject.GetComponent<Rigidbody>().velocity = curVel;
    }

    //Bounce
    void Bounce()
    {
        bounce = false;
        applyBounce();
    }

    void activateBounce()
    {

    }

    void applyBounce()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce((-transform.right+transform.up)*bounceIntensity, ForceMode.Impulse); 
    }

    //Health Pickup
    public void Heal(int amount)
    {
        curHealth += amount;
    }

    //Getting hit by something with Damager component
    public void Hit(int amount, Vector3 kb, float stun)     
    {
        if (vulnerable)
        {
            curHealth -= amount;
            if (curHealth <= 0 && alive)
            {
                Die();
            }
        }
        else if (amount >=999)
        {
            Die();
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
        //Damager component overlap
        if (hitbox.GetComponent<Damager>())
        {
            Hit(other.gameObject.GetComponent<Damager>().damage,
                other.gameObject.GetComponent<Damager>().knockback,
                other.gameObject.GetComponent<Damager>().hitstun);
        }

        //Health component overlap
        if (hitbox.GetComponent<HealthPickup>())
        {
            Heal(other.gameObject.GetComponent<HealthPickup>().healthAmt);
            other.gameObject.GetComponent<HealthPickup>().gameObject.SetActive(false);
        }
    }
}
