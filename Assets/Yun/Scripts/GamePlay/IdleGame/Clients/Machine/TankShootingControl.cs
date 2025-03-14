using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Weapons;
using Yun.Scripts.GamePlay.IdleGame.Weapons.Gun;

namespace Yun.Scripts.GamePlay.IdleGame.Clients.Machine
{
    public class TankShootingControl : YunBehaviour
    {
        [SerializeField] private Pistol pistol;
        private Tween _shootingTween;
        private bool _isShoot;
        private bool _isFighting;

        protected override void Start()
        {
            Debug.Log("Run In Tanks");
            Shoot();
        }
        private void Shoot()
        {
            pistol.Shoot();

            _shootingTween?.Kill();
            _shootingTween = DOVirtual.DelayedCall(2.0f, Shoot);
            AddTweenToTweenManager(_shootingTween);
        }
        
        public void StartShooting()
        {

        }
        public void StopFighting()
        {

        }
        public void RemoveTarget(GameObject target)
        {
   
        }
        public void AddTarget(GameObject target)
        {
            
        }
        public void ShowLose()
        {
            
        }
        
        
    }
}
