using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemyController : MonoBehaviour
{
    public Rigidbody enemybody;
    public CapsuleCollider wallDetector;
    public Transform StartingPoint;

    public float motionSpeed;
    public float rotationSpeed;


    private Transform currentPosition;

    private float currentRotation;
    private float firstRotation;

    private bool canMove;
    private bool startingRotation;
    private bool endingRotation;


    private bool movingLeft;

    private bool wallDetected;



    void Start()
    {
        currentPosition = this.GetComponent<Transform>();
        enemybody = this.GetComponent<Rigidbody>();
        currentPosition.SetPositionAndRotation(StartingPoint.position, currentPosition.rotation);
        firstRotation = currentPosition.rotation.eulerAngles.y;
        movingLeft = false;
        canMove = true;
        startingRotation = false;
        endingRotation = true;

    }



    private void turnAround()
    {
        if (wallDetected)
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
        if (!canMove && startingRotation)
        {

            startingRotation = false;

            endingRotation = true;
        }

        if (!canMove && endingRotation)
        {

            if (currentRotation - firstRotation >= 180f)
            {
                enemybody.angularVelocity = Vector3.zero;
                canMove = true;
                firstRotation = currentRotation;
            }
        }

    }

    private void jump()
    {
        
    }

    private void MoveToPosition()
    {
        if (canMove)
        {
            enemybody.angularVelocity = Vector3.zero;

            enemybody.velocity = new Vector3(motionSpeed * enemybody.transform.forward.z, 0, 0);

        }
    }

    void Update()
    {

        wallDetected = wallDetector.GetComponent<FeetCheck>().grounded;


        currentRotation = currentPosition.rotation.eulerAngles.y;

    }

    void FixedUpdate()
    {
        stopRotation();
        MoveToPosition();
        jump();
        turnAround();
    }
}
