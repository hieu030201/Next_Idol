using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Rooms.Others;

namespace Yun.Scripts.GamePlay.IdleGame.Weapons
{
    public class BulletTakeDame : GameUnit
    {
        private bool isDone;
        private int powerNumber;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<IReceiveProjectile>() != null)
            {
                collision.gameObject.GetComponent<IReceiveProjectile>().TakeDamage(powerNumber);
                if (collision.gameObject.GetComponent<ColorOverlay>())
                {
                    collision.gameObject.GetComponent<ColorOverlay>().VictimHit();
                }
            }
          
            //SoundManager.Ins.PlayOnShot(SoundType.SoundOnHit, VolumDistancePlayer());
            DOVirtual.DelayedCall(1f, () => OnDespawn());
        }

        public void TakeDameNumber(int number)
        {
            powerNumber = number;
        }

        public void OnDespawn()
        {
            SimplePool.Despawn(this);
        }
    }
}