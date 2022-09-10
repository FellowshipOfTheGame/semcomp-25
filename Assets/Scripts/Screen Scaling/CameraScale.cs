using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{
    private const float minWidth = 6.75f;
    private void Awake()
    {
        // if on portrait mode
        if (Screen.height > Screen.width)
        {
            float newSize = minWidth * Screen.height / (2 * Screen.width);
            if (Camera.main != null) Camera.main.orthographicSize = newSize;
        }
    }
}
