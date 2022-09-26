using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class MatchRequestHandler
{
    public static IEnumerator StartMatch(MatchData matchData, Action OnSuccess = null, Action<UnityWebRequest> OnFailure = null)
    {
        RaycastBlockEvent.Invoke(true);
        
        var data = JsonUtility.ToJson(matchData);
        var request = WebRequestFactory.AuthPostJson(Endpoints.MatchStart, data);
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
                Debug.LogError(request.downloadHandler.text);
            }
        }
        else
        {
            OnFailure?.Invoke(request);
            Debug.LogError(request.downloadHandler.text);
        }
    }

    public static IEnumerator SaveMatch(MatchData matchData, Action<UnityWebRequest> OnSuccess = null, Action<UnityWebRequest> OnFailure = null)
    {
        matchData.sign = Cryptography.GetSignature(matchData);
        var data = JsonUtility.ToJson(matchData);
        // Debug.Log(data);
        var request = WebRequestFactory.AuthPostJson(Endpoints.MatchSave, data);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            OnSuccess?.Invoke(request);
        }
        else
        {
            OnFailure?.Invoke(request);
        }
    }
}