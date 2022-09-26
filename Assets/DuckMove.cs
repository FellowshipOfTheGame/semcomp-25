using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckMove : MonoBehaviour
{
    [SerializeField] Transform t1;
    [SerializeField] Transform t2;
    Transform target;
    bool going=true;
    [SerializeField]float speed = 0.1f;

    void Start()
    {
        target = t1;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if(going)
                target=t2;
            else
                target=t1;
            going = !going;
        }
    }
}
