using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "SpinWheelDataConfig", menuName = "GameConfig/SpinWheelDataConfig")]
    public class SpinWheelConfig : MonoBehaviour
    {
        public List<SpinWheelData> SpinWheelDatas = new List<SpinWheelData>();
    }

    [Serializable]
    public class SpinWheelData
    {
        public int id;
        public string name;
        public SpinRewardType spinRewardType;
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public Sprite icon;
        public int amount;
        public int probability;
        public string hexCode;
    }

    public enum SpinRewardType
    {
        Cash = 0,
        Token = 1,
        Gem = 2,
        Skin = 3,
        Booster = 4,
    }
}

