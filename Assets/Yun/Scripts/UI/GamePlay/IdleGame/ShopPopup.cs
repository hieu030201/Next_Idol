using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using Firebase.Analytics;
using Gley.EasyIAP;
using Sirenix.OdinInspector;
using TMPro;
using TwistedTangle.HieuUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class ShopPopup : BasePopup
    {
        [SerializeField] private GameObject packNoAds;
        [SerializeField] private GameObject packNoAdsVip;
        [SerializeField] private GameObject statusBoughtNoAds;
        [SerializeField] private GameObject statusBoughtNoAdsVip;
        [SerializeField] private Image statusBoughtNoAdsImage;
        [SerializeField] private Image statusBoughtNoAdsVipImage;
        [SerializeField] private GameObject noAdsTxt;
        [SerializeField] private GameObject noAdsVipTxt;
        [SerializeField] private GameObject pack1Txt;
        [SerializeField] private GameObject pack2Txt;
        [SerializeField] private GameObject pack3Txt;
        
        [SerializeField] private TextMeshProUGUI money1Txt;
        [SerializeField] private TextMeshProUGUI money2Txt;
        [SerializeField] private TextMeshProUGUI money3Txt;
        
        [SerializeField] private CountDownUIShopFirst countDownUIShopFirst;
        [SerializeField] private CountDownUIShopSecond countDownUIShopSecond;
        [SerializeField] private CountDownUIShopThird countDownUIShopThird;
        
        [SerializeField] public int CashFreeReward;
        [SerializeField] public int TokenFreeReward;
        [SerializeField] public int gemFreeReward;
        [SerializeField] public List<SticketMinusTxt> sticketMinusTxtList = new List<SticketMinusTxt>();
        [SerializeField] public RectTransform titleRectTransform;
        [SerializeField] public List<Transform> listCashVaultTransform = new List<Transform>();
        [SerializeField] public List<Transform> listGemVaultTransform = new List<Transform>();
        [SerializeField] public List<Transform> listDailyGiftTransform = new List<Transform>();
        [SerializeField] private UIButton closeButton;
        
        public ScrollRect scrollRect;
        public ScrollRect scrollRectClone;
        public float scrollDuration = 0.5f;
        public GameObject bgTutorial;
  
        //[SerializeField] private List<ItemDailyShop> itemDailyShops = new List<ItemDailyShop>();
        protected override void Awake()
        {
            base.Awake();

            // Debug.Log("super yun shop awake: " + IAPManager.IsInstanceExisted());
            
            var priceNoAds = IAPManager.IsInstanceExisted()
                ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.NoAds)
                : "";
            noAdsTxt.GetComponent<YunTextShadow>().DisplayText = priceNoAds;
            
            var priceNoAdsVip = IAPManager.IsInstanceExisted() ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.NoAdsVip)
                : "";
            noAdsVipTxt.GetComponent<YunTextShadow>().DisplayText = priceNoAdsVip;
            
            var price1 = IAPManager.IsInstanceExisted()
                ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.Money099)
                : "";
            pack1Txt.GetComponent<YunTextShadow>().DisplayText = price1;

            var price2 = IAPManager.IsInstanceExisted()
                ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.Money199)
                : "";
            pack2Txt.GetComponent<YunTextShadow>().DisplayText = price2;

            var price3 = IAPManager.IsInstanceExisted()
                ? IAPManager.Instance.GetLocalizedPrice(ShopProductNames.Money299)
                : "";
            pack3Txt.GetComponent<YunTextShadow>().DisplayText = price3;
            
            foreach (var product in FacilityManager.Instance.iapConfig.ProductsList)
            {
                if (product.productNames == ShopProductNames.Money099)
                    money1Txt.text = product.valueString;
                else if (product.productNames == ShopProductNames.Money199)
                    money2Txt.text = product.valueString;
                else if (product.productNames == ShopProductNames.Money299)
                    money3Txt.text = product.valueString;
            }

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
            
            scrollRect.verticalNormalizedPosition = 1f;
            scrollRectClone.verticalNormalizedPosition = 0f;
            
            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_REMOVE_ADS_CLICK);
            
            FirebaseAnalytics.LogEvent(
                "test_event",
                new Parameter[]
                {
                    new Parameter("time", 100),        // ID quảng cáo
                    new Parameter("area_id", 1)         // Loại quảng cáo
                }
            );
            
            FireBaseManager.Instance.LogEvent("test_shop_popup");
        }

        public override void Show()
        {
            var content = transform.Find("Content");
            AddEffectShow(titleRectTransform, -200f);
            AddEffectShow(content, 100f);
            var valueData = FacilityManager.Instance.GameSaveLoad.StableGameData;
            if (valueData.freeInShopAmount > 0)
            {
                countDownUIShopFirst.SetActiveAdsIcon(false);
                countDownUIShopFirst.SetBackGroundPack(0);
                countDownUIShopFirst.SetActiveIconImage(0);
                countDownUIShopFirst.TurnOffCountDownContent();
            }
            else if(valueData.freeInShopAmount == 0)
            {
                if (valueData.rewardAdsPackOne > 0)
                {
                    countDownUIShopFirst.SetActiveAdsIcon(true);
                }
                else
                {
                    countDownUIShopFirst.SetActiveAdsIcon(false);
                    countDownUIShopFirst.SetBackGroundPack(1);
                    countDownUIShopFirst.SetActiveIconImage(1);
                    countDownUIShopFirst.TurnOnCountDownContent();
                }
            }

            if (valueData.rewardAdsPackTwo > 0)
            {
                countDownUIShopSecond.SetBackGroundPack(0);
                countDownUIShopSecond.SetActiveIconImage(0);
                countDownUIShopSecond.SetActiveAdsIcon(true);
                countDownUIShopSecond.TurnOffCountDownContent();
            }
            else
            {
                countDownUIShopSecond.SetActiveAdsIcon(false);
                countDownUIShopSecond.SetBackGroundPack(1);
                countDownUIShopSecond.SetActiveIconImage(1);
                countDownUIShopSecond.TurnOnCountDownContent();
            }
            
            
            if (valueData.rewardAdsPackThree > 0)
            {
                countDownUIShopThird.SetBackGroundPack(0);
                countDownUIShopThird.SetActiveIconImage(0);
                countDownUIShopThird.SetActiveAdsIcon(true);
                countDownUIShopThird.TurnOffCountDownContent();
            }
            else
            {
                countDownUIShopThird.SetActiveAdsIcon(false);
                countDownUIShopThird.SetBackGroundPack(1);
                countDownUIShopThird.SetActiveIconImage(1);
                countDownUIShopThird.TurnOnCountDownContent();
            }
       
        }

        public void BuyRemoveAdsClick()
        {
            BuyPack(ShopProductNames.NoAds);
        }

        public void BuyRemoveAdsVipClick()
        {
            BuyPack(ShopProductNames.NoAdsVip);
        }

        public void BuyMoney1Click()
        {
            FacilityManager.Instance.AddMoneyWhenUseGem(1000, 6,listCashVaultTransform[0].position);
            FireBaseManager.Instance.LogEvent(FireBaseManager.CASH_GEM_1);
        }
        
        public void BuyMoney2Click()
        {
            FacilityManager.Instance.AddMoneyWhenUseGem(2000, 10, listCashVaultTransform[1].position);
            FireBaseManager.Instance.LogEvent(FireBaseManager.CASH_GEM_2);
        }
        
        public void BuyMoney3Click()
        {
            FacilityManager.Instance.AddMoneyWhenUseGem(3000, 15,listCashVaultTransform[2].position);
            FireBaseManager.Instance.LogEvent(FireBaseManager.CASH_GEM_3);
        }
        
        public void BuyPack1Click()
        {
            BuyPack(ShopProductNames.Money099);
        }

        public void BuyPack2Click()
        {
            BuyPack(ShopProductNames.Money199);
        }

        public void BuyPack3Click()
        {
            BuyPack(ShopProductNames.Money299);
        }

        public void GetCashCurrencyFree()
        {
            var valueData = FacilityManager.Instance.GameSaveLoad.StableGameData;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            if (valueData.freeInShopAmount <= 0 && valueData.rewardAdsPackOne <= 0) return;
            if (valueData.freeInShopAmount > 0)  
            {  
                valueData.freeInShopAmount--;  
                FacilityManager.Instance.AddMoney(CashFreeReward, listDailyGiftTransform[0].position, true,gameObject.transform);
                if (valueData.freeInShopAmount == 0)  
                {  
                    countDownUIShopFirst.SetActiveAdsIcon(true);
                    FacilityManager.Instance.PlayerInfoUI.HideShopAnimation();
                }  
            }
            else if(valueData.freeInShopAmount == 0) 
            {
                sticketMinusTxtList[0].OnInit();
                AdsManager.Instance.ShowReward(SetStateCashCurrencyFree, AdsManager.RewardType.SHOP_ADS_CASH);
                OnStateIconAdsInPack();
            }   
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            if (bgTutorial.activeSelf)
            {
                bgTutorial.SetActive(false);
                listDailyGiftTransform[0].gameObject.SetActive(true);
            }
        }

        public void SetStateCashCurrencyFree()
        {
            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }
            var valueData = FacilityManager.Instance.GameSaveLoad.StableGameData;
            
            if (valueData.rewardAdsPackOne > 0)  
            {  
                countDownUIShopFirst.SetActiveAdsIcon(true);
                valueData.rewardAdsPackOne--;  
                countDownUIShopFirst.activeAdsTurn.text = valueData.rewardAdsPackOne.ToString();
                if (valueData.rewardAdsPackOne == 0)  
                {  
                    countDownUIShopFirst.StartCountDown();
                    countDownUIShopFirst.SetBackGroundPack(1);
                    countDownUIShopFirst.SetActiveIconImage(1);
                    countDownUIShopFirst.TurnOnCountDownContent();
                }  
            }  
            FacilityManager.Instance.AddMoney(CashFreeReward, listDailyGiftTransform[0].position, true,gameObject.transform);
        }

        public void GetGemCurrencyFree()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            var valueData = FacilityManager.Instance.GameSaveLoad.StableGameData;
            if (valueData.rewardAdsPackThree <= 0) return;
            sticketMinusTxtList[2].OnInit();
            AdsManager.Instance.ShowReward(SetStateGemCurrencyFree, AdsManager.RewardType.SHOP_ADS_GEM);
            OnStateIconAdsInPack();
        }

        public void SetStateGemCurrencyFree()
        {
            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }
            var valueData = FacilityManager.Instance.GameSaveLoad.StableGameData;
            if (valueData.rewardAdsPackThree > 0)  
            {  
                valueData.rewardAdsPackThree--;  
                countDownUIShopThird.activeAdsTurn.text = valueData.rewardAdsPackThree.ToString();
                if (valueData.rewardAdsPackThree == 0)  
                {  
                    countDownUIShopThird.StartCountDown();
                    countDownUIShopThird.SetBackGroundPack(1);
                    countDownUIShopThird.SetActiveIconImage(1);
                    countDownUIShopThird.TurnOnCountDownContent();
                }  
            }  
            FacilityManager.Instance.AddGem(gemFreeReward, listDailyGiftTransform[2].position, true,gameObject.transform);
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true); 
        }

        public void GetTokenCurrencyFree()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            var valueData = FacilityManager.Instance.GameSaveLoad.StableGameData;
            if (valueData.rewardAdsPackTwo <= 0) return;
            sticketMinusTxtList[1].OnInit();
            AdsManager.Instance.ShowReward(SetStateTokenCurrencyFree, AdsManager.RewardType.SHOP_ADS_TOKEN);
            OnStateIconAdsInPack();
        }

        public void SetStateTokenCurrencyFree()
        {
            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }
            var valueData = FacilityManager.Instance.GameSaveLoad.StableGameData;
            if (valueData.rewardAdsPackTwo
                > 0)  
            {  
                valueData.rewardAdsPackTwo--; 
                countDownUIShopSecond.activeAdsTurn.text = valueData.rewardAdsPackTwo.ToString();
                if (valueData.rewardAdsPackTwo == 0)  
                {  
                    countDownUIShopSecond.StartCountDown();
                    countDownUIShopSecond.SetBackGroundPack(1);
                    countDownUIShopSecond.SetActiveIconImage(1);
                    countDownUIShopSecond.TurnOnCountDownContent();
                }  
            } 
            FacilityManager.Instance.AddToken(TokenFreeReward, listDailyGiftTransform[1].position, true,true,gameObject.transform);
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true); 
        }

        public void OnStateIconAdsInPack()
        {
            countDownUIShopFirst.SetStateIconAdsInPackage();
            countDownUIShopSecond.SetStateIconAdsInPackage();
            countDownUIShopThird.SetStateIconAdsInPackage();
        }

        public override void Close()
        {
            var content = transform.Find("Content");
            AddEffectClose(content, 100f);
            AddEffectClose(titleRectTransform, -100f);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
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
                            FireBaseManager.Instance.LogEvent(FireBaseManager.GEM_1);
                            break;
                        case ShopProductNames.Money199:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_2_SUCCESS);
                            FireBaseManager.Instance.LogEvent(FireBaseManager.GEM_2);
                            break;
                        case ShopProductNames.Money299:
                            FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_IAP_PACK_3_SUCCESS);
                            FireBaseManager.Instance.LogEvent(FireBaseManager.GEM_3);
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

                if (value != 0)
                {
                    if (productNames == ShopProductNames.Money099)
                    {
                        FacilityManager.Instance.AddGemWhenInAppPurchase(value, listGemVaultTransform[0]);
                    }
                    else if(productNames == ShopProductNames.Money199)
                    {
                        FacilityManager.Instance.AddGemWhenInAppPurchase(value, listGemVaultTransform[1]);
                    }
                    else if(productNames == ShopProductNames.Money299)
                    {
                        FacilityManager.Instance.AddGemWhenInAppPurchase(value, listGemVaultTransform[2]);
                    }
                }
                else if(productNames == ShopProductNames.NoAds)
                {
                    FacilityManager.Instance.RemoveAdsWhenInAppPurchase();
                    StatusWhenBoughtNoAds();
                }
                else if(productNames == ShopProductNames.NoAdsVip)
                {
                    FacilityManager.Instance.RemoveAdsVipWhenInAppPurchase();
                    StatusWhenBoughtNoAdsVip();
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

        private void AddEffectShow(Transform transformTarget, float moveDistance)
        {
            if (!transformTarget.gameObject.GetComponent<CanvasGroup>())
                transformTarget.gameObject.AddComponent<CanvasGroup>();
            var canvasGroup = transformTarget.GetComponent<CanvasGroup>();
            // Reset trạng thái ban đầu  
            canvasGroup.alpha = 0f;
            RectTransform rectTransform = transformTarget.GetComponent<RectTransform>();  
            Vector3 originalPosition = rectTransform.anchoredPosition;  

            // Tham số hiệu ứng  
            float fadeDuration = 0.4f;  
            float moveDuration = 0.4f;  
            float bounceAmplitude = 15f;  

            // Tạo sequence cho hiệu ứng  
            Sequence showSequence = DOTween.Sequence()  
                .SetDelay(0.3f) // Delay ban đầu  
                .SetUpdate(true); // Đảm bảo hoạt động ngay cả khi game đang pause  

            // Vị trí bắt đầu (dịch sang trái)  
            rectTransform.anchoredPosition = originalPosition - new Vector3(moveDistance, 0, 0);  

            // Hiệu ứng fade và di chuyển  
            showSequence
                // Fade in và di chuyển về vị trí gốc  
                .Append(canvasGroup.DOFade(1f, fadeDuration)
                    .SetEase(Ease.OutQuad))
                .Join(rectTransform.DOAnchorPos(originalPosition, moveDuration)
                    .SetEase(Ease.OutBack)).OnComplete(() =>
                    {
                        if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActivatedTutorialItemShop) return;
                        DOVirtual.DelayedCall(fadeDuration, () =>
                        {
                            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActivatedTutorialItemShop) return;
                            closeButton.interactable = false;
                            ScrollToBottom();  
                        });
                    });
        }
        
        private void AddEffectClose(Transform transformTarget, float moveDistance)
        {
            var canvasGroup = transformTarget.GetComponent<CanvasGroup>();
            RectTransform rectTransform = transformTarget.GetComponent<RectTransform>();
            Vector3 originalPosition = rectTransform.anchoredPosition;
    
            // Keep same timing parameters for consistency
            float fadeDuration = 0.3f;
            float moveDuration = 0.3f;
    
            // Create sequence for close effect
            Sequence closeSequence = DOTween.Sequence()
                .SetUpdate(true); // Ensure it works even when game is paused
    
            // Target position (move left, matching the show distance)
            Vector3 targetPosition = originalPosition - new Vector3(moveDistance, 0, 0);
    
            // Fade out and move animation
            closeSequence
                .Append(canvasGroup.DOFade(0f, fadeDuration)
                    .SetEase(Ease.InQuad))  // Use InQuad for fade out
                .Join(rectTransform.DOAnchorPos(targetPosition, moveDuration)
                    .SetEase(Ease.InBack));  // Use InBack for reverse bounce effect
            
            // Optional: Add callback for cleanup
            closeSequence.OnComplete(() => 
            {
                FacilityManager.Instance.PlayerInfoUI.HideShopBg();
                Destroy(gameObject);
            });
        }
        
        public void ScrollToBottom()  
        {  
            StartCoroutine(ScrollCoroutine());  
        }  

        private IEnumerator ScrollCoroutine()  
        {  
            float elapsedTime = 0f;  
            float startingPosition = scrollRect.verticalNormalizedPosition;  
            float targetPosition = 0f; // 0 để cuộn xuống dưới cùng  

            while (elapsedTime < scrollDuration)  
            {  
                // Tính toán vị trí cuộn mới và cập nhật  
                elapsedTime += Time.deltaTime;  
                float t = elapsedTime / scrollDuration;  
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(startingPosition, targetPosition, t);  
                yield return null; // Chờ cho frame tiếp theo  
            }  

            // Đảm bảo vị trí cuối  
            scrollRect.verticalNormalizedPosition = targetPosition;
            if (scrollRect.verticalNormalizedPosition <= 0.1f)
            {
                scrollRectClone.vertical = false;
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    closeButton.interactable = true;
                    bgTutorial.SetActive(true);
                    listDailyGiftTransform[0].gameObject.SetActive(false);
                });
            }
        }  
    }
}