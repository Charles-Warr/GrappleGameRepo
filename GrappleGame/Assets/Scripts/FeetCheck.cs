using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCheck : MonoBehaviour
{
    public bool grounded;

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
        if(other.gameObject.GetComponent<Ground>() || other.gameObject.GetComponent<GrabbableObject>())
        {
            grounded = true;
        }
 
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Ground>())
        {
            grounded = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Ground>() || other.gameObject.GetComponent<GrabbableObject>())
        {
            grounded = false;
        }
    }

 
}
