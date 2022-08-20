using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    // variables
    [SerializeField] int startingTime;
    [SerializeField] private float timeAddedOnPass;

    // state variables
    private float currentTime;
    private bool paused = false;
    
    public float CurrentTime
    {
        get => currentTime;
        private set
        {
            currentTime = value;
            UpdateTimerTextUI();
        }
    }

    // cached references
    [SerializeField] TextMeshProUGUI timerText;

    private void Start()
    {
        if (!timerText)
            Debug.Log("Link timer Text UI");

        CurrentTime = startingTime;
        MapManager.OnSuccessfulPass += SuccessfulPassScored;
    }

    private void Update()
    {
        if (!paused)
        {
            CurrentTime -= Time.deltaTime;
            if (CurrentTime < 0)
            {
                CurrentTime = 0;
                Debug.Log("call game over");
                SetPaused(true);
            }
        }
    }

    private void SuccessfulPassScored()
    {
        AddTime(timeAddedOnPass);
    }

    public void SetPaused(bool pause)
    {
        paused = pause;
    }

    public void AddTime(float seconds)
    {
        if (paused) return;

        CurrentTime += seconds;
    }

    private void UpdateTimerTextUI()
    {
        if (!timerText) return;

        timerText.text = CurrentTime.ToString("00.00") + "";

        /*int minutes = (int) (CurrentTime / 60f);
        int secondsLeft = (int) (CurrentTime % 60f);

        if (secondsLeft < 10)
            timerText.text = minutes + ":0" + secondsLeft;
        else
            timerText.text = minutes + ":" + secondsLeft;*/
    }
}
