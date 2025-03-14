using Yun.Scripts.Core;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay
{
    public abstract class Entity : YunBehaviour, IReceiveProjectile
    {
        private int _health;

        public int Health
        {
            get => _health;
            set { _health = value; }
        }

        private float _speed;

        public float Speed
        {
            get => _speed;
            set { _speed = value; }
        }

        public virtual void Move()
        {
        }

        public virtual void Follow()
        {
        }

        public virtual void Attack()
        {
        }

        public virtual void Shoot()
        {
        }

        public virtual void TakeDamage(int damage)
        {
            if (Health - damage < 0)
                Health = 0;
            else
                Health -= damage;
        }

        public virtual void ChangeWeapon()
        {
        }

        public virtual void Reload()
        {
        }
    }
}