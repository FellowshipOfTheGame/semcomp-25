using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldComponent : MonoBehaviour
{
    [SerializeField] private float goalOffset; // goal offset from edge of image when not scaled

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetGoalPosition(float startGoalY, float endGoalY)
    {
        float parentScale = transform.parent.localScale.y;

        float newHeight = (goalOffset * 2) + (endGoalY - startGoalY) / parentScale;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, newHeight);
    }
    
    // used for sprite "LINHAS" which is divided in half
    public void SetGoalPosition(float goalFromCenter)
    {
        float parentScale = transform.parent.localScale.y;

        float newHeight = goalOffset + (goalFromCenter) / parentScale;
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, newHeight);
    }
}
