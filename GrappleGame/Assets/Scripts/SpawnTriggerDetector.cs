using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTriggerDetector : MonoBehaviour
{
    [SerializeField] GameObject playerTrigger;
    public GameObject objectToSpawn;
    public bool SpawnedIn;

    // Start is called before the first frame update
    void Start()
    {
        playerTrigger = FindObjectOfType<SpawnTrigger>().gameObject;
        objectToSpawn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(SpawnedIn)
            objectToSpawn.SetActive(true);
        else
            objectToSpawn.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerTrigger)
        {
            SpawnedIn = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerTrigger)
        {
            SpawnedIn = false;
        }
    }
}
