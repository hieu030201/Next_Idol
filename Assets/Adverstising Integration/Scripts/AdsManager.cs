using System;
using System.Collections;
using System.Collections.Generic;
using Advertising;
using DG.Tweening;
using Gley.MobileAds;
using Gley.MobileAds.Scripts.ToUse;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Adverstising_Integration.Scripts
{
    public class AdsManager : Singleton<AdsManager>
    {
        [SerializeField] private bool isRemoveAds;
        [SerializeField] public AdsConfig adsConfig;

        public string mrecAdsUnitId;

        private int openAdCount;

        public int OpenAdCount
        {
            get => openAdCount;
            private set => openAdCount = value;
        }

        private int _bannerRetryAttempt;
        private int _appOpenAdsPauseCount;
        private DateTime _timeOutGame;
        private DateTime _lastTimeAdsShown;
        private bool _isAdShowing;
        private const string OPEN_AD_COUNT = "OpenAdCount";
        private bool _isRecentlyShowAd;

        public bool IsRecentlyShowAd
        {
            get => _isRecentlyShowAd;
            set => _isRecentlyShowAd = value;
        }

        #region Unity Methods

        private void Start()
        {
            Debug.Log(MaxSdk.Version);

            API.Initialize(OnInitialized);
            API.RemoveAds(isRemoveAds);


            // RegisterBannerEvents();
            // RegisterInterstitialEvents();
            // RegisterRewardEvents();
            // RegisterAOAEvents();

            ResetLastTime();
            ResetTimeOutGame();

            openAdCount = PlayerPrefs.GetInt(OPEN_AD_COUNT, 0);
        }

        /*private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _appOpenAdsPauseCount++;
                ResetTimeOutGame();
            }
            else
            {
                ShowAppOpenAd();
            }
        }*/

        #endregion

        #region Banner

        public bool IsCalledShowBanner;

        public void ShowBanner()
        {
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isPurchasedRemoveAds)
            {
                Debug.Log("super yun no ShowBanner");
                return;
            }

            if (FacilityManager.Instance.testGameConfig.IsTestGame)
                return;
            IsCalledShowBanner = true;
            if (!IsInitSuccess)
                return;
            Debug.Log("ShowBanner");
            LogEvent(FireBaseManager.SHOW_BANNER_ADS);
            API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
            
        }

        public void ShowMrec()
        {
            return;
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isPurchasedRemoveAds)
            {
                Debug.Log("super yun no ShowMrec");
                return;
            }

            if (FacilityManager.Instance.testGameConfig.IsTestGame)
                return;
            if (!IsInitSuccess)
                return;
            Debug.Log("ShowMrec");
            // LogEvent(FireBaseManager.SHOW_BANNER_ADS);
            // API.ShowMRec(BannerPosition.Center);

            // Load quảng cáo MREC
            // MaxSdk.CreateMRec("58867bb52074e4b5", MaxSdkBase.AdViewPosition.BottomCenter);

            // Hiển thị quảng cáo
            MaxSdk.ShowMRec(mrecAdsUnitId);
        }

        public void HideMrec()
        {
            MaxSdk.HideMRec(mrecAdsUnitId);
        }

        public void HideBanner()
        {
            API.HideBanner();
        }

        private void RegisterBannerEvents()
        {
            Events.onBannerLoadSuccess += OnBannerLoadSuccess;
            Events.onBannerLoadFailed += OnBannerLoadFailedEvent;
            Events.onBannerClicked += OnBannerClicked;
            Events.onBannerDisplaySuccess += OnBannerDisplaySuccess;
            Events.onBannerRevenue += (value, currencyCode) =>
                OnRevenueImpression(FireBaseManager.BANNER_AD_REVENUE, "Banner", value, currencyCode);
        }

        private void OnBannerLoadSuccess()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.BANNER_AD_LOAD_SUCCESS);
            _bannerRetryAttempt = 0;
        }

        private void OnBannerLoadFailedEvent(string err)
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.BANNER_AD_LOAD_FAILED, err);
            var retryDelay = Math.Pow(2, Math.Min(6, _bannerRetryAttempt));
            _bannerRetryAttempt++;
            Invoke(nameof(ShowBanner), (float)retryDelay);
        }

        private void OnBannerDisplaySuccess()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.BANNER_AD_DISPLAYED);
        }

        private void OnBannerClicked()
        {
        }

        #endregion

        #region Interstitial Ads

        private Tween _tweenToConinueShowInter;

        public void ShowInterstitialAd()
        {
            var isUnityEditor = false;
#if UNITY_EDITOR
            isUnityEditor = true;
#endif

            if (FacilityManager.Instance.testGameConfig.IsTestGame && !isUnityEditor)
            {
                if (FacilityManager.Instance)
                    FacilityManager.Instance.OnShowInterAdsSuccess();
                return;
            }

            LogEvent(FireBaseManager.SHOW_INTER_ADS);
            if (API.IsInterstitialAvailable())
            {
                _isAdShowing = true;

                API.ShowInterstitial(() =>
                {
                    _isRecentlyShowAd = true;
                    StartCoroutine(EndOfInter());
                });
            }
        }

        private IEnumerator EndOfInter()
        {
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();

            if (FireBaseManager.Instance.showTakeABreak)
            {
                if (FacilityManager.Instance)
                    FacilityManager.Instance.OnShowInterAdsSuccess();
            }

            LogEvent(FireBaseManager.SHOW_INTER_ADS_SUCCESS);
            _isAdShowing = false;
            ResetLastTime();
            ResetTimeOutGame();
        }

        private IEnumerator EndOfReward()
        {
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();

            if (isCancelRewardedAd)
            {
                LogEvent(FireBaseManager.SHOW_REWARD_ADS_CANCEL);
            }
            else
            {
                switch (_rewardType)
                {
                    case RewardType.VIP_BED_ROOM:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_VIP_BED_ROOM);
                        break;
                    case RewardType.X2_DAILY_GIFT:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_X2_DAILY_GIFT);
                        break;
                    case RewardType.SPEED_BOOSTER:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_SPEED_BOOSTER);
                        break;
                    case RewardType.GET_MONEY:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_MONEY);
                        break;
                    case RewardType.GET_MONEY_2:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_MONEY_2);
                        break;
                    case RewardType.GET_LADY_WORKER:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_LADY_WORKER);
                        break;
                    case RewardType.GET_VIP_SOLDIER:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_VIP_SOLDIER);
                        break;
                    case RewardType.CLAIM_X2_LEVEL_UP:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_X2_LEVEL_UP);
                        break;
                    case RewardType.SPIN_WHEEL:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_SPIN_WHEEL);
                        break;
                    case RewardType.SHOP_ADS_GEM:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_SHOP_GEM);
                        break;
                    case RewardType.SHOP_ADS_TOKEN:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_SHOP_TOKEN);
                        break;
                    case RewardType.SHOP_ADS_CASH:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_SHOP_CASH);
                        break;
                    case RewardType.WORKER_SPEED_BOOSTER:
                        LogEvent(FireBaseManager.SHOW_REWARD_ADS_SUCCESS_WORKER_SPEED_BOOSTER);
                        break;
                    case RewardType.RETAL_SKIN:
                        LogEvent(FireBaseManager.Show_REWARD_ADS_SUCCESS_RETAL_SKIN);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_rewardType), _rewardType, null);
                }
            }

            _onRewardedAdCompleted?.Invoke();
            ResetLastTime();
            ResetTimeOutGame();
        }

        public void  ShowTakeABreakPopup()
        {
            if (FacilityManager.Instance.testGameConfig.isForVideoRecording)
                return;

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isPurchasedRemoveAds)
                return;

            if (!FireBaseManager.Instance.showInter)
                return;
            
            var nextInterMinTime = FireBaseManager.Instance
                ? FireBaseManager.Instance.nextInterMinTime
                : adsConfig.minTimeToNextAds;

            var totalSecondsFromLastAds = (DateTime.Now - _lastTimeAdsShown).TotalSeconds;
            if (totalSecondsFromLastAds <
                nextInterMinTime )
            {
                DOVirtual.DelayedCall((float) (nextInterMinTime + 1 - totalSecondsFromLastAds), ShowTakeABreakPopup);
                return;
            }

            if (FacilityManager.Instance.BattleManager.isRunningBattle)
            {
                _tweenToConinueShowInter?.Kill();
                _tweenToConinueShowInter = DOVirtual.DelayedCall(10, ShowTakeABreakPopup);
                return;
            }

            if (!API.IsInterstitialAvailable())
            {
                _tweenToConinueShowInter?.Kill();
                _tweenToConinueShowInter = DOVirtual.DelayedCall(10, ShowTakeABreakPopup);
                return;
            }

            _tweenToConinueShowInter?.Kill();
            _tweenToConinueShowInter = DOVirtual.DelayedCall(FireBaseManager.Instance.interAdsDelay, ShowTakeABreakPopup);

            if (!FireBaseManager.Instance.showTakeABreak)
            {
                ShowInterstitialAd();
                return;
            }

            if (FacilityManager.Instance)
                FacilityManager.Instance.ShowTakeABreakPopup();
        }

        private void RegisterInterstitialEvents()
        {
            Events.onInterstitialClicked += OnInterstitialClicked;
            Events.onInterstitialLoadSuccess += OnInterstitialLoadSuccess;
            Events.onInterstitialLoadFailed += OnInterstitialLoadFailed;
            Events.onInterstitialDisplaySuccess += OnInterstitialDisplaySuccess;
            Events.onInterstitialDisplayFailed += OnInterstitialDisplayFailed;
            Events.onInterstitialRevenue += (value, currencyCode) =>
                OnRevenueImpression(FireBaseManager.INTER_AD_REVENUE, "Interstitial", value, currencyCode);
        }

        private void OnInterstitialClicked()
        {
        }

        private void OnInterstitialLoadSuccess()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.INTER_AD_LOAD_SUCCESS);
        }

        private void OnInterstitialLoadFailed(string message)
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.INTER_AD_LOAD_FAILED, message);
        }

        private void OnInterstitialDisplaySuccess()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.INTER_AD_DISPLAYED);
        }

        private void OnInterstitialDisplayFailed(string message)
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.INTER_AD_DISPLAY_FAILED, message);
        }

        private void ResetLastTime()
        {
            _lastTimeAdsShown = DateTime.Now;
        }

        #endregion

        #region Rewarded Ads

        private Action _onRewardedAdCompleted;

        [FormerlySerializedAs("IsCancelRewardedAd")]
        public bool isCancelRewardedAd;

        public enum RewardType
        {
            VIP_BED_ROOM,
            X2_DAILY_GIFT,
            SPEED_BOOSTER,
            GET_MONEY,
            GET_MONEY_2,
            GET_LADY_WORKER,
            GET_VIP_SOLDIER,
            CLAIM_X2_LEVEL_UP,
            SPIN_WHEEL,
            SHOP_ADS_TOKEN,
            SHOP_ADS_GEM,
            SHOP_ADS_CASH,
            WORKER_SPEED_BOOSTER,
            RETAL_SKIN,
        }

        private RewardType _rewardType;

        public void ShowReward(Action onCompleted, RewardType rewardType)
        {
            _rewardType = rewardType;
            if (FacilityManager.Instance.testGameConfig.isForVideoRecording)
            {
                return;
            }

            if (FacilityManager.Instance.testGameConfig.IsTestGame && FacilityManager.Instance.testGameConfig.isTestAds)
            {
                onCompleted?.Invoke();
                return;
            }

            if (_isAdShowing)
            {
                return;
            }

            // FireBaseManager.Instance.LogEvent(FireBaseManager.REWARDED_AD_ELIGIBLE);
            switch (_rewardType)
            {
                case RewardType.VIP_BED_ROOM:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_VIP_BED_ROOM);
                    break;
                case RewardType.X2_DAILY_GIFT:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_X2_DAILY_GIFT);
                    break;
                case RewardType.SPEED_BOOSTER:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_SPEED_BOOSTER);
                    break;
                case RewardType.WORKER_SPEED_BOOSTER:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_WORKER_SPEED_BOOSTER);
                    break;
                case RewardType.GET_MONEY:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_MONEY);
                    break;
                case RewardType.GET_MONEY_2:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_MONEY_2);
                    break;
                case RewardType.GET_LADY_WORKER:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_LADY_WORKER);
                    break;
                case RewardType.GET_VIP_SOLDIER:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_VIP_SOLDIER);
                    break;
                case RewardType.CLAIM_X2_LEVEL_UP:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_X2_LEVEL_UP);
                    break;
                case RewardType.SPIN_WHEEL:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_SPIN_WHEEL);
                    break;
                case RewardType.SHOP_ADS_GEM:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_SHOP_GEM);
                    break;
                case RewardType.SHOP_ADS_TOKEN:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_SHOP_TOKEN);
                    break;
                case RewardType.SHOP_ADS_CASH:
                    LogEvent(FireBaseManager.SHOW_REWARD_ADS_SHOP_CASH);
                    break;
                case RewardType.RETAL_SKIN:
                    LogEvent(FireBaseManager.Show_REWARD_ADS_RETAL_SKIN);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rewardType), rewardType, null);
            }

            if (!API.CanShowAds())
            {
                onCompleted?.Invoke();
                return;
            }

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds--;
                FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
                FacilityManager.Instance.PlayerInfoUI.UpdateSticketRemoveAdsReward();
                onCompleted?.Invoke();
                return;
            }

            if (API.IsRewardedVideoAvailable())
            {
                _isAdShowing = true;
                _onRewardedAdCompleted = onCompleted;
                isCancelRewardedAd = true;
                API.ShowRewardedVideo(isShownSuccess => OnRewardHandle(isShownSuccess, onCompleted));
            }
        }

        private void RegisterRewardEvents()
        {
            Events.onRewardedVideoClicked += OnRewardClicked;
            Events.onRewardedVideoLoadSuccess += OnRewardLoadSuccess;
            Events.onRewardedVideoLoadFailed += OnRewardLoadFailed;
            Events.onRewardedDisplaySuccess += OnRewardDisplaySuccess;
            Events.onRewardedDisplayFailed += OnRewardDisplayFailed;
            Events.onRewardedRevenue += (value, currencyCode) =>
                OnRevenueImpression(FireBaseManager.REWARDED_AD_REVENUE, "Reward Ad", value, currencyCode);
        }

        public bool IsRewardAdAvailable() => API.IsRewardedVideoAvailable();

        private void OnRewardClicked()
        {
        }

        private void OnRewardLoadSuccess()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.REWARDED_AD_LOAD_SUCCESS);
        }

        private void OnRewardLoadFailed(string message)
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.REWARDED_AD_LOAD_FAILED, message);
        }

        private void OnRewardDisplaySuccess()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.REWARDED_AD_DISPLAYED);
        }

        private void OnRewardDisplayFailed(string message)
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.REWARDED_AD_DISPLAY_FAILED, message);
        }

        private void OnRewardHandle(bool isCompleted, Action onCompleted)
        {
            _isAdShowing = false;
            _isRecentlyShowAd = true;
            isCancelRewardedAd = !isCompleted;
            StartCoroutine(EndOfReward());
            if (isCompleted)
            {
            }
        }

        #endregion

        #region AOA

        public void ShowAppOpenAd(Action onCompleted, bool ignoreCappingTime = false)
        {
            if (FacilityManager.Instance && FacilityManager.Instance.GameSaveLoad.StableGameData.isPurchasedRemoveAds)
            {
                return;
            }

            if (_isRecentlyShowAd)
            {
                _isRecentlyShowAd = false;
                return;
            }
            
            var nextInterMinTime = FireBaseManager.Instance
                ? FireBaseManager.Instance.nextInterMinTime
                : adsConfig.minTimeToNextAds;

            var totalSecondsFromLastAds = (DateTime.Now - _lastTimeAdsShown).TotalSeconds;
            if (totalSecondsFromLastAds <
                nextInterMinTime )
            {
                return;
            }

            if (API.IsAppOpenAvailable())
            {
                _isAdShowing = true;
                API.ShowAppOpen(() =>
                {
                    _isAdShowing = false;
                    ResetTimeOutGame();
                    ResetLastTime();
                    onCompleted?.Invoke();
                });
            }
        }

        public void FirstShowAppOpenAd(Action onCompleted, TextMeshProUGUI txt)
        {
            if (API.IsAppOpenAvailable())
            {
                _isAdShowing = true;
                API.ShowAppOpen(() =>
                {
                    _isAdShowing = false;
                    ResetTimeOutGame();
                    onCompleted?.Invoke();
                });
            }
            else
            {
                txt.text = "IsAppOpen not Available";
                onCompleted?.Invoke();
            }
        }

        private void RegisterAOAEvents()
        {
            Events.onAppOpenClicked += OnAOAClicked;
            Events.onAppOpenLoadSuccess += OnAOALoadSuccess;
            Events.onAppOpenLoadFailed += OnAOALoadFailed;
            Events.onAppOpenDisplaySuccess += OnAOADisplaySuccess;
            Events.onAppOpenDisplayFailed += OnAOADisplayFailed;
            Events.onAppOpenRevenue += (value, currencyCode) =>
                OnRevenueImpression(FireBaseManager.AOA_REVENUE, "Open Ad", value, currencyCode);
        }

        private void OnAOAClicked()
        {
        }

        private void OnAOALoadSuccess()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.AOA_LOAD_SUCCESS);
        }

        private void OnAOALoadFailed(string message)
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.AOA_LOAD_FAILED, message);
        }

        private void OnAOADisplaySuccess()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.AOA_DISPLAYED);
        }

        private void OnAOADisplayFailed(string message)
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.AOA_DISPLAY_FAILED, message);
        }

        #endregion

        #region Public Methods

        public void HandleOpenAdCount()
        {
            PlayerPrefs.SetInt(OPEN_AD_COUNT, openAdCount + 1);
        }

        public void ResetTimeOutGame()
        {
            _timeOutGame = DateTime.Now;
        }

        #endregion

        #region Private Methods

        public bool IsInitSuccess;

        private void OnInitialized()
        {
            Debug.Log("Ads is initialized!");
            IsInitSuccess = true;
            // FacilityManager.Instance.OnInitAdsSuccess();
            var isFirstTimePlayGame = PlayerPrefs.GetInt("isFirstTimePlayGame", 1);
            _tweenToConinueShowInter?.Kill();

            // MaxSdk.CreateMRec(mrecAdsUnitId, MaxSdkBase.AdViewPosition.BottomCenter);
            // MaxSdk.SetMRecExtraParameter(mrecAdsUnitId, "adaptive_mrec", "false");
            // MaxSdk.HideMRec(mrecAdsUnitId);

            if (isFirstTimePlayGame == 1)
            {
                if (FireBaseManager.Instance)
                {
                    // Debug.Log("Delay inter after init Ads");
                    DelayToShowTakeABreakPopupInFirstTime();
                }
                // Debug.Log("FIRST TIME SHOW ADS: " + adsConfig.firstDelayToInterAds);
            }
            else
            {
                // Debug.Log("DELAY SHOW INTER ADS WHEN ADS IS INITIALIZED: " + adsConfig.interAdsDelay);

                if (FireBaseManager.Instance)
                {
                    // Debug.Log("Delay inter after init Ads");
                    DelayToShowTakeABreakPopup();
                }
            }

            PlayerPrefs.SetInt("isFirstTimePlayGame", 0);
        }

        public void DelayToShowTakeABreakPopupInFirstTime()
        {
            _tweenToConinueShowInter?.Kill();
            _tweenToConinueShowInter =
                DOVirtual.DelayedCall(FireBaseManager.Instance.firstTimeShowInterAdsDelay, ShowTakeABreakPopup);
        }

        public void DelayToShowTakeABreakPopup()
        {
            // Debug.Log("DelayToShowTakeABreakPopup: " + FireBaseManager.Instance.Inter_Ads_Delay);
            _tweenToConinueShowInter?.Kill();
            _tweenToConinueShowInter =
                DOVirtual.DelayedCall(FireBaseManager.Instance.interAdsDelay, ShowTakeABreakPopup);
        }

        private void OnRevenueImpression(string eventName, string adType, long value, string currencyCode)
        {
            var impressionParameters = new[]
            {
                new Firebase.Analytics.Parameter("value", value),
                new Firebase.Analytics.Parameter("currency", currencyCode),
            };
            FireBaseManager.Instance.LogEventWithParameterArray(eventName,
                impressionParameters);

            Dictionary<string, string> dic = new Dictionary<string, string>
            {
                { AFAdRevenueEvent.COUNTRY, "US" },
                { AFAdRevenueEvent.AD_TYPE, adType },
                { AFAdRevenueEvent.PLACEMENT, "place" },
            };
            // AppsflyerManager.Instance.LogAdRevenue("Google Ad Mob", value, currencyCode,
            //     dic);
        }

        private void LogEvent(string eventName, string paramName = "", int paramValue = 0)
        {
            if (FireBaseManager.IsInstanceExisted())
                FireBaseManager.Instance.LogEvent(eventName, paramName, paramValue);
        }

        #endregion
    }
}