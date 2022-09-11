using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePowerup : PowerUp
{
    [SerializeField] float duration;
    [SerializeField] float scaleSize;
    [SerializeField] Sprite sprite;
    public override void Collected(Collider2D col)
    {
        Manager.ChangeTemporarilyBallSize(scaleSize,duration);
        Manager.SetPowerUpHud(sprite, duration);
    }
}
