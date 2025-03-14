using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States.BossState_Level_1
{
    public class BossIdleStateLevel1 : EntityState
    {
        public BossIdleStateLevel1(StateMachine stateMachine) : base(stateMachine)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play(Idle);
        }
    }
}
