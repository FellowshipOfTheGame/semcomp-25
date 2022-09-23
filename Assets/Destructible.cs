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
        AudioManager.instance.PlaySFX("Glass");
        if (currentSprite >= spriteCount)
        {
            SpawnFx(transform);
            DestroyThis();
        }
        else
        {
            spriteRenderer.sprite = sprites[currentSprite];
            currentSprite++;
        }
    }
}
