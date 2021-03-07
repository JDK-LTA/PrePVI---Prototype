using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private bool shouldStartWithMainMenu = false;
    private static bool mainMenuStart = false, hasStarted = false, isPaused = false;
    private void Start()
    {
        if (!hasStarted)
        {
            mainMenuStart = shouldStartWithMainMenu;
            hasStarted = true;
        }

        if (mainMenuStart)
        {
            Time.timeScale = 0f;
            RefsManager.I.MainMenu.SetActive(true);
        }
    }
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePauseGame();
        }
    }
    public void PlayGame()
    {
        Time.timeScale = 1f;
        RefsManager.I.MainMenu.SetActive(false);
    }
    public void RestartGame()
    {
        mainMenuStart = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void TogglePauseGame()
    {
        isPaused = Time.timeScale == 1f;
        Time.timeScale = isPaused ? 0f : 1f;
        RefsManager.I.PauseMenu.SetActive(isPaused);
    }
}
