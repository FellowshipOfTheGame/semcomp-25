using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    // variables
    [SerializeField] int startingTime;


    // state variables
    private float currentTime;
    private bool paused = false;

    // cached references
    [SerializeField] TextMeshProUGUI timerText;

    private void Start()
    {
        if (!timerText)
            Debug.Log("Link timer Text UI");

        currentTime = startingTime;
        UpdateTimerText(currentTime);
    }

    private void Update()
    {
        if (!paused)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                UpdateTimerText(0);
                Debug.Log("call game over");
                SetPaused(true);
                return;
            }
            
            UpdateTimerText(currentTime); 
        }
    }

    public void SetPaused(bool pause)
    {
        paused = pause;
    }

    public void AddTime(float seconds)
    {
        if (paused) return;

        currentTime += seconds;
        UpdateTimerText(currentTime);
    }

    private void UpdateTimerText(float time)
    {
        if (!timerText) return;

        int minutes = (int) (time / 60f);
        int secondsLeft = (int) (time % 60f);

        if (secondsLeft < 10)
            timerText.text = minutes + ":0" + secondsLeft;
        else
            timerText.text = minutes + ":" + secondsLeft;
    }
}
