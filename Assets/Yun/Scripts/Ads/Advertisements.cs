using System;
using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Loading;

// using Ultility;
// using DVAH;

namespace Yun.Scripts.Ads
{
    public class Advertisements : MonoBehaviour
    {
        [SerializeField] private TestGameConfig testGameConfig;
        [SerializeField] private LoadingScene loadingScene;

        //NativeAd nativeAd;
        public string nativeAdUnitId;
        public string bannerAdUnitId;

        private static Advertisements _instant = null;

        public int TYPE_REWARD = 0;

        public int REWARD_INDEX = 0;

        public bool is_offBanner = false, is_offInter = false, is_offMrec = false;
        public bool isMrec = false;

        public bool is_fisrt_show_app_open = true;
        public bool is_resume_app = false;
        public bool canLoadBanner = false;
        [SerializeField] private float count_time_countdown_loadbanner = 0f;
        [SerializeField] private float count_time_countdown_inter = 0f;
        public bool start_countdown_show_inter = false;
        public bool can_show_inter = false;

        private bool start_countdown_app_open = false;
        private float count_app_open = 0f;
        float timer = 0;

        public bool is_test = true;

        float rateTimeCount = 0;
        public bool canShowRate = false;

        public static Advertisements Instance
        {
            get
            {
                if (_instant == null)
                {
                    _instant = FindObjectOfType<Advertisements>();
                }

                return _instant;
            }
        }

        private void Start()
        {
            // var num1 = PlayerPrefs.GetInt(common.DataKey_RemoveAds_1);

            // if (num1 != 0)
            // {
            start_countdown_show_inter = false;
            is_offBanner = true;
            is_offInter = true;
            is_offMrec = true;
            // }
            // start_countdown_collap = true;
            // canShowCollap = true;

            if (testGameConfig.isTestAds)
            {
                nativeAdUnitId = "ca-app-pub-3940256099942544/2247696110";
                bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
            }

            initAdmob();
        }

