using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
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
        if (other.gameObject.GetComponent<Wall>())
        {
            grounded = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Wall>())
        {
            grounded = false;
        }
    }


}
