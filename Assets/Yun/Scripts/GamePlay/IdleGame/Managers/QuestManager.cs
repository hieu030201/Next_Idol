using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yun.Scripts.Cores;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Managers
{
    public class QuestManager : YunBehaviour
    {
        public static void CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId id)
        {
            var dailyQuestConfig = FacilityManager.Instance.GameSaveLoad.GameData.dailyQuestData;
            foreach (var quest in dailyQuestConfig.questList.Where(quest => quest.id == id))
            {
                quest.countProgress++;
                if (quest.countProgress >= quest.quantity && !quest.isReceived)
                {
                    FacilityManager.Instance.PlayerInfoUI.ShowDailyQuestAnimation();
                }
            }

            if (dailyQuestConfig.countProgress >= dailyQuestConfig.questList.Count && !dailyQuestConfig.isReceived)
                FacilityManager.Instance.PlayerInfoUI.ShowDailyQuestAnimation();
            
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData();
        }

        public void ResetQuestConfig()
        {
            var dailyQuestConfig = FacilityManager.Instance.GameSaveLoad.GameData.dailyQuestData;
            foreach (var quest in dailyQuestConfig.questList)
            {
                quest.countProgress = 0;
                quest.isReceived = false;
            }

            dailyQuestConfig.countProgress = 0;
            dailyQuestConfig.isReceived = false;
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData();
        }
    }
}