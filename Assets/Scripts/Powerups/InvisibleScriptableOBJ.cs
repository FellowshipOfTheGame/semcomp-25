using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Invisible", menuName = "ScriptableObjects/PowerUps/Invisible",order = 1)]
public class InvisibleScriptableOBJ : PowerUpInfo
{
    public float invisibleTime;

    InvisibleScriptableOBJ(){
        Tag = "Invisible";
    }

}
