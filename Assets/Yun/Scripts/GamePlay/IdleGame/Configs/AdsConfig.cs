using System.Collections.Generic;
using Gley.EasyIAP;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "AdsConfig", menuName = "GameConfig/Ads")]
    public class AdsConfig : ScriptableObject
    {
        public int interAdsDelay = 180;
        public int minTimeToNextAds = 30;
    }
}
