using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class RoomData
    {
        public bool IsDamaged;
        public bool IsBuilt;
        public bool IsUnLocked;
        public int Level;
        public int AreaNumber;
        public string Name;
        public int Id;
        public int Cash;
        public int Hp;
        public int Type;
        public List<ClientData> ClientsList = new List<ClientData>();
        public List<ClientData> ReinforcesList = new List<ClientData>();
        public List<ClientData> WaitingClientsList = new List<ClientData>();
    }
}
