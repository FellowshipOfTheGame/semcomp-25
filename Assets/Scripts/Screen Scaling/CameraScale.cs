using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScale : MonoBehaviour
{
    private Vector2Int resolution;
    private const float MinWidth = 6.75f;
    private const float LandscapeCameraSize = 6f;
    
    public delegate void ResolutionChanged();
    public static event ResolutionChanged OnResolutionChanged;
    
    private void Awake()
    {
        ScaleCamera();
    }
    
    private void Start()
    {
        resolution = new Vector2Int(Screen.width, Screen.height);
    }
 
    private void Update ()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            ScaleCamera();
            
            OnResolutionChanged?.Invoke();

            resolution.x = Screen.width;
            resolution.y = Screen.height;
        }
    }
    
    private void ScaleCamera()
    {
        // if on portrait mode
        if (Screen.height > Screen.width)
        {
            float newSize = MinWidth * Screen.height / (2 * Screen.width);
            if (Camera.main != null) Camera.main.orthographicSize = newSize;
        }
        else
        {
            if (Camera.main != null) Camera.main.orthographicSize = LandscapeCameraSize;
        }
    }
}
