using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private GameOver gameOver;

    [SerializeField] private Slider levelProgressSlider;
    [SerializeField] private TMP_Text currLevelView;
    [SerializeField] private TMP_Text nextLevelView;

    int level = 0;
    public int Level => level;

    void Start()
    {
        gameOver = GameOver.Instance;
        SetLevelView();
    }
    
    IEnumerator ProgressAnim(float p)
    {
        float x = levelProgressSlider.value;
        if (x > p)
            x = 0;
        const float speed = 0.005f;
        while (x <= p)
        {
            x += speed;

            levelProgressSlider.value = x;

            yield return new WaitForEndOfFrame();
        }
        if (Math.Abs(p - 1f) < 0.001f)
        {
            levelProgressSlider.value = 1;
        }
    }
    
    public void SetLevelProgress(float p)
    {
        StartCoroutine(ProgressAnim(p));
    }
    
    public void PassLevel(bool changeUI)
    {
        level++;
        if (changeUI)
            SetLevelView();
    }

    public void SetLevelView()
    {
        currLevelView.text = level + "";
        nextLevelView.text = (level + 1) + "";
    }

    public void GameOverScene()
    {
        gameOver.OnGameOver();
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
