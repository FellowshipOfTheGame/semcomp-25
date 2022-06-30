using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMove : MonoBehaviour
{
    [SerializeField] float max;
    [SerializeField] float min;
    [SerializeField] float speed;
    [SerializeField] private bool going=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            Vector3 pos=transform.position;
        if(going){
            pos.x+=speed * Time.deltaTime;
            transform.position=pos;
            if(Mathf.Abs(pos.x-max)<0.01f)
                going=!going;
        }else{
            pos.x-=speed*Time.deltaTime;
            if(Mathf.Abs(pos.x-min)<0.01f)
                going=!going;
        }
        transform.position=pos;
    }
}
