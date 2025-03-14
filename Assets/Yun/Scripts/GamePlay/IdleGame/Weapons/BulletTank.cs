using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Audios;

namespace Yun.Scripts.GamePlay.IdleGame.Weapons
{
    public class BulletTank : GameUnit
    {
        private bool isDone;
        private int powerNumber;

        public void OnCollisionEnter(Collision collision)
        {
            IReceiveProjectile projectileReceiver = collision.gameObject.GetComponent<IReceiveProjectile>();  
            
            if (projectileReceiver != null)  
                projectileReceiver.TakeDamage(powerNumber);  
            
            ParticlePool.Play(ParticleType.HitTank, TF.position);  
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Hit_Tank); 
            OnDespawn();  
        }

        public void SetDame(int bulletDame)
        {
            // Debug.Log("Run in SetDameTank" + bulletDame);
            powerNumber = bulletDame;
        }

        public void OnDespawn()
        {
            SimplePool.Despawn(this);
        }
    }
}