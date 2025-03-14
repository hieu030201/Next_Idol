using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States.BossState_Level_1
{
    public class BossWalkStateLevel1 : EntityState
    {
        public BossWalkStateLevel1(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _stateMachine.Entity.GetComponent<Enemy>().animator.Play("Walk");
        }
    }
}