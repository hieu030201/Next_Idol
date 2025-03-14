using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class ClientData
    {
        public int Id;
        public int Level;
        public WarBaseClient.ClientType Type;
        public Dictionary<WarBaseClient.ClientEmotionState, int> DoneList;
    }
}
