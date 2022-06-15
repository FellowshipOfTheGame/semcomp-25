using System;
using UnityEngine;

public class Field : MonoBehaviour
{
    public enum States
    {
        Reaching,
        OnTarget,
        Leaving
    }

    private float _speed = 5f;
    
    public States state;
    
    private void Update()
    {
        switch (state)
        {
            case States.Reaching:
                GetToTarget();
                break;
            case States.Leaving:
                LeaveTarget();
                break;
            case States.OnTarget:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GetToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero,
            Time.deltaTime * _speed);

        if (!(Vector2.Distance(transform.position, Vector2.zero) < 0.001f)) return;
        
        state = States.OnTarget;
    }
    
    private void LeaveTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0f, -30f, 0f),
                Time.deltaTime * _speed);
    }
}
