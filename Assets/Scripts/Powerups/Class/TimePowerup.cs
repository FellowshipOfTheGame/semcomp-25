using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimePowerup : PowerUp
{
    [SerializeField] float duration;
    [SerializeField] float timeScale;
    [SerializeField] bool hasIceFx;

    [SerializeField] GameObject icePrefab;
    [SerializeField] Sprite sprite;
    public void SpawnIce()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            Vector3 pos = e.transform.position;
            pos.y -= 0.1f;
            GameObject fx=Instantiate(icePrefab,pos, Quaternion.identity, e.transform);
            fx.GetComponent<AnimationManager>().PlayAnimDelayed("FxEnd", duration - 0.5f);
        }
    }
    public override void Collected(Collider2D col)
    {
        Manager.ChangeEnemyTime(duration, timeScale);
        Manager.SetPowerUpHud(sprite, duration);
        if (hasIceFx)
        {
            SpawnIce();
        }
    }
}
