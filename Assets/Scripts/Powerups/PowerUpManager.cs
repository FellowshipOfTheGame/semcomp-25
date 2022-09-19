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
    [SerializeField] private float teleportDelay;
    [SerializeField] private PowerUpHud powerUpHud;
    [SerializeField] private SpriteRenderer ghostSprite;
    private int lives;  // number of lives of the player
    private bool isInvisible;

    private float initialBallRadius;

    // The scene need an indetructible object with the script 'BarHorizontalMovement'
    // and this object attached here
    [SerializeField] private BarHorizontalMovement barHorizontalMovement;

    private AudioManager audioManager;

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
        audioManager = AudioManager.instance;
    }
    public void SetPowerUpHud(Sprite sprite, float duration)
    {
        powerUpHud.CreateTimer(sprite, duration);
    }

    public void SetLifePowerUpHud(Sprite sprite)
    {
        powerUpHud.SetLifeView(sprite, lives);
    }
    public void AddLife(int n)
    {
        lives += n;
    }
    public void LoseLife()
    {
        lives--;
        powerUpHud.SetLifeView(lives);
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
        coroutine = GhostBallAnim(invisibilityTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator GhostBallAnim(float invisibilityTime)
    {
        Color oldColor = new Color(1f, 1f, 1f, 0);
        Color newColor = new Color(1f,1f,1f,1f);
        isInvisible = true; 

        ballFx.SetActive(false);

         yield return StartCoroutine(ColorAnimation(ghostSprite, oldColor, newColor)); 
        yield return new WaitForSeconds(invisibilityTime);
        yield return StartCoroutine(ColorAnimation(ghostSprite,newColor, oldColor));


        ballFx.SetActive(true);
        isInvisible = false;
    }

    public void ChangeTemporarilyBallSize(float radius, float time)
    {
        coroutine = ChangeBallSizeAndWait(radius, time);
        StartCoroutine(coroutine);
    }

    private IEnumerator ColorAnimation(SpriteRenderer _sprite, Color startColor, Color endColor, float speed=0.02f)
    {

        float t = 0;
        Color currColor;
        do
        {
            currColor = Color.Lerp(startColor, endColor, t);
            _sprite.color = currColor;
            t += speed;
            yield return new WaitForEndOfFrame();
        } while (t <= 1f);

        _sprite.color = endColor;
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
        float speed = 2f;
        do
        {
            currRadius = Mathf.Lerp(start, finish, t);
            transform.localScale = new Vector3(currRadius, currRadius, 1f);
            t += speed * Time.deltaTime;
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
        if (radius > this.transform.localScale.x)
            audioManager.PlaySFX("GrowBall");
        else
            audioManager.PlaySFX("ShrinkBall");

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
            audioManager.PlaySFX("TimeBonus");
            timeManager.AddTime(time);
        }
    }
    public void PowerupScore(int score)
    {

        if (!scoreManager)
        {
            Debug.Log("Score Added:" + score);
        }
        else
        {
            scoreManager.AddPowerupScore(score);
        }

    }


    // Portal Teleport

    private bool canTeleport = true;
    public void Teleport(Vector3 pos,Vector2 vel)
    {
        if (canTeleport)
        {

            transform.position = pos;
            gameObject.GetComponent<Rigidbody2D>().velocity = vel;
            StartCoroutine(CanTeleportDelay());
        }
    }
    IEnumerator CanTeleportDelay()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportDelay);
        canTeleport = true;
    }
    // Ice (freeze ball)
    [SerializeField] float freezeDuration = 1f;
    [SerializeField] GameObject icePrefab;
    [SerializeField] float iceSpeed = 10f;

    public void IceBall(GameObject ball)
    {
        Vector3 pos = ball.transform.position;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        GameObject fx = Instantiate(icePrefab, pos, Quaternion.identity,ball.transform);
        //fx.GetComponent<AnimationManager>().PlayAnim("FxEnd");
        StartCoroutine(changeSpeedFx(rb, fx,freezeDuration, iceSpeed));
    }
    [SerializeField] GameObject lamaPrefab;
    [SerializeField] float lamaSpeed = 3f;
    public void LamaBall(GameObject ball)
    {
        Vector3 pos = ball.transform.position;
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        GameObject fx = Instantiate(lamaPrefab, pos, Quaternion.identity, ball.transform);
        //fx.GetComponent<AnimationManager>().PlayAnim("FxEnd");
        StartCoroutine(changeSpeedFx(rb, fx, freezeDuration, lamaSpeed));
    }

    private IEnumerator changeSpeedFx(Rigidbody2D body,GameObject fx,float duration,float speed)
    {
        Vector2 initialVel = body.velocity;
        body.velocity =initialVel.normalized*speed;
        yield return new WaitForSeconds(duration);
        body.velocity = initialVel;
        yield return StartCoroutine(ColorAnimation(fx.transform.GetChild(0).GetComponent<SpriteRenderer>(),
            new Color(1f, 1f, 1f, 1f),
            new Color(1f, 1f, 1f, 0f),
            0.1f));
        Destroy(fx);
    }

}
