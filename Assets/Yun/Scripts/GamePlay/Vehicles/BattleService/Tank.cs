using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Animations;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Yun.Scripts.GamePlay.IdleGame.Weapons;
using Random = UnityEngine.Random;

namespace Yun.Scripts.GamePlay.Vehicles.BattleService
{
    public class Tank : BattleVehicle
    {
        [SerializeField] private bool isShooting;
        [SerializeField] private Vector3 currentTarget;
        [SerializeField] private GameObject directionTop;
        [SerializeField] private Transform muzzle;

        public Vector3 targetPositionTank;
        private Tween _shootingTankTween;

        private List<WarBaseClient> _targetsList;
        private float _speedShooting;
        private int _numberDamage;

        public float DelayShoot { get; set; }

        protected override void Start()
        {
            
        }


        public void SetTargetsList(List<WarBaseClient> targetsList)
        {
            _targetsList = targetsList;
        }

        public void SetIsShootingTank(bool isShoot)
        {
            isShooting = isShoot;
        }

        public void StartShootingTank(BattleRoom battleRoom, int numberFaction)
        {
            ChooseLocation(battleRoom, numberFaction);
            _shootingTankTween?.Kill();
            _shootingTankTween =
                DOVirtual.DelayedCall(_speedShooting, () => StartShootingTank(battleRoom, numberFaction));
        }

        public void ChooseLocation(BattleRoom battle, int numberFaction)
        {
            if (isShooting)
            {
                var randomNumber = Random.Range(0, _targetsList.Count);
                targetPositionTank = _targetsList[randomNumber].transform.position;
                currentTarget = targetPositionTank;
                StartCoroutine(RotationToTarget());
            }
        }

        private IEnumerator RotationToTarget()
        {
            var direction = (currentTarget - directionTop.transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            var rotationSpeed = 4f;
            while (Quaternion.Angle(directionTop.transform.rotation, lookRotation) >
                   0.1f) // Check if rotation is "close enough"  
            {
                directionTop.transform.rotation = Quaternion.Slerp(directionTop.transform.rotation, lookRotation,
                    Time.deltaTime * rotationSpeed);
                yield return null;
            }

            Shoot();
        }

        private int count;
        private void Shoot()
        {
            _gunRecoil.Fire();
            ParticlePool.Play(ParticleType.MuzzleTank, muzzle.position);
            DOVirtual.DelayedCall(0.05f, () =>
            {
                var bullet = SimplePool.Spawn<BulletTank>(PoolType.BulletTank, targetPositionTank, Quaternion.identity);
                bullet.SetDame(_numberDamage);
            });
        }

        public void StopShoot()
        {
            _shootingTankTween?.Kill();
            isShooting = false;
        }

        public override void GetPropertyForPrefabs(int index)
        {
            foreach (var levelPrefab in levelPrefabs) levelPrefab.SetActive(false);
            var prefab = levelPrefabs[index];
            prefab.SetActive(true);
            _gunRecoil = prefab.transform.Find("Top").Find("Barrel").GetComponent<GunRecoil>();
            directionTop = prefab.transform.Find("Top").gameObject;
            muzzle = directionTop.transform.Find("Barrel").Find("Muzzle").transform;
            var explosePrefab = GetComponent<ExploseAnimation>();
            explosePrefab.tankParts.Clear();
            explosePrefab.tankParts.Add(gameObject);
            foreach (Transform child in prefab.transform) explosePrefab.tankParts.Add(child.gameObject);

            directionTop.transform.rotation = transform.parent.rotation;
            levelPrefabs[index].transform.localScale = Vector3.one * initialScale;
            levelPrefabs[index].transform.DOScale(Vector3.one, durationShow).SetEase(Ease.OutBack);

            ParticlePool.Play(ParticleType.BuyAbility, levelPrefabs[index].transform.position);

            _speedShooting = index switch
            {
                0 => FacilityManager.Instance.BattleVehiclesConfig.TankSpeedShootingLv1,
                1 => FacilityManager.Instance.BattleVehiclesConfig.TankSpeedShootingLv2,
                2 => FacilityManager.Instance.BattleVehiclesConfig.TankSpeedShootingLv3,
                _ => _speedShooting
            };

            _numberDamage = index switch
            {
                0 => FacilityManager.Instance.BattleVehiclesConfig.TankDamageLevel1,
                1 => FacilityManager.Instance.BattleVehiclesConfig.TankDamageLevel2,
                2 => FacilityManager.Instance.BattleVehiclesConfig.TankDamageLevel3,
                _ => _numberDamage
            };

            var room = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
            // Nếu là battle 1 của map 1 thì cho dam của tank = 0 , tránh để battle kết thúc quá nhanh
            if (room.isFirstBattle)
                _numberDamage = 0;
        }

        public override void Show()
        {
            base.Show();
            if (_level == 0)
            {
                _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.TankSpeedShootingLv1;
                _numberDamage = FacilityManager.Instance.BattleVehiclesConfig.TankDamageLevel1;
            }
            
            if (gameObject.transform.Find("Top")) directionTop = transform.Find("Top").gameObject;
            var room = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
            if (room && room.isFirstBattle)
                _numberDamage = 0;
        }

        // [Button]
        // public void IsLose()
        // {
        //     ParticlePool.Play(ParticleType.ExplosionAbility, transform.position);
        //     gameObject.GetComponent<ExploseAnimation>().Explode();
        //     isActive = false;
        // }
    }
}