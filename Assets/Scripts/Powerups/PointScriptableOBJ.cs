using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Point", menuName = "ScriptableObjects/PowerUps/PointBonus",order = 1)]
public class PointScriptableOBJ : PowerUpInfo
{
    public int PointAmmount;

    PointScriptableOBJ(){
        Tag = "Point";
    }

}
