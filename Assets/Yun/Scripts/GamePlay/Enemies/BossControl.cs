using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms.Others;
using Yun.Scripts.GamePlay.IdleGame.Weapons.Gun;
using Yun.Scripts.GamePlay.States;
using Yun.Scripts.UI;
using Yun.Scripts.Utilities;
using Random = System.Random;
public class BossControl : YunBehaviour, IReceiveProjectile
{
    public HealthBar healthBar;
    [SerializeField] protected Transform healthBarPoint;
    //[SerializeField] public IdlePlayerActionStateMachine stateMachine;
    //[SerializeField] public Animator animator;
    //[SerializeField] protected GameObject modelClient;
    [SerializeField] protected GameObject enemyRotateBody;
    [SerializeField] protected Pistol pistol;

    #region INDICATOR ENEMY

    public int Hp = 10;  //{ get; private set; }
    protected int _maxHp;
    protected int _damageNumber;
    protected float _speedShooting;
    protected int _missingPercentBullet; 
    protected int _hp;
    protected BossLevelType bossLevel;
    #endregion
    
    [ShowInInspector] protected List<WarBaseClient> _targetList;
    [ShowInInspector] protected GameObject _currentTarget;
    
    protected Tween _shootingTween;
    protected bool _isFighting;
    protected bool _isDead;

    protected override void Awake()
    {
        base.Awake();
    }
    
    protected override void Start()
    {
        base.Start();
        _maxHp = Hp;
    }
    
    //Nhan dame
    public void TakeDamage(int damage)
    {
        Hp -= damage;
         
        if (healthBar != null)
        {
            healthBar.SetAlpha(1);
            healthBar.SetHealth(Hp, _maxHp);
        }
        FacilityManager.Instance.BattleManager.UpdateTakeDamage(gameObject, damage);
        if (Hp > 0)
        {
            //modelClient.GetComponent<ColorOverlay>().VictimHit();
            //stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.HitPistol;
            return;
        }

        if (IsDead)
            return;
        IsDead = true;
    }

    //Nhan danh sach target
    public void SetTargetList(List<WarBaseClient> targetList)
    {
        _targetList = targetList;
    }
    
    //State win
    protected void ShowWin()
    {
        StopFighting();
        // stateMachine.ActionState = ClientActionStateMachine.State.Win;
        // var random = new Random();
        // var randomNumber = random.NextDouble() * 0.5;
        // DOVirtual.DelayedCall((float)randomNumber,
        //     () => { stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.Win; }).SetAutoKill(true);
        if (healthBar != null) healthBar.OnDestroy();
    }

    #region Shoot
    // Goi khi bat dau chien dau
    public void StartFighting(string bulletLayer)
    {
        _isDead = false;
        GetComponent<Collider>().enabled = true;
        pistol.SetBulletLayer(bulletLayer);
        //stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.GatlingIdle;
        _isFighting = true;
        ActiveHealthBar();
    }
    public void StartShooting()
    {
        IsDead = false;
        _shootingTween?.Kill();
        var randomNumber = (float)(new Random().NextDouble() * 0.8f);
        _shootingTween = DOVirtual.DelayedCall(0.2f + randomNumber, Shoot);
        AddTweenToTweenManager(_shootingTween);
    }
    public void Shoot()
    {
        var randomOffset = new Vector3(
            (UnityEngine.Random.value - 2f) * 0.5f,
            (UnityEngine.Random.value - 2f) * 0.5f,
            (UnityEngine.Random.value - 2f) * 0.5f
        );
        if (!_currentTarget)
        {
            _currentTarget = FindRandomTarget();
            var direction = (_currentTarget.transform.position - enemyRotateBody.transform.position).normalized;

            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            enemyRotateBody.transform.rotation = lookRotation;
        }

        
        pistol.Shoot();
        // stateMachine.ActionState = ClientActionStateMachine.State.ShootPistol;
        //stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.HitPistol;
        _shootingTween?.Kill();
        _shootingTween = DOVirtual.DelayedCall(0.5f, Shoot);
        AddTweenToTweenManager(_shootingTween);
        
        DOVirtual.DelayedCall(0.5f, (() =>
        {
            _currentTarget = FindRandomTarget();
     
            var posTarget = _currentTarget.transform.position;
            var direction =
                ((UtilitiesFunction.Chance(100) ? posTarget : posTarget + randomOffset) - transform.position)
                .normalized;
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                //var lookWeaponRotation = Quaternion.LookRotation(new Vector3(0, direction.y, 0));
                //pistol.gameObject.transform.DORotate(lookWeaponRotation.eulerAngles, 1).SetAutoKill(true);
            enemyRotateBody.transform.DORotate(lookRotation.eulerAngles, 1).SetAutoKill(true);
  
        })).SetAutoKill(true);
    }
    
    public void StopFighting()
    {
        StopShoot();
        _isFighting = false;
        SimplePool.Despawn(healthBar);
    }
    
    protected void StopShoot()
    {
        _shootingTween?.Kill();
    }
    #endregion
    
    //Hien thi HealthBar
    public void ActiveHealthBar()
    {
        healthBar = SimplePool.Spawn<HealthBar>(PoolType.HeathBar);
        healthBar.SetTarget(healthBarPoint);
        healthBar.SetAlpha(0);
    }

    protected GameObject FindRandomTarget()
    {
        if (_targetList.Count == 0)
            return null;

        var random = new Random();
        // Lấy index ngẫu nhiên
        var x = _targetList.Count;
        var randomIndex = random.Next(0, x);

        return _targetList[randomIndex].gameObject;
    }
    
    protected bool IsDead
    {
        get => _isDead;
        set
        {
            _isDead = value;
            if (value)
            {
                StopFighting();
                GetComponent<Collider>().enabled = false;
                //stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.DeadPistol;
                if (healthBar != null)
                {
                    healthBar.OnDestroy();
                }
                FacilityManager.Instance.BattleManager.FinishBattle(true);
            }
        }
    }
}
