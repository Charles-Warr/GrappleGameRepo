using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBounds : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TakeDamage>())
        {
            other.gameObject.GetComponentInParent<PlayerController>().curHealth = 0;
        }

        if(other.gameObject.GetComponent<EnemyHealth>() || other.gameObject.GetComponent<GrabbableObject>())
        {
            other.gameObject.GetComponent<EnemyHealth>().Die();
            other.gameObject.GetComponent<GrabbableObject>().die();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
