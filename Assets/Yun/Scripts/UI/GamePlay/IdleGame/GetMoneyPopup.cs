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
    public class GetMoneyPopup : MonetizationPopup
    {
        [SerializeField] private TextMeshProUGUI rewardTxt;
        protected override void Awake()
        {
            base.Awake();
            
            rewardTxt = RootTransform.Find("Content").transform.Find("Reward_txt").GetComponent<TextMeshProUGUI>();
            rewardTxt.text = "";

            if (NativeAds && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeGetMoneyPopup)
                NativeAds.SetActive(false);
            

            if (FireBaseManager.Instance && FireBaseManager.Instance.showMrecGetMoneyPopup)
            {
                Advertisements.Instance.HideBanner();
                AdsManager.Instance.ShowMrec();
            }
        }

        public void SetReward(int reward)
        {
            rewardTxt.text = reward.ToString();
        }
        
        public void SetType(int type)
        {
            Transform modelTransform;
            if (FireBaseManager.Instance.showNoel)
            {
                RenderModel.transform.Find("Noel").gameObject.SetActive(true);
                RenderModel.transform.Find("Normal").gameObject.SetActive(false);
                modelTransform = RenderModel.transform.Find("Noel");
            }
            else
            {
                RenderModel.transform.Find("Noel").gameObject.SetActive(false);
                RenderModel.transform.Find("Normal").gameObject.SetActive(true);
                modelTransform = RenderModel.transform.Find("Normal");
            }

            if (modelTransform)
            {
                modelTransform.Find("Money_Bag").gameObject.SetActive(false);
                modelTransform.Find("Money_Case").gameObject.SetActive(false);
                modelTransform.Find("Money_Safe").gameObject.SetActive(false);
                if (type == 0)
                    modelTransform.Find("Money_Case").gameObject.SetActive(true);
                else if (type == 1)
                    modelTransform.Find("Money_Bag").gameObject.SetActive(true);
                else if (type == 2)
                    modelTransform.Find("Money_Case").gameObject.SetActive(true);
            }
        }
        
      
    }
}