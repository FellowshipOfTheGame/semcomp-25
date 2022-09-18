using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowerUpView : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_Text counter;
    
    public void SetImage(Sprite _sprite)
    {
        image.sprite = _sprite;
    }
    public void StartTimer(float time)
    {
        StartCoroutine(Timer(time));
    }
    public void SetLife(int lives)
    {
        lives--;
        counter.text = lives + "";
        if (lives == 0)
            Destroy(gameObject);
    }
    IEnumerator Timer(float initial)
    {
        float t = initial;
        while (t>=0)
        {
            counter.text =t.ToString("0.0");
            t -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
