using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public float checkPointNumber;
    private Transform spawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = gameObject.GetComponentInParent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TakeDamage>())
        {
            GameObject.FindGameObjectWithTag("Respawn").transform.position = new Vector3(spawnPosition.position.x, spawnPosition.position.y, GameObject.FindGameObjectWithTag("Respawn").transform.position.z);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
