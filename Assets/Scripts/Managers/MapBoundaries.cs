using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundaries : MonoBehaviour
{
    [SerializeField] private GameObject mapTopCollider;
    [SerializeField] private GameObject mapBottomCollider;

    private void Start()
    {
        mapTopCollider.transform.position = new Vector3(0,
            Camera.main!.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height)).y + 0.12f
        );
        mapBottomCollider.transform.position = new Vector3(0,
            Camera.main!.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0)).y - 0.12f
        );
    }
}
