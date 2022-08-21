using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float bottomY;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bottomY = transform.position.y - spriteRenderer.size.y / 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SetGoalPosition(100f);
        else if (Input.GetKeyDown(KeyCode.R))
            ResetPosition();
    }

    // sets a height but only grows upwards, keeping the bottom in place
    private void SetFieldHeight(float height)
    {
        var transform1 = transform;
        var size = spriteRenderer.size;
        
        float localBottomY = transform1.localPosition.y - size.y / 2;
        float newY = localBottomY + height / 2;
        
        spriteRenderer.size = new Vector2(size.x, height);
        transform1.localPosition = new Vector3(transform1.position.x, newY);
    }

    public void SetGoalPosition(float goalY)
    {
        //Vector3 goalPos = new Vector3(transform1.position.x, goalY);
        //goalPos = transform1.InverseTransformPoint(goalPos);
        
        float currentBottomY = transform.position.y - spriteRenderer.size.y / 2;
        float newHeight = goalY - currentBottomY;
        SetFieldHeight(newHeight);
    }

    // resets bottom position 
    public void ResetPosition()
    {
        float newY = bottomY + spriteRenderer.size.y / 2;
        transform.position = new Vector3(transform.position.x, newY);
    }
}
