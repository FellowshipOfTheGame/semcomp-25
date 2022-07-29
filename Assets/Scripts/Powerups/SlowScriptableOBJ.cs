using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slow", menuName = "ScriptableObjects/PowerUps/Slow",order = 1)]
public class SlowScriptableOBJ : PowerUpInfo
{
    public float slowTime;

    public float slowFactor;

    SlowScriptableOBJ(){
        Tag = "Slow";
    }

}
