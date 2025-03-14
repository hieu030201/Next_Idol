using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "DailyQuestConfig", menuName = "GameConfig/DailyQuest")]
    public class DailyQuestConfig : ScriptableObject
    {
        public List<DailyQuestDataConfig> questList = new List<DailyQuestDataConfig>();
        public int reward;
        public bool isReceived;
        public DailyQuestDataConfig.RewardType rewardType;
        public string strRewardType;
        
        [Button]
        public void ClickToFillData()  
        {  
            foreach (var quest in questList)  
            {  
                quest.PopulateChangeEnumString(); 
            }

            strRewardType = rewardType.ToString();
        } 
    }
}
