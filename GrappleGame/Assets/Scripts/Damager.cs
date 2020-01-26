using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damage;
    public Vector2 knockback;
    public float hitstun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TakeDamage>())
        {
            Debug.Log("Player");

            other.gameObject.GetComponent<TakeDamage>().applyDamage(damage);
                 
            //other.gameObject
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
