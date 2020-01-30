using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker_Animator : MonoBehaviour
{
    private Animator anim;
    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        body = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(body.velocity.normalized.x));
    }
}
