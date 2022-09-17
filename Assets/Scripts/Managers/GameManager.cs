using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverPointsView;
    [SerializeField] private TMP_Text highScoreView;
    [SerializeField] private GameObject gameOverObj;

    [SerializeField] private Slider levelProgressSlider;

    [SerializeField] private TMP_Text currLevelView;
    [SerializeField] private TMP_Text nextLevelView;

    int level = 0;
    public int Level => level;

    private int highscore = 0;

    // Start is called before the first frame update
    private void Start()
    {
        SetLevelView();
        // highscore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private IEnumerator ProgressAnim(float p)
    {
        float x = levelProgressSlider.value;
        if (x > p)
            x = 0;
        const float speed = .7f;
        while (x <= p)
        {
            x += Time.deltaTime * speed;
            x = Mathf.Min(x, p);
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

    public void GameOverScene(int _highscore)
    {
        int points = FindObjectOfType<ScoreSystem>().ScoreAmount;
        highscore = Mathf.Max(_highscore, points);
        Debug.Log($"{(float)(points / _highscore) * 100}% happy");
        // PlayerPrefs.SetInt("HighScore", highscore);
        GameObject.FindWithTag("Ball").GetComponent<BallController>().enabled = false;
        GameObject.FindObjectOfType<PlayerInputManager>().SetCanMove(false) ;
        gameOverPointsView.text = points + "";
        highScoreView.text = "High Score: "+highscore + "";
        gameOverObj.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
