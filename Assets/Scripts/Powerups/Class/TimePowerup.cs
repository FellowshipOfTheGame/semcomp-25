using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimePowerup : PowerUp
{
    [SerializeField] float duration;
    [SerializeField] float timeScale;
    public override void Collected(Collider2D col)
    {
        Manager.ChangeEnemyTime(duration, timeScale);
    }
}
