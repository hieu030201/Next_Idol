using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;
using Yun.Scripts.Managers;

namespace Yun.Scripts.GamePlay.States.BossState_Level_1
{
    public class BossScreamStateLevel1 : EntityState
    {
        public BossScreamStateLevel1(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play("IdleToScream");
        }

        protected override void OnAnimationComplete()
        {
            base.OnAnimationComplete();
            GameManager.Instance.StartKillBoss();
        }
    }
}