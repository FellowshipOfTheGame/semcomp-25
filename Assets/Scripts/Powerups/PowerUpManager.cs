using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{


    private SpriteRenderer spriteRenderer;

    /*
        Adicoes Lucas Ebling =  Conectar a pontos e a tempo/ 
    */

    //Tempo e Score Managers precisam ser conectados com os da cena em questão.
    [SerializeField] private ScoreSystem scoreManager;

    [SerializeField] private Timer timeManager;
    [SerializeField] private GameObject ballFx;
    private int lives;  // number of lives of the player
    private bool isInvisible;

    private float initialBallRadius;

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

    private void Start()
    {
        initialBallRadius = this.GetComponent<CircleCollider2D>().radius;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void AddLife(int n)
    {
        lives += n;
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
    public void ChangeTemporarilyVisibility(float invisibilityTime)
    {
        coroutine = ChangeVisibilityAndWait(invisibilityTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator ChangeVisibilityAndWait(float invisibilityTime)
    {
        Color invisibleColor = new Color(1f, 1f, 1f, 0.2f);
        Color oldColor = spriteRenderer.color;
        isInvisible = true; 

        ballFx.SetActive(false);

         yield return StartCoroutine(ColorAnimation(oldColor, invisibleColor)); 
        yield return new WaitForSeconds(invisibilityTime);
        yield return StartCoroutine(ColorAnimation(invisibleColor, oldColor));


        ballFx.SetActive(true);
        isInvisible = false;
    }

    public void ChangeTemporarilyBallSize(float radius, float time)
    {
        coroutine = ChangeBallSizeAndWait(radius, time);
        StartCoroutine(coroutine);
    }

    private IEnumerator ColorAnimation(Color startColor, Color endColor)
    {

        float t = 0;
        Color currColor;
        float speed = 0.02f;
        do
        {
            currColor = Color.Lerp(startColor, endColor, t);
            spriteRenderer.color = currColor;
            t += speed;
            yield return new WaitForEndOfFrame();
        } while (t <= 1f);

        spriteRenderer.color = endColor;

    }
    private IEnumerator ScaleAnimation(float start, float finish)
    {

        float t = 0;
        float currRadius;
        float speed = 0.02f;
        do
        {
            currRadius = Mathf.Lerp(start, finish, t);
            transform.localScale = new Vector3(currRadius, currRadius, 1f);
            t += speed;
            yield return new WaitForEndOfFrame();
        } while (t <= 1f);

        transform.localScale = new Vector3(finish, finish, 1f);

    }
    public void ChangeEnemyTime(float duration, float timeFactor)
    {
        barHorizontalMovement.ChangeSpeedFactorTemporarily(duration, timeFactor);
    }

    private IEnumerator ChangeBallSizeAndWait(float radius, float time)
    {
        yield return StartCoroutine(ScaleAnimation(0.5f, radius));
        yield return new WaitForSeconds(time);
        yield return StartCoroutine(ScaleAnimation(radius, 0.5f));
    }
    public void AddTime(int time)
    {

        if (!timeManager)
        {
            Debug.Log("Time Added:" + time);
        }
        else
        {
            timeManager.AddTime(time);
        }
    }
    public void AddScore(int score)
    {

        if (!scoreManager)
        {
            Debug.Log("Score Added:" + score);
        }
        else
        {
            scoreManager.addScore(score);
        }

    }

    private bool canTeleport = true;
    public void Teleport(Vector3 pos,Vector2 vel)
    {
        if (canTeleport)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = vel;
            transform.position = pos;
            StartCoroutine(CanTeleportDelay());
        }
    }
    IEnumerator CanTeleportDelay()
    {
        canTeleport = false;
        yield return new WaitForSeconds(0.5f);
        canTeleport = true;
    }
}
