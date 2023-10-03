using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject tutorialMenu;
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] TextMeshProUGUI gameOverMoneyText;

    public static int TotalEarnedMoney = 0;
    bool paused;

    // Start is called before the first frame update
    void Start()
    {
        TotalEarnedMoney = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Debug.Log("Paused!");
            if (paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Unpause()
    {
        paused = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Pause()
    {
        paused = true;
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        Time.timeScale = 0;
    }

    public void OpenGameOverScreen()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        tutorialMenu.SetActive(false);
        gameOverMoneyText.text = TotalEarnedMoney.ToString();
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        tutorialMenu.SetActive(false);
    }
    public void OpenBaseMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        tutorialMenu.SetActive(false);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
