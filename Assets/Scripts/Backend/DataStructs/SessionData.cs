[System.Serializable]
public struct SessionData
{
    public SessionData(string token) => id_token = token;
    public string id_token;
}