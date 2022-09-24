using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePowerup : PowerUp
{
    [SerializeField]int lifeQuantity=1;
    [SerializeField] Sprite sprite;
    public override void Collected(Collider2D col)
    {
        Manager.AddLife(lifeQuantity);
        Manager.SetLifePowerUpHud(sprite);
    }
}
