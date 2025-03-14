using System;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class SaveIdleGameData
    {
        public string version;
        public bool isFinishedBattleGuide;
        public bool isShowedStartBattleTutorial;
        public bool isShowedCountSoldierTutorial;
        public bool isShowedRateUs;
        public bool isShowedChangeMapTutorial;
        public bool isShowedSpinWheelTutorial;
        public bool isShowedBattleFundTutorial;
        public bool isShowedPlayerSkinTutorial;
        public bool isFirstTimeShowUpgradeWorker = true;
        public long firstPlayTimeOfTheDay;
        public bool isFirstTimeShowVipSoldier = true;
        public int countFirstClient;
        public bool isFirstTimePlaying = true;
        public int money;
        public int gem;
        public int token;
        public int capacity;
        public int worker1Capacity;
        public int worker2Capacity;
        public int worker3Capacity;
        public int worker4Capacity;
        public float speed;
        public int level;
        public int levelProgress;
        public List<RoomData> roomsList;
        public int countCompletedGuide = 0;
        public Dictionary<string, bool> HireEmployeePointsDictionary;
        // Biến này dùng để kiểm tra lần đầu xây 1 phòng chức năng thì ưu tiên lính ra task về phòng đó , và lần đầu có lính về phòng đó thì sẽ có mũi tên hướng dẫn
        public Dictionary<WarBaseClient.ClientEmotionState, int> CountClientActionDictionary;
        public DailyQuestData dailyQuestData;
        public List<EnemyData> enemiesList = new ();
        public List<BuyWorkerData> buyBedRoomStaffsList;
        public List<BuyWorkerData> buyBoxingStaffsList;
        public List<BuyWorkerData> buyTrainingStaffsList;
        public List<BuyWorkerData> buyDiningStaffsList;
        public List<BattleVehicleData> tanksList = new ();
        public List<BattleVehicleData> armoredList = new ();
        public List<BattleVehicleData> missilesList = new ();
        public int metalTagNumber = 0;
        public int battlePassLevel = 0;
        public float levelPercentageBattlePass = 0;
     
        public List<int> listIDBattlePassFree = new();
        public List<int> listIDBattlePassHero = new();
        public List<int> listIDBattlePassLegend = new();
        public List<string> logList = new();

        public long spinLastTime;
    }
}
