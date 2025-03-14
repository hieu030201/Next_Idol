using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Audios;
using Yun.Scripts.Cameras;
using Yun.Scripts.Core;
using Yun.Scripts.GamePlay.Enemies;
using Yun.Scripts.GamePlay.Vehicles;
using Yun.Scripts.Managers;

namespace Yun.Scripts.GamePlay.Transition
{
    public class TransitionControllerLevel1 : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain cineMachineBrain;
        [SerializeField] private CinemachineVirtualCamera raceCamera,
            bossAppearCamera,
            fightBossCamera,
            introCamera,
            topCamera,
            centerCamera;

        [SerializeField] private GameObject player,
            boss,
            raceDestination,
            bossAppearStartPoint,
            fightBossDestination,
            raceStartPoint,
            fightBossStartPointList,
            spawn;

        private enum TransitionControllerState
        {
            Intro,
            Race,
            ReduceSpeed,
            FightBoss,
            GameOver
        }

        private TransitionControllerState _state;

        private TransitionControllerState State
        {
            get => _state;
            set
            {
                _state = value;
                switch (value)
                {
                    case TransitionControllerState.Intro:
                        break;
                    case TransitionControllerState.Race:
                        break;
                    case TransitionControllerState.ReduceSpeed:
                        break;
                    case TransitionControllerState.FightBoss:
                        break;
                    case TransitionControllerState.GameOver:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private YunCameraController _cameraController;

        private const float ReduceSpeedDelay = 10;
        private const float BossChaseDelay = 0.5f;
        private const float ReduceSpeedDistance = 20;

        public bool isBackToStartPoint;

        private void Awake()
        {
            spawn.SetActive(false);
        }

        private void Start()
        {
            _cameraController = new YunCameraController(cineMachineBrain);
            _cameraController.RegisterCamera(raceCamera);
            _cameraController.RegisterCamera(bossAppearCamera);
            _cameraController.RegisterCamera(fightBossCamera);
            _cameraController.RegisterCamera(introCamera);
            _cameraController.RegisterCamera(topCamera);
            _cameraController.RegisterCamera(centerCamera);
            
            _cameraController.SwitchCamera(centerCamera);

            if (isBackToStartPoint)
                player.transform.localPosition = raceStartPoint.transform.localPosition;
            
            // StartGame();
            
            player.GetComponent<YunVehicle>().ShowIntro();
        }

        public void StartGame()
        {
            _cameraController.SwitchCamera(topCamera,1);
            DOVirtual.DelayedCall(1, (() =>
            {
                State = TransitionControllerState.Intro;
                _cameraController.SwitchCamera(introCamera, 1);
                introCamera.GetComponent<DollyCameraController>().StartMoving();
                DOVirtual.DelayedCall(1, ShowIntro);
            }));
        }

        private void ShowIntro()
        {
            player.GetComponent<YunVehicle>().ShowIntro();
            DOVirtual.DelayedCall(0.5f, (StartRacing));
        }

        private void StartRacing()
        {
            spawn.SetActive(true);
            _cameraController.SwitchCamera(raceCamera);
            State = TransitionControllerState.Race;
            player.GetComponent<YunVehicle>().StartRacing(raceDestination.transform.localPosition);

            DOVirtual.DelayedCall(2, (() =>
            {
                introCamera.gameObject.SetActive(false);
                topCamera.gameObject.SetActive(false);
            }));
        }

        private void ReduceSpeed()
        {
            // Debug.Log("START REDUCE SPEED");
            AudioManager.Instance.PlayBackgroundMusic(AudioManager.Instance.Fight_Boss_BG);
            State = TransitionControllerState.ReduceSpeed;
            player.GetComponent<YunVehicle>().ReduceSpeed(ReduceSpeedDelay);
            _cameraController.SwitchCamera(bossAppearCamera, 2);
            bossAppearCamera.GetComponent<DollyCameraController>().StartMoving();
            boss.GetComponent<Enemy>().Active(player.transform, true);
            boss.GetComponent<BossLevel1>().WalkToStartPoint(bossAppearStartPoint.transform.localPosition);
        }
        
        private void ShowLog(string log)
        {
            var currentString = GameManager.Instance.debugText.text;
            currentString += ", " + log;
            GameManager.Instance.debugText.text = currentString;
        }

        private void FixedUpdate()
        {
            if (State != TransitionControllerState.Race)
                return;
            if(player == null)
                return;
            if (!(player.transform.localPosition.z >
                  raceDestination.transform.localPosition.z - ReduceSpeedDistance)) return;
            ReduceSpeed();
        }

        public void StartFightBoss()
        {
            player.GetComponent<YunVehicle>().LookatBoss(boss);
            player.GetComponent<YunVehicle>().MoveToFightBossStartPosition(fightBossStartPointList);
            var bossLevel1 = boss.GetComponent<BossLevel1>();
            bossLevel1
                .FirstAttack();
            _cameraController.SwitchCamera(fightBossCamera, 1);

            DOVirtual.DelayedCall(BossChaseDelay, (() =>
            {
                player.GetComponent<YunVehicle>()
                    .StartFightBoss(fightBossDestination.transform.localPosition);
                bossLevel1.StartFollowPlayer(player.transform);
                GameManager.Instance.GameState = GameManager.GameStateType.Play;
            }));
        }
    }
}