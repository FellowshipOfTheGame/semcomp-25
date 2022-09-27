using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Portal : PowerUp
{
    [SerializeField] Transform targetPortal;
    [SerializeField] bool fixedAngle = true;
    Vector2 rotated;
    [SerializeField] float speed = 8f;
    private void Start()
    {
    }
    void CalculateTargetRotation()
    {

        Quaternion rot = targetPortal.rotation;
        float angle = Mathf.Deg2Rad * rot.eulerAngles.z;
        rotated = new Vector2(-(float)Math.Round(Mathf.Sin(angle), 2), (float)Math.Round(Mathf.Cos(angle), 2));
    }
    public override void Collected(Collider2D col)
    {
        AudioManager.instance.PlaySFX("Portal");
        CalculateTargetRotation();
        Manager.Teleport(targetPortal.position,rotated*speed);
    }
}
