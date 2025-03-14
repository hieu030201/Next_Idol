using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States.BossState_Level_1
{
    public class BossFirstAttackStateLevel1 : EntityState
    {
        public BossFirstAttackStateLevel1(StateMachine stateMachine) : base(stateMachine) { }

        private bool _isCalledAttack;
        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play("RightAttack");
        }
    }
}