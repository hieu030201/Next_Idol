using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Animations;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Yun.Scripts.GamePlay.IdleGame.Weapons;
using Random = UnityEngine.Random;

namespace Yun.Scripts.GamePlay.Vehicles.BattleService
{
    public class Missile : BattleVehicle
    {
        public float attackDelay = 2;
        public float bulletSpeed = 10.0f;

        [FormerlySerializedAs("powerDame")] [SerializeField]
        private int powerDamage;

        [SerializeField] private List<Transform> pointSpawnBullet;

        public bool canHit;
        public bool canRotate;
        [ShowInInspector] private Vector3 _currentTarget;

        private bool _isShooting;
        private Tween _shootingMissileTween;

        [ShowInInspector] private List<WarBaseClient> _targetsList;
        private int _bulletNumber;
        [ShowInInspector] private GameObject _directionTop;
        private float _speedShooting;

        private int _turn;

        public int PowerDamage
        {
            get => powerDamage;
            set => powerDamage = value;
        }

        public float DelayShoot { get; set; }

        // Start is called before the first frame update

        protected override void Start()
        {
            if (!gameObject.transform.Find("Missle")) return;
            _directionTop = gameObject.transform.Find("Missle").gameObject;
            foreach (Transform child in _directionTop.transform) pointSpawnBullet.Add(child.gameObject.transform);
        }

        public override void Show()
        {
            base.Show();
            if (_level == 0)
            {
                _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.MissileSpeedShootingLv1;
                powerDamage = FacilityManager.Instance.BattleVehiclesConfig.MissileDamageLevel1;
                _bulletNumber = FacilityManager.Instance.BattleVehiclesConfig.MisslileBulletNumberLv1;
            }
        }

        private int NumberTurnOnMissile()
        {
            return pointSpawnBullet.Count / _bulletNumber;
        }

        private void ReloadBullet()
        {
            for (var i = 0; i < pointSpawnBullet.Count; i++)
            {
                var index = i; // Lưu giá trị index để dùng trong callback  
                DOVirtual.DelayedCall(i * 0.3f, () => { pointSpawnBullet[index].gameObject.SetActive(true); });
            }
        }

        public void StartShootingMissile(BattleRoom battleRoom, int numberFaction)
        {
            ChooseLocation(battleRoom, numberFaction);
            _shootingMissileTween?.Kill();
            _shootingMissileTween =
                DOVirtual.DelayedCall(_speedShooting, () => { StartShootingMissile(battleRoom, numberFaction); });
        }

        public void ChooseLocation(BattleRoom battle, int numberFaction)
        {
            if (_isShooting)
            {
                // _currentTarget = ExplosionZone(numberFaction);
                var randomNumber = Random.Range(0, _targetsList.Count);
                _currentTarget = _targetsList[randomNumber].transform.position;
                StartCoroutine(RotationToTarget());
            }
        }

        public void SetTargetsList(List<WarBaseClient> targetsList)
        {
            _targetsList = targetsList;
        }

        private IEnumerator RotationToTarget()
        {
            {
                var direction = (_currentTarget - _directionTop.transform.position).normalized;
                var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

                var rotationSpeed = 4f;
                while (Quaternion.Angle(_directionTop.transform.rotation, lookRotation) >
                       0.1f) // Check if rotation is "close enough"  
                {
                    _directionTop.transform.rotation = Quaternion.Slerp(_directionTop.transform.rotation, lookRotation,
                        Time.deltaTime * rotationSpeed);
                    yield return null;
                }

                Shoot();
            }
        }

        private void Shoot()
        {
            _turn++;
            if (_turn % 2 == 1)
            {
                for (var i = 0; i < pointSpawnBullet.Count / NumberTurnOnMissile(); i++)
                {
                    var index = i; // Nắm giữ giá trị của i  
                    DOVirtual.DelayedCall(0.6f * i, () =>
                    {
                        SpawnBullet(index);
                        pointSpawnBullet[index].gameObject.SetActive(false);
                    });
                }
            }
            else
            {
                for (var i = pointSpawnBullet.Count; i >= pointSpawnBullet.Count / NumberTurnOnMissile(); i--)
                {
                    var index = i; // Nắm giữ giá trị của i  
                    DOVirtual.DelayedCall(0.3f * i, () =>
                    {
                        SpawnBullet(index);
                        pointSpawnBullet[index].gameObject.SetActive(false);
                    });
                }

                DOVirtual.DelayedCall(2f, ReloadBullet);
            }
        }

