using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceObstacle : PowerUp
{

    public override void Collected(Collider2D col)
    {
        Manager.IceBall(col.gameObject);
    }
}
