using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Rooms.Others;

namespace Yun.Scripts.GamePlay.IdleGame.Weapons
{
    public class Grenade : GameUnit
    {
        public Rigidbody body;

        //public float bounceFactor = 0.05f;
        public float bulletSpeed = 6f;
        public bool hit;
        public float m_life = 1.0f;

        private Vector3 _currentTarget;
        [SerializeField] private ParticleSystem grenadeTrail;

        public int PowerNumber { get; set; } = 1;

        private void Update()
        {
            // if (!hit) transform.forward = body.velocity;
            //
            // if (hit)
            // {
            //     m_life -= Time.deltaTime;
            //     if (m_life < 0) SimplePool.Despawn(this);
            // }

            if (grenadeTrail != null)
            {
                grenadeTrail.transform.position = transform.position;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log(other.gameObject.name);
            if (other.gameObject.name == "Plane" || other.gameObject.name == "Floor" ||
                other.gameObject.name == "Wall_Battle" || other.gameObject.name == "clinder_house_A2" ||
                other.gameObject.layer == LayerMask.NameToLayer("FloorMap"))
            {
                var bullet =
                    SimplePool.Spawn<BulletTakeDame>(PoolType.BulletTakeDame, TF.position, Quaternion.identity);
                bullet.TakeDameNumber(PowerNumber);
                ParticlePool.Play(ParticleType.HitGrenade, TF.position);

                SimplePool.Despawn(this);
            }
        }

        public void OnInit(Transform target)
        {
            grenadeTrail = Instantiate(grenadeTrail, transform.position, Quaternion.identity);
            grenadeTrail.transform.SetParent(transform);
            ThrowGrenade(target);
        }

        public void Go(Vector3 speed)
        {
            body.velocity = speed;
        }


        public void ThrowGrenade(Transform pointSpawnBullet)
        {
            float time = 0;
            var hitPoint = GetHitPoint(pointSpawnBullet.position, Vector3.zero, transform.position, bulletSpeed,
                out time);

            var aim = hitPoint - transform.position;
            aim.y = 0;
            var posQ = Random.Range(0.8f, 1.1f);
            var antiGravity = -Physics.gravity.y * time / 2 * posQ;
            var deltaY = (hitPoint.y - gameObject.transform.position.y) / time;

            var arrowSpeed = aim.normalized * bulletSpeed;
            arrowSpeed.y = antiGravity + deltaY;
            Go(arrowSpeed);
        }

        private Vector3 GetHitPoint(Vector3 targetPosition, Vector3 targetSpeed, Vector3 attackerPosition,
            float bulletSpeed, out float time)
        {
            var q = targetPosition - attackerPosition;
            //Ignoring Y for now. Add gravity compensation later, for more simple formula and clean game design around it
            q.y = 0;
            targetSpeed.y = 0;

            //solving quadratic ecuation from t*t(Vx*Vx + Vy*Vy - S*S) + 2*t*(Vx*Qx)(Vy*Qy) + Qx*Qx + Qy*Qy = 0

            var a = Vector3.Dot(targetSpeed, targetSpeed) -
                    bulletSpeed *
                    bulletSpeed; //Dot is basicly (targetSpeed.x * targetSpeed.x) + (targetSpeed.y * targetSpeed.y)
            var b = Vector3.Dot(targetSpeed, q); //Dot is basicly (targetSpeed.x * q.x) + (targetSpeed.y * q.y)
            var c = Vector3.Dot(q, q); //Dot is basicly (q.x * q.x) + (q.y * q.y)

            //Discriminant
            var D = Mathf.Sqrt(b * b - 4 * a * c);

            var t1 = (-b + D) / (2 * a);
            var t2 = (-b - D) / (2 * a);

            //Debug.Log("t1: " + t1 + " t2: " + t2);

            time = Mathf.Max(t1, t2);

            var ret = targetPosition + targetSpeed * time;
            return ret;
        }
    }
}