        private void Awake()
        {
            if (_instant != null && _instant.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
                Destroy(this.gameObject);
            else
                _instant = this.GetComponent<Advertisements>();

            DontDestroyOnLoad(this.gameObject);
        }

        public void startCountAppOpen()
        {
            count_app_open = 0f;
            //AdManager.Instant.canShowAOA = false;
            start_countdown_app_open = true;
            start_countdown_show_inter = true;
        }

        private void Update()
        {
            // App Open Count show again
            //if (start_countdown_app_open)
            //{
            //    if (AdManager.Instant.canShowAOA == false)
            //    {
            //        count_app_open += Time.deltaTime;
            //        if (count_app_open >= common.open_ad_capping_time)
            //        {
            //            AdManager.Instant.canShowAOA = true;
            //            start_countdown_app_open = false;
            //            count_app_open = 0f;
            //        }
            //    }
            //}

            /*if (start_countdown_collap)
        {
            count_collap += Time.deltaTime;
            if (count_collap >= common.capping_collapse_banner)
            {
                count_collap -= count_collap;
                canShowCollap = true;
                start_countdown_collap = false;
            }
        }
        if(common.rate_on_off)
        {
            if(!canShowRate)
            {
                rateTimeCount += Time.deltaTime;
                if(rateTimeCount > common.rate_time)
                {
                    rateTimeCount -= rateTimeCount;
                    canShowRate = true;
                }
            }
        }*/

            /*if (!canRequestNative && startCountRequestNative)
            {
                countNative += Time.deltaTime;
                if (countNative >= cappingNative)
                {
                    startCountRequestNative = false;
                    canRequestNative = true;
                    countNative = 0f;
                    RequestNativeAd();
                }
            }*/

            /*var num2 = PlayerPrefs.GetInt(common.DataKey_RemoveAds_2);

        if (num2 != 0)
        {
            start_countdown_show_inter = false;
        }
        // Inter
        if (start_countdown_show_inter == false)
            return;

        if (can_show_inter == true)
            return;

        count_time_countdown_inter += Time.deltaTime;

        if (count_time_countdown_inter >= common.config_time_countdown_show_inter)
        {
            can_show_inter = true;
            count_time_countdown_inter = 0;
        }*/
        }

        /*public void Mrec()
    {
        ShowMrec();
        hideBannerAd();
    }

    public void Banner()
    {
        hideMrec();
        ShowBannerCollapse();
    }

    public void showPopupRate()
    {
        if(common.showRateInTurn == false)
        {
            DVAH3rdLib.Instant.ShowPopUpRate();
            common.showRateInTurn = true;
        }
    }

    public void LoadBannerAfterAOA()
    {
        StartCoroutine(OpenBanner());
    }

    IEnumerator OpenBanner()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        ShowBannerCollapse();
    }

    public void Native()
    {
        //CallRequestNextNativeAd(GameManager.instance.NativeStartMenu);
    }

    public void ShowBannerCollapse()
    {
        if (is_offBanner)
            return;
        if(common.is_collapse == true)
            RequestBannerCollapsiveAd();
        else
            AdManager.Instant.ShowBanner();
    }

    IEnumerable ShowBanner()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        bannerView.Show();
    }

    public void HideCollapseBanner()
    {
        if(bannerView != null)
            bannerView.Hide();
    }

    public void hideBannerAd()
    {
        if(common.is_collapse == true)
        {
            if(bannerView != null)
                bannerView.Hide();
        }
        else
            AdManager.Instant.DestroyBanner();
    }

    public void ShowMrec()
    {
        if (is_offBanner)
            return;
        AdManager.Instant.ShowMrec();
    }

    public void DestroyMrec()
    {
        AdManager.Instant.DestroyMrec();
    }

    public void hideMrec()
    {
        AdManager.Instant.DestroyMrec();
    }

    Action callbackInter;
    public void ShowInterstitialAd(Action callback = null)
    {
        if (is_offInter)
            return;

        if (can_show_inter == false)
            return;

        if (callback != null)
            callbackInter = callback;

        AdManager.Instant.ShowInterstitial((status) =>
        {
            can_show_inter = false;
            start_countdown_show_inter = true;
            if (callbackInter != null)
            {
                try
                {
                    callbackInter?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError("callback inter error: " + e.ToString() + " <==");
                }

                callbackInter = null;
            }
        }, showNoAds: false);
    }

    public void ShowRewardedAd()
    {
        AdManager.Instant.ShowRewardVideo((status) =>
        {
            //Do sth when user watched reward
            if (status == RewardVideoState.Watched)
            {
                StartCoroutine(endofReward());
                GameManager.instance.LogFirebaseEventLevel("Reward_ads_count");
            }
            else
            {
                //Do sth when reward fail to show, not ready, etc....
            }
        }, showNoAds: true);
    }

    private IEnumerator endofReward()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();
        if (TYPE_REWARD != 0)
        {
            switch(TYPE_REWARD)
            {
                case 1:
                    GameManager2.instance.unlock_replay_map();
                    break;
                case 2:
                    GameManager2.instance.Get_reload_ads();
                    break;
                case 3:
                    GameManager2.instance.Get_half_hald_reward();
                    break;
                case 4:
                    GameManager.instance.UnlockMap2();
                    break;
            }

            TYPE_REWARD = 0;
        }

        if(REWARD_INDEX != 0)
        {
            PlayerPrefs.SetInt("Locked" + REWARD_INDEX.ToString(), 1);
            var reward = GameManager.instance.rewardButton;
            if (reward != null)
            {
                reward.GetComponent<DragObjectManager>().isUnlock = true;
                reward.GetComponent<DragObjectManager>().lean.enabled = true;
                reward.GetComponent<DragObjectManager>().locked.SetActive(false);
            }
        }

        REWARD_INDEX = 0;

    }*/

        public void initAdmob()
        {
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                initAdmobSuccess = true;
                HandleInitCompleteAction(initStatus);

            });
        }
        
        void HandleInitCompleteAction(InitializationStatus initstatus)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                RequestNativeAd();

