using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeCheck : MonoBehaviour
{
    public bool grounded;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Ground>())
        {
            grounded = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<Ground>())
        {
            grounded = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Ground>())
        {
            grounded = true;
        }
    }

}
