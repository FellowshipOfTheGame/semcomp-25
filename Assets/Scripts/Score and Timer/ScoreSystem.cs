using System.Collections;
using System.Collections.Generic;
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
    }

    private void WallHitScored()
    {
        if (enableWallHitScore)
            ScoreAmount += wallHitScore;
    }

    private void SuccessfulPassScored()
    {
        if (enableSuccessfulPassScore)
            ScoreAmount += successfulPassScore;
    }
    [SerializeField] private GameObject goalPrefab;
    private void GoalScored()
    {
        if (enableGoalScore)
        {

            ScoreAmount += goalScore;
            Instantiate(goalPrefab);
        }
    }

    private void UpdateScoreTextUI()
    {
        if (!scoreTextUI) return;

        scoreTextUI.text = ScoreAmount.ToString();
    }

    public void AddScore(int points){
        ScoreAmount += points;
    }
}
