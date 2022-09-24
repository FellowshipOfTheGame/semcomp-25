[System.Serializable]
public struct SessionData
{
    public SessionData(string token) => id_token = token;
    public string id_token;
}

[System.Serializable]
public struct SessionByCodeData
{
    public SessionByCodeData(string code) => this.code = code;
    public string code;
}