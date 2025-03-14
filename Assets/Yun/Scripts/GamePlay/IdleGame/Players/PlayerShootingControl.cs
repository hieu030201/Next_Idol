using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Weapons;
using Yun.Scripts.Utilities;
using Random = System.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Players
{
    public class PlayerShootingControl : YunBehaviour, IReceiveProjectile
    {
        [SerializeField] private IdlePlayerActionStateMachine stateMachine;
        [SerializeField] private List<Firearm> _firearms;
        [SerializeField] private List<GameObject> firearmModels;
        [SerializeField] private GameObject pistolModel;
        [SerializeField] private GameObject rifleModel;
        [SerializeField] private GameObject ThompsonModel;
        [SerializeField] private GameObject ShootGunModel;
        [SerializeField] private GameObject GatlingGunModel;
        [SerializeField] private GameObject AkaFunBotModel;
        [SerializeField] private GameObject AkaOrenModel;
        [SerializeField] private GameObject AkaOWAKCXModel;
        [SerializeField] private GameObject AkaPinKiModel;
        [SerializeField] private GameObject AkaWendaModel;
        [SerializeField] private GameObject playerRotateBody;

        private List<WarBaseClient> _targetList = new();
        
        private Tween _shootingTween;
        private CharacterController _characterController;
        private NavMeshAgent _navMeshAgent;
        private int indexGunType;
        private int _damageNumber;
        private float _speedShooting;
        private int _missingPercentBullet; 
        private int _hp;

        private bool _isDead;

        private bool IsDead
        {
            get => _isDead;
            set
            {
                _isDead = value;
                if (value)
                {
                    StopFighting();
                    GetComponent<Collider>().enabled = false;
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.DeadPistol;
                    FacilityManager.Instance.BattleManager.UpdateDead(gameObject);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.enabled = false;
        }

        private string _nameModelCurrent;
  
        protected override void Start()
        {
            base.Start();
            ResetActiveFirearm();
            OnInit();
            rifleModel.SetActive(false);
            _characterController = GetComponent<CharacterController>();
            _characterController.enabled = true;
        }
        public void OnInit()
        {
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;
            
            if (data.isActiveRetal)
                _nameModelCurrent = FacilityManager.Instance.SkinDataCollection.GetNameItem(data.idSkinRented);
            else
                _nameModelCurrent = FacilityManager.Instance.SkinDataCollection.GetNameItem(data.idSkinSelected);
            ShowFirearm();
        }

        public void ResetData()
        {
            // Debug.Log("RESET DATA =====================================>");
            _currentTarget = null;
        }

        public void SetTargetList(List<WarBaseClient> targetList)
        {
            _targetList = targetList;
        }
        public void SetBossTarget(BossControl boss)
        {
            _currentTarget = boss.gameObject;
        }

        public void StartFighting(string bulletLayer)
        {
            IsDead = false;
            _nameModelCurrent = FacilityManager.Instance.SkinDataCollection.GetNameItem(FacilityManager.Instance.GameSaveLoad.StableGameData
                .idSkinSelected);
            var index = 0;
            Debug.Log("_nameModelCurrent:" + _nameModelCurrent);
            if (_nameModelCurrent != null)
            {
                index = FacilityManager.Instance.WeaponDataCollection.GetIdByName(_nameModelCurrent);
            }
            
            var configIndex = FacilityManager.Instance.WeaponDataCollection[index];
            var defaultConfig = FacilityManager.Instance.WeaponDataCollection[0];
            switch (_nameModelCurrent)
            { 
                case "Terminator":
                    indexGunType = (int)GunType.shootGun;
                    _missingPercentBullet = configIndex.dontMissingPercent;
                    _speedShooting = configIndex.speedShooting + (float) (new Random().NextDouble() * 0.8f);
                    _damageNumber = configIndex.damageNumber;
                    break;
                case "Mafia":
                    indexGunType = (int)GunType.thompson;;
                    _missingPercentBullet = configIndex.dontMissingPercent;;
                    _speedShooting = configIndex.speedShooting;
                    _damageNumber = configIndex.damageNumber;
                    break;
                case "Rambo":
                    indexGunType = (int)GunType.gatlingGun;
                    _missingPercentBullet = configIndex.dontMissingPercent;
                    _speedShooting = configIndex.speedShooting;
                    _damageNumber = configIndex.damageNumber;
                    break;
                case "Sprunki_FunBot":
                    AkaFunBotModel.SetActive(true);
                    indexGunType = 4;
                    _missingPercentBullet = defaultConfig.dontMissingPercent;
                    _speedShooting = defaultConfig.speedShooting + (float) (new Random().NextDouble() * 0.8f);
                    _damageNumber = defaultConfig.damageNumber;
                    break;
                case "Sprunki_Oren":
                    AkaOrenModel.SetActive(true);
                    indexGunType = 5;
                    _missingPercentBullet = defaultConfig.dontMissingPercent;
                    _speedShooting = defaultConfig.speedShooting + (float) (new Random().NextDouble() * 0.8f);
                    _damageNumber = defaultConfig.damageNumber;
                    break;
                case "Sprunki_OWAKCX":
                    AkaOWAKCXModel.SetActive(true);
                    indexGunType = 6;
                    _missingPercentBullet = defaultConfig.dontMissingPercent;
                    _speedShooting = defaultConfig.speedShooting + (float) (new Random().NextDouble() * 0.8f);
                    _damageNumber = defaultConfig.damageNumber;
                    break;
                case "Sprunki_Pinki":
                    AkaPinKiModel.SetActive(true);
                    indexGunType = 7;
                    _missingPercentBullet = defaultConfig.dontMissingPercent;
                    _speedShooting = defaultConfig.speedShooting + (float) (new Random().NextDouble() * 0.8f);
                    _damageNumber = defaultConfig.damageNumber;
                    break;
                case "Sprunki_Wenda":
                    AkaWendaModel.SetActive(true);
                    indexGunType = 8;
                    _missingPercentBullet = defaultConfig.dontMissingPercent;
                    _speedShooting = defaultConfig.speedShooting + (float) (new Random().NextDouble() * 0.8f);
                    _damageNumber = defaultConfig.damageNumber;
                    break;
                default:
                    pistolModel.SetActive(true);
                    indexGunType = (int)GunType.pistol;
                    _missingPercentBullet = defaultConfig.dontMissingPercent;
                    _speedShooting = defaultConfig.speedShooting + (float) (new Random().NextDouble() * 0.8f);
                    _damageNumber = defaultConfig.damageNumber;
                    break;
            }
            _firearms[indexGunType].SetBulletLayer(bulletLayer);
            _firearms[indexGunType].SetDamageNumber(_damageNumber);
            firearmModels[indexGunType].SetActive(true);
        }

        private Tween _delayTweenToStartBattle;

        public void StopFighting()
        {
            StopShoot();
            DOVirtual.DelayedCall(1.5f, () =>
            {
                if (indexGunType == 0 || indexGunType == 4 || indexGunType == 5 || indexGunType == 6 || indexGunType == 7 || indexGunType == 8)
                {
                    firearmModels[indexGunType].SetActive(false);
                }
            });
        }

        public void StartShooting()
        {
            _shootingTween?.Kill();
            var randomNumber = (float) (new Random().NextDouble() * 0.8f);
            _shootingTween = DOVirtual.DelayedCall(0.2f + randomNumber, Shoot);
            AddTweenToTweenManager(_shootingTween);
            Shoot();
        }

        public void UpdateHp(int hp)
        {
            _hp = hp;
        }

        private GameObject _currentTarget;

        private GameObject FindRandomTarget()
        {
            if (_targetList.Count == 0)
                return null;
            var random = new Random();
            // Lấy index ngẫu nhiên
            var randomIndex = random.Next(0, _targetList.Count);

            return _targetList[randomIndex].gameObject;
        }

        private void StopShoot()
        {
            _shootingTween?.Kill();
        }
        
        private void Shoot()
        {
            var randomOffset = new Vector3(
                (UnityEngine.Random.value - 1.5f) * 0.5f,
                (UnityEngine.Random.value - 0.5f) * 0.5f,
                (UnityEngine.Random.value - 1.5f) * 0.5f
            );

            if (!_currentTarget)
            {
                _currentTarget = FindRandomTarget();
                if (_currentTarget.GetComponent<ClientShootingControl>() &&
                    !_currentTarget.GetComponent<ClientShootingControl>().IsDead)
                {
                    playerRotateBody.transform.LookAt(_currentTarget.transform);
                }
            }

            _firearms[indexGunType].Shoot();
            switch (_nameModelCurrent)
            {
                case "Player":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShootPistol;
                    break;
                case "Rambo":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.GatlingShooting;
                    break;
                case "Mafia":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ThompsonShooting;
                    break;
                case "Terminator":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShotgunShooting;
                    break;
                case "Sprunki_FunBot":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.SprunkyShooting;
                    break;
                case "Sprunki_Oren":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.SprunkyShooting;
                    break;
                case "Sprunki_OWAKCX":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.SprunkyShooting;
                    break;
                case "Sprunki_Pinki":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.SprunkyShooting;
                    break;
                case "Sprunki_Wenda":
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.SprunkyShooting;
                    break;
                default:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShootPistol;
                    break;
            }
       
            _shootingTween?.Kill();
            _shootingTween = DOVirtual.DelayedCall(_speedShooting, Shoot);
            AddTweenToTweenManager(_shootingTween);

            DOVirtual.DelayedCall(0.5f, (() =>
            {
                if (_currentTarget.name != "boss_1")
                {
                    _currentTarget = FindRandomTarget();
                }
                
                if (_currentTarget && transform)
                {
                    var posTarget = _currentTarget.transform.position;
                    var direction =
                        ((UtilitiesFunction.Chance(_missingPercentBullet) ? posTarget : posTarget + randomOffset) - transform.position)
                        .normalized;
                    // Tạo rotation mới hướng về mục tiêu
                    var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
                    playerRotateBody.transform.DORotate(lookRotation.eulerAngles, 1).SetAutoKill(true);
                }
            })).SetAutoKill(true);
        }

        public void TakeDamage(int damage)
        {
            // _hp--;
            // if (_hp > 0)
            // {
                stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.HitPistol;
                return;
            // }

            if (IsDead)
                return;
            IsDead = true;
        }
        [Button]
        public void HideFirearm()
        {
            ResetActiveFirearm();
        }
        
        [Button]
        public void ShowFirearm()
        {
            switch (_nameModelCurrent)
            {
                case "Player":
                        ResetActiveFirearm();
                    break;
                case "Rambo":
                    if (GatlingGunModel != null)
                    {
                        ResetActiveFirearm();
                        GatlingGunModel.SetActive(true);
                    }
                    break;
                case "Mafia":
                    if (ThompsonModel != null)
                    {
                        ResetActiveFirearm();
                        ThompsonModel.SetActive(true);
                    }
                    break;
                case "Terminator":
                    if (ShootGunModel != null)
                    {
                        ResetActiveFirearm();
                        ShootGunModel.SetActive(true);
                    }
                    break;
                default:
                    ResetActiveFirearm();
                    //stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.Idle;
                    break;
            }
            stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.Idle;
        }

        private void ResetActiveFirearm()
        {
            foreach (var model in firearmModels)
            {
                model.SetActive(false);
            }
        }
    }
}