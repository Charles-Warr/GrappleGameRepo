using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] Vector3 offset;
    [SerializeField] float smoothSpeed;
    [SerializeField] float speedSmoothSpeed;
    [SerializeField] float xMin;
    [SerializeField] float xMax;
    [SerializeField] float yMin;
    [SerializeField] float yMax;

    private bool moveCamera;

    void Start()
    {

    }

    void Update()
    {
        moveCamera = checkPlayerPos();

        
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition;
       
        desiredPosition = player.transform.position + offset;

        
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);

        Vector3 speedPosition = Vector3.Lerp(transform.position, desiredPosition, speedSmoothSpeed * Time.deltaTime);

        if(moveCamera)
        {
            transform.position = speedPosition;
        }
        else
            transform.position = smoothPosition;
        
    }

    bool checkPlayerPos()
    {

        Vector3 playerPos = Camera.main.WorldToViewportPoint(player.transform.position);

        if (playerPos.x > xMax || playerPos.x < xMin || playerPos.y > yMax || playerPos.y < yMin)
            return true;
        else
            return false;
        
    }

}
