using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerUpHud : MonoBehaviour
{
    [SerializeField] private GameObject PowerUpViewPrefab;
    private bool hasLifeView=false;
    [SerializeField] private PowerUpView lifeView;

    public void CreateTimer(Sprite powerUpSprite, float time)
    {
        GameObject obj=Instantiate(PowerUpViewPrefab, transform);
        PowerUpView view = obj.GetComponent<PowerUpView>();
        view.SetImage(powerUpSprite);
        view.StartTimer(time);
    }
    public void DeleteViews(Sprite checkSprite)
    {
        foreach(Transform powerUp in transform)
        {
            if (powerUp.GetComponent<PowerUpView>().GetImage().sprite == checkSprite)
            {
                Destroy(powerUp.gameObject);
            }
        }
    }
    public void SetLifeView(Sprite powerUpSprite, int lives)
    {
        if (!hasLifeView)
        {
            GameObject obj = Instantiate(PowerUpViewPrefab, transform);
            lifeView = obj.GetComponent<PowerUpView>();
            lifeView.SetImage(powerUpSprite);
            hasLifeView = true;
        }
        lifeView.SetLife(lives);
    }
    public void SetLifeView(int lives)
    {
        if (lifeView==null) return;
        lifeView.SetLife(lives);
        if (lives == 0)
            hasLifeView = false;
    }
}
