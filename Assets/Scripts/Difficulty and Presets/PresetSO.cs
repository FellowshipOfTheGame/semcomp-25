using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty { Easy, Normal, Hard};

[CreateAssetMenu(fileName = "New PresetSO", menuName = "ScriptableObjects/Preset")]
public class PresetSO : ScriptableObject
{
    public GameObject presetPrefab;
    public Difficulty difficulty;
}
