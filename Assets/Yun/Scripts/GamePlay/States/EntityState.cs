using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.States
{
    public abstract class EntityState
    {
        public const string Idle = "Idle";
        public const string Chase = "Chase";
        public const string Attack = "Attack";
        public const string Run = "Run";
        public const string Walk = "Walk";
        public const string Death = "Death";

        protected bool _isActive;

        public bool IsActive
        {
            set
            {
                _isActive = value;
            }
        }
        
        protected StateMachine _stateMachine;

        protected EntityState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void Enter()
        {
            _isAnimationComplete = false;
            IsActive = true;
        }

        private bool _isAnimationComplete;
        public virtual void UpdateLogic()
        {
            if(!_isActive)
                return;
            if (_stateMachine.Entity.GetComponent<Enemy>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_isAnimationComplete)
            {
                _isAnimationComplete = true;
                OnAnimationComplete();
            }
        }

        protected virtual void OnAnimationComplete()
        {
            
        }

        public virtual void UpdatePhysics()
        {
            if(!_isActive)
                return;
        }

        public virtual void Exit()
        {
            IsActive = false;
        }
    }
}
