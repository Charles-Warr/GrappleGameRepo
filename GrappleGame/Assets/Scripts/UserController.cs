using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class UserController : MonoBehaviour
{
    private Rigidbody player;
    
    [System.Serializable]
    public class Settings
    {
        public float acceleration = 10f;
        public float deceleration = 3f;
        public float maxVelocity = 7f;
        public float dashForce = 6f;
        public float dashDistance = 5f;
        public float bounceForce;
        public float upVelocity;
        public float maxHealth =3f;
        public float currentHealth;
    }

    [System.Serializable]
    public class CharacterComponents
    {
        public GameObject grabTrigger;
        public GameObject grabbedObject;
        public GameObject feet;
        public Collider hitBox;
        public GameObject HealthUI;
    }

    [System.Serializable]
    public class JumpMoves
    {
        public Vector2 up;
        public Vector2 front;
        public Vector2 back;
        public Vector2 down;
    }



    private bool acceptInput;
    private bool grounded;
    private bool dashing;
    private bool holding;
    private bool jumping;
    private bool falling;
    private bool bouncing;
    private bool vulnerable;

    private float dirInf;
    private float slopeVel;

    private Vector2 dashStart;
    private Vector2 jumpStart;
    private Vector2 chosenMove;

    public Settings settings = new Settings();
    public CharacterComponents comp = new CharacterComponents();
    public JumpMoves powerJump = new JumpMoves();

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<Rigidbody>();
        comp.HealthUI = GameObject.FindGameObjectWithTag("Health");
        acceptInput = true;
        dashing = false;
        holding = false;
        jumping = false;
        bouncing = false;
        falling = false;
        vulnerable = true;

        settings.currentHealth = settings.maxHealth;
        comp.hitBox = this.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        dirInf = Input.GetAxis("Horizontal");
        grounded = comp.feet.GetComponent<FeetCheck>().grounded;

        if(vulnerable && GetComponent<TakeDamage>().hit)
        {
            hit(GetComponent<TakeDamage>().damage);
            GetComponent<TakeDamage>().setHit(false);
        }

        {
            comp.HealthUI.GetComponent<TextMeshProUGUI>().text = "Health:" + settings.currentHealth;

            if (settings.currentHealth <= 0)
            {
                settings.currentHealth = 0;

               
            }

            if (settings.currentHealth >= settings.maxHealth)
                settings.currentHealth = settings.maxHealth;
        }

            if (Input.GetButtonDown("Fire3") && !dashing && !holding && !jumping)
        {
            dashing = true;
            acceptInput = false;
            dashStart = player.transform.position;
        }

        if(Input.GetButtonDown("Fire3") && holding)
        {
            holding = false;
        }

        if(Input.GetButtonDown("Fire3") && falling)
        {
            
            falling = false;
            holding = false;
            bouncing = true;

            player.velocity = Vector3.zero;
            jumpStart = player.transform.position;
            
        }

        if(Input.GetButtonDown("Jump") && holding && !falling && !jumping && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            int choice;

            if(Input.GetAxis("Horizontal") > 0)
            {
                if(Mathf.Sign(player.transform.right.x) == 1)
                {
                    choice = 1;
                }
                else
                {
                    choice = 2;
                }
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                if (Mathf.Sign(player.transform.right.x) == 1)
                {
                    choice = 2;
                }
                else
                {
                    choice = 1;
                }
            }
            else if(Input.GetAxis("Vertical") > 0)
            {
                choice = 3;
            }
            else
            {
                choice = 4;
            }

            switch(choice)
            {
                case 1:
                    chosenMove = powerJump.front;
                    break;
                case 2:
                    chosenMove = powerJump.back;
                    break;
                case 3:
                    chosenMove = powerJump.up;
                    break;
                case 4:
                    chosenMove = powerJump.down;
                    break;
            }

            //jumpStart = player.transform.position;

            jumping = true;
        }

       // Debug.Log(Mathf.Sign(player.transform.right.x));
        
    }

    private void applyDamage()
    {
        if(comp.grabbedObject.GetComponent<EnemyHealth>())
        {
            comp.grabbedObject.GetComponent<EnemyHealth>().Hit(1);
        }
    }

    public void heal(int amount)
    {
        settings.currentHealth += amount;
    }

    public void die()
    {
        gameObject.GetComponent<UserController>().enabled = false;
        //settings.currentHealth = settings.maxHealth;
    }

    private void hit(int amount)
    {
        if (vulnerable)
        {
            settings.currentHealth -= amount;
            jumpStart = player.transform.position;
            bouncing = true;

            if (settings.currentHealth <= 0)
            {
                die();
            }
        }
        else if (amount >= 999)
        {
            die();
        }
    }

    void OnTriggerEnter(Collider other)
    {

        //Health component overlap
        if (other.gameObject.GetComponent<HealthPickup>())
        {
            heal(other.gameObject.GetComponent<HealthPickup>().healthAmt);
            other.gameObject.GetComponent<HealthPickup>().gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        bounce();
        fall();
        jump();
        hold();
        dash();
        move();
        if (!grounded)
            player.velocity += Physics.gravity * Time.deltaTime;
    }

    private void jump()
    {
        if(jumping)
        {
            player.AddForce(new Vector2(chosenMove.x*player.transform.right.x, chosenMove.y), ForceMode.VelocityChange);
            jumping = false;
            falling = true;
        }
        
    }

    private void fall()
    {
        if(falling)
        {
        
            player.velocity += new Vector3(dirInf, Physics.gravity.y, 0) * Time.deltaTime;

            if(grounded && player.velocity.y < 0)
            {
                applyDamage();
                falling = false;
                holding = false;
                bouncing = true;

                player.velocity = Vector3.zero;
                jumpStart = player.transform.position;

                
            }

            //Debug.Log("Falling");
        }
    }

    private void bounce()
    {
        if(bouncing)
        {
            if (Mathf.Abs(player.position.x - jumpStart.x) < 1f && Mathf.Abs(player.position.y - jumpStart.y) < 1f)
                player.AddForce(new Vector3(-player.transform.right.x * settings.bounceForce / 4, settings.bounceForce/2), ForceMode.Impulse);
            else
                bouncing = false;
        }
    }

    private void hold()
    {
        if(holding)
        {
            dashing = false;

            if(!jumping && !falling)
                player.velocity = new Vector2(0f, player.velocity.y);

            if (comp.grabbedObject != null)
            {

                comp.grabbedObject.transform.parent = comp.grabTrigger.transform;
                comp.grabbedObject.transform.position = comp.grabTrigger.transform.position;
                comp.grabbedObject.GetComponent<Rigidbody>().velocity = player.velocity;


            }

        }
        else
        {
            if (comp.grabTrigger.GetComponent<GrabCheck>().objectInRange != null)
            {
                comp.grabTrigger.GetComponent<GrabCheck>().objectInRange.transform.parent = null;
                comp.grabbedObject = null;
            }
        }
    }

    private void dash()
    {
        if(dashing)
        {
            vulnerable = false;

            if(Mathf.Abs(player.transform.position.x - dashStart.x) < settings.dashDistance)
            {
                player.AddForce(player.transform.right * settings.dashForce , ForceMode.VelocityChange);

                if (comp.grabTrigger.GetComponent<GrabCheck>().grabbable)
                {
                    holding = true;
                    dashing = false;

                    comp.grabbedObject = comp.grabTrigger.GetComponent<GrabCheck>().objectInRange;

                    if (comp.grabbedObject != null)
                    {

                        comp.grabbedObject.transform.parent = comp.grabTrigger.transform;
                        comp.grabbedObject.transform.position = comp.grabTrigger.transform.position;
                        comp.grabbedObject.GetComponent<Rigidbody>().velocity = player.velocity;


                    }
                }
            }
            else
            {
                player.velocity = new Vector2(player.velocity.x - (player.velocity.x * Time.deltaTime * settings.deceleration), player.velocity.y);

                if(comp.grabTrigger.GetComponent<GrabCheck>().grabbable)
                {
                    holding = true;
                    dashing = false;

                    comp.grabbedObject = comp.grabTrigger.GetComponent<GrabCheck>().objectInRange;

                    if (comp.grabbedObject != null)
                    {

                        comp.grabbedObject.transform.parent = comp.grabTrigger.transform;
                        comp.grabbedObject.transform.position = comp.grabTrigger.transform.position;
                        comp.grabbedObject.GetComponent<Rigidbody>().velocity = player.velocity;


                    }
                }

                if (Mathf.Abs(player.velocity.x) < 1f)
                {
                    dashing = false;
                }
            }

        }
        else if(!holding && !jumping && !falling && !bouncing)
        {
            vulnerable = true;
            acceptInput = true;
        }
    }

    private void move()
    {

        if(Input.GetButton("Horizontal"))
        {
            if(Mathf.Sign(player.velocity.x) != Mathf.Sign(player.transform.right.x))
            {
                transform.rotation = Quaternion.LookRotation(-1*player.transform.forward, Vector3.up);
            }
        }

        Debug.DrawRay(new Vector3(player.position.x + (.5f * player.transform.right.x), player.position.y - .5f, player.position.z), gameObject.transform.right, Color.red);

        RaycastHit slopes;

        if (Physics.Raycast(new Vector3(player.position.x + (.5f * player.transform.right.x), player.position.y - .5f, player.position.z), gameObject.transform.right, out slopes, 1f))
        {
            if (slopes.collider.GetComponent<Ground>())
            {
                slopeVel = settings.upVelocity;
                //curVel = new Vector2(dirInput.x * groundSpeed, curVel.y + slopeVelocity);
                //curVel += Vector2.up * slopeVelocity;
            }
            else
            {
                slopeVel = 0;
            }
        }

        


        if (acceptInput)
        {
            if(Input.GetButton("Horizontal"))
            {
                if(!(Mathf.Abs(player.velocity.x) >= settings.maxVelocity))
                {
                    player.AddForce(Input.GetAxis("Horizontal") * Vector2.right * settings.acceleration + (Vector2.up*slopeVel), ForceMode.Acceleration);
                }
                else
                {
                    player.velocity = new Vector2(player.velocity.x - (player.velocity.x*Time.deltaTime), player.velocity.y);
                }
                    
            }
            else
            {
                player.velocity = new Vector2(player.velocity.x-(player.velocity.x*Time.deltaTime*settings.deceleration), player.velocity.y);
            }
            //player.velocity += Input.GetAxis("Horizontal") * player.transform.right;
        }
    }
}
