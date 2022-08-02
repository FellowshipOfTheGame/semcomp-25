using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DifficultyProgression : MonoBehaviour
{
    // Attribute presets scriptable objetcs and level progression on the editor
    [SerializeField] PresetSO[] presetSOList;
    [SerializeField] LevelPresetProportions[] levelProgression;

    // Pools of presets by difficulty
    List<PresetSO> easyPresets = new List<PresetSO>();
    List<PresetSO> normalPresets = new List<PresetSO>();
    List<PresetSO> hardPresets = new List<PresetSO>();

    // Queue of Presets based on level progression
    Queue<PresetSO> presetsQueue = new Queue<PresetSO>();
    int levelToQueue = 0;

    int playersOnLevel=0;
    public int PlayersOnLevel => playersOnLevel;
    [Serializable]
    public struct LevelPresetProportions
    {
        public int easy;
        public int normal;
        public int hard;
    }

    private void Awake()
    {
        // Separate PresetSOs by difficulty
        foreach (PresetSO p in presetSOList)
        {
            switch(p.difficulty)
            {
                case Difficulty.Easy:
                    easyPresets.Add(p);
                    break;
                case Difficulty.Normal:
                    normalPresets.Add(p);
                    break;
                case Difficulty.Hard:
                    hardPresets.Add(p);
                    break;
            }
        }
    }
    public GameObject NextPreset()
    {
        // enqueue presets of current level
        if (presetsQueue.Count == 0)
        {
            List<Difficulty> difficulties = new List<Difficulty>();
            for (int i = 0; i < levelProgression[levelToQueue].easy; i++)
                difficulties.Add(Difficulty.Easy);
            for (int i = 0; i < levelProgression[levelToQueue].normal; i++)
                difficulties.Add(Difficulty.Normal);
            for (int i = 0; i < levelProgression[levelToQueue].hard; i++)
                difficulties.Add(Difficulty.Hard);
            playersOnLevel = 0;
            while (difficulties.Count > 0)
            {
                int rd_idx = UnityEngine.Random.Range(0, difficulties.Count);
                PresetSO p = GetRandomPresetSO(difficulties[rd_idx]);
                int playerCount=p.presetPrefab.transform.Find("Players").childCount;
                playersOnLevel += playerCount;
                presetsQueue.Enqueue(p);

                difficulties.RemoveAt(rd_idx);
            }

            if (levelToQueue < levelProgression.Length-1)
                levelToQueue++;

        }



        return presetsQueue.Dequeue().presetPrefab;
    }

    PresetSO GetRandomPresetSO(Difficulty difficulty)
    {
        PresetSO p = null;
        switch (difficulty)
        {
            case Difficulty.Easy:
                p = easyPresets[UnityEngine.Random.Range(0, easyPresets.Count)];
                break;
            case Difficulty.Normal:
                p = normalPresets[UnityEngine.Random.Range(0, normalPresets.Count)];
                break;
            case Difficulty.Hard:
                p = hardPresets[UnityEngine.Random.Range(0, hardPresets.Count)];
                break;
        }

        return p;
    }

}
