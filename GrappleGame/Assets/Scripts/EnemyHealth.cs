using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public bool vulnerable = true;
    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0)
        {
            Die();
        }
    }
    public void Hit(int amount)     //Getting hit by something with Damager component
    {
        if (vulnerable)
        {
            curHealth -= amount;
            if (curHealth <= 0 && alive)
            {
                Die();
            }
        }
        else if (amount >= 999)
        {
            Die();
        }
    }
    void Die()
    {
        alive = false;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        //Damager component overlap
        if (other.gameObject.GetComponent<Damager>())
        {
            Hit(other.gameObject.GetComponent<Damager>().damage);
        }
    }
}