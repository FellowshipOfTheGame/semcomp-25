using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField] private MapManager map;
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

    private void Awake()
    {
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
                    // mouse clicado pr�ximo ao jogador
                    if (Vector2.SqrMagnitude(mousePosition - currentPlayer.transform.position) < (0.5f)*(0.5f))
                    {
                        mousePressed = true;
                        playerManager.SetCanMove(false);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ally"))
        {
            currentPlayer = collision.gameObject;
            lastPlayer = currentPlayer;
            lockedOntoPlayer = true;
            rb2d.velocity = Vector2.zero;
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            map.StartTransition(currentPlayer.transform.parent);
        }else if (collision.CompareTag("MapTop"))
        {
            transform.position = lastPlayer.transform.position;
        }
    }
}
