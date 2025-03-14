using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Employees;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Client = Yun.Scripts.GamePlay.IdleGame.Clients.Client;
using Random = System.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Logics
{
    public abstract class IdleGameLogic
    {
        public static bool CheckHaveDailyGift()
        {
            var data = GetDailyGiftData();
            return data.currentDay != FacilityManager.Instance.GameSaveLoad.StableGameData.lastReceivedGiftDay;
        }

        public static void CheckAnotherDay()
        {
            var currentTime = DateTime.Now;
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.lastCheckedDate.Date != currentTime.Date)
            {
                FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds = 15;
                FacilityManager.Instance.GameSaveLoad.StableGameData.lastCheckedDate = currentTime;
                FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            }
        }

        public static void CheckAnotherDayDailyShop()
        {
            var currentTime = DateTime.Now;
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.lastCheckedRewardDailyShop.Date !=
                currentTime.Date)
            {
                FacilityManager.Instance.GameSaveLoad.StableGameData.freeInShopAmount = 1;
                FacilityManager.Instance.GameSaveLoad.StableGameData.rewardAdsPackOne = 2;
                FacilityManager.Instance.GameSaveLoad.StableGameData.rewardAdsPackTwo = 3;
                FacilityManager.Instance.GameSaveLoad.StableGameData.rewardAdsPackThree = 3;
                FacilityManager.Instance.GameSaveLoad.StableGameData.lastCheckedRewardDailyShop = currentTime;

                FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            }
        }

        public static void CheckAnotherDaySpin()
        {
            var currentTime = DateTime.Now;
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.lastCheckedRewardDailyShop.Date !=
                currentTime.Date)
            {
                FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinInSpin =
                    FacilityManager.Instance.SkinDataCollection.GetItemRewardNotUnlock().id;
                FacilityManager.Instance.GameSaveLoad.StableGameData.lastCheckedRewardDailyShop = currentTime;
            }
        }
        // public static int GetDayInData()
        // {
        //     var currentTime = DateTime.Now;
        //     Debug.Log(currentTime);
        //     TimeSpan elapsedTime = currentTime -
        //                            new DateTime(FacilityManager.Instance.GameSaveLoad.StableGameData.firstPlayTime);
        //     var days = elapsedTime.Days % 30;
        //     Debug.Log("Day Ads:" + days);
        //     return days;
        // }

        public static DailyGiftData GetDailyGiftData()
        {
            var currentTime = DateTime.Now;
            if (FacilityManager.Instance.testGameConfig.IsTestGame)
                currentTime = currentTime.AddDays(FacilityManager.Instance.GameSaveLoad.StableGameData.addDay);
            // Tính toán khoảng thời gian giữa thời gian hiện tại và thời gian đã lưu
            var elapsedTime = currentTime -
                              new DateTime(FacilityManager.Instance.GameSaveLoad.StableGameData.firstPlayTime);

            var minutes = Math.Floor(elapsedTime.TotalMinutes) % (24 * 60);
            var minutesLeft = 24 * 60 - minutes;
            var hoursLeft = Math.Floor(minutesLeft / 60);
            var hoursLeftString = hoursLeft < 10 ? "0" + hoursLeft : hoursLeft.ToString(CultureInfo.InvariantCulture);
            minutesLeft -= hoursLeft * 60;
            var minutesLeftString =
                minutesLeft < 10 ? "0" + minutesLeft : minutesLeft.ToString(CultureInfo.InvariantCulture);

            var data = new DailyGiftData
            {
                currentDay = elapsedTime.Days % 30,
                minutesLeftString = minutesLeftString,
                hoursLeftString = hoursLeftString
            };

            return data;
        }

        public static DailyQuestData GetDailyQuestData()
        {
            var currentTime = DateTime.Now;
            // Tính toán khoảng thời gian giữa thời gian hiện tại và thời gian đã lưu
            var elapsedTime = currentTime -
                              new DateTime(FacilityManager.Instance.GameSaveLoad.GameData.firstPlayTimeOfTheDay);

            if (elapsedTime.TotalMinutes / 60 >= 24)
            {
                var newDay = new DateTime(FacilityManager.Instance.GameSaveLoad.GameData.firstPlayTimeOfTheDay);
                newDay = newDay.AddDays(elapsedTime.Days);
                FacilityManager.Instance.GameSaveLoad.GameData.firstPlayTimeOfTheDay = newDay.Ticks;
                FacilityManager.Instance.QuestManager.ResetQuestConfig();
            }

            var minutes = Math.Floor(elapsedTime.TotalMinutes) % (24 * 60);
            var minutesLeft = 24 * 60 - minutes;
            var hoursLeft = Math.Floor(minutesLeft / 60);
            var hoursLeftString = hoursLeft < 10 ? "0" + hoursLeft : hoursLeft.ToString(CultureInfo.InvariantCulture);
            minutesLeft -= hoursLeft * 60;
            var minutesLeftString =
                minutesLeft < 10 ? "0" + minutesLeft : minutesLeft.ToString(CultureInfo.InvariantCulture);

            var data = new DailyQuestData()
            {
                minutesLeftString = minutesLeftString,
                hoursLeftString = hoursLeftString
            };

            Debug.Log("GetDailyGiftData: " + data);

            return data;
        }

        public static bool CheckHaveCompletedQuest()
        {
            var dailyQuestConfig = FacilityManager.Instance.GameSaveLoad.GameData.dailyQuestData;
            foreach (var quest in dailyQuestConfig.questList)
            {
                if (quest.quantity == 0)
                {
                    if (!quest.isReceived && quest.countProgress > 0)
                        return true;
                }
                else if (!quest.isReceived && quest.countProgress >= quest.quantity)
                {
                    return true;
                }
            }

            if (dailyQuestConfig.countProgress >=
                dailyQuestConfig.questList.Count)
            {
                if (!dailyQuestConfig.isReceived)
                    return true;
            }

            return false;
        }

        public static UpgradePopupData CreatePlayerUpgradePopupData(int currentLevel, int currentMoney,
            float currentSpeed, int currentCapacity,
            PlayerConfig playerConfig, int currentWorker1Capacity, int currentWorker2Capacity,
            int currentWorker3Capacity, int currentWorker4Capacity, WorkerConfig workerConfig)
        {
            var upgradePopupData = new UpgradePopupData
            {
                PlayerMoney = currentMoney
            };
            SpeedData playerSpeedData = null;
            for (var i = 0; i < playerConfig.UpgradeSpeedsList.Count; i++)
            {
                if (!Mathf.Approximately(playerConfig.UpgradeSpeedsList[i].SpeedNumber, currentSpeed)) continue;
                if (i >= playerConfig.UpgradeSpeedsList.Count - 1) continue;
                playerSpeedData = playerConfig.UpgradeSpeedsList[i + 1];
                upgradePopupData.UpgradePlayerSpeedLevel = i + 2;
                i = playerConfig.UpgradeSpeedsList.Count;
            }

            if (currentSpeed < playerConfig.UpgradeSpeedsList[0].SpeedNumber)
            {
                playerSpeedData = playerConfig.UpgradeSpeedsList[0];
                upgradePopupData.UpgradePlayerSpeedLevel = 1;
            }

            if (playerSpeedData != null)
            {
                upgradePopupData.UpgradePlayerSpeedPrice = playerSpeedData.Price;
                upgradePopupData.UpgradePlayerSpeedLevelActive =
                    currentLevel < playerSpeedData.LevelActive ? playerSpeedData.LevelActive : 0;
            }

            CapacityData playerCapacityData = null;
            for (var i = 0; i < playerConfig.UpgradCapacitysList.Count; i++)
            {
                if (playerConfig.UpgradCapacitysList[i].CapacityNumber == currentCapacity)
                {
                    if (i < playerConfig.UpgradCapacitysList.Count - 1)
                    {
                        playerCapacityData = playerConfig.UpgradCapacitysList[i + 1];
                        upgradePopupData.UpgradePlayerCapacityLevel = i + 2;
                        i = playerConfig.UpgradCapacitysList.Count;
                    }
                }
            }

            if (currentCapacity < playerConfig.UpgradCapacitysList[0].CapacityNumber)
            {
                playerCapacityData = playerConfig.UpgradCapacitysList[0];
                upgradePopupData.UpgradePlayerCapacityLevel = 1;
            }

            if (playerCapacityData != null)
            {
                upgradePopupData.UpgradePlayerCapacityPrice = playerCapacityData.Price;
                upgradePopupData.UpgradePlayerCapacityLevelActive = currentLevel < playerCapacityData.LevelActive
                    ? playerCapacityData.LevelActive
                    : 0;
            }

            CapacityData worker1CapacityData = null;
            for (var i = 0; i < workerConfig.UpgradeWorker1CapacitysList.Count; i++)
            {
                if (workerConfig.UpgradeWorker1CapacitysList[i].CapacityNumber == currentWorker1Capacity)
                {
                    if (i < workerConfig.UpgradeWorker1CapacitysList.Count - 1)
                    {
                        worker1CapacityData = workerConfig.UpgradeWorker1CapacitysList[i + 1];
                        upgradePopupData.UpgradeWorker1CapacityLevel = i + 2;
                        i = workerConfig.UpgradeWorker1CapacitysList.Count;
                    }
                }
            }

            if (currentWorker1Capacity < workerConfig.UpgradeWorker1CapacitysList[0].CapacityNumber)
            {
                worker1CapacityData = workerConfig.UpgradeWorker1CapacitysList[0];
                upgradePopupData.UpgradeWorker1CapacityLevel = 1;
            }

            if (worker1CapacityData != null)
            {
                upgradePopupData.UpgradeWorker1CapacityPrice = worker1CapacityData.Price;
                upgradePopupData.UpgradeWorker1CapacityLevelActive = currentLevel < worker1CapacityData.LevelActive
                    ? worker1CapacityData.LevelActive
                    : 0;
            }

            CapacityData worker2CapacityData = null;
            for (var i = 0; i < workerConfig.UpgradeWorker2CapacitysList.Count; i++)
            {
                if (workerConfig.UpgradeWorker2CapacitysList[i].CapacityNumber == currentWorker2Capacity)
                {
                    if (i < workerConfig.UpgradeWorker2CapacitysList.Count - 1)
                    {
                        worker2CapacityData = workerConfig.UpgradeWorker2CapacitysList[i + 1];
                        upgradePopupData.UpgradeWorker2CapacityLevel = i + 2;
                        i = workerConfig.UpgradeWorker2CapacitysList.Count;
                    }
                }
            }

            if (currentWorker2Capacity < workerConfig.UpgradeWorker2CapacitysList[0].CapacityNumber)
            {
                worker2CapacityData = workerConfig.UpgradeWorker2CapacitysList[0];
                upgradePopupData.UpgradeWorker2CapacityLevel = 1;
            }

            if (worker2CapacityData != null)
            {
                upgradePopupData.UpgradeWorker2CapacityPrice = worker2CapacityData.Price;
                upgradePopupData.UpgradeWorker2CapacityLevelActive = currentLevel < worker2CapacityData.LevelActive
                    ? worker2CapacityData.LevelActive
                    : 0;
            }

            CapacityData worker3CapacityData = null;
            for (var i = 0; i < workerConfig.UpgradeWorker3CapacitysList.Count; i++)
            {
                if (workerConfig.UpgradeWorker3CapacitysList[i].CapacityNumber == currentWorker3Capacity)
                {
                    if (i < workerConfig.UpgradeWorker3CapacitysList.Count - 1)
                    {
                        worker3CapacityData = workerConfig.UpgradeWorker3CapacitysList[i + 1];
                        upgradePopupData.UpgradeWorker3CapacityLevel = i + 2;
                        i = workerConfig.UpgradeWorker3CapacitysList.Count;
                    }
                }
            }

            if (currentWorker3Capacity < workerConfig.UpgradeWorker3CapacitysList[0].CapacityNumber)
            {
                worker3CapacityData = workerConfig.UpgradeWorker3CapacitysList[0];
                upgradePopupData.UpgradeWorker3CapacityLevel = 1;
            }

            if (worker3CapacityData != null)
            {
                upgradePopupData.UpgradeWorker3CapacityPrice = worker3CapacityData.Price;
                upgradePopupData.UpgradeWorker3CapacityLevelActive = currentLevel < worker3CapacityData.LevelActive
                    ? worker3CapacityData.LevelActive
                    : 0;
            }

            CapacityData worker4CapacityData = null;
            for (var i = 0; i < workerConfig.UpgradeWorker4CapacitysList.Count; i++)
            {
                if (workerConfig.UpgradeWorker4CapacitysList[i].CapacityNumber == currentWorker4Capacity)
                {
                    if (i < workerConfig.UpgradeWorker4CapacitysList.Count - 1)
                    {
                        worker4CapacityData = workerConfig.UpgradeWorker4CapacitysList[i + 1];
                        upgradePopupData.UpgradeWorker4CapacityLevel = i + 2;
                        i = workerConfig.UpgradeWorker4CapacitysList.Count;
                    }
                }
            }

            if (currentWorker4Capacity < workerConfig.UpgradeWorker4CapacitysList[0].CapacityNumber)
            {
                worker4CapacityData = workerConfig.UpgradeWorker4CapacitysList[0];
                upgradePopupData.UpgradeWorker4CapacityLevel = 1;
            }

            if (worker4CapacityData == null) return upgradePopupData;
            upgradePopupData.UpgradeWorker4CapacityPrice = worker4CapacityData.Price;
            upgradePopupData.UpgradeWorker4CapacityLevelActive = currentLevel < worker4CapacityData.LevelActive
                ? worker4CapacityData.LevelActive
                : 0;

            return upgradePopupData;
        }

        public static bool CheckCanBuyWorker()
        {
            var bedroomStaffsList = FacilityManager.Instance.GameSaveLoad.GameData.buyBedRoomStaffsList;
            foreach (var data in bedroomStaffsList)
            {
                if (data.isBought) continue;
                if (data.levelActive > FacilityManager.Instance.IdleGameData.Level) continue;
                if (FacilityManager.Instance.IdleGameData.Money >= data.price)
                    return true;
            }

            var trainingStaffsList = FacilityManager.Instance.GameSaveLoad.GameData.buyTrainingStaffsList;
            foreach (var data in trainingStaffsList)
            {
                if (data.isBought) continue;
                if (data.levelActive > FacilityManager.Instance.IdleGameData.Level) continue;
                if (FacilityManager.Instance.IdleGameData.Money >= data.price)
                    return true;
            }

            var boxingStaffsList = FacilityManager.Instance.GameSaveLoad.GameData.buyBoxingStaffsList;
            foreach (var data in boxingStaffsList)
            {
                if (data.isBought) continue;
                if (data.levelActive > FacilityManager.Instance.IdleGameData.Level) continue;
                if (FacilityManager.Instance.IdleGameData.Money >= data.price)
                    return true;
            }

            var diningStaffsList = FacilityManager.Instance.GameSaveLoad.GameData.buyDiningStaffsList;
            foreach (var data in diningStaffsList)
            {
                if (data.isBought) continue;
                if (data.levelActive > FacilityManager.Instance.IdleGameData.Level) continue;
                if (FacilityManager.Instance.IdleGameData.Money >= data.price)
                    return true;
            }

            return false;
        }

        public static bool CheckCanUpgrade(int currentLevel, float currentSpeed, int currentCapacity,
            PlayerConfig playerConfig)
        {
            SpeedData speedData = null;
            for (var i = 0; i < playerConfig.UpgradeSpeedsList.Count; i++)
            {
                if (!Mathf.Approximately(playerConfig.UpgradeSpeedsList[i].SpeedNumber, currentSpeed)) continue;
                if (i >= playerConfig.UpgradeSpeedsList.Count - 1) continue;
                speedData = playerConfig.UpgradeSpeedsList[i + 1];
                i = playerConfig.UpgradeSpeedsList.Count;
            }

            if (currentSpeed < playerConfig.UpgradeSpeedsList[0].SpeedNumber)
            {
                speedData = playerConfig.UpgradeSpeedsList[0];
            }

            if (speedData != null)
            {
                if (currentLevel < speedData.LevelActive)
                {
                }
                else
                {
                    return true;
                }
            }

            CapacityData capacityData = null;
            for (var i = 0; i < playerConfig.UpgradCapacitysList.Count; i++)
            {
                if (playerConfig.UpgradCapacitysList[i].CapacityNumber == currentCapacity)
                {
                    if (i < playerConfig.UpgradCapacitysList.Count - 1)
                    {
                        capacityData = playerConfig.UpgradCapacitysList[i + 1];
                        i = playerConfig.UpgradCapacitysList.Count;
                    }
                }
            }

            if (currentCapacity < playerConfig.UpgradCapacitysList[0].CapacityNumber)
            {
                capacityData = playerConfig.UpgradCapacitysList[0];
            }

            if (capacityData != null)
            {
                if (currentLevel < capacityData.LevelActive)
                {
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckMoneyEnoughToBuildMinRoom(List<BaseRoom> roomsList, int money)
        {
            var minPrice = 0;
            foreach (var room in roomsList)
            {
                if (room.IsBuildable)
                {
                    if (room.prices.Count >= room.Level && room.prices[room.Level - 1] < minPrice)
                        minPrice = room.prices[room.Level - 1];
                }
            }

            return money >= minPrice;
        }

        public static bool CheckMoneyEnoughToHireEmployee(List<HireEmployeePoint> hireEmployeePoints, int money)
        {
            foreach (var point in hireEmployeePoints)
            {
                if (point && point.gameObject.activeSelf && point.Price > money)
                    return false;
            }

            return true;
        }

        public static WarBaseClient.ClientEmotionState GetAvailableTask(List<BaseRoom> roomsList,
            List<WarBaseClient> clientsList, WarBaseClient checkClient)
        {
            // Lọc ra các phòng mà lính chưa từng đến
            var availableRooms = new List<BaseRoom>();
            var optionalRooms = new List<BaseRoom>();
            var doneList = checkClient.DoneList;
            var availableSlots = new Dictionary<WarBaseClient.ClientEmotionState, int>();
            foreach (var room in roomsList)
            {
                if (!room.isFunctionalRoom)
                    continue;
                if (room.isLevelUpRoom)
                {
                    doneList.TryAdd(room.emotionStateToGetIn, 0);
                    if (room.IsBuilt && doneList[room.emotionStateToGetIn] == 0 && !room.IsFull)
                    {
                        availableRooms.Add(room);
                        var countSlots = room.CountSlots;
                        var countClients = room.CountClients();
                        availableSlots.TryAdd(room.emotionStateToGetIn, 0);
                        availableSlots[room.emotionStateToGetIn] += countSlots - countClients;
                    }
                }
                else
                {
                    if (room.IsBuilt && !room.IsFull)
                        optionalRooms.Add(room);
                }
            }

            if (FacilityManager.Instance.MapIndex == 1)
            {
                // Kiểm tra xem có phòng chức năng nào mới xây thì ưu tiên cho lính có task về phòng đó luôn
                var countClientsDict = FacilityManager.Instance.GameSaveLoad.GameData.CountClientActionDictionary;
                foreach (var room in availableRooms)
                {
                    countClientsDict.TryAdd(room.emotionStateToGetIn, 0);
                    if (countClientsDict.ContainsKey(room.emotionStateToGetIn) &&
                        countClientsDict[room.emotionStateToGetIn] == 0)
                        return room.emotionStateToGetIn;
                }

                foreach (var room in optionalRooms)
                {
                    countClientsDict.TryAdd(room.emotionStateToGetIn, 0);
                    if (countClientsDict.ContainsKey(room.emotionStateToGetIn) &&
                        countClientsDict[room.emotionStateToGetIn] == 0)
                        return room.emotionStateToGetIn;
                }
            }

            // if (availableRooms.Count == 0)
            //     return WarBaseClient.ClientEmotionState.WaitToBattle;

            // Số lính đang chờ vào phòng ở các phòng ngủ
            var waitingForRoom = new Dictionary<WarBaseClient.ClientEmotionState, int>();
            foreach (var warBaseClient in clientsList.Select(client => client))
            {
                foreach (var room in availableRooms)
                {
                    waitingForRoom.TryAdd(room.emotionStateToGetIn, 0);
                    if (room.emotionStateToGetIn == warBaseClient.EmotionState)
                    {
                        waitingForRoom[warBaseClient.EmotionState]++;
                    }
                }
            }

            // Kiểm tra xem nếu các slot trống của phòng trừ đi các lính đang chờ mà full thì xóa phòng đó khỏi list
            for (var i = availableRooms.Count - 1; i >= 0; i--)
            {
                if (availableSlots[availableRooms[i].emotionStateToGetIn] -
                    waitingForRoom[availableRooms[i].emotionStateToGetIn] < 0)
                    availableRooms.RemoveAt(i);
            }

            if (availableRooms.Count != 0)
            {
                // Tính toán trọng số cho mỗi phòng dựa trên số người hiện tại
                // Đây là những phòng bắt buộc lính phải qua, là những phòng up level
                var roomWeights = new Dictionary<WarBaseClient.ClientEmotionState, float>();

                foreach (var room in availableRooms)
                {
                    var currentOccupants = room.CountClients() + waitingForRoom[room.emotionStateToGetIn];
                    // Phòng càng đông, trọng số càng thấp
                    roomWeights.TryAdd(room.emotionStateToGetIn, 0);
                    roomWeights[room.emotionStateToGetIn] = 1f / (currentOccupants + 1);
                }

                float maxWeight = 0;
                var state = WarBaseClient.ClientEmotionState.WaitToBattle;

                foreach (var kvp in roomWeights)
                {
                    if (kvp.Value > maxWeight)
                    {
                        maxWeight = kvp.Value;
                        state = kvp.Key;
                    }
                }

                if (state != WarBaseClient.ClientEmotionState.WaitToBattle)
                    return state;
            }

            foreach (var room in optionalRooms)
            {
                doneList.TryAdd(room.emotionStateToGetIn, 0);
                if (doneList[room.emotionStateToGetIn] > 0)
                    return WarBaseClient.ClientEmotionState.WaitToBattle;
            }

            if (optionalRooms.Count == 0) return WarBaseClient.ClientEmotionState.WaitToBattle;
            // var randomNumber = UnityEngine.Random.Range(0, 4);
            // if (randomNumber != 0) return WarBaseClient.ClientEmotionState.WaitToBattle;
            var random = new Random();
            return optionalRooms[random.Next(optionalRooms.Count)].emotionStateToGetIn;
        }
    }
}