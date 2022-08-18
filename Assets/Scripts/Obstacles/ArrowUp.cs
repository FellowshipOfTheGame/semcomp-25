using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ArrowUp : MonoBehaviour
{

    private Rigidbody2D ballRb;
    GameObject ballObj;
    [SerializeField] float speed = 8f;
    Vector2 rotated;
    private void Awake()
    {
        Quaternion rot = transform.rotation;
        float angle = Mathf.Deg2Rad*rot.eulerAngles.z;
        print(rot.eulerAngles.z);
        rotated = new Vector2(-(float)Math.Round(Mathf.Sin(angle),2), (float)Math.Round(Mathf.Cos(angle), 2));

        ballObj = GameObject.FindGameObjectWithTag("Ball");
        ballRb = ballObj.GetComponent<Rigidbody2D>();
    }
    float t = 0;
    Vector2 lastVel;
    private void BallUp()
    {
        Vector2 vel = Vector2.Lerp(ballRb.velocity, rotated * speed,t);
        ballRb.velocity =vel;
        t += 0.05f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ball"))
            return;
        BallUp();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ball"))
            return;
        lastVel = ballRb.velocity;
        t = 0;
    }
}
