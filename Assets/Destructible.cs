using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    string BALL_TAG = "Ball";
    [SerializeField] List<Sprite> sprites;
    [SerializeField] GameObject fxObj;
    int spriteCount;
    int currentSprite = 0;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteCount = sprites.Count;
    }
    void DestroyThis()
    {
        Destroy(gameObject);
    }
    public void SpawnFx(Transform targetTransf)
    {
        Instantiate(fxObj, targetTransf.position, Quaternion.identity);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (currentSprite >= spriteCount)
        {
            if (fxObj.name == "MadeiraFx")
                AudioManager.instance.PlaySFX("Madeira2");
            else
                AudioManager.instance.PlaySFX("Glass");

            SpawnFx(transform);
            DestroyThis();
        }
        else
        {
            AudioManager.instance.PlaySFX("Madeira1");
            spriteRenderer.sprite = sprites[currentSprite];
            currentSprite++;
        }
    }
}
