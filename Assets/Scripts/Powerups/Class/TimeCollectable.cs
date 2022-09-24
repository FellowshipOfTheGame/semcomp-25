using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Powerups.Class
{
    public class TimeCollectable : PowerUp
    {

        [SerializeField] int time;

        public override void Collected(Collider2D col)
        {
            Manager.AddTime(time);
        }
    }
}