using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScale : MonoBehaviour
{
    private void Awake()
    {
        if (Screen.height > Screen.width)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 0f;
        }
        else
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
        }
    }
}
