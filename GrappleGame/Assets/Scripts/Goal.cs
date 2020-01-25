using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] bool endLevel;
    float timer;
    float timerStart = 5f;
    // Start is called before the first frame update
    void Start()
    {
        endLevel = false;
        timer = timerStart;
    }

    // Update is called once per frame
    void Update()
    {
        if(endLevel)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0f)
        {
            nextLevel();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<TakeDamage>())
        {
            endLevel = true;
            other.gameObject.GetComponentInParent<PlayerController>().enabled = false;
        }
    }

    void nextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
