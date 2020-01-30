using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCheck : MonoBehaviour
{
    public GameObject objectInRange;
    public bool grabbable;

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
        if (other.gameObject.GetComponent<GrabbableObject>() && objectInRange == null)
        {
            objectInRange = other.gameObject;
            grabbable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<GrabbableObject>())
        {
            objectInRange = null;
            grabbable = false;
        }
    }
}
