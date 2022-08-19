using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverPointsView;
    [SerializeField] private TMP_Text highScoreView;
    [SerializeField] private GameObject gameOverObj;

    [SerializeField] private Slider levelProgressSlider;

    [SerializeField] private TMP_Text currLevelView;
    [SerializeField] private TMP_Text nextLevelView;

    float progress = 0;
    int level = 0;
    public int Level => level;

    private int highscore = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetLevelView();
        highscore=PlayerPrefs.GetInt("HighScore", 0);
    }
    
    IEnumerator ProgressAnim(float min,float p)
    {
        if (min == 1f)
        {
            min = 0;
        }
        float _x = min;
        float speed = 0.005f;
        while (_x <= p)
        {
            _x += speed;

            levelProgressSlider.value = _x;

            yield return new WaitForEndOfFrame();
        }
        if (p == 1f)
        {
            levelProgressSlider.value = 0;
        }
    }
    
    public void SetLevelProgress(float p)
    {
        float initial = progress;
        progress = p;
        StartCoroutine(ProgressAnim(initial,progress));
    }
    
    public void PassLevel()
    {
        level++;
        SetLevelView();
    }

    public void SetLevelView()
    {
        currLevelView.text = level + "";
        nextLevelView.text = (level + 1) + "";
    }

    public void GameOverScene()
    {
        int points = FindObjectOfType<ScoreSystem>().ScoreAmount;
        highscore = (int)Mathf.Max(highscore, points);
        PlayerPrefs.SetInt("HighScore", highscore);
        GameObject.FindWithTag("Ball").GetComponent<BallController>().enabled = false;
        GameObject.FindObjectOfType<PlayerInputManager>().SetCanMove(false) ;
        gameOverPointsView.text = points + "";
        highScoreView.text = "Highest: "+highscore + "";
        gameOverObj.SetActive(true);
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
