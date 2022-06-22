using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] float range = 2.2f;
    [SerializeField] float speed = 0.5f;
    [SerializeField] float targetX = 0;
    [SerializeField] float startX = 0;
    [SerializeField] float startMouseX = 0;
    [SerializeField] bool selected = false;
    public bool Selected => selected;

    public void SetStartX(float _x)
    {
        startX = _x;
    }
    public void SetStartMouseX(float _x)
    {
        startMouseX = _x;
    }

    public void SetSelected(bool val)
    {
        selected = val; 
    }

    public void SetTarget(float currentMouseX)
    {
        float dirX = currentMouseX-startMouseX;
        targetX = startX + dirX;
        targetX = Mathf.Min(targetX, range);
        targetX = Mathf.Max(targetX, -range);
    }

    // Update is called once per frame
    void Update()
    {
        // move to targetX position
        if (selected)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, targetX, speed * Time.deltaTime);
            transform.position = pos;
        }
    }
}
