using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public bool hit;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        damage = 0;
        hit = false;
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Damager>())
        {
            damage = collision.gameObject.GetComponent<Damager>().damage;
            hit = true;
        }
    }
    */
    public void applyDamage(int amount)
    {
        damage = amount;
        hit = true;
    }

    public void setHit(bool flag)
    {
        hit = flag;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
