using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Random = UnityEngine.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Employees
{
    public class FunctionRoomEmployee : Employee
    {
        [SerializeField] private WarBaseClient.ClientEmotionState emotionStateToGetIn;

        public enum EmployeeWorkState
        {
            IDLE,
            FOLLOWING_CLIENT,
            LEADING_CLIENT_TO_FUNCTION_ROOM,
            LEADING_CLIENT_TO_REST,
            PATROL_TO_BED_ROOM,
            PATROL_TO_FUCNTION_ROOM,
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

        public override void StartWorking()
        {
            base.StartWorking();
            WorkState = EmployeeWorkState.IDLE;
            var randomNumber = Random.Range(0f, 2f);
            InvokeRepeating(nameof(FindingClient), 1f + randomNumber, 4f);
            PatrolToBedRoom();
        }

        private const float MinDistanceWhenFollowClient = 1.5f;

        private void PatrolToBedRoom()
        {
            var bedroom = FindRandomBedRoom();
            if (bedroom)
            {
                FollowTarget(bedroom.EntryPoint.gameObject, 1, 1.5f);
                WorkState = EmployeeWorkState.PATROL_TO_BED_ROOM;
            }
            else
            {
                WorkState = EmployeeWorkState.IDLE;
            }
        }

        private void PatrolToFunctionRoom()
        {
            var functionRoom = FacilityManager.Instance.GetCurrentFunctionRoom(emotionStateToGetIn, false);
            if (functionRoom)
            {
                FollowTarget(functionRoom.EntryPoint.gameObject, 1, 1.5f);
                WorkState = EmployeeWorkState.PATROL_TO_FUCNTION_ROOM;
            }
            else
            {
                WorkState = EmployeeWorkState.IDLE;
            }
        }

        private WarBaseClient _currentClient;
        private BedRoom _currentBedRoom;
        private BaseRoom _currentFunctionRoom;

        private void FindingClient()
        {
            // DOVirtual.DelayedCall(1, (FindingClient));

            if (WorkState == EmployeeWorkState.FOLLOWING_CLIENT && _currentClient.isConnectedToPlayer)
            {
                // WorkState = EmployeeWorkState.IDLE;
                PatrolToBedRoom();
            }

            if (WorkState is EmployeeWorkState.IDLE or EmployeeWorkState.PATROL_TO_BED_ROOM
                or EmployeeWorkState.PATROL_TO_FUCNTION_ROOM)
            {
                foreach (var client in FacilityManager.Instance.ClientsList)
                {
                    if (client.isConnectedToPlayer || client.IsMarked || !client.LastRoom || client.IsDeserting)
                        continue;
                    if (client.EmotionState == emotionStateToGetIn &&
                        client.LastRoom.emotionStateToGetIn != emotionStateToGetIn)
                    {
                        if (!client.LastRoom.GetComponent<BedRoom>()) continue;
                        // if (client.LastRoom.GetComponent<BedRoom>().AreaNumber != areaNumber) continue;
                        client.IsMarked = true;
                        _tweenToContinuePatrol?.Kill();
                        FollowTarget(client.gameObject, MinDistanceWhenFollowClient);
                        stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
                        _currentClient = client;
                        WorkState = EmployeeWorkState.FOLLOWING_CLIENT;
                        _currentBedRoom = client.LastRoom.GetComponent<BedRoom>();
                        return;
                    }

                    if (client.EmotionState != WarBaseClient.ClientEmotionState.WaitToRest) continue;
                    if (client.LastRoom.emotionStateToGetIn != emotionStateToGetIn) continue;
                    client.IsMarked = true;
                    _tweenToContinuePatrol?.Kill();
                    FollowTarget(client.gameObject, MinDistanceWhenFollowClient);
                    stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.Run;
                    _currentClient = client;
                    WorkState = EmployeeWorkState.FOLLOWING_CLIENT;
                    return;
                }
            }
        }

        private BedRoom FindRandomBedRoom()
        {
            var randomList = new List<BedRoom>();
            foreach (var room in FacilityManager.Instance.BedRoomsList)
            {
                if (room.IsBuilt)
                    randomList.Add(room);
            }

            return randomList[Random.Range(0, randomList.Count)];
        }

        private Tween _tweenToContinuePatrol;
        private Tween _delayTween;
        
        public override void StopWorking()
        {
            base.StopWorking();
            _delayTween?.Kill();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (_isWaiting)
                return;
            if (navMeshAgent.velocity.magnitude == 0)
            {
                if (navMeshAgent.pathPending ||
                    navMeshAgent.remainingDistance > MinDistanceWhenFollowClient + 0.5f) return;

                switch (WorkState)
                {
                    case EmployeeWorkState.FOLLOWING_CLIENT:
                        if (_currentClient && !_currentClient.isConnectedToPlayer && !_currentClient.IsDeserting)
                        {
                            if (_currentClient.EmotionState ==
                                emotionStateToGetIn)
                            {
                                if (_currentClient)
                                    _currentClient.ShowConnectAnim(1.5f);
                                DOVirtual.DelayedCall(1.5f, (() =>
                                {
                                    if (!_currentClient.isConnectedToPlayer && !_currentClient.IsDeserting)
                                    {
                                        _currentFunctionRoom =
                                            FacilityManager.Instance.GetCurrentFunctionRoom(emotionStateToGetIn, true);
                                        if (_currentFunctionRoom != null)
                                        {
                                            _currentClient.FollowPlayer(gameObject, navMeshAgent.speed);
                                            _currentClient.HideConnectPoint();
                                            _currentBedRoom.GetComponent<BaseRoom>().RemoveClient(_currentClient);
                                            WorkState = EmployeeWorkState.LEADING_CLIENT_TO_FUNCTION_ROOM;
                                            FollowTarget(_currentFunctionRoom.EntryPoint.gameObject);
                                        }
                                        else
                                        {
                                            Waiting(3);
                                        }
                                    }
                                    else
                                    {
                                        if (_currentClient)
                                        {
                                            _currentClient.IsMarked = false;
                                        }
                                    }
                                }));
                            }
                            else if (_currentClient.EmotionState ==
                                     WarBaseClient.ClientEmotionState.WaitToRest)
                            {
                                try
                                {
                                    if (_currentClient)
                                        _currentClient.ShowConnectAnim(1.5f);
                                    DOVirtual.DelayedCall(1.5f, (() =>
                                    {
                                        if (!_currentClient.isConnectedToPlayer)
                                        {
                                            _currentClient.FollowPlayer(gameObject, navMeshAgent.speed);
                                            _currentClient.HideConnectPoint();
                                            _currentBedRoom = FacilityManager.Instance.FindAvailableBedRoom();
                                            if (_currentBedRoom)
                                            {
                                                if (_currentClient.LastRoom)
                                                    _currentClient.LastRoom.RemoveClient(_currentClient);
                                                FollowTarget(_currentBedRoom.EntryPoint.gameObject);
                                                _currentClient.FollowPlayer(gameObject, navMeshAgent.speed);
                                                _currentClient.HideConnectPoint();
                                                WorkState = EmployeeWorkState.LEADING_CLIENT_TO_REST;
                                            }
                                            else
                                            {
                                                // WorkState = EmployeeWorkState.IDLE;
                                                PatrolToFunctionRoom();
                                            }
                                        }
                                    }));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    // WorkState = EmployeeWorkState.IDLE;
                                    PatrolToFunctionRoom();
                                    throw;
                                }
                            }
                            else
                            {
                                StopFollow();
                                // WorkState = EmployeeWorkState.IDLE;
                                PatrolToFunctionRoom();
                            }
                        }
                        else
                        {
                            StopFollow();
                            // WorkState = EmployeeWorkState.IDLE;
                            PatrolToFunctionRoom();
                        }

                        break;
                    case EmployeeWorkState.LEADING_CLIENT_TO_FUNCTION_ROOM:
                        var success2 =
                            FacilityManager.Instance.OnEmployeeActiveEntryPoint(_currentFunctionRoom, _currentClient);
                        if (success2)
                        {
                            _currentClient.StopFollow();
                            // WorkState = EmployeeWorkState.IDLE;
                            PatrolToBedRoom();
                        }
                        else
                        {
                            // Debug.Log("waiting");
                            Waiting(3);
                        }

                        break;
                    case EmployeeWorkState.LEADING_CLIENT_TO_REST:
                        var success =
                            FacilityManager.Instance.OnEmployeeActiveEntryPoint(_currentBedRoom, _currentClient);
                        if (success)
                        {
                            _currentClient.StopFollow();
                            // WorkState = EmployeeWorkState.IDLE;
                            PatrolToFunctionRoom();
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
                            _delayTween = DOVirtual.DelayedCall(randomNumber, (PatrolToFunctionRoom)).SetAutoKill(true);
                        }
                        break;
                    case EmployeeWorkState.PATROL_TO_FUCNTION_ROOM:
                        WorkState = EmployeeWorkState.IDLE;
                        FindingClient();
                        if (WorkState == EmployeeWorkState.IDLE)
                        {
                            var randomNumber = UnityEngine.Random.Range(0, 100) * 0.02f;
                            _delayTween = DOVirtual.DelayedCall(randomNumber, (PatrolToBedRoom)).SetAutoKill(true);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}