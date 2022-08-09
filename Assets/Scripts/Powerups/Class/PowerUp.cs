using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PowerUp : MonoBehaviour
{
    [SerializeField] UnityEvent OnBallCollision;
    private PowerUpManager manager;
    private GameObject ballObj;
    public PowerUpManager Manager => manager;
    private void Awake()
    {
        ballObj = GameObject.FindGameObjectWithTag("Ball");
        manager = ballObj.GetComponent<PowerUpManager>();
    }
    public abstract void Collected(Collider2D col);

    // Realizam a função collected ao colidir com player/missilBaby
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ball"))
            return;
        Collected(collision);
        OnBallCollision.Invoke();
        Destroy(gameObject);
    }
}
