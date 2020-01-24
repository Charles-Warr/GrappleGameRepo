using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public GameObject objBottom;
    public bool objGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void die()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        objGrounded = objBottom.GetComponent<FeetCheck>().grounded;
    }
}
