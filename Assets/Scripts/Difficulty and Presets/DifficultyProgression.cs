using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DifficultyProgression : MonoBehaviour
{
    // Attribute presets scriptable objetcs and level progression on the editor
    [SerializeField] private PresetSO[] presetSOList;
    [SerializeField] private PresetSO[] goalPresetSOList;
    [SerializeField] private LevelPresetProportions[] levelProgression;

    // Pools of presets by difficulty
    private List<PresetSO> easyPresets = new List<PresetSO>();
    private List<PresetSO> normalPresets = new List<PresetSO>();
    private List<PresetSO> hardPresets = new List<PresetSO>();
    
    private List<PresetSO> easyGoalPresets = new List<PresetSO>();
    private List<PresetSO> normalGoalPresets = new List<PresetSO>();
    private List<PresetSO> hardGoalPresets = new List<PresetSO>();

    // Queue of Presets based on level progression
    private Queue<PresetSO> presetsQueue = new Queue<PresetSO>();
    private int levelToQueue = 0;

    // Player count of each level
    private List<int> playersOnLevels = new List<int>();
    
    [Serializable]
    public struct LevelPresetProportions
    {
        public int easy;
        public int normal;
        public int hard;
        public Difficulty goalDifficulty;
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
        
        // Separate Goal PresetSOs by difficulty
        foreach (PresetSO p in goalPresetSOList)
        {
            switch(p.difficulty)
            {
                case Difficulty.Easy:
                    easyGoalPresets.Add(p);
                    break;
                case Difficulty.Normal:
                    normalGoalPresets.Add(p);
                    break;
                case Difficulty.Hard:
                    hardGoalPresets.Add(p);
                    break;
            }
        }
    }
    public GameObject NextPreset()
    {
        // enqueue presets of current level plus goal
        if (presetsQueue.Count == 0)
        {
            List<Difficulty> difficulties = new List<Difficulty>();
            for (int i = 0; i < levelProgression[levelToQueue].easy; i++)
                difficulties.Add(Difficulty.Easy);
            for (int i = 0; i < levelProgression[levelToQueue].normal; i++)
                difficulties.Add(Difficulty.Normal);
            for (int i = 0; i < levelProgression[levelToQueue].hard; i++)
                difficulties.Add(Difficulty.Hard);
            
            // add presets randomized according to the level difficulty proportion
            int playerCount = 0;
            while (difficulties.Count > 0)
            {
                int rdIdx = UnityEngine.Random.Range(0, difficulties.Count);
                PresetSO p = GetRandomPresetSO(difficulties[rdIdx]);
                playerCount += p.presetPrefab.transform.Find("Players").childCount;
                presetsQueue.Enqueue(p);

                difficulties.RemoveAt(rdIdx);
            }
            
            // add goal
            PresetSO g = GetRandomGoalPresetSO(levelProgression[levelToQueue].goalDifficulty);
            playerCount += g.presetPrefab.transform.Find("Players").childCount;
            presetsQueue.Enqueue(g);
            
            // store player count of this level
            playersOnLevels.Add(playerCount);

            if (levelToQueue < levelProgression.Length-1)
                levelToQueue++;
        }

        return presetsQueue.Dequeue().presetPrefab;
    }

    private PresetSO GetRandomPresetSO(Difficulty difficulty)
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

    private PresetSO GetRandomGoalPresetSO(Difficulty difficulty)
    {
        PresetSO p = null;
        switch (difficulty)
        {
            case Difficulty.Easy:
                p = easyGoalPresets[UnityEngine.Random.Range(0, easyGoalPresets.Count)];
                break;
            case Difficulty.Normal:
                p = normalGoalPresets[UnityEngine.Random.Range(0, normalGoalPresets.Count)];
                break;
            case Difficulty.Hard:
                p = hardGoalPresets[UnityEngine.Random.Range(0, hardGoalPresets.Count)];
                break;
        }

        return p;
    }

    public int GetTotalPlayersInLevel(int level)
    {
        if (playersOnLevels.Count > level)
            return playersOnLevels[level];
        else
            return -1;
    }
}
