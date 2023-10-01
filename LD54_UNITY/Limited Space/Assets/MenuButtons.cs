using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;

    bool paused;

    // Start is called before the first frame update
    void Start()
    {
        bool menuIsOpen = pauseMenu.activeInHierarchy || optionsMenu.activeInHierarchy;
        if (menuIsOpen)
        {
            Pause();
        }
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
        Time.timeScale = 1;
    }
    public void Pause()
    {
        paused = true;
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0;
    }

    public void OpenOptionsMenu()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void OpenBaseMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
