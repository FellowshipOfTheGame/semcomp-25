using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class MatchRequestHandler
{
    public static IEnumerator StartMatch(Action OnSuccess = null, Action<UnityWebRequest> OnFailure = null)
    {
        RaycastBlockEvent.Invoke(true);
        
        var request = WebRequestFactory.AuthPostJson(Endpoints.MatchStart);
        yield return request.SendWebRequest();

        RaycastBlockEvent.Invoke(false);
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            
            SessionAuthRequestHandler.SaveAuthCookie(request);
            OnSuccess?.Invoke();
        }
        else
        {
            OnFailure?.Invoke(request);
        }
    }

    public static IEnumerator FinishMatch(MatchData matchData, Action<FinishMatchData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
    {
        RaycastBlockEvent.Invoke(true);
        
        matchData.sign = Cryptography.GetSignature(matchData);
        var data = JsonUtility.ToJson(matchData);
        
        var request = WebRequestFactory.AuthPostJson(Endpoints.MatchFinish, data);
        
        yield return request.SendWebRequest();

        RaycastBlockEvent.Invoke(false);

        if (request.result == UnityWebRequest.Result.Success)
        {
            try
            {
                var finishMatchData = JsonUtility.FromJson<FinishMatchData>(request.downloadHandler.text);
                OnSuccess?.Invoke(finishMatchData);
            }
            catch
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