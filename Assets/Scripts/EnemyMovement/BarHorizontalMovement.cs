using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarHorizontalMovement : MonoBehaviour
{
    // Limit is where the Position cannot surpass
    // Position is related to the playable bar ends
    [SerializeField] public Transform minPosition;
    [SerializeField] public Transform maxPosition;
    [SerializeField] public Transform minLimit;
    [SerializeField] public Transform maxLimit;
    [SerializeField] public float speed;
    // Depois mudar para uma lista de strings com as opções
    [SerializeField] public int type;

    private double minDistance = 0.01;
    private Vector3 nextPosition;

    void Start()
    {
        // It could be randomized too
        nextPosition = maxLimit.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Change the 'nextPosition' when the bar limit achieves the 
        // table wall
        if (Vector3.Distance(maxPosition.position, maxLimit.position) < minDistance) 
        {
            nextPosition = minLimit.position;
        }
        if (Vector3.Distance(minPosition.position, minLimit.position) < minDistance)
        {
            nextPosition = maxLimit.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
    }
}

