using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyHealth : MonoBehaviour
{
    public int health = 1;

    private GameObject self;

    // Start is called before the first frame update
    void Start()
    {
        self = this.gameObject;
    }

    void checkHealth()
    {
        if(health <= 0)
        {
            self.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        checkHealth();
    }
}