                RequestBanner();
                // Ẩn banner ngay lập tức
                HideBanner();
            });
        }

        private BannerView _bannerView;

        private void RequestBanner()
        {
            if (_bannerView != null)
            {
                // Hủy đăng ký sự kiện trước khi hủy banner cũ
                _bannerView.OnBannerAdLoaded -= HandleOnBannerAdLoaded;
                _bannerView.OnBannerAdLoadFailed -= HandleBannerAdLoadFailed;
                _bannerView.OnAdPaid -= OnAdPaid;
        
                _bannerView.Destroy(); // Hủy banner cũ
            }
            var adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            
            _bannerView = new BannerView(bannerAdUnitId,
                adaptiveSize, AdPosition.Bottom);

            _bannerView.OnBannerAdLoaded += HandleOnBannerAdLoaded;
            _bannerView.OnBannerAdLoadFailed += HandleBannerAdLoadFailed;
            _bannerView.OnAdPaid += OnAdPaid;

            // Tạo request quảng cáo
            var request = new AdRequest();

            // Tải và hiển thị banner
            _bannerView.LoadAd(request);
            
            if (FacilityManager.Instance && FacilityManager.Instance.testGameConfig.isShowLogPrivate)
            {
                var currentTime = DateTime.Now;
                var hour = currentTime.Hour;     // Giờ (0-23)
                var minute = currentTime.Minute;
                var second = currentTime.Second;
                var log = hour + ":" + minute + ":" + second + " - " + "RequestBanner";
                FacilityManager.Instance.AddLogPrivate(log);
            }
        }

        private BannerView _oldBannerView;
        
        private void RefreshBanner()
        {
            if (FacilityManager.Instance && FacilityManager.Instance.testGameConfig.isShowLogPrivate)
            {
                var currentTime = DateTime.Now;
                var hour = currentTime.Hour;     // Giờ (0-23)
                var minute = currentTime.Minute;
                var second = currentTime.Second;
                var log = hour + ":" + minute + ":" + second + " - " + "RefreshBanner";
                FacilityManager.Instance.AddLogPrivate(log);
            }
            /*if (_bannerView != null)
            {
                // Tạo request quảng cáo
                var request = new AdRequest();
                _bannerView.LoadAd(request);
            }*/

            if (_oldBannerView == null)
            {
                _oldBannerView = _bannerView;
            
                if (_oldBannerView != null)
                {
                    // Hủy đăng ký sự kiện trước khi hủy banner cũ
                    _oldBannerView.OnBannerAdLoaded -= HandleOnBannerAdLoaded;
                    _oldBannerView.OnBannerAdLoadFailed -= HandleBannerAdLoadFailed;
                    _oldBannerView.OnAdPaid -= OnAdPaid;
                }   
            }
            
            var adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            
            _bannerView = new BannerView(bannerAdUnitId,
                adaptiveSize, AdPosition.Bottom);

            _bannerView.OnBannerAdLoaded += HandleOnBannerAdLoaded;
            _bannerView.OnBannerAdLoadFailed += HandleBannerAdLoadFailed;
            _bannerView.OnAdPaid += OnAdPaid;

            // Tạo request quảng cáo
            var request = new AdRequest();

            // Tải và hiển thị banner
            _bannerView.LoadAd(request);
        }
        
        // Sự kiện khi banner tải thành công
        private void HandleOnBannerAdLoaded()
        {
            // Debug.LogWarning("Banner Loaded Successfully!");
            CancelInvoke(nameof(RefreshBanner));
            Invoke(nameof(RefreshBanner), FireBaseManager.Instance ? FireBaseManager.Instance.refreshBannerDelay : 30);
            if (FacilityManager.Instance && FacilityManager.Instance.testGameConfig.isShowLogPrivate)
            {
                var currentTime = DateTime.Now;
                var hour = currentTime.Hour;     // Giờ (0-23)
                var minute = currentTime.Minute;
                var second = currentTime.Second;
                var log = hour + ":" + minute + ":" + second + " - " + "Banner Loaded Successfully!";
                FacilityManager.Instance.AddLogPrivate(log);
            }

            _oldBannerView?.Destroy();
            _oldBannerView = null;
        }

        private Tween _reloadBannerTween;
        // Sự kiện khi banner tải thất bại
        private void HandleBannerAdLoadFailed(LoadAdError args)
        {
            Debug.LogWarning("Banner Failed to Load: " + args.GetMessage());
            
            _bannerView?.Destroy();
            CancelInvoke(nameof(RefreshBanner));
            Invoke(nameof(RefreshBanner), 5);
            
            if (FacilityManager.Instance && FacilityManager.Instance.testGameConfig.isShowLogPrivate)
            {
                var currentTime = DateTime.Now;
                var hour = currentTime.Hour;     // Giờ (0-23)
                var minute = currentTime.Minute;
                var second = currentTime.Second;
                var log = hour + ":" + minute + ":" + second + " - " + "Banner Failed to Load";
                FacilityManager.Instance.AddLogPrivate(log);
            }
        }

        private void OnAdPaid(AdValue adValue)
        {
            //string msg = string.Format("{0} (currency: {1}, value: {2}",
            //                            "Banner ad received a paid event.",
            //                            adValue.CurrencyCode,
            //                            adValue.Value);
            //PrintStatus(msg);

            var realRevenue = (double)adValue.Value / 1000000;

            var impressionParameters = new[]
            {
                new Firebase.Analytics.Parameter("ad_platform", "Admob"),
                new Firebase.Analytics.Parameter("ad_source", "Admob"),
                new Firebase.Analytics.Parameter("ad_unit_name", bannerAdUnitId),
                new Firebase.Analytics.Parameter("ad_format", "Banner Adaptive"),
                new Firebase.Analytics.Parameter("value", realRevenue),
                new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode),
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
        }

        // Hiển thị banner
        public void ShowBanner()
        {
            if (FacilityManager.Instance.testGameConfig.isForVideoRecording)
            {
                return;
            }
            // CancelInvoke(nameof(RefreshBanner));
            // InvokeRepeating(nameof(RefreshBanner), FireBaseManager.Instance ? FireBaseManager.Instance.refreshBannerDelay : 30,FireBaseManager.Instance ? FireBaseManager.Instance.refreshBannerDelay : 30);
            _bannerView.Show();
        }

        // Ẩn banner
        public void HideBanner()
        {
            _bannerView.Hide();
        }

        /*#region APPOPEN ADS

    private AppOpenAd appOpenAd;
    private readonly TimeSpan APPOPEN_TIMEOUT = TimeSpan.FromHours(4);
    private DateTime appOpenExpireTime;
    public string adUnitBannerCollapsive, adUnitAppOpen;

    public void RequestAndLoadAppOpenAd()
    {
        PrintStatus("Requesting App Open ad.");
        string adUnitId;

        adUnitId = adUnitAppOpen;

        // destroy old instance.
        if (appOpenAd != null)
        {
            DestroyAppOpenAd();
        }

        // Create a new app open ad instance.
        AppOpenAd.Load(adUnitId, ScreenOrientation.Portrait, CreateAdRequest(),
            (AppOpenAd ad, LoadAdError loadError) =>
            {
                if (loadError != null)
                {
                    PrintStatus("App open ad failed to load with error: " +
                        loadError.GetMessage());
                    return;
                }
                else if (ad == null)
                {
                    PrintStatus("App open ad failed to load.");
                    return;
                }

                PrintStatus("App Open ad loaded. Please background the app and return.");
                this.appOpenAd = ad;
                this.appOpenExpireTime = DateTime.Now + APPOPEN_TIMEOUT;


                ad.OnAdFullScreenContentOpened += () =>
                {
                    PrintStatus("App open ad opened.");
                };
                ad.OnAdFullScreenContentClosed += () =>
                {
                    PrintStatus("App open ad closed.");

                    StartCoroutine(waitLoadAdOpen(1f));
                    try
                    {
                        AdManager.Instant._callbackOpenAD?.Invoke(false);
                        AdManager.Instant._callbackOpenAD = null;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("[3rdLib]==>Callback ad open error: " + e.ToString() + "<==");
                    }

                    if (is_resume_app)
                    {
                        is_resume_app = false;
                    }
                    //if(start.activeSelf || loading.activeSelf)
                    //    Native();
                };
                ad.OnAdImpressionRecorded += () =>
                {
                    PrintStatus("App open ad recorded an impression.");
                };
                ad.OnAdClicked += () =>
                {
                    PrintStatus("App open ad recorded a click.");
                };
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    PrintStatus("App open ad failed to show with error: " +
                        error.GetMessage());
                    LoadingScreen.instance.waitAdOpen();
                };
                ad.OnAdPaid += (AdValue adValue) =>
                {
                    var realRevenue = (double)adValue.Value / 1000000;

                    var impressionParameters = new[] {
                      new Firebase.Analytics.Parameter("ad_platform", "Admob"),
                      new Firebase.Analytics.Parameter("ad_source", "Admob"),
                      new Firebase.Analytics.Parameter("ad_unit_name", adUnitAppOpen),
                      new Firebase.Analytics.Parameter("ad_format", "AOA_Admob"),
                      new Firebase.Analytics.Parameter("value", realRevenue),
                      new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode),
                    };

                    Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);

                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    LogAdRevenue("Admob", realRevenue, adValue.CurrencyCode, dic);
                };
            });
    }

    public async Task LogAdRevenue(string monetiztionNetwork, double eventRevenue, string revenueCurrency, Dictionary<string, string> dic)
    {
        // use under appsflyer
        //while (AppsFlyer.instance == null)
        //{
        //    await Task.Delay(100);
        //}

        //UnityMainThread.wkr.AddJob(() =>
        //{
        //    AppsFlyerAdRevenue.logAdRevenue(monetiztionNetwork, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob, eventRevenue, revenueCurrency, dic);
        //});
    }

    IEnumerator waitLoadAdOpen(float time)
    {
        yield return new WaitForSeconds(time);
        RequestAndLoadAppOpenAd();
    }

    public void DestroyAppOpenAd()
    {
        if (this.appOpenAd != null)
        {
            this.appOpenAd.Destroy();
            this.appOpenAd = null;
        }
    }

    public bool IsAppOpenAdAvailable
    {
        get
        {
            return (appOpenAd != null
                    && appOpenAd.CanShowAd()
                    && DateTime.Now < appOpenExpireTime);
        }
    }

    public void ShowAppOpenAd()
    {
        Debug.Log("IsAppOpenAdAvailable = " + IsAppOpenAdAvailable);
        if (!IsAppOpenAdAvailable)
        {
            //LoadingScreen.instance.waitAdOpen();
            return;
        }
        //LoadingScreen.instance.waitAdOpen();
        appOpenAd.Show();
        Debug.Log("Ads_open_show");
    }

    #endregion*/

        /*#region Collapbanner

    private BannerView bannerView;
    private bool _bannerShowing = false;
    private int bannerRetryAttempt;

    public bool start_countdown_collap = false;
    [SerializeField] private float count_collap = 0f;
    public bool canShowCollap = true;


    public void RequestBannerCollapsiveAd()
    {
        //MessageBox.instance.showMessage("RequestBannerCollapsiveAd");

        //PrintStatus("Requesting Banner ad.");

        // These ad units are configured to always serve test ads.
        if (canShowCollap == false)
        {
            bannerView.Show();
            return;
        }
        canShowCollap = false;
        start_countdown_collap = true;
        string adUnitId;
        adUnitId = adUnitBannerCollapsive;


        if (bannerView != null)
            bannerView.Destroy();

        //PrintStatus("Requesting Banner ad.111");
        // Create a 320x50 banner at bottom of the screen
        //bannerView = new BannerView(adUnitId, AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom);
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        var adRequest = new AdRequest();

        // Create an extra parameter that aligns the bottom of the expanded ad to the
        // bottom of the bannerView.
        adRequest.Extras.Add("collapsible", "bottom");

        // Load a banner ad
        bannerView.LoadAd(adRequest);

        // Add Event Handlers
        bannerView.OnBannerAdLoaded += () =>
        {
            PrintStatus("Banner ad loaded.");
            _bannerShowing = true;
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            PrintStatus("Banner ad failed to load with error: " + error.GetMessage());

            _bannerShowing = false;
            bannerRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, bannerRetryAttempt));

            Invoke("ManuallyLoadBanner", (float)retryDelay);
        };
        bannerView.OnAdImpressionRecorded += () =>
        {
            PrintStatus("Banner ad recorded an impression.");
        };
        bannerView.OnAdClicked += () =>
        {
            PrintStatus("Banner ad recorded a click.");
        };
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            PrintStatus("Banner ad opening.");
            //AdManager.Instant.DestroyBanner();
            //Invoke("AwaitShowBannerMax", common.banner_CB_delay);
        };
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            PrintStatus("Banner ad closed.");
            _bannerShowing = false;
        };
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            //string msg = string.Format("{0} (currency: {1}, value: {2}",
            //                            "Banner ad received a paid event.",
            //                            adValue.CurrencyCode,
            //                            adValue.Value);
            //PrintStatus(msg);

            var realRevenue = (double)adValue.Value / 1000000;

            var impressionParameters = new[] {
              new Firebase.Analytics.Parameter("ad_platform", "Admob"),
              //new Firebase.Analytics.Parameter("ad_source", "Admob"),
              //new Firebase.Analytics.Parameter("ad_unit_name", adUnitBannerCollapsive),
              //new Firebase.Analytics.Parameter("ad_format", "Banner Collapsive"),
              new Firebase.Analytics.Parameter("value", realRevenue),
              new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode),
            };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            _ = LogAdRevenue("Admob", realRevenue, adValue.CurrencyCode, dic);
        };
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder()
               //.AddExtra("max_ad_content_rating", "1") // works fine
               .AddExtra("collapsible", "bottom") // do nothing
               .Build();
    }

    #endregion*/

        private int total_param_remote = 13;
        private int cur_param_done = 0;

        void checkAllRemoteGetDone()
        {
            cur_param_done++;
            //Debug.Log("cur_param_done == " + cur_param_done);
            if (cur_param_done >= total_param_remote) /// done
            {
                //Debug.LogError("remote config done == ");

                //if (common.reward_ads_id != "")
                //    AdManager.Instant._RewardedAdUnitID = common.reward_ads_id;
                //else
                //    AdManager.Instant._RewardedAdUnitID = "94504180894b62f1";

                //if(common.off_ads_new_ver == true)
                //    common.is_off_ads = true;

                initAdmob();

                /*if (common.AOA_Admob_on_off == false)
                AdManager.Instant.InitAdOpen();*/
            }
        }

        public bool loadRemoteConfig()
        {
            /*FireBaseManager.Instant.GetValueRemoteAsync("AOA_Admob_on_off", (value) =>
        {
            common.AOA_Admob_on_off = value.BooleanValue;
            Debug.Log("AOA_ADMOB == " + common.AOA_Admob_on_off);
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("adUnitAppOpen", (value) =>
        {
            adUnitAppOpen = value.StringValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("adUnitBannerCollapsive", (value) =>
        {
            adUnitBannerCollapsive = value.StringValue;
            checkAllRemoteGetDone();
        });*/

            /*FireBaseManager.Instant.GetValueRemoteAsync("NativeAdUnitId", (value) =>
        {
            NativeAdUnitId = value.StringValue;
            checkAllRemoteGetDone();
        });*/

            /*FireBaseManager.Instant.GetValueRemoteAsync("AOA_first_time", (value) =>
        {
            common.AOA_first_time = value.BooleanValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("AOA_open", (value) =>
        {
            common.AOA_open = value.BooleanValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("is_test", (value) =>
        {
            is_test = value.BooleanValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("is_off_ads", (value) =>
        {
            common.is_off_ads = value.BooleanValue;
            checkAllRemoteGetDone();
        });*/

            /*FireBaseManager.Instant.GetValueRemoteAsync("native_admob_on_off", (value) =>
        {
            common.native_admob_on_off = value.BooleanValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("native_loading_on_off", (value) =>
        {
            common.native_loading_on_off = value.BooleanValue;
            NativeManager.instance.ShowNativeLoading(common.native_loading_on_off);
            checkAllRemoteGetDone();
        });*/

            /*FireBaseManager.Instant.GetValueRemoteAsync("is_collapse", (value) =>
        {
            common.is_collapse = value.BooleanValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("capping_collapse_banner", (value) =>
        {
            common.capping_collapse_banner = (int)value.DoubleValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("capping_native", (value) =>
        {
            common.capping_native = (int)value.DoubleValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("config_time_countdown_show_inter", (value) =>
        {
            common.config_time_countdown_show_inter = (int)value.DoubleValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("AOA_resume", (value) =>
        {
            common.AOA_resume = value.BooleanValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("ad_break_count", (value) =>
        {
            common.ad_break_count = (int)value.DoubleValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("ads_break", (value) =>
        {
            common.ads_break = value.BooleanValue;
            checkAllRemoteGetDone();
        });


        FireBaseManager.Instant.GetValueRemoteAsync("rate_on_off", (value) =>
        {
            common.rate_on_off = value.BooleanValue;
            checkAllRemoteGetDone();
        });

        FireBaseManager.Instant.GetValueRemoteAsync("rate_time", (value) =>
        {
            common.rate_time = (int)value.DoubleValue;
            checkAllRemoteGetDone();
        });*/

            //FireBaseManager.Instant.GetValueRemoteAsync("off_ads_new_ver", (value) =>
            //{
            //    common.off_ads_new_ver = value.BooleanValue;
            //    checkAllRemoteGetDone();
            //});
            return true;
        }

        private void PrintStatus(string message)
        {
            Debug.Log(message);
            //MessageBox.instance.showMessage(message);
        }

        public void btnDebugMode()
        {
            MaxSdk.ShowMediationDebugger();
        }


        #region NATIVE ADS

        [Header("Native")] bool initAdmobSuccess = false;
        GameObject _nativeObjectCalled;

        [SerializeField] int countNativeTexture = 0;

        private bool
            _startCountRequestNative =
                false; // sau khi tắt UI native đang show thì cho phép count down request native mới. UI đang show sẽ ko thay đổi native khi đang bật nhưng đảm bảo request ko thừa

        bool canRequestNative = true; // biến check xem Native request trước đó đã xử dụng hay chưa
        float countNative = 0f;
        float cappingNative = 1f;

        public List<GameObject> _native_object_list;
        public List<NativeAd> _nativead_list = new List<NativeAd>();

        int maxRequestNative = 1; // Request 1 lần cho mỗi UI sử dụng, không làm 2 native trên 1 màn hình

        public NativeAd _nativead;
        private bool _isRequestingNativeAd;

        public void RequestNativeAd()
        {
            return;
            // if (!canRequestNative)
            //     return;

            /// Destroy phần native đã request trước và đã sử dụng cho UI native showing.
            /// Nativeads đã sử dụng cho 1 UI show, nếu sử dụng tiếp cho UI khác sẽ gây lỗi, ko click được ads

            for (var i = 0; i < _nativead_list.Count; i++)
            {
                if (_nativead_list[i] != null)
                {
                    _nativead_list[i].Destroy();
                }
            }

            countNativeTexture = 0;
            _nativead_list.Clear();

            Debug.Log("Start Request Native ");

            AdLoader adLoader = new AdLoader.Builder(nativeAdUnitId)
                .ForNativeAd()
                .SetNumberOfAdsToLoad(maxRequestNative)
                .Build();
            adLoader.OnNativeAdLoaded += HandleNativeAdLoaded;
            adLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
            adLoader.OnNativeAdClicked += HandleNativeAdClicked;
            adLoader.LoadAd(new AdRequest());
        }

        [Obsolete("Obsolete")]
        private void OnPaidEvent(object sender, AdValueEventArgs adValueEventArgs)
        {
            if (sender is NativeAd nativeAdSender)
            {
                //Debug.Log(String.Format("Banner view paid {0} {1}.", 
                //        adValue.AdValue, 
                //        adValue.CurrencyCode)); 
                AdValue adValue = adValueEventArgs.AdValue;

                //long trueValue = adValue.Value / 1000000L; 

                var trueValue = (double)adValue.Value / 1000000;

                //AdValue adValue = args.AdValue; 


                string currencyCode = adValue.CurrencyCode;
                ResponseInfo responseInfo = nativeAdSender.GetResponseInfo();
                string responseId = responseInfo.GetResponseId();

                AdapterResponseInfo loadedAdapterResponseInfo = responseInfo.GetLoadedAdapterResponseInfo();
                string adSourceId = loadedAdapterResponseInfo.AdSourceId;
                string adSourceInstanceId = loadedAdapterResponseInfo.AdSourceInstanceId;
                string adSourceInstanceName = loadedAdapterResponseInfo.AdSourceInstanceName;
                string adSourceName = loadedAdapterResponseInfo.AdSourceName;
                string adapterClassName = loadedAdapterResponseInfo.AdapterClassName;
                long latencyMillis = loadedAdapterResponseInfo.LatencyMillis;
                Dictionary<string, string> credentials = loadedAdapterResponseInfo.AdUnitMapping;

                var impressionParameters = new[]
                {
                    new Firebase.Analytics.Parameter("ad_platform", "Admob"),
                    new Firebase.Analytics.Parameter("ad_source", adSourceName),
                    new Firebase.Analytics.Parameter("ad_unit_name", nativeAdUnitId),
                    new Firebase.Analytics.Parameter("ad_format", "NATIVE"),
                    new Firebase.Analytics.Parameter("value", trueValue),
                    new Firebase.Analytics.Parameter("currency", currencyCode), // All AppLovin revenue is sent in USD 
                };
                Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
                Debug.Log("impression_logged");
                //StartCoroutine(Init.Instance.LogEventFirebase("ad_impression", impressionParameters));   
            }
        }

        private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log("Native ad failed to load: " + args.LoadAdError.GetMessage());
            Invoke("CallRequestNextNativeAd", 5f);
        }

        private void HandleNativeAdClicked(object sender, EventArgs args)
        {
            AdsManager.Instance.IsRecentlyShowAd = true;
        }

        private void HandleNativeAdLoaded(object sender, NativeAdEventArgs args)
        {
            Debug.Log("Native ad loaded.");
            //nativeAd = args.nativeAd;
            // _nativead_list.Add(args.nativeAd);
            _nativead = args.nativeAd;
            countNativeTexture++;

            if (loadingScene)
                loadingScene.ShowNativeAds();

            if (countNativeTexture >= maxRequestNative)
            {
                if (_nativeObject)
                    SetTextureAndDetail();
                // check UI native có đang active và cần data để show luôn không
                // if (NativeManager.instance.checkHaveNativeShowing())
                //     SetTextureAndDetail();
            }
        }

        public void SetTextureAndDetail()
        {
            if (_nativead == null)
            {
                return;
            }


            /*if (firstShowNative == false)
            {
                /// Sử dụng khi có show native phần loading đợi aoa, Invoke sau 2s hiện native loading

                firstShowNative = true;
                Invoke("ShowNativeLoadingDone", 2f);
            }*/

            /*if (!canRequestNative)
                return;

            if (_native_object_list.Count <= 0)
                return;*/

            // if(FacilityManager.Instance)
            //     FacilityManager.Instance.AddLog("_native_object_list: " + _native_object_list.Count);

            // for (var i = 0; i < _native_object_list.Count; i++)
            // {
            // NativeAd nativeAd = _nativead_list[i];
            // if (nativeAd == null)
            //     continue;

            // var NADS = _native_object_list[i];

            NativeAd nativeAd = _nativead;
            var NADS = _nativeObject;
            // if (NADS == null) continue;

            GameObject AdLoaded = NADS.transform.GetChild(0).gameObject;
            GameObject AdLoading = NADS.transform.GetChild(1).gameObject;

            // if (AdLoaded == null) continue;
            // if (AdLoading == null) continue;

            AdLoaded.gameObject.SetActive(false);
            AdLoading.gameObject.SetActive(false);

            RawImage AdIconTexture = AdLoaded.transform.GetChild(1).GetComponent<RawImage>();
            RawImage AdIconChoice = AdLoaded.transform.GetChild(2).GetComponent<RawImage>();

            Text AdHeadline = AdLoaded.transform.GetChild(3).GetComponent<Text>();
            Text Advertiser = AdLoaded.transform.GetChild(4).GetComponent<Text>();
            Text BodyNative = AdLoaded.transform.GetChild(5).GetComponent<Text>();
            Text CallActionText = AdLoaded.transform.GetChild(7).transform.GetChild(0).GetComponent<Text>();

            Texture2D iconTexture = nativeAd.GetIconTexture();
            Texture2D iconAdChoices = nativeAd.GetAdChoicesLogoTexture();

            if (iconTexture)
                AdIconTexture.texture = iconTexture;
            else
                AdIconTexture.gameObject.SetActive(false);

            if (iconAdChoices)
                AdIconChoice.texture = iconAdChoices;
            else
                AdIconChoice.gameObject.SetActive(false);

            AdHeadline.text = nativeAd.GetHeadlineText();
            BodyNative.text = nativeAd.GetBodyText();
            CallActionText.text = nativeAd.GetCallToActionText();
            Advertiser.text = nativeAd.GetAdvertiserText();

            //register gameobjects with native ads api
            if (!nativeAd.RegisterIconImageGameObject(AdIconTexture.gameObject))
            {
                Debug.Log("error registering AdIconTexture");
            }

            if (!nativeAd.RegisterAdChoicesLogoGameObject(AdIconChoice.gameObject))
            {
                Debug.Log("error registering AdIconChoice");
            }

            if (!nativeAd.RegisterHeadlineTextGameObject(AdHeadline.gameObject))
            {
                Debug.Log("error registering AdHeadline");
            }

            if (!nativeAd.RegisterBodyTextGameObject(BodyNative.gameObject))
            {
                Debug.Log("error registering BodyNative");
            }

            if (!nativeAd.RegisterCallToActionGameObject(CallActionText.gameObject))
            {
                Debug.Log("error registering CallActionText");
            }

            if (!nativeAd.RegisterAdvertiserTextGameObject(Advertiser.gameObject))
            {
                Debug.Log("error registering Advertiser");
            }

            nativeAd.OnPaidEvent += OnPaidEvent;

            //disable loading and enable ad object
            AdLoaded.gameObject.SetActive(true);
            AdLoading.gameObject.SetActive(false);

            _nativead = null;
            _nativeObject = null;
            RequestNativeAd();
            // }

            _native_object_list.Clear();
            // canRequestNative = false;
        }

        public void CallRequestNextNativeAd()
        {
            // if (canRequestNative) 
                RequestNativeAd();
        }

        bool firstShowNative = false;
        public bool showNativeLoadingDone = false;

        public static void startCountRequestNativeNow()
        {
            
        }

        private GameObject _nativeObject;

        public void setNativeObject(GameObject obj)
        {
            _nativeObject = obj;
            // _native_object_list.Clear();
            // _native_object_list.Add(obj);
        }

        #endregion
    }
}