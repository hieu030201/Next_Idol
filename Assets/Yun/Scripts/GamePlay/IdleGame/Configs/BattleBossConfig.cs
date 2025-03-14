using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "BossDataConfig", menuName = "GameConfig/BossDataConfig")]
    public class BattleBossConfig : MonoBehaviour
    {
        public List<BossData> BossDatas = new List<BossData>();
    }

    [Serializable]
    public class BossData
    {
        public int id;
        public GameObject bossModel;
        public BossLevelType bossLevel;
        public int hp;
        public int damageNumber;
        public float speedShooting;
        public int missingPercent;
    }

    public enum BossLevelType
    {
        BossLevel1 = 0,
        BossLevel2 = 1,
        BossLevel3 = 2,
        BossLevel4 = 3,
        BossLevel5 = 4,
        BossLevel6 = 5,
    }
}
