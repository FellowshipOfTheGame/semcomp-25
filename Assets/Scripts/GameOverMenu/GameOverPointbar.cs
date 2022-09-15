using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPointbar : MonoBehaviour
{
    [SerializeField] private GameObject _pointBar;
    [SerializeField] private GameObject _goal;
    [SerializeField] private GameObject _pass;
    [SerializeField] private GameObject _wallhit;
    [SerializeField] private GameObject _powerup;
    [SerializeField] private TMP_Text _lastIndicatorPoints;

    public static GameOverPointbar Instance;

    private const int barBodyMaxWidth = 498;
    private const int barHeight = 79;

    private RectTransform _goal_rt;
    private RectTransform _pass_rt;
    private RectTransform _wh_rt;
    private RectTransform _pw_rt;

    private double _firstPos;

    private ScoreSystem _scoreSystem;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _scoreSystem = FindObjectOfType<ScoreSystem>();
        _goal_rt = _goal.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        _pass_rt = _pass.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        _wh_rt = _wallhit.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        _pw_rt = _powerup.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
    }
    void Start()
    {
        
    }

    private void ResetBodyRect()
    {
        _goal_rt.sizeDelta = new Vector2(barBodyMaxWidth, barHeight);
        _goal_rt.anchoredPosition = new Vector2(0, 0);
        _pass_rt.sizeDelta = new Vector2(barBodyMaxWidth, barHeight);
        _pass_rt.anchoredPosition = new Vector2(0, 0);
        _wh_rt.sizeDelta = new Vector2(barBodyMaxWidth, barHeight);
        _wh_rt.anchoredPosition = new Vector2(0, 0);
        _pw_rt.sizeDelta = new Vector2(barBodyMaxWidth, barHeight);
        _pw_rt.anchoredPosition = new Vector2(0, 0);
    }

    public void LoadPointBar()
    {
        int totalPoints = _scoreSystem.ScoreAmount;
        int highscore = PlayerPrefs.GetInt("HighScore", 0);
        int goalScore = _scoreSystem.GoalScoreAmount;

        double goalRatio = _scoreSystem.GoalScoreAmount * (1.0) / totalPoints;
        double passRatio = _scoreSystem.PassScoreAmount * (1.0) / totalPoints;
        double wallRatio = _scoreSystem.WallhitScoreAmount * (1.0) / totalPoints;
        double powerUpRatio = _scoreSystem.PowerupScoreAmount * (1.0) / totalPoints;

        ResetBodyRect();    // Resets all sizes and positions of all bar Bodies





        ResetBodyRect();
        _lastIndicatorPoints.text = _scoreSystem.ScoreAmount + "";
    }
}
