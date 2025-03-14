using UnityEngine;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.Weapons
{
    public abstract class Weapon : GameUnit
    {
        [SerializeField] protected GameObject projectilePrefab;

        public abstract void Shoot();

        public abstract void StopShoot();
    }
}
