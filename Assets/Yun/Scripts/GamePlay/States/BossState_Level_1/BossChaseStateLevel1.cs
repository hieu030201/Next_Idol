using DG.Tweening;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States.BossState_Level_1
{
    public class BossChaseStateLevel1 : EntityState
    {
        public BossChaseStateLevel1(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play(Chase);
            // WaitToAttack();
        }

        private Tween _attackTween;
        private void WaitToAttack()
        {
            _attackTween = DOVirtual.DelayedCall(3, () =>
            {
                _stateMachine.ChangeState(new BossAttackStateLevel1(_stateMachine));
            });
            
        }

        public override void Exit()
        {
            base.Exit();
            _attackTween?.Kill();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
    }
}
