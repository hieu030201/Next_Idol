using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.GamePlay.States;
using Yun.Scripts.GamePlay.States.BossState_Level_1;
using Yun.Scripts.Managers;
using Yun.Scripts.UI;

namespace Yun.Scripts.GamePlay.Enemies
{
    public class Enemy : Entity
    {
        [SerializeField] private int healthNumber;
        [SerializeField] private int attackPowerNumber;
        [SerializeField] private float delayDestroy = 3;

        [SerializeField] private GameObject approachRadius;
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private GameObject healthBarPosition;
        [SerializeField] public Animator animator;
        [SerializeField] public StateMachine StateMachine;
        [SerializeField] private bool isBoss;
        [SerializeField] private GameObject explosion;

        private NavMeshAgent _navMeshAgent;
        private Transform _target;
        private GameObject _healthBar;
        private RectTransform _canvasRect;
        private int _totalHealth;
        private float _attackRange;

        private int _attackPower;

        public int AttackPower
        {
            get => _attackPower;
        }

        protected override void Awake()
        {
            base.Awake();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            if (_navMeshAgent)
            {
                _navMeshAgent.stoppingDistance = approachRadius.transform.localScale.x / 2 + _navMeshAgent.baseOffset;
                _navMeshAgent.stoppingDistance = approachRadius.transform.localScale.x / 2 + _navMeshAgent.baseOffset;
                _attackRange = _navMeshAgent.stoppingDistance * 1.1f;
            }

            approachRadius.SetActive(false);
            healthBarPosition.SetActive(false);
            IsBoss = isBoss;
            if(!isBoss)
                explosion.SetActive(false);
            Health = healthNumber;
            _totalHealth = healthNumber;
            _attackPower = attackPowerNumber;
        }

        private void Update()
        {
            if (_healthBar)
            {
                Vector2 viewportPosition = MainCamera.WorldToViewportPoint(healthBarPosition.transform.position);

                var sizeDelta = _canvasRect.sizeDelta;
                var screenPoint = new Vector2(
                    ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                    ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));
                _healthBar.transform.localPosition = screenPoint;
            }

            if (_isFollow && _target && !_isNoFollowTarget && _navMeshAgent)
            {
                _navMeshAgent.SetDestination(_target.position);
            }

            if (!GameManager.Instance.boss)
            {
                StopFollow();
                return;
            }

            if (GameManager.Instance.boss.GetComponent<Enemy>().isDead)
                Dead();
            if (!GameManager.Instance.player)
            {
                StopFollow();
                animator.Play(EntityState.Idle);
            }
        }

        public bool IsBoss { get; set; }

        public bool IsInAttackRange
        {
            get
            {
                if (!_navMeshAgent)
                    return false;
                if (_navMeshAgent.remainingDistance <= _attackRange)
                    return true;
                return false;
            }
        }

        private bool _isFollow;

        public void StartFollow(Transform target)
        {
            _target = target;
            _isFollow = true;
        }

        public void StopFollow()
        {
            _isFollow = false;
            if (!_navMeshAgent)
                return;
            _navMeshAgent.speed = 0;
            _navMeshAgent.destination = _navMeshAgent.transform.position;
        }

        private bool _isNoFollowTarget;

        public void Active(Transform target, bool isNoFollowTarget = false)
        {
            _target = target;
            _isNoFollowTarget = isNoFollowTarget;
            StateMachine.Init(gameObject, _target.gameObject);
            _healthBar = Instantiate(healthBarPrefab, CanvasManager.Instance.overlayUILayer.transform);
            _healthBar.SetActive(false);
            _canvasRect = CanvasManager.Instance.canvas.GetComponent<RectTransform>();
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            // if (isBoss)
            //     CanvasManager.Instance.ShowTakeDamageText(damage, transform.position);
            if (Health <= 0)
            {
                Dead();
            }
            else
            {
                _healthBar.SetActive(true);
                _healthBar.GetComponent<HealthBar>().Health = (float) Health / _totalHealth;
            }
        }

        public bool isDead;

        private void Dead()
        {
            isDead = true;
            StopFollow();
            GetComponent<Collider>().enabled = false;
            _healthBar.SetActive(false);
            
            if (isBoss)
            {
                StateMachine.Dead();
            }
            else
            {
                animator.gameObject.SetActive(false);
                explosion.SetActive(true);
                explosion.GetComponent<ParticleSystem>().Play();
            }

            var tween = DOVirtual.DelayedCall(delayDestroy, (() =>
            {
                if (gameObject)
                    Destroy(gameObject);
            }));
            AddTweenToTweenManager(tween);
        }
    }
}