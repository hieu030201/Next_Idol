using System;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Employees
{
    public class EmployeeActionStateMachine : YunBehaviour
    {
        [SerializeField] private Animator animator;

        public enum EmployeeActionState
        {
            Idle,
            Idle_CanhGac,
            Computer,
            Run,
            RunFast,
            Walk,
            RunBox,
            Sit,
            IdleKhoanhTay,
            FemaleSoldierShowup,
            FemaleSoldierShowupIdle,
        }

        private EmployeeActionState _actionState;

        public EmployeeActionState ActionState
        {
            get => _actionState;
            set
            {
                _actionState = value;
                switch (value)
                {
                    case EmployeeActionState.Idle:
                        if (gameObject.activeSelf)
                            RandomIdleState();
                        break;
                    case EmployeeActionState.IdleKhoanhTay:
                        animator.Play("Idle_KhoanhTay");
                        break;
                    case EmployeeActionState.Run:
                        if (GetComponent<NavMeshAgent>())
                        {
                            if (GetComponent<NavMeshAgent>().speed < 2)
                            {
                                animator.speed = GetComponent<NavMeshAgent>().speed / 1.3f;
                                animator.Play("Walk");
                            }
                            else if (GetComponent<NavMeshAgent>().speed < 4.5f)
                            {
                                animator.speed = GetComponent<NavMeshAgent>().speed / 2.5f;
                                animator.Play("Run_Jogging");
                            }
                            else
                            {
                                animator.speed = GetComponent<NavMeshAgent>().speed / 6;
                                animator.Play("Run_Sprint");
                            }
                        }
                        break;
                    case EmployeeActionState.RunFast:
                        animator.Play("Run_Sprint");
                        break;
                    case EmployeeActionState.Walk:
                        animator.Play("Walk");
                        break;
                    case EmployeeActionState.RunBox:
                        animator.Play("Run_Box");
                        break;
                    case EmployeeActionState.Idle_CanhGac:
                        animator.Play("Idle_CanhGac");
                        break;
                    case EmployeeActionState.Sit:
                        animator.Play("Sit");
                        break;
                    case EmployeeActionState.Computer:
                        animator.Play("Computer");
                        break;
                    case EmployeeActionState.FemaleSoldierShowup:
                        animator.Play("Female_Soldier_Showup");
                        break;
                    case EmployeeActionState.FemaleSoldierShowupIdle:
                        animator.Play("Female_Soldier_Showup_Idle");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void RandomIdleState()
        {
            animator.Play("Breath");
        }
    }
}