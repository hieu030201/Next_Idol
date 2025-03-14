using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "DailyGiftConfig", menuName = "GameConfig/DailyGift")]
    public class DailyGiftConfig : ScriptableObject
    {
        public List<DailyGiftDataConfig> giftList = new List<DailyGiftDataConfig>();
        
        [Button]
        public void ClickToFillData()  
        {  
            foreach (var gift in giftList)  
            {  
                gift.PopulateChangeEnumString(); 
            }  
        } 
    }
}
