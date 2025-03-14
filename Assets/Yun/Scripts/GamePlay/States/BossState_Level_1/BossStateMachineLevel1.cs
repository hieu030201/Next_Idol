using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Yun.Scripts.GamePlay.States.BossState_Level_1
{
    public class BossStateMachineLevel1 : StateMachine
    {
        public UnityAction OnAttack;

        public override void Init(GameObject entity, GameObject target)
        {
            Entity = entity;
            Target = target;
            ChangeState(new BossIdleStateLevel1(this));
        }

        public void Walk()
        {
            ChangeState(new BossWalkStateLevel1(this));
        }

        public void Scream()
        {
            ChangeState(new BossScreamStateLevel1(this));
        }

        public void FirstAttack()
        {
            ChangeState(new BossFirstAttackStateLevel1(this));
            var tween = DOVirtual.DelayedCall(0.35f, (Attack));
            AddTweenToTweenManager(tween);
        }

        public override void Dead()
        {
            ChangeState(new BossDeadLevel1(this));
        }

        public void Attack()
        {
            OnAttack();
            var tween = DOVirtual.DelayedCall(4f, (Attack));
        }

        public void Chase()
        {
            ChangeState(new BossChaseStateLevel1(this));
            var tween = DOVirtual.DelayedCall(0.35f, (Attack));
            AddTweenToTweenManager(tween);
        }
    }
}