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

    private ScoreSystem _scoreSystem;
    private int highscore = 0;

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


    public void LoadPointBar()
    {
        int totalPoints = _scoreSystem.ScoreAmount;
        _lastIndicatorPoints.text = _scoreSystem.ScoreAmount + "";
    }
}
