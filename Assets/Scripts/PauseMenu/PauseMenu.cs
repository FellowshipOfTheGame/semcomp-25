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

    // Receive the PauseMenu game object
    public GameObject pauseMenuUI;
    public GameObject volumePanel;

    public TMP_Text menuLabelText;

    private ScoreSystem scoreSystem;

    [SerializeField] private RetryMenu retryMenu;
    
    [SerializeField] private Slider masterVolumeSlider, effectsVolumeSlider, musicVolumeSlider;

    private void Awake()
    {
        Resume();
        scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the user press ESC button
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchPaused();
        }*/
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
    public void FinishAndLoadMenu()
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

    private float effecttmp;
    
    public void Volume()
    {
        menuLabelText.text = "VOLUME";
        
        // var audioMixer = FindObjectOfType<SettingsMenu>().audioMixer;
        
        // audioMixer.GetFloat("main-volume", out var masterValue);
        // masterVolumeSlider.value = Mathf.Log10(masterValue);
        // masterVolumeSlider.value = masterValue;
        // masterVolumeSlider.value = Remap(masterValue, -80f, 0f, 0f, 1f);
        
        // audioMixer.GetFloat("effects-volume", out var effectsValue);
        // sfxVolumeSlider.value = Mathf.Log10(effectsValue);
        // effectsVolumeSlider.value = effectsValue;
        // effecttmp = effectsValue;
        // sfxVolumeSlider.value = Remap(effectsValue, -80f, 0f, 0f, 1f);
        
        // audioMixer.GetFloat("music-volume", out var musicValue);
        // musicVolumeSlider.value = Mathf.Log10(musicVolume);
        // musicVolumeSlider.value = musicValue;
        // musicVolumeSlider.value = Remap(musicValue, -80f, 0f, 0f, 1f);
                
        volumePanel.SetActive(true);
    }
    
    float Remap(float value, float min1, float max1, float min2, float max2) 
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }
}
