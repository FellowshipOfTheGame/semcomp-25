using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer.sprite = SpriteRotation.enemySprite;
    }
}
