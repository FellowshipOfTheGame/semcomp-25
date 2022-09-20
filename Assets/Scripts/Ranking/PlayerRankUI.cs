using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerRankUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTextMesh;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;
    [SerializeField] private TextMeshProUGUI positionTextMesh;
    [SerializeField] private GameObject[] personalMarkers;
    [SerializeField] private Image entryImage;
    
    [Header("Position images")]
    [SerializeField] private Sprite firstPlaceSprite;
    [SerializeField] private Sprite secondPlaceSprite;
    [SerializeField] private Sprite thirdPlaceSprite;
    [SerializeField] private Sprite otherPlaceSprite;

    public void SetStatus(string name, int score, int position)
    {
        nameTextMesh.SetText(name);
        scoreTextMesh.SetText(score.ToString());
        positionTextMesh.SetText(position.ToString());

        entryImage.sprite = position switch
        {
            1 => firstPlaceSprite,
            2 => secondPlaceSprite,
            3 => thirdPlaceSprite,
            _ => otherPlaceSprite
        };
    }

    public void IsPersonal(bool value)
    {
        foreach (var pm in personalMarkers)
        {
            pm.SetActive(value);
        }
    }

    public void IsDisplayed(bool value)
    {
        gameObject.SetActive(value);
    }
}