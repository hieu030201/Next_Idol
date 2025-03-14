using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Clients
{
    public class Client : Person
    {
        public bool isConnectedToPlayer;

        // Biến để đánh dấu là lính đang làm dở 1 việc gì đó
        public bool IsBusy { get; protected set; }
        public bool IsMarked { get; set; }
        public bool IsGuidingClient { get; set; }
        public BaseRoom LastRoom { get; set; }

        protected NavMeshAgent NavMeshAgent;
        private float _startSpeed;
        private GameObject _clientConnectPoint;

        protected override void Awake()
        {
            base.Awake();
            // Debug.LogWarning("Awake Client");
            _clientConnectPoint = transform.Find("Client_Connect_Point").gameObject;
            _clientConnectPoint.SetActive(false);

            NavMeshAgent = GetComponent<NavMeshAgent>();
            _startSpeed = NavMeshAgent.speed;
        }

        public override void Reset()
        {
            base.Reset();
            IsBusy = false;
            IsMarked = false;
            IsGuidingClient = false;
            LastRoom = null;
            TargetSlot = null;
            IsMovingToSlot = false;
            isConnectedToPlayer = false;
        }

        public void ActiveNavmeshAgent()
        {
            GetComponent<NavMeshAgent>().enabled = true;
        }

        public void DeactivateNavmeshAgent()
        {
            // Debug.Log("DeactivateNavmeshAgent");
            if (GetComponent<NavMeshAgent>())
                GetComponent<NavMeshAgent>().enabled = false;
        }

        protected override void FixedUpdate()
        {
            if (!gameObject.activeSelf)
                return;
            base.FixedUpdate();
            if (_followTarget)
            {
                NavMeshAgent.SetDestination(_followTarget.transform.position);
            }
        }

        protected void ShowConnectPoint()
        {
            _clientConnectPoint.SetActive(true);
        }

        public void HideConnectPoint()
        {
            _clientConnectPoint.SetActive(false);
        }

        protected Transform TargetSlot;
        protected bool IsMovingToSlot;
        protected Action OnMoveToSlotCompleted;

        public virtual void MoveToSlot(Transform slot, bool immediately = false, Action moveToSlotCompleted = null)
        {
            TargetSlot = slot;
            if (immediately)
            {
                DeactivateNavmeshAgent();
                var transform1 = transform;
                transform1.position = slot.position;
                transform1.rotation = slot.rotation;
                if (LastRoom)
                    LastRoom.OnClientMoveToSlotCompleted(this);
            }
            else
            {
                IsBusy = true;
                NavMeshAgent.enabled = true;
                NavMeshAgent.speed = _startSpeed;
                NavMeshAgent.stoppingDistance = 0;
                NavMeshAgent.SetDestination(slot.position);
                IsMovingToSlot = true;
                OnMoveToSlotCompleted = moveToSlotCompleted;
            }
        }

        public virtual void SetData(ClientData clientData)
        {
            Id = clientData.Id;
            Level = clientData.Level;
        }

        private GameObject _followTarget;

        public virtual void FollowPlayer(GameObject player, float speed = 0, int index = 1)
        {
            ActiveNavmeshAgent();
            IsGuidingClient = false;
            TargetSlot = null;
            NavMeshAgent.enabled = true;
            IsMarked = false;
            isConnectedToPlayer = true;

            NavMeshAgent.stoppingDistance = 1.5f + (index - 1);
            if (speed != 0)
                NavMeshAgent.speed = speed;
            _followTarget = player;
        }

        public virtual void StopFollow()
        {
            isConnectedToPlayer = false;
            _followTarget = null;
        }

        public virtual void LevelUp()
        {
            Level++;
        }

        public void UpdateSpeed(float speed)
        {
            NavMeshAgent.speed = speed;
        }
    }
}