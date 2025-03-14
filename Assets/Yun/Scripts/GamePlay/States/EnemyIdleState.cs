using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States
{
    public class EnemyIdleState : EntityState
    {
        public EnemyIdleState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play(Idle);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            if (_stateMachine.Target)
            {
                _stateMachine.ChangeState(new EnemyChaseState(_stateMachine));
            }
        }
    }
}