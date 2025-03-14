using System;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Random = UnityEngine.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Clients
{
    public class ClientActionStateMachine : YunBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private WarBaseClient client;

        private bool _isWaitingAnimComplete;
        private State _previousState;

        public enum State
        {
            Idle,
            Idle2,
            Idle3,
            IdleKhoanhTay,
            Run,
            RunFast,
            Sleep,
            Exercise,
            Eat,
            Sit,
            Transform,
            Boxing,
            WeightLifting,
            WalkAndAimPistol,
            StopAndAimPistol,
            ShootPistol,
            DeadPistol,
            HitPistol,
            HitRifle,
            DeadRifle,
            ShootRifle,
            IdleRiffleRandom,
            IdleRiffle,
            WinRiffle,
            Win,
            Lose,
            Look,
            PushUp,
            Salute,
            WakeUp,
            ThrowBomb,
            InTreating,
            WalkInjured,
            Chopping_Axe,
            Shovel,
        }

        private State _actionState;

        public State ActionState
        {
            get => _actionState;
            set
            {
                // Debug.Log("ActionState: " + value);
                _previousState = _actionState;
                _actionState = value;
                if (value != State.Idle)
                    StopIdle();
                // Debug.Log("ActionState: " + value + ", " + gameObject.name);
                animator.speed = 1;
                switch (value)
                {
                    case State.Idle:
                        StartIdle();
                        break;
                    case State.Run:
                        if (GetComponent<NavMeshAgent>())
                        {
                            // Debug.Log("animator run");
                            var currentPlayerSpeed = GetComponent<NavMeshAgent>().speed;
                            if (currentPlayerSpeed < 2)
                            {
                                animator.speed = currentPlayerSpeed / 1.3f;
                                animator.Play("Walk");
                            }
                            else if (currentPlayerSpeed < 4.5f)
                            {
                                animator.speed = currentPlayerSpeed / 2.5f;
                                animator.Play("Run_Jogging");
                            }
                            else
                            {
                                animator.speed = currentPlayerSpeed / 6;
                                animator.Play("Run_Sprint");
                            }
                        }
                        break;
                    case State.WalkInjured:
                        if (GetComponent<NavMeshAgent>())
                        {
                            // Debug.Log("animator run");
                            var currentPlayerSpeed = GetComponent<NavMeshAgent>().speed;
                            animator.speed = currentPlayerSpeed / 2;
                        }
                        var randomWalk = Random.Range(0, 4);
                        switch (randomWalk)
                        {
                            case 0:
                                animator.Play("Walk_Injured_1");
                                break;
                            case 1:
                                animator.Play("Walk_Injured_2");
                                break;
                            case 2:
                                animator.Play("Walk_Injured_3");
                                break;
                            case 3:
                                animator.Play("Walk_Injured_4");
                                break;
                            default:
                                animator.Play("Walk_Injured_1");
                                break;
                        }
                        break;
                    case State.Idle2:
                        animator.Play("Idle_2");
                        break;
                    case State.Idle3:
                        animator.Play("Idle_3");
                        break;
                    case State.IdleKhoanhTay:
                        animator.Play("Idle_KhoanhTay");
                        break;
                    case State.WakeUp:
                        animator.Play("WakeUp");
                        break;
                    case State.Shovel:
                        animator.Play("Shovel");
                        break;
                    case State.Chopping_Axe:
                        animator.Play("Chopping_Axe");
                        break;
                    case State.Sleep:
                        StopIdle();
                        animator.Play("Go_To_Sleep");
                        break;
                    case State.Exercise:
                        animator.Play("Shooting_Pistol");
                        animator.speed = 0.5f;
                        break;
                    case State.Boxing:
                        animator.Play("Training_Fight1");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.InTreating:
                        animator.Play("Training_Fight1");
                        break;
                    case State.WeightLifting:
                        // animator.Play("WeightLift");
                        animator.Play("Idle_to_WeightLift");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.ShootPistol:
                        animator.Play("Shooting_Pistol");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.ShootRifle:
                        animator.Play("Rifle_Shooting");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.IdleRiffle:
                        animator.Play("Rifle_Idle");
                        break;
                    case State.IdleRiffleRandom:
                        var randomNumber = Random.Range(0, 4);
                        switch (randomNumber)
                        {
                            case 0:
                                animator.Play("Rifle_Idle");
                                break;
                            case 1:
                                animator.Play("Rifle_Idle2");
                                break;
                            case 2:
                                animator.Play("Rifle_Idle3");
                                break;
                            case 3:
                                animator.Play("Rifle_Idle4");
                                break;
                            default:
                                animator.Play("Rifle_Idle");
                                break;
                        }
                        break;
                    case State.WinRiffle:
                        _isWaitingAnimComplete = true;
                        randomNumber = Random.Range(0, 5);
                        switch (randomNumber)
                        {
                            case 0:
                                animator.Play("Rifle_Win");
                                break;
                            case 1:
                                animator.Play("Win_2");
                                break;
                            case 2:
                                animator.Play("Win_3");
                                break;
                            case 3:
                                animator.Play("Win_4");
                                break;
                            case 4:
                                animator.Play("Win_5");
                                break;
                            default:
                                animator.Play("Rifle_Win");
                                break;
                        }
                        // _isWaitingAnimComplete = true;
                        break;
                    case State.WalkAndAimPistol:
                        animator.speed = 0.7f;
                        animator.Play("Run_Pistol");
                        break;
                    case State.StopAndAimPistol:
                        animator.Play("Shooting_Pistol", -1, 0f);
                        animator.speed = 0;
                        break;
                    case State.DeadPistol:
                        animator.Play("Pistol_Dead");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.DeadRifle:
                        var randomDead = Random.Range(0, 7);
                        switch (randomDead)
                        {
                            case 0:
                                animator.Play("Rifle_Dead");
                                break;
                            case 1:
                                animator.Play("Dead_2");
                                break;
                            case 2:
                                animator.Play("Dead_3");
                                break;
                            case 3:
                                animator.Play("Dead_4");
                                break;
                            case 4:
                                animator.Play("Dead_5");
                                break;
                            case 5:
                                animator.Play("Dead_6");
                                break;
                            case 6:
                                animator.Play("Dead_7");
                                break;
                            default:
                                animator.Play("Rifle_Dead");
                                break;
                        }

                        break;
                    case State.Lose:
                        animator.Play("Lose");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.Win:
                        animator.Play("Pistol_Win");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.PushUp:
                        // animator.Play("PushUp");
                        animator.Play("IdleToPushUp");
                        break;
                    case State.Salute:
                        animator.speed = 0.7f;
                        animator.Play("Salute");
                        break;
                    case State.Look:
                        animator.Play("Look");
                        break;
                    case State.HitPistol:
                        animator.Play("Pistol_Hit");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.HitRifle:
                        animator.speed = 0.7f;
                        animator.Play("Rifle_Hit");
                        _isWaitingAnimComplete = true;
                        break;
                    case State.Transform:
                        switch (Type)
                        {
                            case WarBaseClient.ClientType.Captain_A:
                                animator.Play("Captain_Transform");
                                break;
                            case WarBaseClient.ClientType.Iron_Man:
                                animator.Play("Iron_Man_Transform");
                                break;
                            case WarBaseClient.ClientType.Wonder_Woman:
                                animator.Play("Wonder_Women_Transform");
                                break;
                            case WarBaseClient.ClientType.Cyborg_Commando:
                                animator.Play("Cyborg_Transform");
                                break;
                        }

                        _isWaitingAnimComplete = true;
                        break;
                    case State.Eat:
                        animator.Play("Eat");
                        break;
                    case State.Sit:
                        animator.Play("Sit");
                        break;
                    case State.RunFast:
                        animator.Play("Run_Sprint");
                        break;
                    case State.ThrowBomb:
                        animator.Play("Throw_Bomb");
                        animator.speed = 1f;
                        _isWaitingAnimComplete = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private int _countAnim;
        private GameObject _dumbbell;
        private GameObject _roomDumbbell;
        private Action _onActionCompleted;
        private float _animSpeed;

        public void StartLiftingWeight(GameObject dumbbell, GameObject roomDumbbell, Action onCompleted)
        {
            _onActionCompleted = onCompleted;
            _dumbbell = dumbbell;
            _roomDumbbell = roomDumbbell;
            _countAnim = Random.Range(5, 12);
            ActionState = State.WeightLifting;
            _animSpeed = Random.Range(0.7f, 1.1f);
        }

        private Action _onBoxingComplete;

        public void StartBoxing(Action onCompleted)
        {
            _onActionCompleted = onCompleted;
            _countAnim = Random.Range(5, 10);
            ActionState = State.Boxing;
            _animSpeed = Random.Range(0.8f, 1.3f);
        }

        public void ShowUpgradeAnimation()
        {
            var randomNumber = Random.Range(1, 3);
            // Debug.Log("animator play other");
            if (randomNumber == 1)
                animator.Play("Update1");
            else
                animator.Play("Update2");
        }

        private WarBaseClient.ClientType _type;

        public WarBaseClient.ClientType Type
        {
            get => _type;
            set { _type = value; }
        }

        private void StartIdle()
        {
            // Debug.Log("animator play breath");
            animator.Play("Breath");
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

        private void RandomIdleAction()
        {
            var randomActionNumber = Random.Range(1, 4);
            var fromTime = 3;
            // Debug.Log("randomIdleAction: " + gameObject.activeSelf + ", " + gameObject.name);
            // Debug.Log("animtor play idle");
            switch (randomActionNumber)
            {
                case 1:
                    animator.Play("Look");
                    fromTime = 4;
                    break;
                case 2:
                    animator.Play("Idle_2");
                    fromTime = 2;
                    break;
                case 3:
                    animator.Play("Idle_3");
                    fromTime = 11;
                    break;
                case 4:
                    animator.Play("Idle_KhoanhTay");
                    fromTime = 5;
                    break;
            }

            RandomToChangeIdle(fromTime, fromTime + 5);
        }

        protected void Update()
        {
            if (!_isWaitingAnimComplete)
                return;
            var animStateInfo = animator.GetCurrentAnimatorStateInfo(0)
                .normalizedTime;

            /*var clipInfo2 = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo2.Length > 0)
                Debug.Log("update: " + clipInfo2[0].clip.name + ", " + animStateInfo);*/
            if (animStateInfo >= 0.98f)
            {
                var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length <= 0) return;
                switch (clipInfo[0].clip.name)
                {
                    case "Rifle_Win":
                    case "Win_2":
                    case "Win_3":
                    case "Win_4":
                    case "Win_5":
                        _isWaitingAnimComplete = false;
                        ActionState = State.IdleRiffleRandom;
                        break;
                }
            }
            if (animStateInfo >= 0.9f)
            {
                // Lấy thông tin về state hiện tại của layer 0
                var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length <= 0) return;
                switch (clipInfo[0].clip.name)
                {
                    case "Shooting_Pistol":
                        _isWaitingAnimComplete = false;
                        ActionState = State.StopAndAimPistol;
                        break;
                    case "Rifle_Shooting":
                        _isWaitingAnimComplete = false;
                        // ActionState = State.StopAndAimPistol;
                        break;
                    case "Pistol_Dead":
                        _isWaitingAnimComplete = false;
                        animator.speed = 0;
                        break;
                    case "Lose":
                        _isWaitingAnimComplete = false;
                        animator.speed = 0;
                        break;
                    case "Greet":
                        _isWaitingAnimComplete = false;
                        animator.speed = 0;
                        break;
                    case "Pistol_Hit":
                        _isWaitingAnimComplete = false;
                        ActionState = _previousState;
                        break;
                    case "Rifle_Hit":
                        _isWaitingAnimComplete = false;
                        ActionState = _previousState;
                        break;
                    case "Idle_to_WeightLift":
                        _roomDumbbell.SetActive(false);
                        _dumbbell.SetActive(true);
                        animator.speed = _animSpeed;
                        break;
                    case "WeightLift_to_Idle":
                        _roomDumbbell.SetActive(true);
                        _dumbbell.SetActive(false);
                        _isWaitingAnimComplete = false;
                        _onActionCompleted?.Invoke();
                        break;
                    case "WeightLift":
                        if (animStateInfo >= _countAnim)
                        {
                            animator.Play("WeightLift_to_Idle");
                            animator.speed = 1;
                            _roomDumbbell.SetActive(true);
                            _dumbbell.SetActive(false);
                        }

                        break;
                    case "Training_Fight1":
                        if (animStateInfo >= _countAnim)
                        {
                            _isWaitingAnimComplete = false;
                            _onActionCompleted?.Invoke();
                            StartIdle();
                        }

                        break;
                }
            }
        }
    }
}