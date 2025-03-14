using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class DailyQuestDataConfig
    {
        public enum QuestId
        {
            TrainRookie,
            BuildTrainingRoom,
            UpgradeSleepRoom,
            HireWorker,
            UpgradeCommanderSpeed,
            UpgradeCommanderLoader,
            PlayerLevelUp,
            UseMoneyBag,
            ExpandAreas,
        }

        public enum RewardType
        {
            Money,
            Gem,
            Token,
            Star
        }
        
        public string startDescription;
        public string endDescription;
        public int quantity;
        public int reward;
        public QuestId id;
        public string idType;
        public int countProgress;
        public bool isReceived;
        public RewardType rewardType;
        public string strRewardType;
        
        public void PopulateChangeEnumString()  
        {  
            idType = id.ToString();
            strRewardType = rewardType.ToString();
        }  
    }
}
