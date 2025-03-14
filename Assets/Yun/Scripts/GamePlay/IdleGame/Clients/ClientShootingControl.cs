using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.Cores;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms.Others;
using Yun.Scripts.GamePlay.IdleGame.Weapons;
using Yun.Scripts.GamePlay.IdleGame.Weapons.Gun;
using Yun.Scripts.UI;
using Yun.Scripts.Utilities;
using Random = System.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Clients
{
    public class ClientShootingControl : YunBehaviour, IReceiveProjectile
    {
        public static readonly List<int> HpByLevel = new() { 3, 6, 9 };
        [SerializeField] public ClientActionStateMachine stateMachine;
        [SerializeField] private Pistol pistol;
        [SerializeField] private GameObject pistolModel;
        [SerializeField] private GameObject rifleModel;
        [SerializeField] private Transform healthBarPoint;
        [SerializeField] private ParticleSystem rebornEffectPrefab;
        [SerializeField] private GameObject body;
        [SerializeField] private Transform grenadePoint;
        [SerializeField] private GameObject modelClient;
        public HealthBar healthBar;
        
        public EnemyData enemyData;

        private int _countTakeDamage;

        [ShowInInspector] private GameObject _currentTarget;

        private bool _isDead;
        private bool _isFighting;
        
        private NavMeshAgent _navMeshAgent;

        private ParticleSystem _rebornEffect;
        private Tween _rebornTween;
        private Tween _shootingTween;
        [ShowInInspector] private List<WarBaseClient> _targetList;

        private float _nextFireTime;

        [ShowInInspector] public int PosID { get; set; }
        private bool _isParticalShoot;

        public bool IsParticalShoot
        {
            get => _isParticalShoot;
            set {
                _isParticalShoot = value;
                if (!_isParticalShoot)
                {
                    pistolModel.SetActive(false);
                }
                
                ShootInPracticeRoom();
            }
        }

        [field: ShowInInspector]
        public int Hp { get; private set; }

        private int _maxHp;

        [ShowInInspector]
        public bool IsDead
        {
            get => _isDead;
            set
            {
                _isDead = value;
                if (!value) return;
                // reviveTurn--;
                StopFighting();
                GetComponent<Collider>().enabled = false;
                stateMachine.ActionState = ClientActionStateMachine.State.DeadRifle;
                if (healthBar != null)
                {
                    healthBar.OnDestroy();
                }
         
                FacilityManager.Instance.BattleManager.UpdateDead(gameObject);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            pistolModel.SetActive(false);
            rifleModel.SetActive(false);
            // _navMeshAgent = GetComponent<NavMeshAgent>();
            // _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        public void Reset()
        {
            StopFighting();
            pistolModel.SetActive(false);
            rifleModel.SetActive(false);
            _rebornTween?.Kill();
            _shootingTween?.Kill();
            body.SetActive(true);
            if (_rebornEffect)
                _rebornEffect.gameObject.SetActive(false);
            GetComponent<WarBaseClient>().enabled = true;
        }

        public void TakeDamage(int damage)
        {
            if(!FacilityManager.Instance.BattleManager.isRunningBattle)
                return;
            
            Hp -= damage;
         
            if (healthBar != null)
            {
                healthBar.SetAlpha(1);
                healthBar.SetHealth(Hp, _maxHp);
            }
            FacilityManager.Instance.BattleManager.UpdateTakeDamage(gameObject, damage);
            
            if (Hp > 0)
            {
                modelClient.GetComponent<ColorOverlay>().VictimHit();
                if (!_isThrowGrenade)
                {
                    stateMachine.ActionState = ClientActionStateMachine.State.HitRifle;
                }
                return;
            }

            if (IsDead)
                return;
                //Nếu lượng HP của đối tượng đã về 0 hoặc âm nhưng đối tượng chưa chết (IsDead == false), thì dòng này đánh dấu rằng đối tượng đã chết bằng cách gán IsDead = true
            IsDead = true;
        }

        [Button]
        public void TestFighting()
        {
            Shoot();
        }

        public void HideBody()
        {
            body.SetActive(false);
            GetComponent<Collider>().enabled = false;
        }

        public void ShowReborn(string bulletLayer)
        {
            if (!_rebornEffect)
                _rebornEffect = Instantiate(rebornEffectPrefab, transform);
            _rebornEffect.gameObject.SetActive(true);
            _rebornEffect.Play();
            body.SetActive(true);
            _rebornTween = DOVirtual.DelayedCall(1.5f, () =>
            {
                _rebornEffect.gameObject.SetActive(false);
                StartFighting(bulletLayer);
                StartShooting();
            }).SetAutoKill(true);
            // test
        }

        public void UpdateHp(int hp)
        {
            Hp = hp;
            _maxHp = hp;
        }

        public void ShowWin()
        {
            GetComponent<Collider>().enabled = true;
            _rebornTween?.Kill();
            body.SetActive(true);
            if (_rebornEffect)
                _rebornEffect.gameObject.SetActive(false);
            StopFighting();
            // stateMachine.ActionState = ClientActionStateMachine.State.Win;
            var random = new Random();
            var randomNumber = random.NextDouble() * 0.5;
            DOVirtual.DelayedCall((float)randomNumber,
                () => { stateMachine.ActionState = ClientActionStateMachine.State.WinRiffle; }).SetAutoKill(true);
            if (healthBar != null) healthBar.OnDestroy();
        }

        public void SetTargetList(List<WarBaseClient> targetList)
        {
            _targetList = targetList;
        }

        public void ShowIdleRiffle()
        {
            rifleModel.SetActive(true);
            stateMachine.StopIdle();
            stateMachine.ActionState = _isFighting
                ? ClientActionStateMachine.State.IdleRiffle
                : ClientActionStateMachine.State.IdleRiffleRandom;
        }

        public void StartFighting(string bulletLayer)
        {
            _isDead = false;
            GetComponent<Collider>().enabled = true;
            pistol.SetBulletLayer(bulletLayer);
            // agent.speed = 2;
            GetComponent<WarBaseClient>().enabled = false;
            stateMachine.ActionState = ClientActionStateMachine.State.IdleRiffle;
            _isFighting = true;
            rifleModel.SetActive(true);
            ActiveHealthBar();
        }

        public void StartShooting()
        {
            IsDead = false;
            _shootingTween?.Kill();
            var valueSpeedShoot = FacilityManager.Instance.isWeakDevice ? 1.5f + (float)(new Random().NextDouble() * 1f) : 1.5f;
            var randomNumber = (float)(new Random().NextDouble() * valueSpeedShoot);
            _shootingTween = DOVirtual.DelayedCall(0.8f + randomNumber, Shoot);
            AddTweenToTweenManager(_shootingTween);
        }

        public void ShootInPracticeRoom()
        {
            if (_isParticalShoot)
            {
                pistolModel.SetActive(true);
                pistol.Shoot();
                _shootingTween = DOVirtual.DelayedCall(0.5f, ShootInPracticeRoom);
                AddTweenToTweenManager(_shootingTween);
            }
        }

        public void StopFighting()
        {
            StopShoot();
            _isFighting = false;
            _spawnGrenadeTween?.Kill();
        }

        private void Shoot()
        {
            if (_isThrowGrenade) return;
            var randomOffset = new Vector3(
                (UnityEngine.Random.value - 2f) * 0.5f,
                (UnityEngine.Random.value - 2f) * 0.5f,
                (UnityEngine.Random.value - 2f) * 0.5f
            );
            if (!_currentTarget)
            {
                _currentTarget = FindRandomTarget();
                var posTarget = _currentTarget.transform.position;
                var direction =
                    ((UtilitiesFunction.Chance(FacilityManager.Instance.isWeakDevice ? 90 : 80) ? posTarget : posTarget + randomOffset) - transform.position)
                    .normalized;
                var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
                transform.rotation = lookRotation;
            }
            
            pistol.Shoot();
            // stateMachine.ActionState = ClientActionStateMachine.State.ShootPistol;
            stateMachine.ActionState = ClientActionStateMachine.State.ShootRifle;
            _shootingTween?.Kill();
            var valueSpeedShoot = FacilityManager.Instance.isWeakDevice ? 1.5f + (float)(new Random().NextDouble() * 1f) : 1.5f;
            _shootingTween = DOVirtual.DelayedCall(valueSpeedShoot + (float)(new Random().NextDouble() * 0.8f), Shoot)
                .SetAutoKill(true);
            AddTweenToTweenManager(_shootingTween);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (_currentTarget.name != "boss_1")
                {
                    _currentTarget = FindRandomTarget();
                }
                
                if (_currentTarget && transform)
                {
                    var posTarget = _currentTarget.transform.position;
                    var direction = ((UtilitiesFunction.Chance(FacilityManager.Instance.isWeakDevice ? 90 : 80) ? posTarget : posTarget + randomOffset) -
                                     transform.position).normalized;
                    var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
                    transform.DORotate(lookRotation.eulerAngles, 1).SetAutoKill(true);
                }
            }).SetAutoKill(true);
        }

        private void StopShoot()
        {
            _shootingTween?.Kill();
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

        private void ActiveHealthBar()
        {
             healthBar = SimplePool.Spawn<HealthBar>(PoolType.HeathBar);
             healthBar.SetTarget(healthBarPoint);
             healthBar.SetAlpha(0);
        }

        private Tween _spawnGrenadeTween;
        private bool _isThrowGrenade;

        public void SpawnGrenade()
        {
            _isThrowGrenade = true;
            stateMachine.ActionState = ClientActionStateMachine.State.ThrowBomb;
            
            DOVirtual.DelayedCall(1.5f, () =>
            {
                _isThrowGrenade = false;
                var grenade =
                    SimplePool.Spawn<Grenade>(PoolType.Grenade, grenadePoint.position, quaternion.identity);
                grenade.OnInit(_currentTarget.transform);
            });
           
        }

    }
}