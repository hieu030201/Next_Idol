using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "AbilitiesConfig", menuName = "GameConfig/AbilitiesConfig")]
    public class AbilitiesConfig : MonoBehaviour
    {
        public List<AbilitiesData> AbilitiesDatas = new List<AbilitiesData>();
    }
    
    [Serializable]
    public class AbilitiesData
    {
        public int id;
        public AbilityItemType abilityItemType;
        public IncomeType incomeType;
        public int pointUpGrade;
        public int percentIncome;
        public bool unlock;
    }

    public enum IncomeType
    {
        CashIncome = 0,
        TokenIncome = 1,
        GemIncome = 2,
        SpeedIncome = 3,
        CashRadius = 4,
        WorkerSpeedIncome = 5,
        VipSoldierIncome = 6,
        Capacity = 7,
    }

    public enum AbilityItemType
    {
        AbilityItemNormal = 0,
        AbilityItemSpecial = 1,
    }

}

