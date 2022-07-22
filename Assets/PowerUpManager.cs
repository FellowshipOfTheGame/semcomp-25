using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private float freezeTime;

    [SerializeField] private float slowTime;
    [SerializeField] private float slowFactor;

    // The scene need an indetructible object with the script 'BarHorizontalMovement'
    // and this object attached here
    [SerializeField] private BarHorizontalMovement barHorizontalMovement;

    private void Awake()
    {
        // Set the initial speed factor (static variable) to 1.0f
        barHorizontalMovement.ChangeSpeedFactorTemporarily(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("pw_freeze"))
        {
            barHorizontalMovement.ChangeSpeedFactorTemporarily(freezeTime, 0.0f);
        }
        else if (collision.CompareTag("pw_slow"))
        {
            barHorizontalMovement.ChangeSpeedFactorTemporarily(slowTime, slowFactor);
        }
    }

    private void ChangeSpeedFactor(float waitTime, float factor)
    {
        barHorizontalMovement = FindObjectOfType<BarHorizontalMovement>();
    }
}
