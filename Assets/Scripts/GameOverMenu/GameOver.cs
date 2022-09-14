using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;
    [SerializeField] private TMP_Text _gameOverPointsView;
    [SerializeField] private TMP_Text _highScoreView;
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _sadMarmossets;
    [SerializeField] private GameObject _neutralMarmossets;
    [SerializeField] private GameObject _happyMarmossets;

    private GameObject _currentMarmoset;

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

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("HighScore", 0); // Antes disso, checar se é a primeira vez que o jogador está jogando?
        _currentMarmoset = _happyMarmossets.transform.GetChild(0).gameObject;
    }

    private void LoadMarmosset(int score)
    {
        int randomMarmossetIndex;

        _currentMarmoset.SetActive(false);

        if (score < highscore * 0.3)
        {
            randomMarmossetIndex = Random.Range(0, _sadMarmossets.transform.childCount);
            _currentMarmoset = _sadMarmossets.transform.GetChild(randomMarmossetIndex).gameObject;
        }
        else if (score > highscore)
        {
            randomMarmossetIndex = Random.Range(0, _happyMarmossets.transform.childCount);
            _currentMarmoset = _happyMarmossets.transform.GetChild(randomMarmossetIndex).gameObject;
        }
        else
        {
            randomMarmossetIndex = Random.Range(0, _neutralMarmossets.transform.childCount);
            _currentMarmoset = _neutralMarmossets.transform.GetChild(randomMarmossetIndex).gameObject;
        }

        _currentMarmoset.SetActive(true);
    }

    public void OnGameOver()
    {
        int totalPoints = _scoreSystem.ScoreAmount;

        LoadMarmosset(totalPoints);

        GameOverPointbar.Instance.LoadPointBar();

        highscore = (int)Mathf.Max(highscore, totalPoints);

        PlayerPrefs.SetInt("HighScore", highscore);

        GameObject.FindWithTag("Ball").GetComponent<BallController>().enabled = false;
        GameObject.FindObjectOfType<PlayerInputManager>().SetCanMove(false);

        _gameOverPointsView.text = totalPoints + "";
        _highScoreView.text = "Melhor Pontuação: " + highscore + "";

        _menu.SetActive(true);
    }
}
