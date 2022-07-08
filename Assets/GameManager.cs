using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsView;
    [SerializeField] private TMP_Text gameOverPointsView;
    [SerializeField] private TMP_Text highscoreView;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private int faseLevel; // Factor used to control the level of the game
    private int points = 0;
    private int highscore = 0;
    // Start is called before the first frame update
    void Start()
    {
        faseLevel = 0;
        SetPointsView();
        highscore=PlayerPrefs.GetInt("HighScore", 0);
    }
    public void AddPoint()
    {
        points+=10;
        SetPointsView();
    }
    public void AddPoint(int n)
    {
        points+=n*10;
        SetPointsView();
    }
    public void ResetPoints()
    {
        points=0;
        SetPointsView();
    }
    public void SetPointsView()
    {
        pointsView.text = points + "";
    }

    public void AddFaseLevel()
    {
        faseLevel++;
    }

    public int FaseLevel()
    {
        return faseLevel;
    }

    public void GameOverScene()
    {
        highscore = (int)Mathf.Max(highscore, points);
        PlayerPrefs.SetInt("HighScore", highscore);
        GameObject.FindWithTag("Ball").GetComponent<BallController>().enabled = false;
        GameObject.FindWithTag("Allies").GetComponent<PlayerInputManager>().SetCanMove(false) ;
        gameOverPointsView.text = points + "";
        highscoreView.text = "Highest: "+highscore + "";
        gameOverObj.SetActive(true);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
