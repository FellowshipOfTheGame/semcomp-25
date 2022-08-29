using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    string BALL_TAG = "Ball";
    [SerializeField] List<Sprite> sprites;
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
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.CompareTag(BALL_TAG));
        if (currentSprite >= spriteCount)
        {
            DestroyThis();
        }
        else
        {
            spriteRenderer.sprite = sprites[currentSprite];
            currentSprite++;
        }
    }
}
