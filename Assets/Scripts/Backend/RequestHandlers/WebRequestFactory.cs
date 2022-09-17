using System.Text;
using UnityEngine;
using UnityEngine.Networking;

static class WebRequestFactory
{
    private const int Timeout = 10;
    
    public static UnityWebRequest PostJson(string url, string data = "{}")
    {
        var req = UnityWebRequest.Post(url, "{}");

        var bodyRaw = Encoding.UTF8.GetBytes(data);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        
        req.SetRequestHeader("Content-Type", "application/json");
        req.timeout = Timeout;
        
        return req;
    }

    public static UnityWebRequest AuthPostJson(string url, string data = "{}")
    {
        var req = PostJson(url, data);

#if !UNITY_WEBGL
        var authCookie = PlayerPrefs.GetString(SessionAuthRequestHandler.AuthKey, string.Empty);
        req.SetRequestHeader("Cookie", authCookie);
#endif
        req.timeout = Timeout;
        
        return req;
    }

    public static UnityWebRequest AuthGet(string url)
    {
        var req = UnityWebRequest.Get(url);

#if !UNITY_WEBGL
        var authCookie = PlayerPrefs.GetString(SessionAuthRequestHandler.AuthKey, string.Empty);
        req.SetRequestHeader("Cookie", authCookie);
#endif
        req.timeout = Timeout;
        
        return req;
    }
}