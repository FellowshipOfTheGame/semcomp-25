using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCollectable : PowerUp
{
    [SerializeField] int score;

    public override void Collected(Collider2D col)
    {
        Manager.PowerupScore(score);
    }
}
