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

    private double distanceBetweenLimits;
    private Vector3 nextPosition;

    void Start()
    {
        // It could be randomized too
        nextPosition = maxLimit.position;
        distanceBetweenLimits = Vector3.Distance(minLimit.position, maxLimit.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Change the 'nextPosition' when the bar limit achieves or passes the limit position
        double distanceBetweenMinPosMaxLimit = Vector3.Distance(minPosition.position, maxLimit.position);
        double distanceBetweenMaxPosMinLimit = Vector3.Distance(maxPosition.position, minLimit.position);

        if (distanceBetweenMaxPosMinLimit > distanceBetweenLimits)
        {
            nextPosition = minLimit.position;
        }
        if (distanceBetweenMinPosMaxLimit > distanceBetweenLimits)
        {
            nextPosition = maxLimit.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
    }
}

