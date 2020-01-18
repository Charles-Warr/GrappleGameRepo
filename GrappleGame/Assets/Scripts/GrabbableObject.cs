using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    [SerializeField] GameObject objBottom;
    public bool objGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (objBottom.GetComponent<Ground>())
        {
            objGrounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (objBottom.GetComponent<Ground>())
        {
            objGrounded = false;
        }
    }
}
