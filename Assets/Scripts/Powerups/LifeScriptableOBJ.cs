using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Life", menuName = "ScriptableObjects/PowerUps/Life",order = 1)]
public class LifeScriptableOBJ : PowerUpInfo
{
    public int extraLife;

    LifeScriptableOBJ(){
        Tag = "Life";
    }

}
