using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using Gley.EasyIAP;
using TMPro;
using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.UI;
using Yun.Scripts.UI.GamePlay.IdleGame;

public class BattleFundPopup : BasePopup
{
    [SerializeField] private GameObject heroPackPrice;
    [SerializeField] private GameObject legendPackPrice;
    [SerializeField] private GameObject legendHeroPackPrice;

    [SerializeField] private List<GameObject> heroRewardText;
    [SerializeField] private List<GameObject> legendRewardText;
    [SerializeField] private List<GameObject> heroLegendRewardText;

    [SerializeField] private List<GameObject> itemPacks = new List<GameObject>();
    protected override void Awake()
    {
        base.Awake();
        var priceBattlefunPack1 = IAPManager.IsInstanceExisted()
            ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.BattlefunPack1)
            : "";
        
        var priceBattlefunPack2 = IAPManager.IsInstanceExisted()
            ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.BattlefunPack2)
            : "";
        
        var priceBattlefunPack3 = IAPManager.IsInstanceExisted()
            ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.BattlefunPack3)
            : "";

        heroPackPrice.GetComponent<YunTextShadow>().DisplayText = priceBattlefunPack1;
        legendPackPrice.GetComponent<YunTextShadow>().DisplayText = priceBattlefunPack2;
        legendHeroPackPrice.GetComponent<YunTextShadow>().DisplayText = priceBattlefunPack3;
        OnInit();
    }

    public override void Show()
    {
        base.Show();
        //indexClose
    }

    private void OnInit()
    {
        var item = FacilityManager.Instance.BattleFundConfig;
        heroRewardText[0].GetComponent<YunTextShadow>().DisplayText = item.heroGem.ToString();
        heroRewardText[1].GetComponent<YunTextShadow>().DisplayText = item.heroToken.ToString();
        heroRewardText[2].GetComponent<YunTextShadow>().DisplayText = item.heroCash.ToString();

        legendRewardText[0].GetComponent<YunTextShadow>().DisplayText = item.legendGem.ToString();
        legendRewardText[1].GetComponent<YunTextShadow>().DisplayText = item.legendToken.ToString();
        legendRewardText[2].GetComponent<YunTextShadow>().DisplayText = item.legendCash.ToString();

        heroLegendRewardText[0].GetComponent<YunTextShadow>().DisplayText = item.heroLegendGem.ToString();
        heroLegendRewardText[1].GetComponent<YunTextShadow>().DisplayText = item.heroLegendToken.ToString();
        heroLegendRewardText[2].GetComponent<YunTextShadow>().DisplayText = item.heroLegendCash.ToString();

        if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassHero)
        {
            itemPacks[0].SetActive(false);
        }

        if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassLegend)
        {
            itemPacks[1].SetActive(false);
        }
    }
    
    public void BuyHeroPackClick()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
        BuyPack(ShopProductNames.BattlefunPack1);
        FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BATTLE_FUND_VIP_1);
    }
    
    public void BuyLegendPackClick()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
        BuyPack(ShopProductNames.BattlefunPack2);
        FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BATTLE_FUND_VIP_2);
    }
    
    public void BuyHeroLegendPackClick()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
        BuyPack(ShopProductNames.BattlefunPack3);
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
                    case ShopProductNames.Money099:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_1_CLICK);
                        break;
                    case ShopProductNames.Money199:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_2_CLICK);
                        break;
                    case ShopProductNames.Money299:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_3_CLICK);
                        break;
                    case ShopProductNames.NoAdsVip:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_REMOVE_ADS_VIP_CLICK);
                        break;
                    case ShopProductNames.BattlefunPack1:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_BATTLE_FUND_1_CLICK);
                        break;
                    case ShopProductNames.BattlefunPack2:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_BATTLE_FUND_2_CLICK);
                        break;
                    case ShopProductNames.BattlefunPack3:
                        FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_BATTLE_FUND_3_CLICK);
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
                        case ShopProductNames.Money099:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_1_SUCCESS);
                            break;
                        case ShopProductNames.Money199:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_2_SUCCESS);
                            break;
                        case ShopProductNames.Money299:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_3_SUCCESS);
                            break;
                        case ShopProductNames.NoAdsVip:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_REMOVE_ADS_VIP_SUCCESS);
                            break;
                        case ShopProductNames.BattlefunPack1:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_BATTLE_FUND_1_SUCCESS);
                            break;
                        case ShopProductNames.BattlefunPack2:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_BATTLE_FUND_2_SUCCESS);
                            break;
                        case ShopProductNames.BattlefunPack3:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_BATTLE_FUND_3_SUCCESS);
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
                
                if(value == 0)
                {
                    switch (productNames)
                    {
                        case ShopProductNames.NoAdsVip:
                            FacilityManager.Instance.RemoveAdsVipWhenInAppPurchase();
                            break;
                        case ShopProductNames.NoAds:
                            FacilityManager.Instance.RemoveAdsWhenInAppPurchase();
                            break;
                        case ShopProductNames.BattlefunPack1:  
                            ProcessBattlePack(0, FacilityManager.Instance.BoughtHeroPack,   
                                FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassLegend);  
                            break;  

                        case ShopProductNames.BattlefunPack2:  
                            ProcessBattlePack(1, FacilityManager.Instance.BoughtLegendPack,   
                                FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassHero);  
                            break;  

                        case ShopProductNames.BattlefunPack3:  
                            ProcessBattlePack(2, FacilityManager.Instance.BoughtHeroLegendPack);  
                            break;  

                    }
                }
            });
#endif
        }
    
    void ProcessBattlePack(int index, Action<Vector3, Transform> purchaseAction, bool unlockCondition = false)  
    {  
        purchaseAction(itemPacks[index].transform.position, transform);  
        UpdateItemPack(itemPacks[index]);  

        if (unlockCondition) // Only true for BattlefunPack1 and BattlefunPack2  
        {  
            itemPacks[2].SetActive(false);  
            DOVirtual.DelayedCall(2.5f, () => Close());  
        }  

        // If it's BattlefunPack3, disable all packs  
        if (index == 2)  
        {  
            foreach (var itemPack in itemPacks)  
            {  
                UpdateItemPack(itemPack);  
            }  
            DOVirtual.DelayedCall(2.5f, () => Close());  
        }  
    }  

    void UpdateItemPack(GameObject itemPack)  
    {  
        var canvasGroup = itemPack.GetComponent<CanvasGroup>();  
        canvasGroup.alpha = 0.7f;  
        canvasGroup.interactable = false;  
    }
    
    public override void Close()
    {
        base.Close();
        FacilityManager.Instance.PlayerInfoUI.HideBattleFundPopupBg();
        if (!FacilityManager.Instance.isShowBattleFundFromSkin)
        {
            FacilityManager.Instance.ShowBattlePass();
        }
        else
        {
            FacilityManager.Instance.ShowSkinPopup();
        }
        
    }
}
