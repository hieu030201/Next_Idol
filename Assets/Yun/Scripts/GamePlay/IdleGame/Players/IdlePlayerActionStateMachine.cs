using System;
using UnityEngine;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Players
{
    public class IdlePlayerActionStateMachine : YunBehaviour
    {
        [SerializeField] public Animator animator;
        [SerializeField] public Animator animatorSpunky;
        public enum PlayerActionState
        {
            Idle,
            Run,
            RunFast,
            WalkAndAimPistol,
            StopAndAimPistol,
            ShootPistol,
            DeadPistol,
            HitPistol,
            Win,
            Lose,
            DriveTank,
            ShowUpTerminal,
            ShowUpRampo,
            ShowUpMafia,
            GatlingShooting,
            ShotgunShooting,
            ThompsonShooting,
            StopAndAimShootGun,
            StopAndAimThompson,
            StopAndAimGatlingGun,
            DriveSleigh,
            SprunkyShooting,
            StopAndAimSprunky,
            ShowUpSprunky,
        }

        private PlayerActionState _actionState;

        public PlayerActionState ActionState
        {
            get => _actionState;
            set
            {
                _previousState = _actionState;
                _actionState = value;
                animator.speed = 1;
                animatorSpunky.speed = 1;
                // Debug.Log("ActionState: " + value + ", " + gameObject.name);
                switch (value)
                {
                    case PlayerActionState.Idle:
                        _isWaitingAnimComplete = false;
                        RandomIdleState();
                        break;
                    case PlayerActionState.DriveTank:
                        if (FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected < 5)
                            animator.Play("Drive_Tank");
                        else
                            animatorSpunky.Play("Sprunky_Drive_Tank");
                        break;
                    case PlayerActionState.DriveSleigh:
                        animator.Play("Booster_Sleight_Move");
                        break;
                    case PlayerActionState.Run:
                        // Debug.Log("Start Run: " + animator.speed);
                        SetAnimRunType();
                        break;
                    case PlayerActionState.ShootPistol:
                        animator.Play("Shooting_Pistol");
                        _isWaitingAnimComplete = true;
                        break;
                    case PlayerActionState.DeadPistol:
                        animator.Play("Pistol_Dead");
                        _isWaitingAnimComplete = true;
                        break;
                    case PlayerActionState.Lose:
                        if (FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected < 5)
                            animator.Play("Lose");
                        else
                            animatorSpunky.Play("Sprunky_Lose");
                        _isWaitingAnimComplete = true;
                        break;
                    case PlayerActionState.Win:
                        if (FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected < 5)
                        {
                            animator.Play("Greet");
                        }
                        else
                            animatorSpunky.Play("Sprunky_Win");
                        _isWaitingAnimComplete = true;
                        break;
                    case PlayerActionState.HitPistol:
                        animator.Play("Pistol_Hit");
                        _isWaitingAnimComplete = true;
                        break;
                    case PlayerActionState.WalkAndAimPistol:
                        animator.speed = 0.7f;
                        animator.Play("Run_Pistol");
                        break;
                    case PlayerActionState.StopAndAimPistol:
                        animator.Play("Shooting_Pistol", -1, 0f);
                        animator.speed = 0;
                        break;
                    case PlayerActionState.StopAndAimShootGun:
                        animator.Play("Shotgun_Shooting", -1, 0f);
                        animator.speed = 0;
                        break;
                    case PlayerActionState.StopAndAimThompson:
                        animator.Play("Thompson_Shooting", -1, 0f);
                        animator.speed = 0;
                        break;
                    case PlayerActionState.StopAndAimGatlingGun:
                        animator.Play("Gatling_Shooting", -1, 0f);
                        animator.speed = 0;
                        break;
                    case PlayerActionState.StopAndAimSprunky:
                        animatorSpunky.Play("Sprunky_Shooting", -1, 0f);
                        animatorSpunky.speed = 0;
                        break;
                    case PlayerActionState.RunFast:
                        animator.Play("Run_Sprint");
                        break;
                    case PlayerActionState.ShowUpTerminal:
                        animator.Play("Showup_Shotgun");
                        break;
                    case PlayerActionState.ShowUpRampo:
                        animator.Play("Showup_Gatling");
                        break;
                    case PlayerActionState.ShowUpMafia:
                        animator.Play("Showup_Thompson");
                        break;
                    case PlayerActionState.ShowUpSprunky:
                        animatorSpunky.Play("Sprunky_Showup");
                        break;
                    case PlayerActionState.ThompsonShooting:
                        animator.Play("Thompson_Shooting");
                        _isWaitingAnimComplete = true;
                        break;
                    case PlayerActionState.GatlingShooting:
                        animator.Play("Gatling_Shooting");
                        _isWaitingAnimComplete = true;
                        break;
                    case PlayerActionState.ShotgunShooting:
                        animator.Play("Shotgun_Shooting");
                        _isWaitingAnimComplete = true;
                        break;
                    case PlayerActionState.SprunkyShooting:
                        animatorSpunky.Play("Sprunky_Shooting");
                        _isWaitingAnimCompleteSprunky = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private void SetAnimRunType()
        {
            var currentPlayerSpeed = FacilityManager.Instance.IdleGameData.RealSpeed;
            // Debug.LogWarning("SetAnimRunType: " + currentPlayerSpeed);
            
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;
            var result = data.isActiveRetal   
                ? data.idSkinRented   
                : data.idSkinSelected;  
            switch (result)
            {
                case 0:
                case 1:
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
                    break;
                case 2:
                    if (currentPlayerSpeed < 2)
                    {
                        animator.speed = currentPlayerSpeed / 1.3f;
                        animator.Play("Walk");
                    }
                    else if (currentPlayerSpeed < 4.5f)
                    {
                        animator.speed = currentPlayerSpeed / 2.5f;
                        animator.Play("Shotgun_Run");
                    }
                    else
                    {
                        animator.speed = currentPlayerSpeed / 6;
                        animator.Play("Run_Sprint");
                    }
                    break;
                case 3:
                    if (currentPlayerSpeed < 2)
                    {
                        animator.speed = currentPlayerSpeed / 1.3f;
                        animator.Play("Walk");
                    }
                    else if (currentPlayerSpeed < 4.5f)
                    {
                        animator.speed = currentPlayerSpeed / 2.5f;
                        animator.Play("Thompson_Run");
                    }
                    else
                    {
                        animator.speed = currentPlayerSpeed / 6;
                        animator.Play("Run_Sprint");
                    }
                    break;
                case 4:
                    if (currentPlayerSpeed < 2)
                    {
                        animator.speed = currentPlayerSpeed / 1.3f;
                        animator.Play("Walk");
                    }
                    else if (currentPlayerSpeed < 4.5f)
                    {
                        animator.speed = currentPlayerSpeed / 2.5f;
                        animator.Play("Gatling_Run");
                    }
                    else
                    {
                        animator.speed = currentPlayerSpeed / 6;
                        animator.Play("Run_Sprint");
                    }
                    break;
                default:
                    animatorSpunky.speed = currentPlayerSpeed / 3f;
                    animatorSpunky.Play("Sprunky_Run");
                    break;
            }
           
        }

        private void RandomIdleState()
        {
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;
            var result = data.isActiveRetal   
                ? data.idSkinRented   
                : data.idSkinSelected;  
            switch (result)
            {
                case 0:
                    animator.Play("Breath");
                    break;
                case 1:
                    animator.Play("Breath");
                    break;
                case 2:
                    animator.Play("Showup_Shotgun_Idle");
                    break;
                case 3:
                    animator.Play("Showup_Thompson_Idle");
                    break;
                case 4:
                    animator.Play("Showup_Gatling_Idle");
                    break;
                default:
                    animatorSpunky.Play("Sprunky_Idle");
                    break;
            }
            // if (CanvasManager.Instance.GetPopup(UIName.Name.SKIN_POPUP))
            // {
            //     animator.Play("Breath");
            // }
        }

        private bool _isWaitingAnimComplete;
        private bool _isWaitingAnimCompleteSprunky;
        private PlayerActionState _previousState;

        protected void Update()
        {
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected;
            if (data < 5)
            {
                if (!_isWaitingAnimComplete)
                    return;
            
                var animStateInfo = animator.GetCurrentAnimatorStateInfo(0)
                    .normalizedTime;
                if (animStateInfo >= 0.9f)
                {
                    var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                    if (clipInfo.Length <= 0) return;
                    switch (clipInfo[0].clip.name)
                    {
                        case "Shooting_Pistol":
                            _isWaitingAnimComplete = false;
                            ActionState = PlayerActionState.StopAndAimPistol;
                            break;
                        case "Gatling_Shooting":
                            _isWaitingAnimComplete = false;
                            ActionState = PlayerActionState.StopAndAimGatlingGun;
                            break;
                        case "Shotgun_Shooting":
                            _isWaitingAnimComplete = false;
                            ActionState = PlayerActionState.StopAndAimShootGun;
                            break;
                        case "Thompson_Shooting":
                            _isWaitingAnimComplete = false;
                            ActionState = PlayerActionState.StopAndAimThompson;
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
                    }
                }

            }
            else
            {
                if (!_isWaitingAnimCompleteSprunky)
                    return;
                
                var animSprunkyStateInfo = animatorSpunky.GetCurrentAnimatorStateInfo(0)
                    .normalizedTime;
                if (animSprunkyStateInfo >= 0.9f)
                {
                    var clipInfoSprunky = animatorSpunky.GetCurrentAnimatorClipInfo(0);
                    if (clipInfoSprunky.Length <= 0) return;
                    switch (clipInfoSprunky[0].clip.name)
                    {
                        case "Sprunky_Shooting":
                            _isWaitingAnimCompleteSprunky = false;
                            ActionState = PlayerActionState.StopAndAimSprunky;
                            break;
                    }
                }
            }
            
        }
    }
}