using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze", menuName = "ScriptableObjects/PowerUps/Freeze",order = 1)]
public class FreezeScriptableOBJ : PowerUpInfo
{
    public float freezeTime;

    FreezeScriptableOBJ(){
        Tag = "Freeze";
    }

}
