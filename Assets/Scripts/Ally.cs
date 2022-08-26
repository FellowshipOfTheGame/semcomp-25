using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    [SerializeField] Animator anim;
    //[SerializeField] bool selected;

    public void Pull()
    {
        //selected = true;
        anim.Play("Pull");
    }

    public void Kick()
    {
        //selected = false;
        anim.Play("Kick");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
