using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeObject : MonoBehaviour
{
    [SerializeField] int addedSeconds;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            FindObjectOfType<Timer>().AddTime(addedSeconds);
            Destroy(gameObject);
        }
    }
}
