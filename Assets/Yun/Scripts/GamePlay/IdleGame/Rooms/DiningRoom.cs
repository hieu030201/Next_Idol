using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Employees;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class DiningRoom : BaseRoom
    {
        [SerializeField] private WaiterEmployee waiterEmployee;
        [SerializeField] private WaiterEmployee waiterEmployee2;

        [SerializeField] private List<GameObject> foodList;
        [SerializeField] private List<GameObject> foodStartPositionList;
        [SerializeField] private List<GameObject> foodTablePositionList;
        [SerializeField] private List<GameObject> foodCounterPositionList;
        [SerializeField] private List<GameObject> tablePositionList;
        [SerializeField] private GameObject waiterStartPosition;

        public override void Init()
        {
            base.Init();
            
            _clientsListToServe = new List<WarBaseClient>();
            waiterEmployee.ToStartPointComplete = WaiterToStartPointComplete;
            waiterEmployee.ToFoodCounterComplete = WaiterToFoodCounterComplete;
            waiterEmployee.ToTableComplete = WaiterToTableComplete;
            waiterEmployee.gameObject.SetActive(false);
            waiterEmployee2.gameObject.SetActive(false);
            
            foreach (var food in foodList)
            {
                food.SetActive(false);
            }
        }

        public override void StartBuild(bool isBuildImmediately = false)
        {
            // Debug.Log("dining room start build");
            base.StartBuild(isBuildImmediately);
            waiterEmployee.gameObject.SetActive(true);
            waiterEmployee2.gameObject.SetActive(true);
            DOVirtual.DelayedCall(3, (() => { waiterEmployee.StartWorking(); })).SetAutoKill(true);
            // Invoke(nameof(waiterEmployee.StartWorking),3);
            // Debug.Log("super yun");
        }

        public override void AddClient(WarBaseClient client, bool immediately = false)
        {
            base.AddClient(client, immediately);
            if (WaitingClientsList.Count == 0)
            {
                if (immediately)
                {
                    client.Eat();
                    DOVirtual.DelayedCall(5, client.ShowWaitToRest);
                }
                else
                {
                    client.EmotionState = WarBaseClient.ClientEmotionState.Eat;
                }

                if (!immediately)
                    AddFoodOrder(client);
            }
            else
            {
                client.HideConnectPoint();
            }
        }

        protected override void AddWaitingClient(WarBaseClient client, bool immediately = false)
        {
            base.AddWaitingClient(client, immediately);
            client.EmotionState = immediately ? WarBaseClient.ClientEmotionState.Idle : WarBaseClient.ClientEmotionState.RuningToSlot;
        }

        public override void RemoveClient(WarBaseClient client, bool isKeepPosition = false, float delay = 0)
        {
            base.RemoveClient(client, true, delay);
            client.HideConnectPoint();
            if (WaitingClientsList.Count <= 0) return;
            var waitingClient = WaitingClientsList[0];
            RemoveWaitingClient(waitingClient);
            AddClientFromWaiting(waitingClient);
        }

        private void AddClientFromWaiting(WarBaseClient client)
        {
            base.AddClient(client, false);
            DOVirtual.DelayedCall(0.3f,
                (() =>
                {
                    client.EmotionState = WarBaseClient.ClientEmotionState.Eat;
                })).SetAutoKill(true);
            AddFoodOrder(client);
        }

        private List<WarBaseClient> _clientsListToServe;
        private bool _isServing;

        private void AddFoodOrder(WarBaseClient client)
        {
            waiterEmployee.ActiveNavmeshAgent();
            // Debug.Log("super yun 1");
            _clientsListToServe.Add(client);
            if (_isServing)
                return;
            // Debug.Log("super yun 2");
            DOVirtual.DelayedCall(3, (() => { StartServing(); })).SetAutoKill(true);
        }

        private int _currentServerIndex;

        private void StartServing()
        {
            if (_clientsListToServe.Count == 0)
                return;
            _isServing = true;
            var client = _clientsListToServe[0];
            // Debug.Log("super yun 3");
            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (client == ClientsList[i])
                {
                    _currentServerIndex = i;
                    MoveWaiterToFoodCounter();
                    return;
                }
            }
        }

        private void CompleteServe()
        {
            if (_clientsListToServe.Count == 0)
                return;
            _clientsListToServe.RemoveAt(0);
            if (_clientsListToServe.Count == 0)
            {
                waiterEmployee.SetTarget(waiterStartPosition);
                waiterEmployee.Role = Employee.EmployeeRole.ToDiningStart;
                _isServing = false;
                return;
            }

            StartServing();
        }

        private void MoveWaiterToFoodCounter()
        {
            // Debug.Log("super yun 3: " + foodList.Count);
            var currentFood = foodList[_currentServerIndex];
            currentFood.transform.parent = transform;
            currentFood.transform.position = foodStartPositionList[_currentServerIndex].transform.position;
            currentFood.SetActive(true);
            // Debug.Log("super yun 4: ");
            waiterEmployee.SetTarget(foodCounterPositionList[_currentServerIndex]);
            waiterEmployee.Role = Employee.EmployeeRole.ToFoodCounter;
        }

        private void WaiterToStartPointComplete()
        {
        }

        private void WaiterToFoodCounterComplete()
        {
            var currentFood = foodList[_currentServerIndex];
            currentFood.transform.parent = waiterEmployee.FoodContainer.transform;
            currentFood.transform.localPosition = new Vector3(0, 0, 0);
            waiterEmployee.SetTarget(tablePositionList[_currentServerIndex]);
            waiterEmployee.Role = Employee.EmployeeRole.ToDiningTable;
        }

        private void WaiterToTableComplete()
        {
            var currentFood = foodList[_currentServerIndex];
            currentFood.transform.parent = transform;
            currentFood.transform.position = foodTablePositionList[_currentServerIndex].transform.position;
            _clientsListToServe[0].Eat();
            DOVirtual.DelayedCall(3, (() => { currentFood.SetActive(false); }));
            CompleteServe();
        }
    }
}