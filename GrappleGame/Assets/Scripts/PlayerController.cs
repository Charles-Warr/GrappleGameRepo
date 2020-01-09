using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject feet;

    Vector2 dirInput;
    [SerializeField] Vector2 curVel;

    float gravity = 9.8f;
    [SerializeField] bool grounded;
    [SerializeField] float groundSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
     
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        dirInput.x = Input.GetAxis("Horizontal");
        dirInput.y = Input.GetAxis("Vertical");

        rb.velocity = curVel;

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

    void FixedUpdate()
    {
        curVel = new Vector3(dirInput.x * groundSpeed, curVel.y);
    }
}
