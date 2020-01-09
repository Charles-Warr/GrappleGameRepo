using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pause;
    [SerializeField] GameObject options;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Resume()
    {
        pause.SetActive(false);
    }

    public void Options()
    {
        pause.SetActive(false);
        options.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.GetActiveScene().Equals(0); 
    }
}
