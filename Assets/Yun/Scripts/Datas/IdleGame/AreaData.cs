using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class AreaData
    {
        public int Id;
        public List<RoomData> RoomsList = new List<RoomData>();
    }
}
