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

    private static float speedFactor;
    private double distanceBetweenLimits;
    private Vector3 nextPosition;

    private IEnumerator coroutine;

    void Start()
    {
        // It could be randomized too
        nextPosition = maxLimit.position;
        distanceBetweenLimits = Vector3.Distance(minLimit.position, maxLimit.position);
    }

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
        Vector3 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, nextPosition.x, speed * speedFactor * Time.deltaTime);
        transform.position = pos;
    }

    // Called to change the speed factor, used to move the enemy bar
    public void ChangeSpeedFactorTemporarily(float waitTime, float factor)
    {
        coroutine = WaitAndChangeSpeedFactor(waitTime, factor);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitAndChangeSpeedFactor(float waitTime, float factor)
    {
        // Change the speedfactor to the new factor
        speedFactor = factor;

        // Wait for 'waitTime' seconds
        yield return new WaitForSeconds(waitTime);

        // Return the factor to 1.0 (default)
        speedFactor = 1.0f;
    }
}

