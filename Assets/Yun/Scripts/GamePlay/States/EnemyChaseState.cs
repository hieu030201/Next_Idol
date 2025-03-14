using Yun.Scripts.GamePlay.Enemies;
using Yun.Scripts.Managers;

namespace Yun.Scripts.GamePlay.States
{
    public class EnemyChaseState : EntityState
    {
        public EnemyChaseState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play(Chase);
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            if (!_stateMachine.Target) return;
            if (_stateMachine.Entity.GetComponent<Enemy>().IsInAttackRange)
            {
                _stateMachine.ChangeState(new EnemyAttackState(_stateMachine));
                GameManager.Instance.OnEnemyAttack(_stateMachine.Entity.GetComponent<Enemy>());
            }
        }
    }
}
