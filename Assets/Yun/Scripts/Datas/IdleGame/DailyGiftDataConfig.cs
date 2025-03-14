using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class DailyGiftDataConfig
    {
        public int reward;
        public DailyQuestDataConfig.RewardType rewardType;
        public string strRewardType;
        public void PopulateChangeEnumString()  
        {  
            strRewardType = rewardType.ToString(); 
        }  
    }
}
