using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public bool vulnerable = true;
    public bool alive = true;
    public GameObject spawnItem;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Hit(int amount)     //Getting hit by something with Damager component
    {
 
            curHealth -= amount;
            if (curHealth <= 0)
            {
                if(spawnItem != null)
                    Instantiate(spawnItem, transform.position, Quaternion.identity);
                
                Die();
            }
        

    }
    public void Die()
    {
        
        alive = false;
        gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        //Damager component overlap
        if (other.gameObject.GetComponent<Damager>())
        {
            Hit(other.gameObject.GetComponent<Damager>().damage);
        }
    }
    */
}