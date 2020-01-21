using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pause;
    [SerializeField] GameObject options;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            paused = !paused;
            if (paused)
            {
                Time.timeScale = 0;
                pause.SetActive(true);
                Cursor.visible = true;
            }
            else
            {
                Resume();

            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        paused = false;
        pause.SetActive(false);
        Cursor.visible = false;
    }

    public void Options()
    {
        pause.SetActive(false);
        options.SetActive(true);
    }

    public void Back()
    {
        options.SetActive(false);
        pause.SetActive(true);
    }

    public void BackToMenu()
    {

        SceneManager.LoadScene("Level_MainMenu");
        Time.timeScale = 1;
    }
}
