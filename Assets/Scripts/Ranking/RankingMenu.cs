using System.Collections;
using UnityEngine;

public class RankingMenu : MonoBehaviour
{
    [SerializeField] private PlayerRankUI[] playerRankingsUI;
    [SerializeField] private RetryMenu retryMenu;

    public void Open()
    {
        StartCoroutine(GetRankingEnumerator());
    }

    private IEnumerator GetRankingEnumerator()
    {
        yield return RankingRequestHandler.GetRanking
        (
            data => PopulateRankingList(data),
            req => retryMenu.InternetConnectionLost(GetRankingEnumerator())
        );
    }

    public void Close()
    {
        EnableUI(false);
    }

    public void PopulateRankingList(RankingData data)
    {
        foreach(var player in playerRankingsUI)
        {
            player.IsDisplayed(false);
        }

        var j = 0;
        for (var i = data.ranking.Length - 1; i >= 0 && j < playerRankingsUI.Length - 1; i--)
        {
            var playerData = data.ranking[i];
            var playerPosition = data.ranking.Length - i;
            playerRankingsUI[j].SetStatus(playerData.name, playerData.score, playerPosition);
            
            bool isPersonal = (playerPosition == data.personal.position);
            playerRankingsUI[j].IsPersonal(isPersonal);
            playerRankingsUI[j].IsDisplayed(true);
            j++;
        }
        
        if(data.personal.position > playerRankingsUI.Length - 1)
        {
            var personal = data.personal;
            playerRankingsUI[playerRankingsUI.Length - 1].SetStatus(personal.name, personal.top_score, personal.position);
            playerRankingsUI[playerRankingsUI.Length - 1].IsDisplayed(true);
            playerRankingsUI[playerRankingsUI.Length - 1].IsPersonal(true);
        }

        EnableUI(true);
    }

    private void EnableUI(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}