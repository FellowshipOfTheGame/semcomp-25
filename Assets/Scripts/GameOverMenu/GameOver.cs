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

    [SerializeField] private GameObject _marmosset;

    private int highscore = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("HighScore", 0); // Antes disso, checar se é a primeira vez que o jogador está jogando?
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadMarmosset()
    {
        // Deactivate all marmosset in the gameover menu
        foreach (Transform marmosset in _marmosset.transform)
            marmosset.gameObject.SetActive(false);

        // Then, activate a random marmosset
        int randomMarmossetIndex = Random.Range(0, _marmosset.transform.childCount);
        _marmosset.transform.GetChild(randomMarmossetIndex).gameObject.SetActive(true);
    }

    public void OnGameOver()
    {
        LoadMarmosset();
        int points = FindObjectOfType<ScoreSystem>().ScoreAmount;
        highscore = (int)Mathf.Max(highscore, points);
        PlayerPrefs.SetInt("HighScore", highscore);
        GameObject.FindWithTag("Ball").GetComponent<BallController>().enabled = false;
        GameObject.FindObjectOfType<PlayerInputManager>().SetCanMove(false);
        _gameOverPointsView.text = points + "";
        _highScoreView.text = "Melhor Pontuação: " + highscore + "";
        _menu.SetActive(true);
    }
}
