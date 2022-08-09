using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibilityPowerup : PowerUp
{
    [SerializeField]float duration;
    public override void Collected(Collider2D col)
    {
        Manager.ChangeTemporarilyVisibility(duration);
    }
}
