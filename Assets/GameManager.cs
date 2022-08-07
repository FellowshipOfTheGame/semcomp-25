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
    [SerializeField] private RectTransform progressFill;

    [SerializeField] private TMP_Text currLevelView;
    [SerializeField] private TMP_Text nextLevelView;

    [SerializeField] private TMP_Text timeView;
    float progress = 0;
    int level = 0;

    private int faseLevel; // Factor used to control the level of the game
    private int points = 0;
    private int highscore = 0;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetLevelView();
        faseLevel = 0;
        SetPointsView();
        highscore=PlayerPrefs.GetInt("HighScore", 0);
    }
    public void AddPoint()
    {
        points+=10;
        SetPointsView();
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
            Vector2 size = progressFill.sizeDelta;
            size.x = Mathf.Lerp(25, 230, _x);
            progressFill.sizeDelta = size;
            _x += speed;
            yield return new WaitForEndOfFrame();
        }
        if (p == 1f)
        {
            Vector2 size = progressFill.sizeDelta;
            size.x = Mathf.Lerp(25, 230, 0);
            progressFill.sizeDelta = size;
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
    public void Update()
    {
        time += Time.deltaTime;
        SetTimeView();
    }
    public void SetTimeView()
    {
        timeView.text = time.ToString("0.00")+"";
    }
    public void SetLevelView()
    {
        currLevelView.text = level + "";
        nextLevelView.text = (level + 1) + "";
    }
    public void AddPoint(int n)
    {
        points +=n*10;

        time = 0;
        SetPointsView();
    }
    public void ResetPoints()
    {
        points=0;
        SetPointsView();
    }
    public void SetPointsView()
    {
        pointsView.text = points.ToString("D4") + "";
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
        GameObject.Find("INPUT_MANAGER").GetComponent<PlayerInputManager>().SetCanMove(false) ;
        gameOverPointsView.text = points + "";
        highscoreView.text = "Highest: "+highscore + "";
        gameOverObj.SetActive(true);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
