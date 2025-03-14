using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States
{
    public class DefaultStateMachine : StateMachine
    {
        public override void Init(GameObject entity, GameObject target)
        {
            Entity = entity;
            Target = target;
            ChangeState(new EnemyIdleState(this));
            entity.GetComponent<Enemy>().StartFollow(target.transform);
        }

        public override void Dead()
        {
            ChangeState(new EnemyDeadState(this));
        }
    }
}
