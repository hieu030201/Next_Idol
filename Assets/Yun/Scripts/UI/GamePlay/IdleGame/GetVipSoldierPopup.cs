using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Ads;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class GetVipSoldierPopup : MonetizationPopup
    {
        protected override void Awake()
        {
            base.Awake();

            if (NativeAds && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeVipSoldierPopup)
                NativeAds.SetActive(false);
            
            if (FireBaseManager.Instance && FireBaseManager.Instance.showMrecVipSoldierPopup)
            {
                Advertisements.Instance.HideBanner();
                AdsManager.Instance.ShowMrec();
            }
        }
    }
}