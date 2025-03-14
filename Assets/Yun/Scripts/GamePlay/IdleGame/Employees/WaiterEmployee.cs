using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Employees
{
    public class WaiterEmployee : Employee
    {
        [SerializeField] public GameObject FoodContainer;

        public UnityAction ToFoodCounterComplete;
        public UnityAction ToTableComplete;
        public UnityAction ToStartPointComplete;

        public override void StartWorking()
        {
            base.StartWorking();
        }

        protected override void Start()
        {
            base.Start();
            navMeshAgent.speed = 3;
            navMeshAgent.stoppingDistance = 0;
        }

        public void SetTarget(GameObject t)
        {
            _target = t;
        }

        protected override void FixedUpdate()
        {
            if(!_isWorking)
                return;
            // base.Update();
            if (_target)
            {
                navMeshAgent.SetDestination(_target.transform.position);
            }

            if (navMeshAgent.velocity.magnitude != 0)
            {
                if (Role == EmployeeRole.ToDiningTable)
                    stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.RunBox;
                else
                    stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Walk;
            }

            if (navMeshAgent.pathPending || !(navMeshAgent.remainingDistance <= 0.1f)) return;

            stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Idle;
            if (Role == EmployeeRole.ToFoodCounter)
            {
                if (_target)
                    transform.rotation = _target.transform.rotation;
                _target = null;
                ToFoodCounterComplete();
            }
            else if (Role == EmployeeRole.ToDiningTable)
            {
                if (_target)
                    transform.rotation = _target.transform.rotation;
                _target = null;
                ToTableComplete();
            }
            else if (Role == EmployeeRole.ToDiningStart)
            {
                if (_target)
                    transform.rotation = _target.transform.rotation;
                _target = null;
                ToStartPointComplete();
            }
        }
    }
}