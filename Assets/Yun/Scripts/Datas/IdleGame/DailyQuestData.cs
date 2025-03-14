using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class DailyQuestData
    {
        public string hoursLeftString;
        public string minutesLeftString;
        public List<DailyQuestDataConfig> questList;
        public int countProgress;
        public int reward;
        public DailyQuestDataConfig.RewardType rewardType;
        public bool isReceived;
    }
}
