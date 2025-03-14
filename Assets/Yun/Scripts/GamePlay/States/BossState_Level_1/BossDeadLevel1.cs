using Yun.Scripts.GamePlay.Enemies;
using Yun.Scripts.Managers;

namespace Yun.Scripts.GamePlay.States.BossState_Level_1
{
    public class BossDeadLevel1 : EntityState
    {
        public BossDeadLevel1(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play(Death);
            GameManager.Instance.GameOver();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
            
        }
    }
}
