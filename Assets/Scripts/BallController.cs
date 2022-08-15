using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private MapManager map;
    [SerializeField] private GameManager manager;
    [SerializeField] private PlayerInputManager playerManager;
    [SerializeField] private LineRenderer line;
    [Header("Stats")]
    [SerializeField]
    private float offsetFromPlayer;
    [SerializeField] private float throwSpeed;

    // state variables
    private GameObject currentPlayer;
    private bool lockedOntoPlayer;

    [Header("Second throw mode")]
    [SerializeField]
    private bool secondThrowMode;

    [Header("Variable force mode")]
    [SerializeField]
    private bool variableForceMode;
    [SerializeField] private float maxDistance = 2f;
    private bool mousePressed;

    // cached references
    private Rigidbody2D rb2d;

    [SerializeField] private int distanceToGoal; // Number of allies to achieve the goal
    private int distanceCount;

    private PowerUpManager powerUpManager;
    [SerializeField] private GameObject ballHitPrefab;
    private Camera camera1;
    
    private void Start()
    {
        camera1 = Camera.main;
    }

    private void Awake()
    {
        powerUpManager = GetComponent<PowerUpManager>();
        distanceCount = 0;
        rb2d = GetComponent<Rigidbody2D>();
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
        // shortcuts
        if (Input.GetKeyDown(KeyCode.T))
        {
            secondThrowMode = !secondThrowMode;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            variableForceMode = !variableForceMode;
        }


        // aim and throw
        if (lockedOntoPlayer)
        {
            var position1 = currentPlayer.transform.position;
            transform.position = new Vector2(position1.x, position1.y + offsetFromPlayer);
            
            Vector3 mousePosition = camera1!.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (variableForceMode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // mouse clicado próximo ao jogador
                    if (Vector2.SqrMagnitude(mousePosition - currentPlayer.transform.position) < (0.5f)*(0.5f))
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
                    Debug.DrawRay(pos1,pos2-position);
                    line.SetPosition(0, pos1);
                    line.SetPosition(1, pos2);

                    if (Input.GetMouseButtonUp(0)) // chuta bola
                    {
                        currentPlayer.GetComponent<Ally>().Kick();
                        StartCoroutine(KickDelay(forceLevel));
                        /*
                        ThrowBall(forceLevel);
                        mousePressed = false;
                        playerManager.SetCanMove(true);
                        line.enabled = false;*/
                    }

                    if (Input.GetMouseButtonDown(1)) // soltou a mira
                    {
                     //   currentPlayer.GetComponent<Ally>().Kick();
                        // release aim
                        mousePressed = false;
                        playerManager.SetCanMove(true);
                        line.enabled = false;
                    }
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

//        currentPlayer = null;
        lockedOntoPlayer = false;
        rb2d.bodyType = RigidbodyType2D.Dynamic;

        if (variableForceMode)
            rb2d.velocity = -(mousePosition - transform.position).normalized * (throwSpeed * forceLevel);
        else if (!secondThrowMode)
            rb2d.velocity = (mousePosition - transform.position).normalized * throwSpeed;
        else
            rb2d.velocity = -(mousePosition - transform.position).normalized * throwSpeed;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        manager.GameOverScene();
        Destroy(gameObject);
    }
    

    private void SetGameOver()
    {
        Instantiate(ballHitPrefab, transform.position, Quaternion.identity);
        rb2d.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        map.SetBallFx(false);
        StartCoroutine(GameOver());
    }

    // Auxiliar function to set the ball freezed to the player position
    private void SetBallToPlayer(GameObject player)
    {
        currentPlayer = player;
        currentPlayer.transform.GetChild(0).gameObject.SetActive(true);
        lockedOntoPlayer = true;
        rb2d.velocity = Vector2.zero;
        rb2d.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ally"))
        {
            distanceCount++;

            SetBallToPlayer(collision.gameObject);

            map.StartTransition(currentPlayer.transform.parent);

            // Check distance to generate the goal
            if (distanceCount >= distanceToGoal)
            {
                distanceCount = -99;
                // Generate goal
                map.SpawnGoal(16);
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
                map.StartTransition(currentPlayer.transform.parent);
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
                }
            }
        }
        else if (collision.CompareTag("Goal"))
        {
            // Reset distance
            distanceCount = 0;

            // Update the fase level
            manager.AddFaseLevel();

            // Update distance to goal 
            //distanceToGoal = manager.FaseLevel() * 10; // used if the distance will be different accordingly to the fase level

            // Show Goal animation


            // Get the Transform of the removed ally (replaced with the Goal object)

            //SetBallToPlayer(map.RemovedAllyTransform().gameObject);

            // Delete goal object and move camera
            map.StartDeleteGoalTransition();
        }

    }
}
