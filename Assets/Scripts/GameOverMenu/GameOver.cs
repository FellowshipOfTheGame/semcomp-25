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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        _scoreSystem = FindObjectOfType<ScoreSystem>();
    }

    private void Start()
    {
        _currentMarmoset = _happyMarmossets.transform.GetChild(0).gameObject;
    }

    private void LoadMarmoset(int score, int highscore)
    {
        int randomMarmosetIndex;

        _currentMarmoset.SetActive(false);

        if (score < highscore * 0.3)
        {
            randomMarmosetIndex = Random.Range(0, _sadMarmossets.transform.childCount);
            _currentMarmoset = _sadMarmossets.transform.GetChild(randomMarmosetIndex).gameObject;
        }
        else if (score > highscore)
        {
            randomMarmosetIndex = Random.Range(0, _happyMarmossets.transform.childCount);
            _currentMarmoset = _happyMarmossets.transform.GetChild(randomMarmosetIndex).gameObject;
        }
        else
        {
            randomMarmosetIndex = Random.Range(0, _neutralMarmossets.transform.childCount);
            _currentMarmoset = _neutralMarmossets.transform.GetChild(randomMarmosetIndex).gameObject;
        }

        _currentMarmoset.SetActive(true);
    }

    public void OnGameOver(int highScore)
    {
        int totalPoints = _scoreSystem.ScoreAmount;

        LoadMarmoset(totalPoints, highScore);
        
        GameObject.FindWithTag("Ball").GetComponent<BallController>().enabled = false;
        GameObject.FindObjectOfType<PlayerInputManager>().SetCanMove(false);

        _gameOverPointsView.text = totalPoints + "";
        _highScoreView.text = "Melhor Pontuação: " + highScore + "";

        _menu.SetActive(true);

        GameOverPointbar.Instance.LoadPointBar();
    }
}
