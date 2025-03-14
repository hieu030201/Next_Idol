using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States.BossState_Level_1
{
    public class BossAttackStateLevel1 : EntityState
    {
        public BossAttackStateLevel1(StateMachine stateMachine) : base(stateMachine) { }

        private bool _isCalledAttack;
        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play(Attack);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            var animStateInfo = _stateMachine.Entity.GetComponent<Enemy>().animator.GetCurrentAnimatorStateInfo(0)
                .normalizedTime;
            if (animStateInfo > 0.35f && !_isCalledAttack)
            {
                _isCalledAttack = true;
                _stateMachine.GetComponent<BossStateMachineLevel1>().Attack();
            }
            // Nếu Anim đã chạy xong
            if (animStateInfo >= 1f)
            {
                _stateMachine.ChangeState(new BossChaseStateLevel1(_stateMachine));
            }
        }
    }
}