using System;
using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.Ads;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class GetBoosterPopup : MonetizationPopup
    {
        protected override void Awake()
        {
            base.Awake();

            if (NativeAds && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeSpeedBoosterPopup)
                NativeAds.SetActive(false);

            if (FireBaseManager.Instance && FireBaseManager.Instance.showMrecSpeedBoosterPopup)
            {
                Advertisements.Instance.HideBanner();
                AdsManager.Instance.ShowMrec();
            }

            var rewardTxt = RootTransform.Find("Content").Find("Reward_txt").GetComponent<TextMeshProUGUI>();
            var mainRewardTxt = rewardTxt.transform.Find("Main_txt").GetComponent<TextMeshProUGUI>();
            var minutes = Math.Floor((double)FacilityManager.Instance.PlayerConfig.SpeedBoosterTime / 60);
            rewardTxt.text = "<size=60>+ 200%</size>  speed in " + minutes + " minutes";
            mainRewardTxt.text = "<color=#FEC501><size=60>+ 300%</size></color>  speed in " + minutes + " minutes";
        }
    }
}