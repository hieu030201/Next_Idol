using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Employees
{
    public class Employee : Person
    {
        [SerializeField] protected EmployeeActionStateMachine stateMachine;
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] public int areaNumber;

        [SerializeField] protected EmployeeActionStateMachine.EmployeeActionState defaultActionState =
            EmployeeActionStateMachine.EmployeeActionState.Idle;

        private ParticleSystem buildWorkerEffect;

        public enum EmployeeRole
        {
            CheckIn,
            BedRoom,
            TransformRoom,
            TrainingRoom,
            DiningRoom,
            ToFoodCounter,
            ToDiningTable,
            ToDiningStart,
            Idle,
        }

        private EmployeeRole _role;

        public EmployeeRole Role
        {
            get => _role;
            set
            {
                _role = value;
                switch (value)
                {
                    case EmployeeRole.CheckIn:
                        break;
                }
            }
        }

        protected float StartSpeed;

        protected override void Awake()
        {
            base.Awake();
            stateMachine.ActionState = defaultActionState;
            if (transform.Find("BuildWorkerEffect"))
            {
                buildWorkerEffect = transform.Find("BuildWorkerEffect").GetComponent<ParticleSystem>();
                buildWorkerEffect.gameObject.SetActive(false);
            }
        }

        public void ShowStartEffect()
        {
            if (buildWorkerEffect)
            {
                buildWorkerEffect.gameObject.SetActive(true);
                buildWorkerEffect.Play();
                DOVirtual.DelayedCall(2, (() => { buildWorkerEffect.gameObject.SetActive(false); })).SetAutoKill(true);
            }
        }

        public void Sit()
        {
            stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Sit;
        }

        public void Computer()
        {
            stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Computer;
        }

        public void ActiveNavmeshAgent()
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }

        public void DeactivateNavmeshAgent()
        {
            GetComponent<NavMeshAgent>().enabled = false;
        }

        public virtual void UpdateSpeed()
        {
            navMeshAgent.speed = FacilityManager.Instance.workerConfig.speed *
                                 FacilityManager.Instance.IdleGameData.WorkerSpeedMultiplier;
            StartSpeed = navMeshAgent.speed;
            if (stateMachine.ActionState == EmployeeActionStateMachine.EmployeeActionState.Run)
                stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
        }
        
        public void UpdateClientSpeed()
        {
            foreach (var client in ClientsList.Where(client => client))
            {
                client.UpdateSpeed(navMeshAgent.speed);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (_isWaiting)
            {
                if (stateMachine.ActionState != EmployeeActionStateMachine.EmployeeActionState.Idle)
                    stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Idle;
                return;
            }

            if (_target)
            {
                navMeshAgent.SetDestination(_target.transform.position);
            }

            if (!navMeshAgent)
                return;
            if (navMeshAgent.velocity.magnitude == 0)
            {
                if (stateMachine.ActionState != EmployeeActionStateMachine.EmployeeActionState.Sit)
                    stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Idle;
            }
            else
            {
                if (stateMachine.ActionState != EmployeeActionStateMachine.EmployeeActionState.Run)
                    stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
            }
        }

        protected bool _isWorking;

        public virtual void StartWorking()
        {
            ActiveNavmeshAgent();
            _isWorking = true;
            UpdateSpeed();
        }
        
        public virtual void StopWorking()
        {
            _isWorking = false;
        }

        protected GameObject _target;

        protected virtual void FollowTarget(GameObject t, float stoppingDistance = 1, float speed = 0)
        {
            GetComponent<NavMeshAgent>().stoppingDistance = stoppingDistance;
            if (speed != 0 && FacilityManager.Instance.IdleGameData.WorkerSpeedMultiplier <= 1)
                GetComponent<NavMeshAgent>().speed = speed;
            else
                GetComponent<NavMeshAgent>().speed = StartSpeed;
            
            if (stateMachine.ActionState == EmployeeActionStateMachine.EmployeeActionState.Run)
                stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
            
            _target = t;
            navMeshAgent.SetDestination(_target.transform.position);
        }

        protected void StopFollow()
        {
            _target = null;
            // _navMeshAgent.speed = 0;
            // _navMeshAgent.destination = _navMeshAgent.transform.position;
        }

        public void PauseWhenStartBattle()
        {
            navMeshAgent.isStopped = true;
        }

        public virtual void ResumeWhenEndBattle()
        {
            navMeshAgent.isStopped = false;
        }

        protected bool _isWaiting;
        private Tween _tweenToWaiting;

        protected void Waiting(int delay)
        {
            _isWaiting = true;
            _tweenToWaiting?.Kill();
            _tweenToWaiting = DOVirtual.DelayedCall(delay, (() => { _isWaiting = false; }));
        }
    }
}