using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamaObstacle : PowerUp
{

    public override void Collected(Collider2D col)
    {
        Manager.LamaBall(col.gameObject);
    }
}
