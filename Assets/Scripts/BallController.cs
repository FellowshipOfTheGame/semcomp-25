using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField] private MapManager map;
    [SerializeField] private GameManager manager;
    [SerializeField] private PlayerInputManager playerManager;
    [SerializeField] private LineRenderer line;
    [Header("Stats")]
    [SerializeField] float offsetFromPlayer;
    [SerializeField] float throwSpeed;

    // state variables
    GameObject currentPlayer;
    GameObject lastPlayer;
    bool lockedOntoPlayer = false;

    [Header("Second throw mode")]
    [SerializeField] bool secondThrowMode = false;

    [Header("Variable force mode")]
    [SerializeField] bool variableForceMode = false;
    [SerializeField] float maxDistance = 2f;
    bool mousePressed = false;

    // cached references
    Rigidbody2D rb2d;

    [SerializeField] private int distanceToGoal; // Number of allies to achieve the goal
    private int distanceCount;

    private void Awake()
    {
        distanceCount = 0;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
            transform.position = new Vector2(currentPlayer.transform.position.x, currentPlayer.transform.position.y + offsetFromPlayer);
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
                        currentPlayer.transform.parent.gameObject.GetComponent<PlayerController>().SetSelected(false);
                    }
                }

                if (mousePressed)
                {
                    line.enabled = true;
                    float forceLevel = GetForceLevel(mousePosition, currentPlayer.transform.position);
                    Vector3 pos1 = transform.position;
                    Vector3 pos2= transform.position+(transform.position - mousePosition).normalized * forceLevel * maxDistance * 2f;
                    Debug.DrawRay(pos1,pos2-transform.position);
                    line.SetPosition(0, pos1);
                    line.SetPosition(1, pos2);

                    if (Input.GetMouseButtonUp(0))
                    {
                        ThrowBall(forceLevel);
                        mousePressed = false;
                        playerManager.SetCanMove(true);
                        line.enabled = false;
                    }

                    if (Input.GetMouseButtonDown(1))
                    {
                        // release aim
                        mousePressed = false;
                        playerManager.SetCanMove(true);
                        line.enabled = false;
                    }
                }
            }
            else if (!secondThrowMode)
            {
                if (Input.GetMouseButton(0))
                {
                    Debug.DrawLine(transform.position, mousePosition);
                }

                if (Input.GetMouseButtonUp(0))
                {
                    ThrowBall();
                }
            }
            else
            {
                // apenas se mouse se encontra na zona abaixo do player
                if (mousePosition.y < currentPlayer.transform.position.y)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Debug.DrawRay(transform.position, (transform.position - mousePosition) * 5f);
                    }

                    if (Input.GetMouseButtonUp(0))
                    {
                        ThrowBall();
                    }
                }
            }
        }  
        
        /*
        //temporary player move for testing
        if (currentPlayer)
        {
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            currentPlayer.transform.position += 5f * Time.deltaTime * new Vector3(horizontalMove, 0, 0);
        }*/
    }

    private float GetForceLevel(Vector2 from, Vector2 to)
    {
        float distance = Vector2.Distance(from, to);

        float distancePercent = distance / maxDistance;

        return Mathf.Clamp(distancePercent, 0.3f, 1f);
    }

    private void ThrowBall(float forceLevel = 1f)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        currentPlayer = null;
        lockedOntoPlayer = false;
        rb2d.bodyType = RigidbodyType2D.Dynamic;

        if (variableForceMode)
            rb2d.velocity = -(mousePosition - transform.position).normalized * throwSpeed * forceLevel;
        else if (!secondThrowMode)
            rb2d.velocity = (mousePosition - transform.position).normalized * throwSpeed;
        else
            rb2d.velocity = -(mousePosition - transform.position).normalized * throwSpeed;
    }
    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);
        manager.GameOverScene();
        Destroy(gameObject);
    }
    [SerializeField] private GameObject ballHitPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ally"))
        {
            distanceCount++;

            currentPlayer = collision.gameObject;
            currentPlayer.transform.GetChild(0).gameObject.SetActive(true);
            lastPlayer = currentPlayer;
            lockedOntoPlayer = true;
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            map.StartTransition(currentPlayer.transform.parent);

            // Check distance to generate the goal
            if (distanceCount >= distanceToGoal)
            {
                distanceCount = -99;
                // Generate goal
                map.SpawnGoal(16);
            }
        } 
        else if (collision.CompareTag("MapTop") || collision.CompareTag("Enemy"))
        {
            //transform.position = lastPlayer.transform.position;
            Instantiate(ballHitPrefab, transform.position, Quaternion.identity);
            rb2d.bodyType=RigidbodyType2D.Static;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            map.SetBallFx(false);
            StartCoroutine(GameOver());
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
            currentPlayer = map.RemovedAllyTransform().gameObject;
            currentPlayer.transform.GetChild(0).gameObject.SetActive(true);
            lastPlayer = currentPlayer;
            lockedOntoPlayer = true;
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;

            // Delete goal object and move camera
            map.StartDeleteGoalTransition();
        }

    }
}
