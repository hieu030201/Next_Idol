using System;
using System.Collections.Generic;
using System.Linq;
using Adverstising_Integration.Scripts;
using Advertising;
using Gley.EasyIAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Ads;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class BuyNoAdsPopup : BasePopup
    {
        [SerializeField] private GameObject priceTxt;
        [SerializeField] private GameObject priceVipTxt;
        [SerializeField] private GameObject nativeAds;
        [SerializeField] private GameObject contentObj;
        [SerializeField] private GameObject packNoAds;
        [SerializeField] private GameObject packNoAdsVip;
        [SerializeField] private GameObject statusBoughtNoAds;
        [SerializeField] private GameObject statusBoughtNoAdsVip;
        [SerializeField] private Image statusBoughtNoAdsImage;
        [SerializeField] private Image statusBoughtNoAdsVipImage;

        protected override void Awake()
        {
            base.Awake();
            float screenRatio = (float)Screen.height / (float)Screen.width;  

            // Adjust the aspect ratio based on screen dimensions  
            if (screenRatio >= 2.0f) // For ultra-wide screens (e.g., 21:9)  
            {  
                contentObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);   // Set to 2.0  
            }  
            else if (screenRatio >= 1.78f) // ~16:9  
            {  
                contentObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f); 
            }  
            else if (screenRatio >= 1.5f) 
            {  
                contentObj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f); 
            }  
            else if (screenRatio >= 1.33f) // ~4:3  
            {  
                contentObj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f); 
            }  
            else  
            {  
                contentObj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }  

            var priceNoAds = IAPManager.IsInstanceExisted()
                ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.NoAds)
                : "";
            priceTxt.GetComponent<YunTextShadow>().DisplayText = priceNoAds;
            
            var priceNoAdsVip = IAPManager.IsInstanceExisted()
                ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.NoAdsVip)
                : "";
            priceVipTxt.GetComponent<YunTextShadow>().DisplayText = priceNoAdsVip;
            
            if(nativeAds && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeNoAdsPopup)
                nativeAds.SetActive(false);
            
            bool removeAds = FacilityManager.Instance.GameSaveLoad.StableGameData.isPurchasedRemoveAds;
            bool removeAdsVip = FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip;
            
            if (removeAds || removeAdsVip)
            {
                packNoAds.SetActive(false);
            }
            
            packNoAdsVip.SetActive(!removeAdsVip);
            statusBoughtNoAds.SetActive(removeAds || removeAdsVip);
            statusBoughtNoAdsVip.SetActive(removeAdsVip);

            if (removeAds || removeAdsVip)
                SetBgStatusImageNoAds();
            if(removeAdsVip)
                SetBgStatusImageNoAdsVip();
            
            if (FireBaseManager.Instance && FireBaseManager.Instance.showMrecNoAdsPopup)
            {
                Advertisements.Instance.HideBanner();
                AdsManager.Instance.ShowMrec();
            }
        }
        
        public override void Show()
        {
            var content = transform.Find("Content");
            ShowEffect(YunEffectType.FlyUpEffect, content.transform);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(nativeAds && nativeAds.activeSelf)
                nativeAds.GetComponent<NativeManager>().ShowNativeLoading(false);
            
            Advertisements.Instance.ShowBanner();
            AdsManager.Instance.HideMrec();
        }

        public void BuyRemoveAdsClick()
        {
            BuyPack(ShopProductNames.NoAds);
        }
        
        public void BuyRemoveAdsVipClick()
        {
            if (FacilityManager.Instance.testGameConfig.isTestAdsVip)
            {
                FacilityManager.Instance.RemoveAdsVipWhenInAppPurchase();
                StatusWhenBoughtNoAdsVip();
            }
            else
                BuyPack(ShopProductNames.NoAdsVip);
        }

        public void BuyPack(ShopProductNames productNames)
        {
#if UNITY_ANDROID
            if (FireBaseManager.IsInstanceExisted())
            {
                switch (productNames)
                {
                    case ShopProductNames.NoAds:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_REMOVE_ADS_CLICK);
                        break;
                    case ShopProductNames.NoAdsVip:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_REMOVE_ADS_VIP_CLICK);
                        break;
                    case ShopProductNames.Money099:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_1_CLICK);
                        break;
                    case ShopProductNames.Money199:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_2_CLICK);
                        break;
                    case ShopProductNames.Money299:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_3_CLICK);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(productNames), productNames, null);
                }
            }
#endif
/*#if UNITY_EDITOR
            var value = 0;
            foreach (var product in FacilityManager.Instance.iapConfig.ProductsList.Where(product =>
                product.productNames == productNames))
            {
                value = product.value;
            }

            if (value != 0)
                FacilityManager.Instance.AddGemWhenInAppPurchase(value);
            else
                FacilityManager.Instance.RemoveAdsWhenInAppPurchase();

            // Close();
            // return;
#endif*/
#if UNITY_ANDROID
            IAPManager.Instance.BuyProduct(productNames, () =>
            {
                if (FireBaseManager.IsInstanceExisted())
                {
                    switch (productNames)
                    {
                        case ShopProductNames.NoAds:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_REMOVE_ADS_SUCCESS);
                            break;
                        case ShopProductNames.NoAdsVip:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_REMOVE_ADS_VIP_SUCCESS);
                            break;
                        case ShopProductNames.Money099:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_1_SUCCESS);
                            break;
                        case ShopProductNames.Money199:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_2_SUCCESS);
                            break;
                        case ShopProductNames.Money299:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_3_SUCCESS);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(productNames), productNames, null);
                    }
                }

                var value = 0;
                foreach (var product in FacilityManager.Instance.iapConfig.ProductsList.Where(product =>
                    product.productNames == productNames))
                {
                    value = product.value;
                }

                if(productNames == ShopProductNames.NoAds )
                {
                    FacilityManager.Instance.RemoveAdsWhenInAppPurchase();
                    StatusWhenBoughtNoAds();
                    //Close();
                }
                else if (productNames == ShopProductNames.NoAdsVip)
                {
                    FacilityManager.Instance.RemoveAdsVipWhenInAppPurchase();
                    StatusWhenBoughtNoAdsVip();
                    //Close();
                }
                // Close();
            });
#endif
        }
        
        private void StatusWhenBoughtNoAds()
        {
            packNoAds.SetActive(false);
            statusBoughtNoAds.SetActive(true);
            SetBgStatusImageNoAds();
        }

        private void StatusWhenBoughtNoAdsVip()
        {
            packNoAdsVip.SetActive(false);
            packNoAds.SetActive(false);
            
            statusBoughtNoAds.SetActive(true);
            statusBoughtNoAdsVip.SetActive(true);

            SetBgStatusImageNoAds();
            SetBgStatusImageNoAdsVip();
        }

        private void SetBgStatusImageNoAds()
        {
            statusBoughtNoAdsImage.color = new Color32(171, 171, 171, 255);
        }

        private void SetBgStatusImageNoAdsVip()
        {
            statusBoughtNoAdsVipImage.color = new Color32(171, 171, 171, 255);
        }

        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeToBottom);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
        }
    }
}