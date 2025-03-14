using Unity.Mathematics;
using UnityEngine;
using Yun.Scripts.Audios;

namespace Yun.Scripts.GamePlay.IdleGame.Weapons
{
    public class BulletMissile : GameUnit
    {
        public ParticleSystem bulletTrail;
        public Rigidbody body;
        public bool hit;
        public float mLife = 1.0f;
        private int _powerNumber;

        protected override void Start()
        {
            bulletTrail = Instantiate(bulletTrail, transform.position, Quaternion.identity);
            bulletTrail.transform.SetParent(transform);
        }

        private void Update()
        {
            switch (hit)
            {
                case false:
                    transform.forward = body.velocity;
                    break;
                case true:
                {
                    mLife -= Time.deltaTime;
                    if (mLife < 0) SimplePool.Despawn(this);
                    break;
                }
            }

            if (bulletTrail != null)
                bulletTrail.transform.position = transform.position; // Đảm bảo particle theo vị trí viên đạn
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Enemies") && other.gameObject.name != "Plane" &&
                other.gameObject.name != "Floor" && other.gameObject.layer != LayerMask.NameToLayer("FloorMap")) return;
            var position = TF.position;
            var bullet =
                SimplePool.Spawn<BulletTakeDame>(PoolType.BulletTakeDame, position, quaternion.identity);
            bullet.TakeDameNumber(_powerNumber);
            ParticlePool.Play(ParticleType.HitRocket, position);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Poly_explosion_rocket);
            //ParticlePool.Play(ParticleType.HitRocket, TF.position);
            SimplePool.Despawn(this);
        }

        public void SetDameBullet(int bulletDame)
        {
            _powerNumber = bulletDame;
        }

        public void Go(Vector3 speed)
        {
            body.velocity = speed;
        }
    }
}