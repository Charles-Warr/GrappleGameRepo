using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody enemybody;
    public CapsuleCollider feet;
    public CapsuleCollider wallDetector;
    public Transform Destination1;
    public Transform Destination2;

    public float motionSpeed;
    public float rotationSpeed;

    public float delay;

    private float timer;

    private Transform currentPosition;

    private float currentRotation;
    private float firstRotation;

    private float facingDirection;

    [SerializeField]private bool canMove;
    [SerializeField]private bool startingRotation;
    [SerializeField]private bool endingRotation;

    private bool movingRight;
    private bool movingLeft;

    [SerializeField] bool wallDetected;
    


    void Start()
    {
        currentPosition = this.GetComponent<Transform>();
        enemybody = this.GetComponent<Rigidbody>();
        currentPosition.SetPositionAndRotation(Destination1.position, currentPosition.rotation);
        firstRotation = currentPosition.rotation.eulerAngles.y;
        movingLeft = false;
        canMove = true;
        startingRotation = false;
        endingRotation = true;
        //facingDirection = enemybody.

    }

    private void stopMotion()
    {
        if((currentPosition.position.x.Equals(Destination2.position.x) && !movingLeft) || ((currentPosition.position.x > Destination2.position.x)&& movingRight))
        {
            movingRight = false;
            canMove = false;
            movingLeft = true;

            enemybody.velocity = Vector3.zero;
            rotate();

            timer = 0;
        }

        if((currentPosition.position.x.Equals(Destination1.position.x) && !movingRight) || ((currentPosition.position.x < Destination1.position.x) && movingLeft))
        {
            movingRight = true;
            canMove = false;
            movingLeft = false;

            enemybody.velocity = Vector3.zero;

            timer = 0;
        }
    }

    private void turnAround()
    {
        if(wallDetected)
        {
            rotate();
       
        }
    }
    

    private void rotate()
    {
        canMove = false;
        startingRotation = true;
        endingRotation = false;
        enemybody.velocity = Vector3.zero;

        enemybody.angularVelocity = new Vector3(0, rotationSpeed);
    }

    private void stopRotation()
    {
       if(!canMove && startingRotation)
        {

            startingRotation = false;
            
            endingRotation = true;
        }

       if(!canMove && endingRotation)
        {
     
            if(currentRotation-firstRotation >= 180f)
            {
                enemybody.angularVelocity = Vector3.zero;
                canMove = true;
                firstRotation = currentRotation;
            }
        }

    }

    private void MoveToPosition()
    {
        if (canMove)
        {
            enemybody.angularVelocity = Vector3.zero;

         //   if (movingRight)
                enemybody.velocity = new Vector3(motionSpeed* enemybody.transform.forward.z, 0, 0);
         //   else
         //       enemybody.velocity = new Vector3(motionSpeed * -1f, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
        Debug.Log(enemybody.transform.forward);
        wallDetected = wallDetector.GetComponent<FeetCheck>().grounded;


        currentRotation = currentPosition.rotation.eulerAngles.y;


        /*

         stopMotion();

         if(((currentPosition.position.x.Equals(Destination1.position.x))||(currentPosition.position.x < Destination2.position.x)) && !movingLeft)
         {
             movingRight = true;
         }
         else
         {
             movingLeft = true;
         }
         */
    }

    void FixedUpdate()
    {
        stopRotation();
        MoveToPosition();
        turnAround();
    }
}

internal class SerializedAttribute : Attribute
{
}