using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemyController : MonoBehaviour
{
    private Rigidbody enemybody;
    [SerializeField] CapsuleCollider wallDetector;
    [SerializeField] CapsuleCollider feet;
    [SerializeField] Transform StartingPoint;

    [SerializeField] float motionSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpForwardForce;

    private Transform currentPosition;

    private float currentRotation;
    private float firstRotation;

    [SerializeField] float delayBeforeJump;
    [SerializeField] float delayAfterJump;

    private float timer;
    private float firstTime;

    private bool canMove;
    private bool rotating;
    private bool rotated;


    private bool jumping;
    private bool jumpInitiated;
    private bool landed;
    private bool jumped;
    private bool timeToJump;
    private bool timeToMove;

    private bool wallDetected;
    private bool grounded;



    void Start()
    {
        currentPosition = this.GetComponent<Transform>();
        enemybody = this.GetComponent<Rigidbody>();
        currentPosition.SetPositionAndRotation(StartingPoint.position, currentPosition.rotation);
        firstRotation = currentPosition.rotation.eulerAngles.y;
        canMove = true;
        firstTime = Time.time;
        jumpInitiated = false;
        landed = true;


    }



    private void turnAround(bool flag, bool status)
    {
        if (flag || status)
        {
            if (!rotating)
            {
                firstRotation = currentRotation;
            }

            rotate();

        }
    }

    private void startJump(bool onGround, bool flag)
    {
        if (onGround && flag)
        {
            if (!jumping)
            {
                jumpInitiated = true;
            }

            jump();
        }
    }

    private IEnumerator jump()
    {
        jumping = true;


        yield return new WaitForSeconds(0);
        if (landed)
        {
            enemybody.velocity = new Vector3(0, enemybody.velocity.y, 0);

            enemybody.AddForce(new Vector3(jumpForwardForce, jumpHeight, 0), ForceMode.Impulse);

            firstTime = Time.time - 1f;

            landed = false;
        }

    }


    private void rotate()
    {

        rotating = true;

        enemybody.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.Self);

    }


    private void MoveToPosition(bool flag)
    {
        if (flag && !rotating && !jumping)
        {
            enemybody.velocity = new Vector3(motionSpeed * enemybody.transform.forward.z, enemybody.velocity.y, 0);
        }
        else if (jumping)
        {
            // do not do anything if currently jumping.
        }
        else
        {
            enemybody.velocity = new Vector3(0, enemybody.velocity.y, 0);
        }
    }

    void Update()
    {

        wallDetected = wallDetector.GetComponent<WallCheck>().grounded;
        grounded = feet.GetComponent<FeetCheck>().grounded;

        canMove = !wallDetected;

        timer = Time.time - firstTime;

        timeToJump = timer >= delayBeforeJump;

        //timeToMove = 

        Debug.Log(timer);

        currentRotation = currentPosition.rotation.eulerAngles.y;

        rotated = Mathf.Abs(currentRotation - firstRotation) >= 180f;

        jumped = endOfJump();

    }

    private bool endOfJump()
    {
        if(!jumpInitiated)
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    void FixedUpdate()
    {
        turnAround(wallDetected, rotating);
        MoveToPosition(canMove);
        startJump(grounded, timeToJump);


        if (rotated)
        {
            rotating = false;

            if (currentRotation >= 180f || currentRotation <= 270f)
            {
                enemybody.rotation.eulerAngles.Set(enemybody.rotation.eulerAngles.x, 180f, enemybody.rotation.eulerAngles.z);
            }
            else
            {
                enemybody.rotation.eulerAngles.Set(enemybody.rotation.eulerAngles.x, 0, enemybody.rotation.eulerAngles.z);
            }

            rotated = false;
        }

        if(jumped)
        {
            jumping = false;


            jumped = false;

        }

    }

}
