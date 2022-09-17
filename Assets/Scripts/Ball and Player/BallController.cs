using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject ballHitPrefab;

    [Header("Stats")]
    [SerializeField] private float offsetFromPlayer;
    [SerializeField] private float throwSpeed;
    [SerializeField] private float ballRadius = 0.5f;
    [SerializeField] private float maxDistance = 2f;

    // state variables
    private GameObject currentPlayer;
    private bool lockedOntoPlayer;
    private bool mousePressed;

    private bool gameOverSet = false;
    public bool GoalTransitionOver { private get; set; }
    
    /* Pass event */
    public delegate void SuccessfulPass();
    public static event SuccessfulPass OnSuccessfulPass;
    private bool ballLaunched;
    private float yPosWhenLaunched;
    private int alliesWhenLaunched;
    private int alliesPassed = 0;
    
    /* Goal event */
    public delegate void GoalScored();
    public static event GoalScored OnGoalScored;
    
    // cached references
    private Rigidbody2D rb2d;
    private MapManager mapManager;
    private GameManager gameManager;
    private AudioManager audioManager;
    private PlayerInputManager playerManager;
    private PowerUpManager powerUpManager;
    private Camera camera1;
    private Timer timer;
    
    private void Awake()
    {
        powerUpManager = GetComponent<PowerUpManager>();
        rb2d = GetComponent<Rigidbody2D>();
        
        mapManager = FindObjectOfType<MapManager>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        playerManager = FindObjectOfType<PlayerInputManager>();
        camera1 = Camera.main;
        timer = FindObjectOfType<Timer>();
    }

    private IEnumerator KickDelay(float force)
    {
        yield return new WaitForSeconds(0.1f);
        ThrowBall(force);
        mousePressed = false;
        playerManager.SetCanMove(true);
        line.enabled = false;
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (PauseMenu.isGamePaused) return;
        
        // aim and throw
        if (lockedOntoPlayer)
        {
            var position1 = currentPlayer.transform.position;
            transform.position = new Vector2(position1.x, position1.y + offsetFromPlayer);
            
            Vector3 mousePosition = camera1!.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (Input.GetMouseButtonDown(0))
            {
                // mouse clicado próximo ao jogador
                if (Vector2.SqrMagnitude(mousePosition - transform.position) < ballRadius*ballRadius)
                {
                    mousePressed = true;
                    playerManager.SetCanMove(false);

                    currentPlayer.GetComponent<Ally>().Pull();
                }
            }

            if (mousePressed)
            {
                // começa a mirar
                line.enabled = true;
                var position = transform.position;
                float forceLevel = GetForceLevel(mousePosition, position);
                Vector3 pos1 = position;
                Vector3 pos2 = position + (mousePosition - position).normalized * (forceLevel * maxDistance * 2f);

                line.SetPosition(0, pos1);
                line.SetPosition(1, pos2);

                if (Input.GetMouseButtonUp(0)) // chuta bola
                {
                    currentPlayer.GetComponent<Ally>().Kick();
                    StartCoroutine(KickDelay(forceLevel));
                }

                if (Input.GetMouseButtonDown(1)) // soltou a mira
                {
                    // release aim
                    mousePressed = false;
                    playerManager.SetCanMove(true);
                    line.enabled = false;
                    currentPlayer.GetComponent<Ally>().Idle();
                }
            }
        }
    }

    private float GetForceLevel(Vector2 from, Vector2 to)
    {
        float distance = Vector2.Distance(from, to);

        float distancePercent = distance / maxDistance;

        return Mathf.Clamp(distancePercent, 0.3f, 1f);
    }

    private void ThrowBall(float forceLevel = 1f)
    {
        Vector3 mousePosition = camera1.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // set pass state
        ballLaunched = true;
        yPosWhenLaunched = transform.position.y;
        alliesWhenLaunched = mapManager.AllyBarsPassed;
        
        lockedOntoPlayer = false;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        
        Vector2 newVelocity = (mousePosition - transform.position).normalized * (throwSpeed * forceLevel);
        rb2d.velocity = newVelocity;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager.GameOverScene();
        Destroy(gameObject);
    }

    public void SetGameOver()
    {
        audioManager.PlaySFX("WrongPass");
        if (gameOverSet)
            return;
        var fx = Instantiate(ballHitPrefab, transform.position, Quaternion.identity);
        rb2d.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        mapManager.SetBallFx(false);
        timer.SetPaused(true);
        StartCoroutine(GameOver());
        gameOverSet = true;
    }

    // Auxiliar function to set the ball freezed to the player position
    private void SetBallToPlayer(GameObject player)
    {
        audioManager.PlaySFX("PassAlly");
        currentPlayer = player;
        currentPlayer.transform.GetChild(0).gameObject.SetActive(true);
        lockedOntoPlayer = true;
        rb2d.velocity = Vector2.zero;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        
        var position1 = currentPlayer.transform.position;
        transform.position = new Vector2(position1.x, position1.y + offsetFromPlayer);
        
        currentPlayer.transform.parent.GetComponentInChildren<AllyBar>().SetPassed();
        
        if (ballLaunched)
        {
            ballLaunched = false;
            alliesPassed += mapManager.AllyBarsPassed - alliesWhenLaunched;

            // if position is greater than when it was launched plus a little tolerance
            if (transform.position.y > yPosWhenLaunched + 0.15f)
            {
                for (int i = 0; i < alliesPassed; i++)
                    OnSuccessfulPass?.Invoke();
                alliesPassed = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collisionCollider = collision.collider;

        if (collisionCollider.CompareTag("LateralWall"))
        {
            // correction for when vertical velocity is too small and ball gets stuck on infinite horizontal movement
            if (Mathf.Abs(rb2d.velocity.y) < 0.2f)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0.2f * Mathf.Sign(rb2d.velocity.y));
            }
            
            audioManager.PlaySFX("HitWall");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ally"))
        {
            if (!lockedOntoPlayer)
            {
                SetBallToPlayer(collision.gameObject);

                mapManager.StartTransition(currentPlayer.transform.parent);
            }
        }
        else if (collision.CompareTag("MapTop"))
        {
            // Checks the number of lives the player has
            if (powerUpManager.GetNumberLives() < 2)
            {
                // Game Over
                SetGameOver();
            }
            // If the number of lives of the player is up to 1:
            else
            {
                // the player loose one life
                powerUpManager.LoseLife();
                // the ball get back to the last ally that 'kicked' the ball
                
                SetBallToPlayer(currentPlayer);
                mapManager.StartTransition(currentPlayer.transform.parent);
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            // Check first if the ball is invisible (by invisible powerup)
            if (!powerUpManager.IsInvisible())
            {
                // Then, checks the number of lives the player has
                if (powerUpManager.GetNumberLives() < 2)
                {
                    // Game Over
                    SetGameOver();
                }
                // If the number of lives of the player is up to 1, the plase loose one life
                else
                {
                    powerUpManager.LoseLife();
                    SetBallToPlayer(currentPlayer);
                }
            }
        }
        else if (collision.CompareTag("Goal"))
        {
            ballLaunched = false;
            alliesPassed = 0;
            OnGoalScored?.Invoke();
            StartCoroutine(GoalTransition());
        }
        
    }

    IEnumerator GoalTransition()
    {
        timer.SetPaused(true);
        audioManager.PlaySFX("Goal");
        rb2d.velocity = rb2d.velocity.normalized * .8f; // slows ball
        gameManager.PassLevel(false); // pass level but dont update UI now
        gameManager.SetLevelProgress(1f);
        mapManager.SpawnPresetsUntilGoal();
        mapManager.RepositionFields();

        yield return new WaitForSeconds(2f);
        rb2d.simulated = false; // so the ball doesnt hit the map boundaries when doing the transition

        gameManager.SetLevelProgress(0f);
        gameManager.SetLevelView(); // finally updates text UI with the levels

        GameObject nextPlayer = mapManager.GetFirstPlayerOfLevel(gameManager.Level);
        nextPlayer = nextPlayer.GetComponentInChildren<Ally>().gameObject;
        SetBallToPlayer(nextPlayer);
        
        GoalTransitionOver = false; // this variable will be set to true when the player transition on MapManager is finished
        mapManager.StartTransition(nextPlayer.transform.parent);
        
        // wait until transition is finished to reposition field
        while (!GoalTransitionOver)
        {
            yield return null;
        }
        
        rb2d.simulated = true;

        timer.SetPaused(false);
    }
}
