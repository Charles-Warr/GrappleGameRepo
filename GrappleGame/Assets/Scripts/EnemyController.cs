using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody enemybody;
    [SerializeField] CapsuleCollider wallDetector;
    [SerializeField] CapsuleCollider ledgeDetector;
    [SerializeField] Transform StartingPoint;

    [SerializeField] float motionSpeed;
    [SerializeField] float rotationSpeed;

    private Transform currentPosition;

    private float currentRotation;
    private float firstRotation;

    private bool canMove;
    private bool rotating;
    private bool rotated;
    
    private bool wallDetected;
    private bool ledgeDetected;
    private float rotationDir;
    


    void Start()
    {
        currentPosition = this.GetComponent<Transform>();
        enemybody = this.GetComponent<Rigidbody>();
        if(StartingPoint != null)
            currentPosition.SetPositionAndRotation(StartingPoint.position, currentPosition.rotation);
        firstRotation = currentPosition.rotation.eulerAngles.y;
        canMove = true;

    }



    private void turnAround(bool flag, bool flag2, bool status)
    {
        if(flag || status || flag2)
        {
            if(!rotating)
            {
                firstRotation = currentRotation;
                rotationDir = -enemybody.transform.right.z;
            }

            rotate();
       
        }
    }
    

    private void rotate()
    {

        rotating = true;

        enemybody.transform.Rotate(0, Time.deltaTime * rotationSpeed * rotationDir, 0, Space.Self);

    }


    private void MoveToPosition(bool flag, bool status)
    {
        if(flag && !rotating)
        {
            enemybody.velocity = new Vector3(motionSpeed * -enemybody.transform.right.z, enemybody.velocity.y,0);
        }
        else
        {
            enemybody.velocity = new Vector3(0,enemybody.velocity.y,0);
        }
    }

    void Update()
    {
        
      
        wallDetected = wallDetector.GetComponent<WallCheck>().grounded;
        ledgeDetected = ledgeDetector.GetComponent<LedgeCheck>().grounded;
        canMove = !(wallDetected || ledgeDetected);

        currentRotation = currentPosition.rotation.eulerAngles.y;

        rotated = Mathf.Abs(currentRotation - firstRotation) >= 180f;

    }

    void FixedUpdate()
    {
        turnAround(wallDetected, ledgeDetected, rotating);
        MoveToPosition(canMove, rotating);

        if(rotated)
        {
           rotating = false;

            if(currentRotation >= 180f || currentRotation <= 270f)
            {
                enemybody.rotation.eulerAngles.Set(enemybody.rotation.eulerAngles.x,180f,enemybody.rotation.eulerAngles.z);
            }
            else
            {
                enemybody.rotation.eulerAngles.Set(enemybody.rotation.eulerAngles.x, 0, enemybody.rotation.eulerAngles.z);
            }

            rotated = false;
        }
        
    }
}

internal class SerializedAttribute : Attribute
{
}