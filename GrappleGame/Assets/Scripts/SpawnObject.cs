using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(obj, this.transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
       // activateTimer();
    }

    
}
