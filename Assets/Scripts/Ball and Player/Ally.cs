using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void Pull()
    {
        anim.Play("Pull");
    }

    public void Kick()
    {
        anim.Play("Kick");
    }

    public void Idle()
    {
        anim.Play("Idle");
    }
}
