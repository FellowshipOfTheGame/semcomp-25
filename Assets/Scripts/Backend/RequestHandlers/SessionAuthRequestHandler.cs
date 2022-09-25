using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class SessionAuthRequestHandler
{
    public const string AuthKey = "Auth";

    public static void SaveAuthCookie(UnityWebRequest req)
    {
        var cookie = req.GetResponseHeader("Set-Cookie");
        
        if (!string.IsNullOrEmpty(cookie))
        {
            PlayerPrefs.SetString(AuthKey, cookie);
        }
    }

    public static IEnumerator Login(SessionData data, Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
    {
        RaycastBlockEvent.Invoke(true);
        
        var req = WebRequestFactory.PostJson(Endpoints.LoginGetSession, JsonUtility.ToJson(data));
        yield return req.SendWebRequest();
        
        RaycastBlockEvent.Invoke(false);

        if (req.result == UnityWebRequest.Result.Success)
        {
            SaveAuthCookie(req);
            OnSuccess?.Invoke();
        }
        else
        {
            OnFailure?.Invoke(req);
        }
    }

    public static IEnumerator Logout(Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
    {
        RaycastBlockEvent.Invoke(true);
        
        var req = WebRequestFactory.AuthGet(Endpoints.Logout);
        yield return req.SendWebRequest();
        
        RaycastBlockEvent.Invoke(false);

        if (req.result == UnityWebRequest.Result.Success)
        {
            UnityWebRequest.ClearCookieCache();
            PlayerPrefs.DeleteKey(AuthKey);
            OnSuccess?.Invoke();
        }
        else
        {
            OnFailure?.Invoke(req);
        }
    }

    public static IEnumerator ValidateSession(Action OnSuccess, Action<UnityWebRequest> OnFailure = null)
    {
        RaycastBlockEvent.Invoke(true);
        
        var req = WebRequestFactory.AuthGet(Endpoints.SessionValidate);
        yield return req.SendWebRequest();
        
        RaycastBlockEvent.Invoke(false);

        if (req.result == UnityWebRequest.Result.Success)
        {
            OnSuccess?.Invoke();
        }
        else
        {
            OnFailure?.Invoke(req);
        }
    }
}