        public void SpawnBullet(int index)
        {
            var bullet = SimplePool.Spawn<BulletMissile>(PoolType.BulletMissile, pointSpawnBullet[index].position,
                Quaternion.identity);
            _gunRecoil.Fire();
            bullet.SetDameBullet(3);
            ParticlePool.Play(ParticleType.MuzzleMissile, pointSpawnBullet[index].transform.position);
            float time;
            var position = transform.position;
            var hitPoint = GetHitPoint(_currentTarget, Vector3.zero, position, bulletSpeed, out time);
            _currentTarget += new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1f, 1.5f));

            var aim = hitPoint - position;
            aim.y = 0;

            var antiGravity = -Physics.gravity.y * time / 2;
            var deltaY = (hitPoint.y - bullet.transform.position.y) / time;

            var arrowSpeed = aim.normalized * bulletSpeed;
            arrowSpeed.y = antiGravity + deltaY;

            bullet.GetComponent<BulletMissile>().Go(arrowSpeed);
        }

        public void SetIsShooting(bool isShooting)
        {
            _isShooting = isShooting;
        }

        //Keep arrow bulletSpeed > targetSpeed or hit position might not exist!
        private Vector3 GetHitPoint(Vector3 targetPosition, Vector3 targetSpeed, Vector3 attackerPosition,
            float bulletSpeedNumber,
            out float time)
        {
            var q = targetPosition - attackerPosition;
            //Ignoring Y for now. Add gravity compensation later, for more simple formula and clean game design around it
            q.y = 0;
            targetSpeed.y = 0;

            //solving quadratic ecuation from t*t(Vx*Vx + Vy*Vy - S*S) + 2*t*(Vx*Qx)(Vy*Qy) + Qx*Qx + Qy*Qy = 0

            var a = Vector3.Dot(targetSpeed, targetSpeed) -
                    bulletSpeedNumber *
                    bulletSpeedNumber; //Dot is basicly (targetSpeed.x * targetSpeed.x) + (targetSpeed.y * targetSpeed.y)
            var b = Vector3.Dot(targetSpeed, q); //Dot is basicly (targetSpeed.x * q.x) + (targetSpeed.y * q.y)
            var c = Vector3.Dot(q, q); //Dot is basicly (q.x * q.x) + (q.y * q.y)

            //Discriminant
            var d = Mathf.Sqrt(b * b - 4 * a * c);

            var t1 = (-b + d) / (2 * a);
            var t2 = (-b - d) / (2 * a);

            //Debug.Log("t1: " + t1 + " t2: " + t2);

            time = Mathf.Max(t1, t2);

            var ret = targetPosition + targetSpeed * time;
            return ret;
        }

        public override void GetPropertyForPrefabs(int index)
        {
            foreach (var levelPrefab in levelPrefabs) levelPrefab.SetActive(false);
            var prefab = levelPrefabs[index];
            _directionTop = prefab.transform.Find("Missle").gameObject;
            _gunRecoil = prefab.transform.Find("Missle").GetComponent<GunRecoil>();
            pointSpawnBullet.Clear();
            // Debug.Log("Clear:" + pointSpawnBullet.Count);
            // if (_level != 0)
            // {
                foreach (Transform child in _directionTop.transform) pointSpawnBullet.Add(child.gameObject.transform);
            // }

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
                    powerDamage = FacilityManager.Instance.BattleVehiclesConfig.MissileDamageLevel1;
                    _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.MissileSpeedShootingLv1;
                    _bulletNumber = FacilityManager.Instance.BattleVehiclesConfig.MisslileBulletNumberLv1;
                    break;
                case 1:
                    powerDamage = FacilityManager.Instance.BattleVehiclesConfig.MissileDamageLevel2;
                    _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.MissileSpeedShootingLv2;
                    _bulletNumber = FacilityManager.Instance.BattleVehiclesConfig.MisslileBulletNumberLv2;
                    break;
                case 2:
                    powerDamage = FacilityManager.Instance.BattleVehiclesConfig.MissileDamageLevel3;
                    _speedShooting = FacilityManager.Instance.BattleVehiclesConfig.MissileSpeedShootingLv3;
                    _bulletNumber = FacilityManager.Instance.BattleVehiclesConfig.MisslileBulletNumberLv3;
                    break;
                default:
                    powerDamage = 0;
                    break;
            }
        }
    }
}