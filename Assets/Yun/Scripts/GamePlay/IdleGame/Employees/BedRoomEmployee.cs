using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Employees
{
    public class BedRoomEmployee : Employee
    {
        public enum EmployeeWorkState
        {
            IDLE,
            PATROL_TO_BED_ROOM,
            PATROL_TO_BATTLE_ROOM,
            FOLLOWING_CLIENT,
            FOLLOWING_WAITING_ROOM,
            LEADING_CLIENT_TO_BATTLE,
            LEADING_CLIENT_TO_REST,
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
                        if (stateMachine.ActionState != EmployeeActionStateMachine.EmployeeActionState.Run)
                            stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
                        break;
                }
            }
        }

        public override void StopWorking()
        {
            base.StopWorking();
            _delayTween?.Kill();
        }

        [Button]
        public override void StartWorking()
        {
            base.StartWorking();
            PatrolToBedRoom();
            CancelInvoke(nameof(FindingClient));
            InvokeRepeating(nameof(FindingClient), 2, 4);
        }

        private const float MinDistanceWhenFollowClient = 1.5f;

        private void PatrolToBedRoom()
        {
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
            if (WorkState == EmployeeWorkState.FOLLOWING_CLIENT && _currentClient.isConnectedToPlayer)
            {
                PatrolToBedRoom();
            }

            if (WorkState is not (EmployeeWorkState.IDLE or EmployeeWorkState.PATROL_TO_BED_ROOM
                or EmployeeWorkState.PATROL_TO_BATTLE_ROOM)) return;
            if (FacilityManager.Instance.waitingRoom.IsFull)
            {
                FollowTarget(FacilityManager.Instance.waitingRoom.ExitPoint.gameObject);
                WorkState = EmployeeWorkState.FOLLOWING_WAITING_ROOM;
                return;
            }

            foreach (var client in FacilityManager.Instance.ClientsList)
            {
                if (client.EmotionState !=
                    WarBaseClient.ClientEmotionState.WaitToBattle) continue;
                if (client.isConnectedToPlayer || client.IsMarked) continue;
                if (!client.LastRoom.GetComponent<BedRoom>()) continue;
                if (client.IsDeserting) continue;

                client.IsMarked = true;
                FollowTarget(client.gameObject, MinDistanceWhenFollowClient);
                _currentClient = client;
                _currentBedRoom = client.LastRoom.GetComponent<BedRoom>();
                WorkState = EmployeeWorkState.FOLLOWING_CLIENT;
                return;
            }

            if (FacilityManager.Instance.waitingRoom.CountClients() <= 0) return;
            FollowTarget(FacilityManager.Instance.waitingRoom.ExitPoint.gameObject);
            WorkState = EmployeeWorkState.FOLLOWING_WAITING_ROOM;
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
                        if (!_currentClient.isConnectedToPlayer &&
                            _currentClient.EmotionState ==
                            WarBaseClient.ClientEmotionState.WaitToBattle &&
                            !_currentClient.IsDeserting)
                        {
                            if (_currentClient)
                                _currentClient.ShowConnectAnim(1.5f);
                            DOVirtual.DelayedCall(1.5f, (() =>
                            {
                                if (_currentClient && !_currentClient.isConnectedToPlayer &&
                                    !_currentClient.IsDeserting)
                                {
                                    _currentClient.FollowPlayer(gameObject, navMeshAgent.speed);
                                    _currentClient.HideConnectPoint();
                                    _currentBedRoom.GetComponent<BaseRoom>().RemoveClient(_currentClient);
                                    var room = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
                                    if (room)
                                    {
                                        WorkState = EmployeeWorkState.LEADING_CLIENT_TO_BATTLE;
                                        if (_currentClient.EmotionState ==
                                            WarBaseClient.ClientEmotionState.WaitToBattle)
                                        {
                                            _currentBattleRoom = room;
                                            FollowTarget(room.EntryPoint.gameObject);
                                        }
                                    }
                                }
                                else
                                {
                                    if (_currentClient)
                                        _currentClient.IsMarked = false;
                                }
                            }));
                        }
                        else
                        {
                            StopFollow();
                            PatrolToBattleRoom();
                        }

                        break;
                    case EmployeeWorkState.LEADING_CLIENT_TO_BATTLE:
                        var success2 =
                            FacilityManager.Instance.OnEmployeeActiveEntryPoint(_currentBattleRoom, _currentClient);
                        if (success2)
                        {
                            _currentClient.StopFollow();
                            PatrolToBedRoom();
                        }
                        else
                        {
                            Waiting(3);
                        }

                        break;

                    case EmployeeWorkState.FOLLOWING_WAITING_ROOM:
                        var client = FacilityManager.Instance.waitingRoom.GetFirstClient();
                        if (client)
                        {
                            _currentClient = client;
                            _currentBedRoom = FacilityManager.Instance.FindAvailableBedRoom();
                            try
                            {
                                if (_currentBedRoom)
                                {
                                    FollowTarget(_currentBedRoom.EntryPoint.gameObject);
                                    _currentClient.FollowPlayer(gameObject, navMeshAgent.speed);
                                    _currentClient.HideConnectPoint();
                                    WorkState = EmployeeWorkState.LEADING_CLIENT_TO_REST;
                                    FacilityManager.Instance.OnEmployeeActiveExitPoint(FacilityManager.Instance
                                        .waitingRoom);
                                }
                                else
                                {
                                    PatrolToBedRoom();
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                PatrolToBedRoom();
                                throw;
                            }
                        }
                        else
                        {
                            PatrolToBedRoom();
                        }

                        break;
                    case EmployeeWorkState.LEADING_CLIENT_TO_REST:
                        var success =
                            FacilityManager.Instance.OnEmployeeActiveEntryPoint(_currentBedRoom, _currentClient);
                        if (success)
                        {
                            _currentClient.StopFollow();
                            PatrolToBattleRoom();
                        }
                        else
                        {
                            _currentBedRoom = FacilityManager.Instance.FindAvailableBedRoom();
                            if (_currentBedRoom)
                            {
                                FollowTarget(_currentBedRoom.EntryPoint.gameObject);
                            }
                            else
                            {
                                Waiting(3);
                            }
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

        public override void ResumeWhenEndBattle()
        {
            base.ResumeWhenEndBattle();

            if (WorkState == EmployeeWorkState.LEADING_CLIENT_TO_BATTLE)
            {
                var room = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
                if (room)
                {
                    _currentBattleRoom = room;
                    FollowTarget(room.EntryPoint.gameObject);
                }
            }
        }
    }
}