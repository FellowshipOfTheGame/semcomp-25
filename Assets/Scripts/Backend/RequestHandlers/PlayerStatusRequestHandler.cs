using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class PlayerStatusRequestHandler
{
    public static IEnumerator GetUserStatus(Action<PlayerStatus> OnSuccess, Action<UnityWebRequest> OnFailure = null)
    {
        RaycastBlockEvent.Invoke(true);
        
        var request = WebRequestFactory.AuthGet(Endpoints.UserStatus);
        yield return request.SendWebRequest();

        RaycastBlockEvent.Invoke(false);

        if (request.result == UnityWebRequest.Result.Success)
        {
            SessionAuthRequestHandler.SaveAuthCookie(request);
            var userStatus = JsonUtility.FromJson<PlayerStatus>(request.downloadHandler.text);

            if (Cryptography.IsSignatureValid(userStatus))
            {
                OnSuccess?.Invoke(userStatus);
            }
            else
            {
                OnFailure?.Invoke(request);
            }
        }
        else
        {
            OnFailure?.Invoke(request);
        }
    }
}