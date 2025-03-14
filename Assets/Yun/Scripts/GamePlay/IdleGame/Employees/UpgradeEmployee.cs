using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Employees
{
    public class UpgradeEmployee : Employee
    {
        [SerializeField] private GameObject body;
        [SerializeField] private ParticleSystem upgradeEffect;

        public enum EmployeeWorkState
        {
            IDLE,
            PATROL_TO_BED_ROOM,
            PATROL_TO_BATTLE_ROOM,
            FOLLOWING_CLIENT,
            FOLLOWING_DESERT_ROOM,
        }

        private EmployeeWorkState _workState;

        public EmployeeWorkState WorkState
        {
            get => _workState;
            set
            {
                _workState = value;
                switch (value)
                {
                    case EmployeeWorkState.IDLE:
                        _currentClient = null;
                        StopFollow();
                        break;
                    case EmployeeWorkState.FOLLOWING_CLIENT:
                        if (stateMachine.ActionState == EmployeeActionStateMachine.EmployeeActionState.Run)
                            stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
                        break;
                    case EmployeeWorkState.FOLLOWING_DESERT_ROOM:
                        if (stateMachine.ActionState == EmployeeActionStateMachine.EmployeeActionState.Run)
                            stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
                        break;
                }
            }
        }

        public override void UpdateSpeed()
        {
            StartSpeed = navMeshAgent.speed;
            if (stateMachine.ActionState == EmployeeActionStateMachine.EmployeeActionState.Run)
                stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
        }

        [Button]
        public override void StartWorking()
        {
            base.StartWorking();
            // WorkState = EmployeeWorkState.IDLE;
            PatrolToBedRoom();
            CancelInvoke(nameof(FindingClient));
            InvokeRepeating(nameof(FindingClient), 2, 4);
        }

        private const float MinDistanceWhenFollowClient = 1.5f;

        private void PatrolToBedRoom()
        {
            if (!_isWorking)
                return;
            var bedroom = FindRandomBedRoom();
            if (bedroom)
            {
                FollowTarget(bedroom.EntryPoint.gameObject, 1, 1f);
                WorkState = EmployeeWorkState.PATROL_TO_BED_ROOM;
            }
            else
            {
                WorkState = EmployeeWorkState.IDLE;
            }
        }

        private void PatrolToBattleRoom()
        {
            if (!_isWorking)
                return;
            var battleRoom = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
            if (battleRoom)
            {
                FollowTarget(battleRoom.EntryPoint.gameObject, 1, 1f);
                WorkState = EmployeeWorkState.PATROL_TO_BATTLE_ROOM;
            }
            else
            {
                WorkState = EmployeeWorkState.IDLE;
            }
        }

        private WarBaseClient _currentClient;
        private BedRoom _currentBedRoom;
        private BattleRoom _currentBattleRoom;

        private void FindingClient()
        {
            if (!_isWorking)
                return;
            if (FacilityManager.Instance.CurrentDesertRoom && WorkState != EmployeeWorkState.FOLLOWING_DESERT_ROOM)
            {
                WorkState = EmployeeWorkState.FOLLOWING_DESERT_ROOM;
                FollowTarget(FacilityManager.Instance.CurrentDesertRoom.CashPoint.gameObject);
                return;
            }

            if (WorkState == EmployeeWorkState.FOLLOWING_DESERT_ROOM)
            {
                if (FacilityManager.Instance.CurrentDesertRoom) return;
                StopFollow();
                WorkState = EmployeeWorkState.IDLE;
                return;
            }

            var nearestClient = FacilityManager.Instance.FindNearestWaitToUpgradeClient(gameObject);
            if (nearestClient != null)
            {
                FollowTarget(nearestClient.gameObject, MinDistanceWhenFollowClient);
                _currentClient = nearestClient;
                WorkState = EmployeeWorkState.FOLLOWING_CLIENT;
            }
        }

        protected override void FollowTarget(GameObject t, float stoppingDistance = 1, float speed = 0)
        {
            GetComponent<NavMeshAgent>().stoppingDistance = stoppingDistance;
            if (speed != 0)
                GetComponent<NavMeshAgent>().speed = speed;
            else
                GetComponent<NavMeshAgent>().speed = StartSpeed;
            _target = t;
            navMeshAgent.SetDestination(_target.transform.position);
        }

        public override void StopWorking()
        {
            StopFollow();
            _delayTween?.Kill();
            base.StopWorking();
            body.SetActive(false);
            upgradeEffect.gameObject.SetActive(true);
            upgradeEffect.Play();
            DOVirtual.DelayedCall(2, () =>
            {
                upgradeEffect.gameObject.SetActive(false);
                gameObject.SetActive(false);
                body.SetActive(true);
                FacilityManager.Instance.ShowUpgradeWorkerPoint();
            }).SetAutoKill(true);
        }

        private BedRoom FindRandomBedRoom()
        {
            var randomList = new List<BedRoom>();
            foreach (var room in FacilityManager.Instance.BedRoomsList)
            {
                if (room.IsBuilt)
                    randomList.Add(room);
            }

            return randomList[UnityEngine.Random.Range(0, randomList.Count)];
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!_isWorking)
                return;
            if (navMeshAgent.velocity.magnitude == 0)
            {
                if (navMeshAgent.pathPending ||
                    navMeshAgent.remainingDistance > MinDistanceWhenFollowClient + 0.5f) return;

                switch (WorkState)
                {
                    case EmployeeWorkState.FOLLOWING_CLIENT:
                        if (_currentClient && !_currentClient.isConnectedToPlayer &&
                            _currentClient.EmotionState ==
                            WarBaseClient.ClientEmotionState.WaitToUpgrade)
                        {
                            _currentClient.ShowUpgradeEffect();
                            // FacilityManager.Instance.RemoveUpgradeClient(_currentClient);
                            QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId
                                .TrainRookie);
                            _currentClient.EmotionState =
                                WarBaseClient.ClientEmotionState.Idle;
                            _currentClient.LevelUp();
                            _currentClient.HideGuideArrowOnPoint();
                            FacilityManager.Instance.AddTokenWhenUpgrade(1, true, false);
                            var client = _currentClient;
                            DOVirtual.DelayedCall(1, (() =>
                            {
                                if (client.LastRoom.GetComponent<BedRoom>())
                                {
                                    client.GetAvailableTask();
                                }
                                else
                                {
                                    client.ShowWaitToRest();
                                }
                            })).SetAutoKill(true);
                            _currentClient = null;
                            WorkState = EmployeeWorkState.IDLE;
                        }
                        else
                        {
                            // Debug.Log("STOP FOLLOW");
                            StopFollow();
                            WorkState = EmployeeWorkState.IDLE;
                        }

                        break;
                    case EmployeeWorkState.FOLLOWING_DESERT_ROOM:
                        StopFollow();
                        if (!FacilityManager.Instance.CurrentDesertRoom.IsConnecting)
                        {
                            var fixedTime = 3;
                            FacilityManager.Instance.CurrentDesertRoom.ShowConnectAnim(fixedTime);
                            DOVirtual.DelayedCall(fixedTime,
                                (() =>
                                {
                                    var currentDesertRoom =
                                        FacilityManager.Instance.CurrentDesertRoom.GetComponent<BedRoom>();
                                    if (currentDesertRoom.IsDesertRoom)
                                        currentDesertRoom.DeactivateDeserters();
                                    PatrolToBedRoom();
                                }));
                        }
                        else
                        {
                            PatrolToBedRoom();
                        }

                        break;
                    case EmployeeWorkState.IDLE:
                        break;
                    case EmployeeWorkState.PATROL_TO_BED_ROOM:
                        WorkState = EmployeeWorkState.IDLE;
                        FindingClient();
                        if (WorkState == EmployeeWorkState.IDLE)
                        {
                            var randomNumber = UnityEngine.Random.Range(0, 100) * 0.02f;
                            _delayTween = DOVirtual.DelayedCall(randomNumber, (PatrolToBattleRoom)).SetAutoKill(true);
                        }
                        break;
                    case EmployeeWorkState.PATROL_TO_BATTLE_ROOM:
                        WorkState = EmployeeWorkState.IDLE;
                        FindingClient();
                        if (WorkState == EmployeeWorkState.IDLE)
                        {
                            var randomNumber2 = UnityEngine.Random.Range(0, 100) * 0.02f;
                            _delayTween = DOVirtual.DelayedCall(randomNumber2, (PatrolToBedRoom)).SetAutoKill(true);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private Tween _delayTween;
    }
}