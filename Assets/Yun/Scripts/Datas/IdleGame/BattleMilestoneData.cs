using System;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class BattleMilestoneData
    {
        public int progress;
        public int reward;
        public DailyQuestDataConfig.RewardType rewardType = DailyQuestDataConfig.RewardType.Money;
    }
}
