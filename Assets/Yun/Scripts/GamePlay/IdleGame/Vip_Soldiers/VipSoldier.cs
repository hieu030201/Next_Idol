using System;
using System.Collections.Generic;
using Advertising;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Yun.Scripts.UI.GamePlay.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Vip_Soldiers
{
    public class VipSoldier : Client
    {
        [SerializeField] private VipSoldierActionStateMachine stateMachine;
        [SerializeField] private GameObject waitToEatPrefab;
        [SerializeField] private ParticleSystem upgradeEffect;
        [SerializeField] private ParticleSystem waitToUpgradeEffect;
        [SerializeField] private GameObject followPoint;
        [SerializeField] private GameObject body;
        [SerializeField] private GameObject digParticlePrefab;
        [SerializeField] private GameObject geometry;

        private int _countDig;
        private BaseRoom _currentRoom;
        private bool _isDig;
        private int _moneyPerOneDig;

        private int _roomIndex;
        private List<BaseRoom> _roomsList;
        private int _totalMoney;

        private Vector3 distance;
        protected override void Awake()
        {
            base.Awake();

            distance = transform.position - followPoint.transform.position;

            if (waitToUpgradeEffect)
                waitToUpgradeEffect.gameObject.SetActive(false);
            if (upgradeEffect)
                upgradeEffect.gameObject.SetActive(false);

            if (geometry)
            {
                geometry.transform.Find("Vip_Soldier").gameObject.SetActive(false);
                geometry.transform.Find("Vip_Soldier_Noel").gameObject.SetActive(false);
                if(FireBaseManager.Instance.showNoel)
                    geometry.transform.Find("Vip_Soldier_Noel").gameObject.SetActive(true);
                else
                    geometry.transform.Find("Vip_Soldier").gameObject.SetActive(true);
            }
        }

        protected override void FixedUpdate()
        {
            try
            {
                base.FixedUpdate();
                followPoint.transform.position = transform.position - distance;
                if (NavMeshAgent.velocity.magnitude == 0)
                {
                    if (!IsMovingToSlot)
                        return;

                    if (NavMeshAgent.pathPending || NavMeshAgent.remainingDistance > 1) return;

                    if (IsMovingToSlot)
                    {
                        IsMovingToSlot = false;
                        OnMoveToSlotCompleted?.Invoke();

                        if (_isDig)
                        {
                            _isDig = false;
                        }
                        else
                        {
                            if (stateMachine.ActionState != VipSoldierActionStateMachine.State.Idle)
                                stateMachine.ActionState = VipSoldierActionStateMachine.State.Idle;
                        }

                        if (TargetSlot)
                            transform.rotation = TargetSlot.transform.rotation;
                    }
                }
                else
                {
                    if (stateMachine.ActionState != VipSoldierActionStateMachine.State.Run)
                        stateMachine.ActionState = VipSoldierActionStateMachine.State.Run;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool IsWorking;
        public void StartWorking(RoomsManager roomsManager, int totalMoney)
        {
            IsWorking = true;
            _roomIndex = 0;
            _countDig = 0;
            _totalMoney = totalMoney;
            _isDig = true;
            _roomsList = new List<BaseRoom>();
            foreach (var t in roomsManager.RoomsList)
                if (t.digPosition && t.IsBuilt)
                    _roomsList.Add(t);

            MoveToSlot(_roomsList[_roomIndex].digPosition.transform, false, StartDigging);
            HideConnectPoint();
        }
        
        private void StartDigging()
        {
            if (_countDig < 4)
            {
                var digPosition = _roomsList[_roomIndex].digPosition;
                stateMachine.ActionState = VipSoldierActionStateMachine.State.Dig;
                DOVirtual.DelayedCall(1.5f,
                    () =>
                    {
                        GameObject particleInstance = Instantiate(digParticlePrefab, transform.position, transform.rotation);
                        DOVirtual.DelayedCall(2.5f, () => Destroy(particleInstance));
                    });

                DOVirtual.DelayedCall(1.8f,
                        () => { _roomsList[_roomIndex].SpawnMoney(_totalMoney / 4, digPosition.transform.position); })
                    .SetAutoKill(true);
                _countDig++;
                DOVirtual.DelayedCall(4,
                        () =>
                        {
                            _isDig = true;
                            _roomIndex++;
                            if (_roomIndex >= _roomsList.Count)
                                _roomIndex = 0;
                            if (_countDig >= 4)
                            {
                                body.SetActive(false);
                                upgradeEffect.gameObject.SetActive(true);
                                upgradeEffect.Play();
                                DOVirtual.DelayedCall(2, () =>
                                {
                                    IsWorking = false;
                                    upgradeEffect.gameObject.SetActive(false);
                                    gameObject.SetActive(false);
                                    body.SetActive(true);
                                    FacilityManager.Instance.WaitToShowVipSoldier();
                                }).SetAutoKill(true);
                                _isDig = false;
                                return;
                            }
                           
                            MoveToSlot(_roomsList[_roomIndex].digPosition.transform, false, StartDigging);
                        })
                    .SetAutoKill(true);
            }
        }

        public override void MoveToSlot(Transform slot, bool immediately = false, Action moveToSlotCompleted = null)
        {
            base.MoveToSlot(slot, immediately, moveToSlotCompleted);
            if (!immediately)
            {
                stateMachine.StopIdle();
                stateMachine.ActionState = VipSoldierActionStateMachine.State.Run;
            }
        }
    }
}