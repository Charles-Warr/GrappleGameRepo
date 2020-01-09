using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] Vector3 camVec;
    [SerializeField] float zDist;
    [SerializeField] Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        transform.position = camVec;
        camVec.x = player.transform.position.x + offset.x;
        camVec.y = player.transform.position.y + offset.y;
        camVec.z = zDist;
    }
}
