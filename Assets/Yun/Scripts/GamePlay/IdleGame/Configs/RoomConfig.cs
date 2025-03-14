using System.Collections.Generic;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "RoomConfig", menuName = "GameConfig/RoomConfig")]
    public class RoomConfig : ScriptableObject
    {
        public List<RoomDataConfig> roomsList_Area_1 = new List<RoomDataConfig>();
        public List<RoomDataConfig> roomsList_Area_2 = new List<RoomDataConfig>();
        public List<RoomDataConfig> roomsList_Area_3 = new List<RoomDataConfig>();
        public List<RoomDataConfig> roomsList_Area_4 = new List<RoomDataConfig>();
        public List<RoomDataConfig> roomsList_Area_5 = new List<RoomDataConfig>();
        public List<RoomDataConfig> roomsList_Area_6 = new List<RoomDataConfig>();
    }
}
