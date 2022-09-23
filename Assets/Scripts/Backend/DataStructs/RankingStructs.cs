using UnityEngine.Serialization;

[System.Serializable]
public struct RankingPlayerData
{
    public string name;
    public int score;
}

[System.Serializable]
public struct RankingPersonalData
{
    public string name;
    public int position;
    public int score;
}

[System.Serializable]
public struct RankingData
{
    public RankingPersonalData personal;
    public RankingPlayerData[] ranking;
}
