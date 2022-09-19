using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnim : MonoBehaviour
{
    public Vector2 offset;
    private Material material;
    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        material.mainTextureOffset = offset;
    }

}
