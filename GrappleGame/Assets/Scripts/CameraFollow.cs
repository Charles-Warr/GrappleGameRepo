using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;

    [SerializeField] Vector3 camVec;
    [SerializeField] float zDist;
    [SerializeField] Vector3 offset;
    [SerializeField] float delay;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator followPlayer(Vector3 location)
    {
        yield return new WaitForSeconds(delay); 

        this.transform.position = new Vector3(location.x + offset.x, location.y + offset.y, zDist);

    }

    void LateUpdate()
    {
        /*
         transform.position = camVec;
         camVec.x = player.transform.position.x + offset.x;
         camVec.y = player.transform.position.y + offset.y;
         camVec.z = zDist;
         */
        
        if(player.GetComponent<PlayerController>().cameraTrigger)
            StartCoroutine(followPlayer(player.transform.position));

        //this.transform.position = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, zDist );
    }
}
