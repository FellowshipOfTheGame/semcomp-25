using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Time", menuName = "ScriptableObjects/PowerUps/TimeBonus",order = 1)]
public class TimeScriptableOBJ : PowerUpInfo
{  
    public float time;

    TimeScriptableOBJ(){
        Tag = "Time";
    }

}
