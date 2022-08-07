using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [Header("Score amounts")]
    [SerializeField] int wallHitScore;
    [SerializeField] int successfulPassScore;

    [Header("Enable score types")]
    [SerializeField] bool enableWallHitScore;
    [SerializeField] bool enableSuccessfulPassScore;

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
    }

    private void OnDisable()
    {
        // unsubscribe to events
        Wall.OnWallHit -= WallHitScored;
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

    private void UpdateScoreTextUI()
    {
        if (!scoreTextUI) return;

        scoreTextUI.text = ScoreAmount.ToString();
    }

    /*
        Modificacoes Lucas = Adicao de um "addscore" generico
    */

    public void addScore(int points){
        ScoreAmount += points;
    }
}
