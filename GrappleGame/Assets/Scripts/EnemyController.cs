using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody enemybody;
    public CapsuleCollider feet;
    public Transform Destination1;
    public Transform Destination2;

    public float motionIncrement;

    public float delay;

    private float timer;

    private Transform currentPosition;

    private bool canMove;
    private bool movingRight;
    private bool movingLeft;

    [Serialized] bool grounded;


    void Start()
    {
        currentPosition = this.GetComponent<Transform>();
        enemybody = this.GetComponent<Rigidbody>();
        movingLeft = false;
        canMove = true;

    }

    private void stopMotion()
    {
        if((currentPosition.position.x.Equals(Destination2.position.x) && !movingLeft) || ((currentPosition.position.x > Destination2.position.x)&& movingRight))
        {
            movingRight = false;
            canMove = false;
            movingLeft = true;

            enemybody.velocity = Vector3.zero;

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


    private void MoveToPosition()
    {
        if (canMove)
        {
            if (movingRight)
                enemybody.velocity = new Vector3(motionIncrement, 0, 0);
            else
                enemybody.velocity = new Vector3(motionIncrement * -1f, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((timer >= delay) && !canMove)
        {
            canMove = true;
            timer = 0;
        }

        stopMotion();

        if(((currentPosition.position.x.Equals(Destination1.position.x))||(currentPosition.position.x < Destination2.position.x)) && !movingLeft)
        {
            movingRight = true;
        }
        else
        {
            movingLeft = true;
        }

    }

    void FixedUpdate()
    {
        MoveToPosition();
    }
}

internal class SerializedAttribute : Attribute
{
}