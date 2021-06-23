using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public static bool isGamePaused=false;

    private void Start()
    {
        isGamePaused = false;
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void ShowPanel(GameObject panelToShow) 
    {
        panelToShow.SetActive(true);
    }
    public void HidePanel(GameObject panelToHide)
    {
        panelToHide.SetActive(false);
    }
}
