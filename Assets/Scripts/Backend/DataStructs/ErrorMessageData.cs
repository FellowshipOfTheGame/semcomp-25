[System.Serializable]
public struct ErrorMessageData
{
    public string message;
    public static implicit operator string(ErrorMessageData e) => e.message;
}