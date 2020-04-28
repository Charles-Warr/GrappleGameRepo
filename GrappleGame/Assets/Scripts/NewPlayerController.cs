using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine;
using TMPro;

public class NewPlayerController : MonoBehaviour
{
    public Rigidbody rb;

    [System.Serializable]
    public class Settings
    {
        public float maxVelocity = 7f;
        public float acceleration = 10f;
        public float deceleration = 3f;
        public float dashDistance = 5f;
        public float dashForce = 6f;
        public float bounceForce;
        public float upVelocity;
        public float maxHealth = 3f;
        public float currentHealth;
    }

    [System.Serializable]
    public class CharacterComponents
    {
        public GameObject grabTrigger;
        public GameObject grabbedObject;
        public GameObject feet;
        public Collider hitBox;
        public GameObject HealthUI;
    }


    public Settings settings = new Settings();
    public CharacterComponents comp = new CharacterComponents();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        dirInf = Input.GetAxis("Horizontal");


    }

    void FixedUpdate()
    {

    }



}
