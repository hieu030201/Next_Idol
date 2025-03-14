using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Animations;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Random = System.Random;

namespace Yun.Scripts.GamePlay.Vehicles.BattleService
{
    public class Armored : BattleVehicle
    {
        [ShowInInspector] private GameObject _currentTarget;
        private Tween _shootingTween;
        [ShowInInspector] private List<WarBaseClient> _targetList;
        [ShowInInspector] private GameObject _directionTop;
        private float _speedShooting;
        
        private BulletArmored _bulletArmored;

        protected override void Start()
        {
            
        }

        public override void Show()
        {
            base.Show();
            if (_level == 0)
            {
                _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.ArmoredSpeedShootingLv1;
            }

            if (!gameObject.transform.Find("Turret")) return;
            _directionTop = gameObject.transform.Find("Turret").gameObject;
            _bulletArmored = _directionTop.transform.Find("BulletPoint").GetComponent<BulletArmored>();
        }

        private void Shoot()
        {
            if (!_currentTarget)
            {
                _currentTarget = FindRandomTarget();
                var direction = (_currentTarget.transform.position - _directionTop.transform.position).normalized;

                var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
                _directionTop.transform.rotation = lookRotation;
            }

            _bulletArmored.Shoot();
            _gunRecoil.Fire();

            _shootingTween?.Kill();
            var valueSpeed = FacilityManager.Instance.isWeakDevice ? _speedShooting +  (float)(new Random().NextDouble() * 1f) : _speedShooting;
            _shootingTween = DOVirtual.DelayedCall(valueSpeed, Shoot)
                .SetAutoKill(true);
            AddTweenToTweenManager(_shootingTween);

            DOVirtual.DelayedCall(0.5f, () =>
            {
                _currentTarget = FindRandomTarget();
                var direction = (_currentTarget.transform.position - _directionTop.transform.position).normalized;
                var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
                _directionTop.transform.DORotate(lookRotation.eulerAngles, 1).SetAutoKill(true);
            }).SetAutoKill(true);
        }

        public void StartFighting(string bulletLayer)
        {
            _bulletArmored.SetBulletLayer(bulletLayer);
        }


        public void StartShooting()
        {
            _shootingTween?.Kill();
            var randomNumber = FacilityManager.Instance.isWeakDevice ? (float) (new Random().NextDouble() * 1.5f) : (float) (new Random().NextDouble() * 1.2f);
            _shootingTween = DOVirtual.DelayedCall(1f + randomNumber, Shoot);
            AddTweenToTweenManager(_shootingTween);
        }

        public void StopShoot()
        {
            _shootingTween?.Kill();
        }

        public void SetTargetList(List<WarBaseClient> targetList)
        {
            _targetList = targetList;
        }

        private GameObject FindRandomTarget()
        {
            if (_targetList.Count == 0)
                return null;

            var random = new Random();
            // Lấy index ngẫu nhiên
            var x = _targetList.Count;
            var randomIndex = random.Next(0, x);

            return _targetList[randomIndex].gameObject;
        }

        public override void GetPropertyForPrefabs(int index)
        {
            foreach (var levelPrefab in levelPrefabs) levelPrefab.SetActive(false);
            var prefab = levelPrefabs[index];
            _directionTop = prefab.transform.Find("Turret").gameObject;
            _gunRecoil = prefab.transform.Find("Turret").GetComponent<GunRecoil>();
            _bulletArmored = _directionTop.transform.Find("BulletPoint").GetComponent<BulletArmored>();
            var explosePrefab = GetComponent<ExploseAnimation>();
            explosePrefab.tankParts.Clear();
            explosePrefab.tankParts.Add(gameObject);
            foreach (Transform child in prefab.transform) explosePrefab.tankParts.Add(child.gameObject);
            levelPrefabs[index].SetActive(true);
            _directionTop.transform.rotation = transform.parent.rotation;
            levelPrefabs[index].transform.localScale = Vector3.one * initialScale;
            levelPrefabs[index].transform.DOScale(Vector3.one, durationShow).SetEase(Ease.OutBack);

            ParticlePool.Play(ParticleType.BuyAbility, levelPrefabs[index].transform.position);

            switch (index)
            {
                case 0:
                    _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.ArmoredSpeedShootingLv1;
                    break;
                case 1:
                    _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.ArmoredSpeedShootingLv2;
                    break;
                case 2:
                    _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.ArmoredSpeedShootingLv3;
                    break;
                default:
                    _speedShooting = 0;
                    break;
            }
        }
    }
}