using System;
using System.Collections.Generic;
using System.Linq;
using Advertising;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Logics
{
    public class IdleGameSaveLoad
    {
        private string IdleGameKey = "IdleGame";
        private string StableGameKey = "StableIdleGame";

        public SaveIdleGameData GameData { get; private set; }
        public SaveIdleGameStableData StableGameData { get; private set; }

        public void Init(IEnumerable<BaseRoom> functionRoomsList, List<HireEmployeePoint> hireEmployeePoints,
            string gameVersion, int startMoney, int startCapacity, float startSpeed)
        {
            // IdleGameKey += gameVersion;
            var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
            if (mapIndex != 1)
            {
                IdleGameKey += mapIndex;
            }
            StableGameData = new SaveIdleGameStableData
            {
                firstPlayTime = DateTime.Now.Ticks
            };
            if (ES3.KeyExists(StableGameKey))
            {
                // Debug.Log("StableGameKey Exists");
                StableGameData = ES3.Load<SaveIdleGameStableData>(StableGameKey);
            }
            var isStartData = true;
            if (ES3.KeyExists(IdleGameKey))
            {
                // Debug.Log("IdleGameKey Exists");
                GameData = ES3.Load<SaveIdleGameData>(IdleGameKey);
                if (GameData != null && GameData.version == gameVersion)
                    isStartData = false;
            }

            if(isStartData)
            {
                // Debug.Log("isStartData");
                
                // foreach(var key in ES3.GetKeys())
                // {
                //     if(key != StableGameKey)
                //         ES3.DeleteKey(key);
                // }
                
                GameData = new SaveIdleGameData
                {
                    firstPlayTimeOfTheDay = DateTime.Now.Ticks,
                    version = gameVersion
                };

                // FacilityManager.Instance.QuestManager.ResetQuestConfig();
                var roomsList = functionRoomsList.Select(room => new RoomData
                {
                    AreaNumber = room.AreaNumber,
                    Id = room.Id,
                    Name = room.name,
                    Type = room.Type,
                }).ToList();
                GameData.roomsList = roomsList;
                GameData.CountClientActionDictionary = new Dictionary<WarBaseClient.ClientEmotionState, int>();
                GameData.HireEmployeePointsDictionary = new Dictionary<string, bool>();
                foreach (var point in hireEmployeePoints)
                {
                    GameData.HireEmployeePointsDictionary[point.name] = false;
                }

                GameData.money = startMoney;
                GameData.capacity = startCapacity;
                GameData.worker1Capacity = startCapacity;
                GameData.worker2Capacity = startCapacity;
                GameData.worker3Capacity = startCapacity;
                GameData.worker4Capacity = startCapacity;
                GameData.speed = startSpeed;
                GameData.level = 1;
                GameData.levelProgress = 0;
                GameData.dailyQuestData = new DailyQuestData
                {
                    reward = FacilityManager.Instance.DailyQuestConfig.reward,
                    questList = FacilityManager.Instance.DailyQuestConfig.questList,
                    rewardType = FacilityManager.Instance.DailyQuestConfig.rewardType
                };

                GameData.buyBedRoomStaffsList = new List<BuyWorkerData>();
                foreach (var data in FacilityManager.Instance.workerConfig.buyBedRoomStaffsList)
                {
                    var buyWorkerData = new BuyWorkerData
                    {
                        id = data.id,
                        price = data.price,
                        levelActive = data.levelActive
                    };
                    GameData.buyBedRoomStaffsList.Add(buyWorkerData);
                }

                GameData.buyBoxingStaffsList = new List<BuyWorkerData>();
                foreach (var data in FacilityManager.Instance.workerConfig.buyBoxingStaffsList)
                {
                    var buyWorkerData = new BuyWorkerData
                    {
                        id = data.id,
                        price = data.price,
                        levelActive = data.levelActive
                    };
                    GameData.buyBoxingStaffsList.Add(buyWorkerData);
                }

                GameData.buyTrainingStaffsList = new List<BuyWorkerData>();
                foreach (var data in FacilityManager.Instance.workerConfig.buyTrainingStaffsList)
                {
                    var buyWorkerData = new BuyWorkerData
                    {
                        id = data.id,
                        price = data.price,
                        levelActive = data.levelActive
                    };
                    GameData.buyTrainingStaffsList.Add(buyWorkerData);
                }

                GameData.buyDiningStaffsList = new List<BuyWorkerData>();
                foreach (var data in FacilityManager.Instance.workerConfig.buyDiningStaffsList)
                {
                    var buyWorkerData = new BuyWorkerData
                    {
                        id = data.id,
                        price = data.price,
                        levelActive = data.levelActive
                    };
                    GameData.buyDiningStaffsList.Add(buyWorkerData);
                }

                // FacilityManager.Instance.QuestManager.ResetQuestConfig();
                
                // Debug.Log("end isStartData");

                // OrderToSaveData();
            }
        }

        public void UpdateHireEmployee(HireEmployeePoint point)
        {
            GameData.HireEmployeePointsDictionary[point.name] = true;

            if (point.name.Contains("bedroom", StringComparison.OrdinalIgnoreCase))
            {
                FireBaseManager.Instance.LogEvent(FireBaseManager.STAFF_BUY_BED_ROOM);
            }
            else if (point.name.Contains("boxing", StringComparison.OrdinalIgnoreCase))
            {
                FireBaseManager.Instance.LogEvent(FireBaseManager.STAFF_BUY_BOXING_ROOM);
            }
            else if (point.name.Contains("training", StringComparison.OrdinalIgnoreCase))
            {
                FireBaseManager.Instance.LogEvent(FireBaseManager.STAFF_BUY_TRAINING_ROOM);
            }
            else if (point.name.Contains("dining", StringComparison.OrdinalIgnoreCase))
            {
                FireBaseManager.Instance.LogEvent(FireBaseManager.STAFF_BUY_DINING_ROOM);
            }
            
            OrderToSaveData(true);
        }

        private DateTime _lastTimeSaveData;
        private Tween _tweenToSaveData;

        public void OrderToSaveData(bool isImmediate = false)
        {
            _tweenToSaveData?.Kill();
#if UNITY_EDITOR
            DelayToSaveData();
            return;
#endif
            switch (isImmediate)
            {
                case false when (DateTime.Now - _lastTimeSaveData).TotalSeconds >= 60:
                    _tweenToSaveData = DOVirtual.DelayedCall(1, DelayToSaveData);
                    break;
                case true:
                    _tweenToSaveData = DOVirtual.DelayedCall(3, DelayToSaveData);
                    break;
            }
        }

        private void DelayToSaveData()
        {
            // Debug.Log("SAVE DATA");
            ES3.Save(IdleGameKey, GameData);
            ES3.Save(StableGameKey, StableGameData);
            _lastTimeSaveData = DateTime.Now;
        }

        public void SaveDataNew()
        {
            if ((DateTime.Now - _lastTimeSaveData).TotalSeconds > 2)
            {
                _lastTimeSaveData = DateTime.Now;
                ES3.Save(IdleGameKey, GameData);
                ES3.Save(StableGameKey, StableGameData);
            }
        }

        public void UpdateRooms(List<BaseRoom> baseRooms)
        {
            for (var i = 0; i < GameData.roomsList.Count; i++)
            {
                foreach (var baseRoom in baseRooms)
                {
                    if (baseRoom && GameData.roomsList[i].Name == baseRoom.name)
                    {
                        var roomData = GetRoomDataByRoomBase(baseRoom);
                        GameData.roomsList[i] = roomData;
                    }
                }
            }

            OrderToSaveData();
        }

        public void UpdateTemporaryBedRoom(BaseRoom baseRoom)
        {
            for (var i = 0; i < GameData.roomsList.Count; i++)
            {
                if (baseRoom && GameData.roomsList[i].Name == baseRoom.name)
                {
                    var roomData = GetRoomDataByRoomBase(baseRoom);
                    roomData.Type = 0;
                    if (baseRoom.IsBuilt)
                    {
                        roomData.Level++;
                        roomData.Type = 3;
                    }

                    roomData.IsBuilt = true;
                    GameData.roomsList[i] = roomData;
                }
            }

            OrderToSaveData(true);
        }

        private RoomData GetRoomDataByRoomBase(BaseRoom room)
        {
            var roomData = new RoomData
            {
                Name = room.name,
                Cash = room.GetTotalCash(),
                IsBuilt = true,
                IsUnLocked = room.IsUnlocked,
                Level = room.Level,
                AreaNumber = room.AreaNumber,
                Id = room.Id,
                Hp = room.TotalHp,
                Type = room.Type,
                IsDamaged = room.IsDamagedRoom,
            };
            var clientList = new List<ClientData>();
            foreach (var client in room.ClientsList)
            {
                if (!client) continue;
                var clientData = new ClientData
                {
                    Id = client.Id,
                    Level = client.Level,
                    Type = client.Type,
                    DoneList = client.DoneList
                };
                clientList.Add(clientData);
            }

            roomData.ClientsList = clientList;

            var reinforcesList = new List<ClientData>();
            foreach (var client in room.reinforceAlliesList)
            {
                if (!client) continue;
                var clientData = new ClientData
                {
                    Id = client.Id,
                    Level = client.Level,
                    Type = client.Type,
                    DoneList = client.DoneList
                };
                reinforcesList.Add(clientData);
            }

            roomData.ReinforcesList = reinforcesList;

            var waitingClientList = new List<ClientData>();
            foreach (var client in room.WaitingClientsList)
            {
                if (!client) continue;
                var clientData = new ClientData
                {
                    Id = client.Id,
                    Level = client.Level,
                    Type = client.Type,
                    DoneList = client.DoneList
                };
                waitingClientList.Add(clientData);
            }

            roomData.WaitingClientsList = waitingClientList;

            return roomData;
        }

        public void UpdateCompletedGuide()
        {
            GameData.countCompletedGuide++;
            OrderToSaveData();
        }
    }
}