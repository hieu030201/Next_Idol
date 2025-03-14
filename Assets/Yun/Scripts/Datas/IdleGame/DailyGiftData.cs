using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class DailyGiftData
    {
        public int currentDay;
        public string hoursLeftString;
        public string minutesLeftString;
    }
}
