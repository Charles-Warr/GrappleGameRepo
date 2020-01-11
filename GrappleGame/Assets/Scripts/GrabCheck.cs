using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCheck : MonoBehaviour
{
    public bool grabbable;
    public GameObject objectGrabbed;

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
        if (other.gameObject.GetComponent<GrabbableObject>())
        {
            grabbable = true;
            objectGrabbed = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<GrabbableObject>())
        {
            grabbable = false;
            objectGrabbed = null;
        }
    }
}
