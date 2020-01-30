using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject obj;

    public float timer = 1f;
    [SerializeField] float timerStart;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(obj, this.transform.position, Quaternion.identity);
        timerStart = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameObject.FindGameObjectWithTag("Player").GetComponent<UserController>().enabled  && GameObject.FindGameObjectWithTag("Player").GetComponent<UserController>().settings.currentHealth == 0)
        { 
            timer -= Time.deltaTime;

            

            if(timer <= 0f)
            {
                
                GameObject.FindGameObjectWithTag("Player").transform.position = this.transform.position;
                GameObject.FindGameObjectWithTag("Player").GetComponent<UserController>().enabled = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<UserController>().settings.currentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<UserController>().settings.maxHealth;
                timer = timerStart;
                
            }

            
        }
       // activateTimer();
    }

    
}
