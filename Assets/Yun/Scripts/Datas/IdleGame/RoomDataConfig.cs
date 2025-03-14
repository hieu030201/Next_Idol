using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class RoomDataConfig
    {
        public string name;
        public List<int> levelsPlayerToActive;
        public List<int> prices;
        public List<int> stars = new List<int>();
        public string nextWorker;
        public string nextRoom;
        public string nextRoom2;
    }
}
