using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScale : MonoBehaviour
{
    private void OnEnable()
    {
        CameraScale.OnResolutionChanged += ScaleCanvas;
    }

    private void OnDisable()
    {
        CameraScale.OnResolutionChanged -= ScaleCanvas;
    }

    private void Awake()
    {
        ScaleCanvas();
    }

    private void ScaleCanvas()
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
