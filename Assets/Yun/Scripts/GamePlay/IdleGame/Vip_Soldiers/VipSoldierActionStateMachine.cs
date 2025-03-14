using System;
using UnityEngine;
using Yun.Scripts.Cores;
using Random = UnityEngine.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Vip_Soldiers
{
    public class VipSoldierActionStateMachine : YunBehaviour
    {
        [SerializeField] private Animator animator;

        private bool _isWaitingAnimComplete;
        private State _previousState;

        public enum State
        {
            Idle,
            Dig,
            Run,
        }

        private State _actionState;

        public State ActionState
        {
            get => _actionState;
            set
            {
                _previousState = _actionState;
                _actionState = value;
                if (value != State.Idle)
                    StopIdle();
                // Debug.Log("ActionState: " + value + ", " + gameObject.name);
                switch (value)
                {
                    case State.Idle:
                        StartIdle();
                        break;
                    case State.Run:
                        // Debug.Log("animator run");
                        animator.speed = 1;
                        animator.Play("Dog_Run");
                        break;
                    case State.Dig:
                        StopIdle();
                        animator.Play("Dog_Digging");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void StartIdle()
        {
            // Debug.Log("animator play breath");
            animator.Play("Dog_Idle");
            RandomToChangeIdle(1, 5);
        }

        public void StopIdle()
        {
            CancelInvoke(nameof(RandomIdleAction));
        }

        private void RandomToChangeIdle(int fromTime, int toTime)
        {
            if (!gameObject.activeSelf)
                return;
            var randomNumber = Random.Range(fromTime, toTime + 1);

            CancelInvoke(nameof(RandomIdleAction));
            Invoke(nameof(RandomIdleAction), randomNumber);
        }

        private int _changeIdleType;

        private void RandomIdleAction()
        {
            var randomActionNumber = Random.Range(1, 3);
            var fromTime = 3;
            // Debug.Log("randomIdleAction: " + gameObject.activeSelf + ", " + gameObject.name);
            // Debug.Log("animtor play idle");
            _changeIdleType = randomActionNumber;
            switch (randomActionNumber)
            {
                case 1:
                    animator.Play("Dog_Idle_Look");
                    fromTime = 5;
                    break;
                case 2:
                    animator.Play("Dog_Bark");
                    fromTime = 5;
                    break;
                case 3:
                    animator.Play("Dog_Idle_3");
                    fromTime = 5;
                    break;
                case 4:
                    animator.Play("Dog_Idle_4");
                    fromTime = 5;
                    break;
                case 5:
                    animator.Play("Dog_Idle_5");
                    fromTime = 5;
                    break;
            }

            RandomToChangeIdle(fromTime, fromTime + 5);
        }

        protected void Update()
        {
            if (_changeIdleType == 0)
                return;
            var animStateInfo = animator.GetCurrentAnimatorStateInfo(0)
                .normalizedTime;

            /*var clipInfo2 = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo2.Length > 0 && clipInfo2[0].clip.name == "Dog_Idle")
                Debug.Log("update: " + clipInfo2[0].clip.name + ", " + (animStateInfo % 1));*/
            
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length <= 0) return;
            
            if (animStateInfo % 1 >= 0.99f)
            {
                // Lấy thông tin về state hiện tại của layer 0
                // var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                // if (clipInfo.Length <= 0) return;
                switch (clipInfo[0].clip.name)
                {
                    case "Dog_Idle":
                        switch (_changeIdleType)
                        {
                            case 1:
                                animator.Play("Dog_Idle_Look");
                                _changeIdleType = 0;
                                break;
                            case 2:
                                animator.Play("Dog_Bark");
                                _changeIdleType = 0;
                                break;
                            case 3:
                                animator.Play("Dog_Idle_3");
                                _changeIdleType = 0;
                                break;
                            case 4:
                                animator.Play("Dog_Idle_4");
                                _changeIdleType = 0;
                                break;
                            case 5:
                                animator.Play("Dog_Idle_5");
                                _changeIdleType = 0;
                                break;
                        }

                        break;
                }
            }
        }
    }
}