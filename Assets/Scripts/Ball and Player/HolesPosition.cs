using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolesPosition : MonoBehaviour
{
    [SerializeField] private Transform rightHole;
    [SerializeField] private Transform leftHole;

    private const float RightX = 3.325f;
    private const float LeftX = -3.325f;

    private void LateUpdate()
    {
        rightHole.position = new Vector3(RightX, rightHole.position.y);
        leftHole.position = new Vector3(LeftX, leftHole.position.y);
    }
}
