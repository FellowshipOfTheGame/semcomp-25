using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private float freezeTime;

    [SerializeField] private float slowTime;
    [SerializeField] private float slowFactor;

    private IEnumerator coroutine;

    [HideInInspector] public float speedFactor;

    // Start is called before the first frame update
    void Start()
    {
        // Set the speed factor to 1 (doesn't change the speed)
        speedFactor = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("pw_freeze"))
        {
            ChangeSpeedFactorTemporarily(freezeTime, 0.0f);
        }
        else if (collision.CompareTag("pw_slow"))
        {
            ChangeSpeedFactorTemporarily(slowTime, slowFactor);
        }
    }

    // Called to change the speed factor, used to move the enemy bar
    public void ChangeSpeedFactorTemporarily(float waitTime, float factor)
    {
        coroutine = WaitAndChangeSpeedFactor(waitTime, factor);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndChangeSpeedFactor(float waitTime, float factor)
    {
        // Change the speedfactor to the new factor
        speedFactor = factor;

        // Wait for 'waitTime' seconds
        yield return new WaitForSeconds(waitTime);

        // Return the factor to 1.0 (default)
        speedFactor = 1.0f;
    }
}
