using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject main;
    [SerializeField] GameObject options;

    private Button playButton;
    private Button optionButton;
    private Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsMenu()
    {
        main.SetActive(false);
        options.SetActive(true);
    }

    public void Back()
    {
        options.SetActive(false);
        main.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
