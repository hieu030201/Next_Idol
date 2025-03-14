using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States
{
    public class EnemyAttackState : EntityState
    {
        public EnemyAttackState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play(Attack);
        }

        protected override void OnAnimationComplete()
        {
            base.OnAnimationComplete();
            _stateMachine.ChangeState(new EnemyChaseState(_stateMachine));
        }
    }
}