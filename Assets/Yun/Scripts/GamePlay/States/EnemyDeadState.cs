using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States
{
    public class EnemyDeadState : EntityState
    {
        public EnemyDeadState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play(Death);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            
        }
    }
}
