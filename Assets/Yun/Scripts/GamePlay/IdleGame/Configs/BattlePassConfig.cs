using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "BattlePassConfig", menuName = "GameConfig/BattlePassConfig")]
    public class BattlePassConfig : ScriptableObject
    {
        public List<BattlePassData> BattlePassDatas = new List<BattlePassData>();
    }
    [Serializable]
    public class BattlePassData
    {
        public int id;
        public bool isUnlock;
        public BattlePassFree battlePassFree;
        public BattlePassHero battlePassHero;
        public BattlePassLegend battlePassLengend;
    }
    [Serializable]
    public class BattlePassFree
    {
        public RewardBattlePassType rewardType;
        public int amount;
        public bool unlock;
        public bool claimed;
    }
    [Serializable]
    public class BattlePassHero
    {
        public RewardBattlePassType rewardType;
        public int amount;
        public bool unlock;
        public bool claimed;
    }
    
    [Serializable]
    public class BattlePassLegend
    {
        public RewardBattlePassType rewardType;
        public int amount;
        public bool unlock;
        public bool claimed;
    }

    public enum RewardBattlePassType
    {
        None = 0,
        Cash = 1,
        Token = 2,
        Diamon = 3,
        Skin = 4,
        RemoveAd = 5,
    }
}

