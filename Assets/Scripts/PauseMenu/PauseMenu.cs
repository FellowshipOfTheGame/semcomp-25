using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    private static bool _isGamePaused = false;

    // Receive the PauseMenu game object
    public GameObject pauseMenuUI;
    public GameObject volumePanel;

    public TMP_Text menuLabelText;

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
        if (_isGamePaused)
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
        //BackgroundImage.SetActive(false);

        // Unfreeze the game
        Time.timeScale = 1f;

        // Set false meaning that the game is NOT paused
        _isGamePaused = false;
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
        _isGamePaused = true;

        //BackgroundImage.SetActive(true);
    }

    // Open the Main Menu
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
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
