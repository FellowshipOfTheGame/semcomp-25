using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [Header("Score amounts")]
    [SerializeField] int wallHitScore;
    [SerializeField] int successfulPassScore;
    [SerializeField] int goalScore;
    
    [Header("Enable score types")]
    [SerializeField] bool enableWallHitScore;
    [SerializeField] bool enableSuccessfulPassScore;
    [SerializeField] bool enableGoalScore;

    private const string HighScoreKey = "HighScore";

    public static int HighScore
    {
        get => PlayerPrefs.GetInt(HighScoreKey);
        set
        {
            if (value < HighScore)
            {
                Debug.LogWarning($"Decreasing high score from {HighScore} to value");
            }
            
            PlayerPrefs.SetInt(HighScoreKey, value);
        }
    }

    private int scoreAmount;
    public int ScoreAmount
    {
        get => scoreAmount;
        private set
        {
            scoreAmount = value;
            UpdateScoreTextUI();
        }
    }

    public int GoalScoreAmount { get; set; }
    public int PowerupScoreAmount { get; set; }
    public int PassScoreAmount { get; set; }
    public int WallhitScoreAmount { get; set; }


    [Header("UI")]
    [SerializeField] TextMeshProUGUI scoreTextUI;

    private void OnEnable()
    {
        // subscribe to events
        Wall.OnWallHit += WallHitScored;
        BallController.OnSuccessfulPass += SuccessfulPassScored;
        BallController.OnGoalScored += GoalScored;
    }

    private void OnDisable()
    {
        // unsubscribe to events
        Wall.OnWallHit -= WallHitScored;
        BallController.OnSuccessfulPass -= SuccessfulPassScored;
        BallController.OnGoalScored -= GoalScored;
    }

    private void Start()
    {
        ScoreAmount = 0;
        GoalScoreAmount = 0;
        PowerupScoreAmount = 0;
        PassScoreAmount = 0;
        WallhitScoreAmount = 0;
    }

    private void WallHitScored()
    {
        if (enableWallHitScore)
        {
            ScoreAmount += wallHitScore;
            WallhitScoreAmount += wallHitScore;

        }
    }

    private void SuccessfulPassScored()
    {
        if (enableSuccessfulPassScore)
        {
            ScoreAmount += successfulPassScore;
            PassScoreAmount += successfulPassScore;
        }
    }
    [SerializeField] private GameObject goalPrefab;
    private void GoalScored()
    {
        if (enableGoalScore)
        {
            ScoreAmount += goalScore;
            GoalScoreAmount += goalScore;
            Instantiate(goalPrefab);
        }
    }

    private void UpdateScoreTextUI()
    {
        if (!scoreTextUI) return;

        scoreTextUI.text = ScoreAmount.ToString();
    }

    public void AddPowerupScore(int points)
    {
        ScoreAmount += points;
        PowerupScoreAmount += points;
    }

    public void PrintScore()
    {
        Debug.Log("TOTAL: " + ScoreAmount + "");
        Debug.Log("GOAL: " + GoalScoreAmount + "");
        Debug.Log("PASS: " + PassScoreAmount + "");
        Debug.Log("WALL: " + WallhitScoreAmount + "");
        Debug.Log("POWERUP: " + PowerupScoreAmount + "");       
    }
}
