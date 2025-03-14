using System;
using System.Collections.Generic;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class BattleData
    {
        public int enemyLv1 = 1;
        public int enemyLv2 = 1;
        public int enemyLv3 = 1;
        public int minSoldierToStart;
        public string battleRoomName;
        public int tank1Price = 100;
        public int tank2Price = 100;
        public int missilePrice = 100;
        public int missile2Price = 100;
        public int armoredPrice = 100;
        public int armored2Price = 100;
        public int reward = 2;
        public List<int> tank1Prices = new List<int>();
        public List<int> tank2Prices = new List<int>();
        public List<int> armored1Prices = new List<int>();
        public List<int> armored2Prices = new List<int>();
        public List<int> missile1Prices = new List<int>();
        public List<int> missile2Prices = new List<int>();
        public List<BattleMilestoneData> milestonesList = new List<BattleMilestoneData>();
    }
}
