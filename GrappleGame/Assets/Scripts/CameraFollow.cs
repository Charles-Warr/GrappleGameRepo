﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;

    [SerializeField] Vector3 offset = new Vector3(0f, 1f, -25f);
    [SerializeField] float smoothSpeed = 0.875f;
    [SerializeField] float speedSmoothSpeed = 3f;
    [SerializeField] float xMin = 0.4f;
    [SerializeField] float xMax = 0.6f;
    [SerializeField] float yMin = 0.4f;
    [SerializeField] float yMax = 0.7f;

    private bool moveCamera;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
