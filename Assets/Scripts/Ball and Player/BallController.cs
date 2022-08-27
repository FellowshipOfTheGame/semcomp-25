using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject ballHitPrefab;

    [Header("Stats")]
    [SerializeField] private float offsetFromPlayer;
    [SerializeField] private float throwSpeed;
    [SerializeField] private float playerRadius = 0.5f;
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
    
    private void Awake()
    {
        powerUpManager = GetComponent<PowerUpManager>();
        rb2d = GetComponent<Rigidbody2D>();
        
        mapManager = FindObjectOfType<MapManager>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        playerManager = FindObjectOfType<PlayerInputManager>();
        camera1 = Camera.main;
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
                if (Vector2.SqrMagnitude(mousePosition - currentPlayer.transform.position) < playerRadius*playerRadius)
                {
                    mousePressed = true;
                    playerManager.SetCanMove(false);

                    currentPlayer.GetComponent<Ally>().Pull();
                    currentPlayer.transform.parent.gameObject.GetComponent<PlayerController>().SetSelected(false);
                }
            }

            if (mousePressed)
            {
                // começa a mirar
                line.enabled = true;
                float forceLevel = GetForceLevel(mousePosition, currentPlayer.transform.position);
                var position = transform.position;
                Vector3 pos1 = position;
                Vector3 pos2= position+(position - mousePosition).normalized * (forceLevel * maxDistance * 2f);

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
        
        lockedOntoPlayer = false;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        
        rb2d.velocity = -(mousePosition - transform.position).normalized * (throwSpeed * forceLevel);
        
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
        Instantiate(ballHitPrefab, transform.position, Quaternion.identity);
        rb2d.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        mapManager.SetBallFx(false);
        FindObjectOfType<Timer>().SetPaused(true);
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

        if (ballLaunched)
        {
            ballLaunched = false;
            // if position is greater than when it was launched plus a little tolerance
            if (transform.position.y > yPosWhenLaunched + 0.15f)
                OnSuccessfulPass?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collisionCollider = collision.collider;

        if (collisionCollider.CompareTag("LateralWall"))
        {
            audioManager.PlaySFX("HitWall");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ally"))
        {            
            SetBallToPlayer(collision.gameObject);

            mapManager.StartTransition(currentPlayer.transform.parent);
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
            OnGoalScored?.Invoke();
            StartCoroutine(GoalTransition());
        }
        
    }

    IEnumerator GoalTransition()
    {
        audioManager.PlaySFX("Goal");
        rb2d.velocity = rb2d.velocity.normalized * .8f; // slows ball
        gameManager.PassLevel(false); // pass level but dont update UI now
        gameManager.SetLevelProgress(1f);
        
        yield return new WaitForSeconds(2f);
        rb2d.simulated = false; // so the ball doesnt hit the map boundaries when doing the transition
        
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
        gameManager.SetLevelProgress(0f);
        gameManager.SetLevelView(); // finally updates text UI with the levels

        mapManager.RepositionField();
    }
}
