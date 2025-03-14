using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.GamePlay.IdleGame.Configs;

namespace Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs
{
    public class SpinWheelItem : MonoBehaviour
    {
        public int ID;
        public Image icon;
        public Image bg;
        public SpinRewardType spinRewardType;
        public TextMeshProUGUI amount;
    }
}

