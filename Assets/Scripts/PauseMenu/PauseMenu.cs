using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public static float PausedTime;

    // Receive the PauseMenu game object
    public GameObject pauseMenuUI;
    public GameObject volumePanel;

    public TMP_Text menuLabelText;

    private ScoreSystem scoreSystem;

    [SerializeField] private RetryMenu retryMenu;    

    private AudioManager audioManager;
    private Timer timer;

    private void Start()
    {
        audioManager = AudioManager.instance;
        Resume();
        scoreSystem = FindObjectOfType<ScoreSystem>();
        timer = FindObjectOfType<Timer>();
        PausedTime = 0f;
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
        audioManager.PlaySFX("Button");
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
        audioManager.PlaySFX("Button");

        menuLabelText.text = "PAUSADO";
        // Show the Pause Menu in the canvas
        pauseMenuUI.SetActive(true);

        // Freeze the game
        Time.timeScale = 0f;
        
        // Set true meaning that the game is paused
        isGamePaused = true;
        
        StartCoroutine(CountPausedTime());

        //BackgroundImage.SetActive(true);
    }

    private IEnumerator CountPausedTime()
    {
        while (isGamePaused)
        {
            yield return new WaitForSecondsRealtime(1f);
            PausedTime += 1f;
            Debug.Log($"Paused for {PausedTime}");
        }
        
        var matchData = new MatchData()
        {
            score = scoreSystem.ScoreAmount,
            rem_time = Mathf.RoundToInt(timer.CurrentTime),
            paused_time = Mathf.RoundToInt(PausedTime)
        };
        
        Debug.Log(PausedTime);

        yield return MatchRequestHandler.SaveMatch(
            matchData,
            req => Debug.Log($"{req.responseCode}: {req.downloadHandler.text}"),
            OnFailure: req => Debug.LogError($"{req.responseCode}: {req.downloadHandler.text}")
        );
    }

    // Open the Main Menu
    public void FinishAndLoadMenu()
    {
        audioManager.PlaySFX("Button");
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

    private float effecttmp;
    
    public void Volume()
    {
        audioManager.PlaySFX("Button");

        menuLabelText.text = "VOLUME";
                        
        volumePanel.SetActive(true);
    }
}
