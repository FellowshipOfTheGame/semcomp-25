using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void PlayAnim(string anim)
    {
        animator.Play(anim);
    }
    IEnumerator AnimDelay(string a, float t)
    {
        yield return new WaitForSeconds(t);
        animator.Play(a);
    }
    public void PlayAnimDelayed(string anim,float time)
    {
        StartCoroutine(AnimDelay(anim, time));
    }

}
