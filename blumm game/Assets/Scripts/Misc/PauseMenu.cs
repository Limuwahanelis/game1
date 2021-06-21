using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public BoolReference isGamePaused;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused.value)
            {
                Pause();
                isGamePaused.value = true;
            }
            else
            {
                Resume();
                isGamePaused.value = false;
            }
        }
    }

    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
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
