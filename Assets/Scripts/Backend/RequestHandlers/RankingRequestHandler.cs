using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class RankingRequestHandler
{
    public static IEnumerator GetRanking(Action<RankingData> OnSuccess, Action<UnityWebRequest> OnFailure = null)
    {
        RaycastBlockEvent.Invoke(true);
        
        var request = WebRequestFactory.AuthGet(Endpoints.Ranking);
        yield return request.SendWebRequest();

        RaycastBlockEvent.Invoke(false);
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            SessionAuthRequestHandler.SaveAuthCookie(request);
            // Debug.Log(request.downloadHandler.text);
            var rankingData = JsonUtility.FromJson<RankingData>(request.downloadHandler.text);
            OnSuccess?.Invoke(rankingData);
        }
        else
        {
            OnFailure?.Invoke(request);
        }
    }
}