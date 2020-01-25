using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public bool hit;
    public int damage;

    private float hitTimer;
    private float hitTimerStart = 2f;
    // Start is called before the first frame update
    void Start()
    {
        damage = 0;
        hit = false;
        hitTimer = 0f;
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
        
        if (hitTimer <= 0f)
        {
            hitTimer = hitTimerStart;
            damage = amount;
            hit = true;

        }
    }

    public void setHit(bool flag)
    {
        hit = flag;
    }
    // Update is called once per frame
    void Update()
    {
        hitTimer -= Time.deltaTime;
    }
}
