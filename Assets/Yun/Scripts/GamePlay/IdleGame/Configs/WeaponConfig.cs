using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.GamePlay.IdleGame.Configs;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "GameConfig/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        public List<WeaponData> WeaponDatas = new List<WeaponData>();

    }
    
    [Serializable]
    public class WeaponData
    {
        public int id;
        public NameType nameType;
        public int damageNumber;
        public float speedShooting;
        public int dontMissingPercent;
        public int bulletOfTurn;
    }

}

