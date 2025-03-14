using UnityEngine;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.States
{
    public abstract class StateMachine : YunBehaviour
    {
        private GameObject _entity;

        public GameObject Entity
        {
            get => _entity;
            set => _entity = value;
        }
        
        private GameObject _target;

        public GameObject Target
        {
            get => _target;
            set => _target = value;
        }
        
        protected EntityState _currentState;

        public void ChangeState(EntityState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public abstract void Init(GameObject entity, GameObject target);

        private void Update()
        {
            _currentState?.UpdateLogic();
        }

        private void FixedUpdate()
        {
            _currentState?.UpdatePhysics();
        }

        public abstract void Dead();
    }
}