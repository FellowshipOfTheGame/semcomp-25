using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityPowerup : PowerUp
{
    [SerializeField]float duration;
    [SerializeField] Sprite sprite;
    public override void Collected(Collider2D col)
    {
        Manager.ChangeTemporarilyVisibility(duration);
        Manager.SetPowerUpHud(sprite, duration);
    }
}