using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAnim : PowerUp
{
    [SerializeField] float maxSize = 1.1f;
    float startSize;
    float endSize;
    [SerializeField] float speed = 0.02f;
    void Start()
    {
        startSize = transform.localScale.x;
        endSize = startSize * maxSize;
    }
    IEnumerator PlayBounceAnim()
    {
        yield return StartCoroutine(ScaleAnimation(startSize, endSize));
        yield return StartCoroutine(ScaleAnimation(endSize, startSize));
    }

    private IEnumerator ScaleAnimation(float start, float finish)
    {

        float t = 0;
        float currRadius;
        do
        {
            currRadius = Mathf.Lerp(start, finish, t);
            transform.localScale = new Vector3(currRadius, currRadius, 1f);
            t += speed;
            yield return new WaitForEndOfFrame();
        } while (t <= 1f);

        transform.localScale = new Vector3(finish, finish, 1f);

    }
    public override void Collected(Collider2D col)
    {
        FindObjectOfType<AudioManager>().PlaySFX("Bouncer");
        StartCoroutine(PlayBounceAnim());
    }
}
