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

    [SerializeField] private RectTransform _goalBody_rt;
    [SerializeField] private RectTransform _passBody_rt;
    [SerializeField] private RectTransform _whBody_rt;
    [SerializeField] private RectTransform _pwBody_rt;

    [SerializeField] private RectTransform _goalIndicator_rt;
    [SerializeField] private RectTransform _passIndicator_rt;
    [SerializeField] private RectTransform _whIndicator_rt;

    public static GameOverPointbar Instance;

    private const float BAR_BODY_MAX_WIDTH = 575;
    private const float BAR_HEIGHT = 79;
    private const float HEAD_TAIL_WIDTH = 45;
    private const float HEAD_TAIL_PERCENTAGE = HEAD_TAIL_WIDTH / BAR_BODY_MAX_WIDTH;

    private int _totalScore;
    private int _highscore;
    private int _goalScore;
    private int _passScore;
    private int _wallhitScore;
    private int _pwScore;

    private float _goalRatio;
    private float _passRatio;
    private float _wallRatio;
    private float _powerUpRatio;


    private float _firstPos;

    private ScoreSystem _scoreSystem;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _scoreSystem = FindObjectOfType<ScoreSystem>();
    }
    void Start()
    {
        
    }

    private void ActivateAllBars()
    {
        _goal.SetActive(true);
        _pass.SetActive(true);
        _wallhit.SetActive(true);
        _powerup.SetActive(true);
    }

    private void ResetRect()
    {
        _goalBody_rt.anchoredPosition = new Vector2(0, 0);
        _goalIndicator_rt.anchoredPosition = new Vector2(0, 0);

        _passBody_rt.anchoredPosition = new Vector2(0, 0);
        _passIndicator_rt.anchoredPosition = new Vector2(0, 0);

        _whBody_rt.anchoredPosition = new Vector2(0, 0);
        _whIndicator_rt.anchoredPosition = new Vector2(0, 0);

        _pwBody_rt.anchoredPosition = new Vector2(0, 0);
        _whIndicator_rt.anchoredPosition = new Vector2(0, 0);
    }

    private void UpdateScores()
    {
        _goalScore = _scoreSystem.GoalScoreAmount;
        _passScore = _scoreSystem.PassScoreAmount;
        _wallhitScore = _scoreSystem.WallhitScoreAmount;
        _pwScore = _scoreSystem.PowerupScoreAmount;
    }

    private void UpdateScoreRatios()
    {
        _goalRatio = _goalScore * (1.0f) / _totalScore;
        _passRatio = _passScore * (1.0f) / _totalScore;
        _wallRatio = _wallhitScore * (1.0f) / _totalScore;
        _powerUpRatio = _pwScore * (1.0f) / _totalScore;

        float ratioAdd = 0.0f;

        if (_goal.activeSelf)
        {
            if (_goalRatio <= HEAD_TAIL_PERCENTAGE)
            {
                _goalRatio = 0.0f;
                ratioAdd = HEAD_TAIL_PERCENTAGE - _goalRatio;
                //_goalIndicator_rt.anchoredPosition = new Vector2(0, 0);
            }
            else
            {
                _goalRatio -= HEAD_TAIL_PERCENTAGE;
            }
            if (!_pass.activeSelf && !_wallhit.activeSelf && !_powerup.activeSelf)
            {
                _goalRatio -= HEAD_TAIL_PERCENTAGE;
            }
        }
        if (_pass.activeSelf)
        {
            if (!_goal.activeSelf)
            {
                if (_passRatio <= HEAD_TAIL_PERCENTAGE)
                {
                    _goalRatio = 0.0f;
                    ratioAdd = HEAD_TAIL_PERCENTAGE - _passRatio;
                }
                else
                {
                    _passRatio -= HEAD_TAIL_PERCENTAGE;
                }
            }
            else
            {
                _passRatio -= ratioAdd;
                ratioAdd = 0.0f;
            }
                
            if (!_wallhit.activeSelf && !_powerup.activeSelf)
            {
                _passRatio -= HEAD_TAIL_PERCENTAGE;
            }
        }
        if (_wallhit.activeSelf)
        {
            if (!_goal.activeSelf && !_pass.activeSelf)
            {
                if (_wallRatio <= HEAD_TAIL_PERCENTAGE)
                {
                    _passRatio = 0.0f;
                    ratioAdd = HEAD_TAIL_PERCENTAGE - _wallRatio;
                }
                else
                {
                    _wallRatio -= HEAD_TAIL_PERCENTAGE;
                }
            }
            else
            {
                _wallRatio -= ratioAdd;
                ratioAdd = 0.0f;
            }
            if (!_powerup.activeSelf)
            {
                _wallRatio -= HEAD_TAIL_PERCENTAGE;
            }
        }
        if (_powerup.activeSelf)
        {
            if (!_goal.activeSelf && !_pass.activeSelf && !_wallhit.activeSelf)
            {
                _powerUpRatio -= HEAD_TAIL_PERCENTAGE;
            }
            else
                _powerUpRatio -= ratioAdd;

            _powerUpRatio -= HEAD_TAIL_PERCENTAGE;
        }

        Debug.Log("TOTAL PERCENTAGE: " + (_goalRatio + _passRatio + _wallRatio + _powerUpRatio + 2 * HEAD_TAIL_PERCENTAGE));
    }

    private void DeactivateBarsWithZeroScore()
    {
        if (_goalScore == 0)
            _goal.SetActive(false);
        if (_passScore == 0)
            _pass.SetActive(false);
        if (_wallhitScore == 0)
            _wallhit.SetActive(false);
        if (_pwScore == 0)
            _powerup.SetActive(false);        
    }

    private void UpdateBarSizes()
    {
        _goalBody_rt.sizeDelta = new Vector2(BAR_BODY_MAX_WIDTH * _goalRatio, BAR_HEIGHT);
        _passBody_rt.sizeDelta = new Vector2(BAR_BODY_MAX_WIDTH * _passRatio, BAR_HEIGHT);
        _whBody_rt.sizeDelta = new Vector2(BAR_BODY_MAX_WIDTH * _wallRatio, BAR_HEIGHT);
        _pwBody_rt.sizeDelta = new Vector2(BAR_BODY_MAX_WIDTH * _powerUpRatio, BAR_HEIGHT);
    }

    private void UpdateBarPosition()
    {

    }

    private void UpdateBarIndicator()
    {

    }

    private void UpdateBar()
    {
        // Checks for the case that there is only one form of score
        // ( the player get only one type of score in a run )
        if (_passRatio == 1.0f)
        {
            _passIndicator_rt.gameObject.SetActive(false);
        }
        else if (_wallRatio == 1.0f)
        {
            _whIndicator_rt.gameObject.SetActive(false);
        }

        UpdateBarSizes();
    }

    public void LoadPointBar()
    {
        _totalScore = _scoreSystem.ScoreAmount;
        _highscore = PlayerPrefs.GetInt("HighScore", 0);

        ActivateAllBars();              // Set Active all bars objects
        ResetRect();                    // Resets all sizes and positions of all bar Bodies and Indicators
        UpdateScores();                 // Updates the local scores in this object with the game current values
        DeactivateBarsWithZeroScore();  // Deactivate the bar if the player have zero points in a score type
        UpdateScoreRatios();            // Updates current ratio for all scores
        UpdateBar();                    // Updates bar and its indicator and points

        _lastIndicatorPoints.text = _scoreSystem.ScoreAmount + "";
    }
}
