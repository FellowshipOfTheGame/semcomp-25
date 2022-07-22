using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private float freezeTime;

    [SerializeField] private float slowTime;
    [SerializeField] private float slowFactor;

    [SerializeField] private float invisibleTime;

    private int lives;  // number of lives of the player
    private bool isInvisible;

    // The scene need an indetructible object with the script 'BarHorizontalMovement'
    // and this object attached here
    [SerializeField] private BarHorizontalMovement barHorizontalMovement;

    private IEnumerator coroutine;

    private void Awake()
    {
        lives = 1;
        isInvisible = false;
        // Set the initial speed factor (static variable) to 1.0f
        barHorizontalMovement.ChangeSpeedFactorTemporarily(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
                
    }

    public void LoseLife()
    {
        lives--;
    }

    public int GetNumberLives()
    {
        return lives;
    }

    public bool IsInvisible()
    {
        return isInvisible;
    }

    // Turns the ball invisible to enemy collision
    private void ChangeTemporarilyVisibility(float invisibilityTime)
    {
        coroutine = ChangeVisibilityAndWait(invisibilityTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator ChangeVisibilityAndWait(float invisibilityTime)
    {
        isInvisible = true;
        yield return new WaitForSeconds(invisibilityTime);
        isInvisible = false;
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
        else if (collision.CompareTag("pw_invisible-ball"))
        {
            ChangeTemporarilyVisibility(invisibleTime);
        }
        else if (collision.CompareTag("pw_new-life"))
        {
            lives++;
        }
    }
}
