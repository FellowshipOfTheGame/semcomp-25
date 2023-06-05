public static class Endpoints
{
    public static string Base { private get; set; }
    public static string Login => Base + "/session/login";
    public static string Logout => Base + "/session/logout";
    public static string LoginGetSession => Base + "/session/login/get-session";
    public static string SessionValidate => Base + "/session/validate";
    public static string MatchStart => Base + "/match/start";
    public static string MatchSave => Base + "/match/savepoint";
    public static string MatchFinish => Base + "/match/finish";
    public static string Ranking => Base + "/player/ranking";
    public static string PlayerStatus => Base + "/player/status";
}