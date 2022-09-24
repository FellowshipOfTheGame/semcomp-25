using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour
{

    public PowerUpInfo data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getTag(){
        return data.Tag;
    }

    public PowerUpInfo getData(){
        return data;
    }

}
