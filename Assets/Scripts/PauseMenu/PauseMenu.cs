using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;

    // Receive the PauseMenu game object
    public GameObject pauseMenuUI;
    public GameObject volumePanel;

    public TMP_Text menuLabelText;

    private ScoreSystem scoreSystem;

    [SerializeField] private RetryMenu retryMenu;

    private void Awake()
    {
        Resume();
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the user press ESC button
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPaused();
        }
    }
    public void SwitchPaused()
    {
        // IF the game is already paused, exits the pause menu
        if (isGamePaused)
        {
            Resume();
        }
        // ELSE join the pause menu
        else
        {
            Pause();
        }
    }
    // Exit the Pause Menu
    public void Resume()
    {
        // Exit the Pause Menu in the canvas
        pauseMenuUI.SetActive(false);

        // Unfreeze the game
        Time.timeScale = 1f;

        // Set false meaning that the game is NOT paused
        isGamePaused = false;
    }

    // Open the Pause Menu
    public void Pause()
    {
        menuLabelText.text = "PAUSADO";
        // Show the Pause Menu in the canvas
        pauseMenuUI.SetActive(true);

        // Freeze the game
        Time.timeScale = 0f;

        // Set true meaning that the game is paused
        isGamePaused = true;

        //BackgroundImage.SetActive(true);
    }

    // Open the Main Menu
    public void LoadMenu()
    {
#if !UNITY_EDITOR
        StartCoroutine(FinishAndLoadMenuEnumerator());
#else
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene(0);
#endif
    }

    private IEnumerator FinishAndLoadMenuEnumerator()
    {
        var matchData = new MatchData()
        {
            score = scoreSystem.ScoreAmount
        };

        yield return StartCoroutine(MatchRequestHandler.FinishMatch(
            matchData,
            data =>
            {
                retryMenu.Close();
                Time.timeScale = 1f;
                isGamePaused = false;
                SceneManager.LoadScene(0);
            },
            req => retryMenu.InternetConnectionLost(FinishAndLoadMenuEnumerator())
            ));
    }

    public void ReloadScene()
    {
        // StartCoroutine(FinishAndLoadSceneEnumerator(SceneManager.GetActiveScene().buildIndex));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }

    public void Volume()
    {
        menuLabelText.text = "VOLUME";
                
        volumePanel.SetActive(true);
    }
}
