using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "GameConfig/Player")]
    public class PlayerConfig : ScriptableObject
    {
        public float Speed;
        public float StartCollectMoneyRadius = 1.5f;
        public float BoosterCollectMoneyRadius = 3f;
        public int StartingMoney = 30;
        public int StartingCapacity = 1;
        public int ShowVipSoldierDelay = 180;
        public int ShowSpeedBoosterDelay = 180;
        public int UpgradeWorkerTime = 180;
        public int VipSoldierTime = 180;
        public int SpeedBoosterTime = 180;
        public int WorkerSpeedBoosterTime = 180;
        public int DesertTime = 60;
        public int GemCostForSkipAds = 3;
        public List<SpeedData> UpgradeSpeedsList = new List<SpeedData>();
        public List<CapacityData> UpgradCapacitysList = new List<CapacityData>();
    }
}
