using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Adverstising_Integration.Scripts;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Loading;

namespace Advertising
{
    public class FireBaseManager : Singleton<FireBaseManager>
    {
        [SerializeField] private LoadingScene loadingScene;

        #region Tracking Events

        public const string FIRST_LOAD = "first_loading_complete";
        
        public static string BOOSTER_SPEED_GEM = "booster_speed_gem";
        public static string BOOSTER_SPEED_ADS = "booster_speed_ads";
        public static string VIP_DOG_GEM = "vip_dog_gem";
        public static string VIP_DOG_ADS = "vip_dog_ads";
        public static string VIP_LADY_GEM = "vip_lady_gem";
        public static string VIP_LADY_ADS = "vip_lady_ads";
        public static string BOOSTER_CASH_GEM = "booster_cash_gem";
        public static string BOOSTER_CASH_ADS = "booster_cash_ads";
        public static string BOOSTER_SPEED_WORKER_GEM = "booster_speed_worker_gem";
        public static string BOOSTER_SPEED_WORKER_ADS = "booster_speed_worker_ads";
        public static string SPIN_WHEEL_FREE = "spin_wheel_free";
        public static string SPIN_WHEEL_GEM = "spin_wheel_gem";
        public static string SPIEN_WHEEL_ADS = "spin_wheel_ads";
        public static string BUY_PACK_HERO = "buy_pack_hero";
        public static string BUY_PACK_LEGEND = "buy_pack_legend";
        public static string SKIN = "skin";
        public static string BUY_SKIN = "buy_skin";
        public static string CASH_GEM_1 = "cash_gem_1";
        public static string CASH_GEM_2 = "cash_gem_2";
        public static string CASH_GEM_3 = "cash_gem_3";
        public static string GEM_1 = "gem_1";
        public static string GEM_2 = "gem_2";
        public static string GEM_3 = "gem_3";
        
        public static string CLICK_BATTLE_FUND = "click_battle_fund";
        public static string CLICK_BATTLE_FUND_VIP_1 = "click_battle_fund_vip_1";
        public static string CLICK_BATTLE_FUND_VIP_2 = "click_battle_fund_vip_2";
        public static string STAFF_BUY_BED_ROOM = "staff_buy_bedroom";
        public static string STAFF_BUY_BOXING_ROOM = "staff_buy_boxing_room";
        public static string STAFF_BUY_TRAINING_ROOM = "staff_buy_training_room";
        public static string STAFF_BUY_DINING_ROOM = "staff_buy_dining_room";
        public static string CLICK_DOG_ICON = "click_dog_icon";
        public static string CLICK_LADY_ICON = "click_lady_icon";
        public static string CLICK_BOOSTER_CASH_ICON = "click_booster_cash_icon";
        public static string CLICK_BOOSTER_SPEED_ICON = "click_booster_speed_icon";
        public static string CLICK_BOOSTER_SPEED_WORKER_ICON = "click_booster_speed_worker_icon";
        public static string OPEN_DOG = "click_dog";
        public static string OPEN_LADY = "click_lady";
        public static string OPEN_BOOSTER_CASH = "click_booster_cash";
        public static string OPEN_BOOSTER_SPEED = "click_booster_speed";
        public static string OPEN_BOOSTER_SPEED_WORKER = "click_booster_speed_worker";
        public static string CLAIM_DAILY_QUEST = "claim_daily_quest";
        public static string BUILD_VIP_BED_ROOM = "build_vip_bed_room";
        public static string BUILD_VIP_BED_ROOM_2 = "build_vip_bed_room_2";
        public static string BUILD_BED_ROOM_2 = "build_bed_room_2";
        public static string BUY_TANK = "buy_tank";
        public static string BUY_MISSILE = "buy_missile";
        public static string BUY_ARMORED = "buy_armored";
        public static string BUILD_TANK = "build_tank";
        public static string BUILD_MISSILE = "build_missile";
        public static string BUILD_ARMORED = "build_armored";
        public static string CLICK_CHECK_CAMERA = "click_check_camera";
        public static string FINISH_TUTORIAL = "finish_tutorial";
        public static string LEVEL_UP = "level_up";
        public static string BUY_IAP_PACK_1_CLICK = "buy_iap_pack_1_click";
        public static string BUY_IAP_PACK_2_CLICK = "buy_iap_pack_2_click";
        public static string BUY_IAP_PACK_3_CLICK = "buy_iap_pack_3_click";
        public static string BUY_REMOVE_ADS_CLICK = "buy_remove_ads_click";
        public static string BUY_REMOVE_ADS_VIP_CLICK = "buy_remove_ads_vip_click";
        public static string BUY_IAP_BATTLE_FUND_1_CLICK = "buy_iap_battle_fund_1_click";
        public static string BUY_IAP_BATTLE_FUND_2_CLICK = "buy_iap_battle_fund_2_click";
        public static string BUY_IAP_BATTLE_FUND_3_CLICK = "buy_iap_battle_fund_3_click";
        public static string BUY_IAP_PACK_1_SUCCESS = "buy_iap_pack_1_success";
        public static string BUY_IAP_PACK_2_SUCCESS = "buy_iap_pack_2_success";
        public static string BUY_IAP_PACK_3_SUCCESS = "buy_iap_pack_3_success";
        public static string BUY_REMOVE_ADS_SUCCESS = "buy_remove_ads_success";
        public static string BUY_REMOVE_ADS_VIP_SUCCESS = "buy_remove_ads_vip_success";
        public static string BUY_IAP_BATTLE_FUND_1_SUCCESS = "buy_iap_battle_fund_1_success";
        public static string BUY_IAP_BATTLE_FUND_2_SUCCESS = "buy_iap_battle_fund_2_success";
        public static string BUY_IAP_BATTLE_FUND_3_SUCCESS = "buy_iap_battle_fund_3_success";
        
        public static string BUY_BED_ROOM_LEVEL_1_CLICK = "buy_bed_room_level_1_click";
        public static string BUY_BED_ROOM_LEVEL_2_CLICK = "buy_bed_room_level_2_click";

        public static string BUY_BED_ROOM_LEVEL_1_SUCCESS = "buy_bed_room_level_1_success";
        public static string BUY_BED_ROOM_LEVEL_2_SUCCESS = "buy_bed_room_level_2_success";

        public static string RECEIVE_GIFT_DAY_1 = "receive_gift_day_1";
        public static string RECEIVE_GIFT_DAY_2 = "receive_gift_day_2";
        public static string RECEIVE_GIFT_DAY_3 = "receive_gift_day_3";
        public static string RECEIVE_GIFT_DAY_4 = "receive_gift_day_4";
        public static string RECEIVE_GIFT_DAY_5 = "receive_gift_day_5";
        public static string RECEIVE_GIFT_DAY_6 = "receive_gift_day_6";
        public static string RECEIVE_GIFT_DAY_7 = "receive_gift_day_7";
        public static string RECEIVE_GIFT_DAY_8 = "receive_gift_day_8";

        public static string RECEIVE_GIFT_DAY_1_X3 = "receive_gift_day_1_x3";
        public static string RECEIVE_GIFT_DAY_2_X3 = "receive_gift_day_2_x3";
        public static string RECEIVE_GIFT_DAY_3_X3 = "receive_gift_day_3_x3";
        public static string RECEIVE_GIFT_DAY_4_X3 = "receive_gift_day_4_x3";
        public static string RECEIVE_GIFT_DAY_5_X3 = "receive_gift_day_5_x3";
        public static string RECEIVE_GIFT_DAY_6_X3 = "receive_gift_day_6_x3";
        public static string RECEIVE_GIFT_DAY_7_X3 = "receive_gift_day_7_x3";
        public static string RECEIVE_GIFT_DAY_8_X3 = "receive_gift_day_8_x3";

        public static string SHOW_BANNER_ADS = "show_banner_ads";
        public static string SHOW_INTER_ADS = "show_inter_ads";
        public static string SHOW_INTER_ADS_SUCCESS = "show_inter_ads_success";
        public static string SHOW_REWARD_ADS_VIP_BED_ROOM = "show_reward_ads_vip_bed_room";
        public static string SHOW_REWARD_ADS_X2_DAILY_GIFT = "show_reward_ads_x2_daily_gift";
        public static string SHOW_REWARD_ADS_SPEED_BOOSTER = "show_reward_ads_speed_booster";
        public static string SHOW_REWARD_ADS_WORKER_SPEED_BOOSTER = "show_reward_ads_worker_speed_booster";
        public static string SHOW_REWARD_ADS_MONEY = "show_reward_ads_money";
        public static string SHOW_REWARD_ADS_MONEY_2 = "show_reward_ads_money_2";
        public static string SHOW_REWARD_ADS_LADY_WORKER = "show_reward_ads_lady_worker";
        public static string SHOW_REWARD_ADS_VIP_SOLDIER = "show_reward_ads_vip_soldier";
        public static string SHOW_REWARD_ADS_X2_LEVEL_UP = "show_reward_ads_x2_level_up";
        public static string SHOW_REWARD_ADS_SPIN_WHEEL = "show_reward_ads_spin_wheel";
        public static string SHOW_REWARD_ADS_SHOP_GEM = "show_reward_ads_shop_gem";
        public static string SHOW_REWARD_ADS_SHOP_CASH = "show_reward_ads_shop_cash";
        public static string SHOW_REWARD_ADS_SHOP_TOKEN = "show_reward_ads_shop_token";
        public static string Show_REWARD_ADS_RETAL_SKIN = "show_reward_ads_retal_skin";
        public static string SHOW_REWARD_ADS_SUCCESS_VIP_BED_ROOM = "show_reward_ads_success_vip_bed_room";
        public static string SHOW_REWARD_ADS_SUCCESS_X2_DAILY_GIFT = "show_reward_ads_success_x2_daily_gift";
        public static string SHOW_REWARD_ADS_SUCCESS_SPEED_BOOSTER = "show_reward_ads_success_speed_booster";
        public static string SHOW_REWARD_ADS_SUCCESS_MONEY = "show_reward_ads_success_money";
        public static string SHOW_REWARD_ADS_SUCCESS_MONEY_2 = "show_reward_ads_success_money_2";
        public static string SHOW_REWARD_ADS_SUCCESS_LADY_WORKER = "show_reward_ads_success_lady_worker";
        public static string SHOW_REWARD_ADS_SUCCESS_VIP_SOLDIER = "show_reward_ads_success_vip_soldier";
        public static string SHOW_REWARD_ADS_SUCCESS_X2_LEVEL_UP = "show_reward_ads_success_x2_level_up";
        public static string SHOW_REWARD_ADS_SUCCESS_SPIN_WHEEL = "show_reward_ads_success_spin_wheel";
        public static string SHOW_REWARD_ADS_SUCCESS_SHOP_GEM = "show_reward_ads_success_shop_gem";
        public static string SHOW_REWARD_ADS_SUCCESS_SHOP_CASH = "show_reward_ads_success_shop_cash";
        public static string SHOW_REWARD_ADS_SUCCESS_SHOP_TOKEN = "show_reward_ads_success_shop_token";
        public static string SHOW_REWARD_ADS_CANCEL = "show_reward_ads_cancel";
        public static string SHOW_REWARD_ADS_SUCCESS_WORKER_SPEED_BOOSTER = "show_reward_ads_success_worker_speed_booster";
        public static string Show_REWARD_ADS_SUCCESS_RETAL_SKIN = "show_reward_ads_success_retal_skin";

        public static string FIRST_TIME_PLAY_GAME = "first_time_play_game";
        public static string VALUE = "value";

        #endregion

        #region Banner Events

        public static string BANNER_AD_LOAD_SUCCESS = "banner_load_success";
        public static string BANNER_AD_LOAD_FAILED = "banner_ad_load_failed";
        public static string BANNER_AD_DISPLAYED = "banner_displayed";
        public static string BANNER_AD_REVENUE = "banner_ad_revenue";

        #endregion

        #region Interstitial Ads Events

        public static string INTER_AD_ELIGIBLE = "inters_ad_eligible";
        public static string INTER_AD_LOAD_SUCCESS = "inters_ad_load_success";
        public static string INTER_AD_LOAD_FAILED = "inters_ad_load_failed";
        public static string INTER_AD_DISPLAYED = "inters_displayed";
        public static string INTER_AD_DISPLAY_FAILED = "inters_ad_display_failed";
        public static string INTER_AD_REVENUE = "inters_ad_revenue";

        #endregion

        #region MyGuardianBase Events

        public static string LOADING_SUCCESSFUL = "loading_successful";
        public static string UNLOCK_RECEPTIONABLE = "unlock_receptiontable";
        public static string UNLOCK_AREA_ID_2 = "unlock_area_id_2";
        public static string UNLOCK_AREA_ID_3 = "unlock_area_id_3";
        public static string UNLOCK_AREA_ID_4 = "unlock_area_id_4";
        public static string UNLOCK_AREA_ID_5 = "unlock_area_id_5";
        public static string UNLOCK_AREA_ID_6 = "unlock_area_id_6";
        public static string BATTLE_AREA_ID_1_WIN = "battle_area_1_win";
        public static string BATTLE_AREA_ID_2_WIN = "battle_area_2_win";
        public static string BATTLE_AREA_ID_3_WIN = "battle_area_3_win";
        public static string BATTLE_AREA_ID_4_WIN = "battle_area_4_win";
        public static string BATTLE_AREA_ID_5_WIN = "battle_area_5_win";
        
        public static string WIN_BATTLE_MAP_1 = "win_battle_map_1";
        public static string WIN_BATTLE_MAP_2 = "win_battle_map_2";
        public static string WIN_BATTLE_MAP_3 = "win_battle_map_3";
        
        public static string LOSE_BATTLE_MAP_1 = "win_battle_map_1";
        public static string LOSE_BATTLE_MAP_2 = "win_battle_map_2";
        public static string LOSE_BATTLE_MAP_3 = "win_battle_map_3";

        #endregion

        #region Rewarded Ads Events

        public static string REWARDED_AD_ELIGIBLE = "rewarded_ad_eligible";
        public static string REWARDED_AD_LOAD_SUCCESS = "rewarded_ad_load_success";
        public static string REWARDED_AD_LOAD_FAILED = "rewarded_ad_load_failed";
        public static string REWARDED_AD_DISPLAYED = "rewarded_ad_displayed";
        public static string REWARDED_AD_DISPLAY_FAILED = "rewarded_ad_display_failed";
        public static string REWARDED_AD_REVENUE = "rewarded_ad_revenue";

        #endregion

        #region App Open Ads

        public static string AOA_LOAD_SUCCESS = "aoa_load_success";
        public static string AOA_LOAD_FAILED = "aoa_load_failed";
        public static string AOA_DISPLAYED = "aoa_displayed";
        public static string AOA_DISPLAY_FAILED = "aoa_display_failed";
        public static string AOA_REVENUE = "aoa_revenue";

        #endregion

        #region Remote Configs

        public const string NEXT_INTER_MIN_TIME = "next_inter_min_time";
        public const string INTER_ADS_DELAY = "inter_ads_delay";
        public const string DESERTER_DELAY = "deserter_delay";
        public const string FIRST_TIME_SHOW_INTER_ADS_DELAY = "first_time_show_inter_ads_delay";
        public const string FIRST_TIME_SHOW_BANNER_DELAY = "first_time_show_banner_delay";
        public const string REFRESH_BANNER_DELAY = "refresh_banner_delay";
        public const string CAPPING_TIME = "capping_time";
        public const string PLAYER_SPEED = "player_speed_new";
        public const string SHOW_AOA = "show_aoa_new";
        public const string SHOW_MREC_BATTLE = "show_mrec_battle";
        public const string SHOW_MREC_DAILY_QUEST_POPUP = "show_mrec_daily_quest_popup";
        public const string SHOW_MREC_GET_MONEY_POPUP = "show_mrec_get_money_popup";
        public const string SHOW_MREC_INTRO = "show_mrec_intro";
        public const string SHOW_MREC_NO_ADS_POPUP = "show_mrec_no_ads_popup";
        public const string SHOW_MREC_SETTING_POPUP = "show_mrec_setting_popup";
        public const string SHOW_MREC_SPEED_BOOSTER_POPUP = "show_mrec_speed_booster_popup";
        public const string SHOW_MREC_UPGRADE_POPUP = "show_mrec_upgrade_popup";
        public const string SHOW_MREC_VIP_SOLIDER_POPUP = "show_mrec_vip_soldier_popup";
        public const string SHOW_NATIVE_LOADING = "show_native_loading";
        public const string SHOW_NATIVE_BATTLE = "show_native_battle";
        public const string SHOW_NATIVE_BUILD_BEDROOM_POPUP = "show_native_build_bedroom_popup";
        public const string SHOW_NATIVE_BUY_WORKER_POPUP = "show_native_buy_worker_popup";
        public const string SHOW_NATIVE_DAILY_QUEST_POPUP = "show_native_daily_quest_popup";
        public const string SHOW_NATIVE_GET_MONEY_POPUP = "show_native_get_money_popup";
        public const string SHOW_NATIVE_INTRO = "show_native_intro";
        public const string SHOW_NATIVE_NO_ADS_POPUP = "show_native_no_ads_popup";
        public const string SHOW_NATIVE_SETTING_POPUP = "show_native_setting_popup";
        public const string SHOW_NATIVE_SHOP_POPUP = "show_native_shop_popup";
        public const string SHOW_NATIVE_SKIN_SHOP_POPUP = "show_native_skin_shop_popup";
        public const string SHOW_NATIVE_SPEED_BOOSTER_POPUP = "show_native_speed_booster_popup";
        public const string SHOW_NATIVE_TRAINING_ROOM_1_POPUP = "show_native_training_room_1";
        public const string SHOW_NATIVE_UPGRADE_POPUP = "show_native_upgrade_popup";
        public const string SHOW_NATIVE_VIP_SOLDIER_POPUP = "show_native_vip_soldier_popup";
        public const string SHOW_NOEL = "show_noel";
        public const string SHOW_INTER = "show_inter";
        public const string SHOW_TAKE_A_BREAK = "show_take_a_break";

        #endregion

        private DependencyStatus _dependencyStatus = DependencyStatus.UnavailableOther;

        #region Unity Methods

        void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                _dependencyStatus = task.Result;
                if (_dependencyStatus == DependencyStatus.Available)
                {
                    InitRemoteConfig();
                    var startEventName = FireBaseManager.LOADING_SUCCESSFUL;
                    // Debug.Log(startEventName);
                    // FireBaseManager.Instance.LogEvent(startEventName);

                    Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
                    Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

                    // if (Application.platform == RuntimePlatform.Android && AndroidSdkVersion() >= 33)
                    // {
                        var isRequestedPermissionNotify = PlayerPrefs.GetInt("requestedPermissionNotify", 0);
                        if (isRequestedPermissionNotify == 0)
                        {
                            askRequst();
                            // common.request_permission_notify = 1;
                            PlayerPrefs.SetInt("requestedPermissionNotify", 1);
                        }
                    // }
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {_dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        int AndroidSdkVersion()
        {
            string sdkVersion = SystemInfo.operatingSystem;
            int version = 0;
            if (sdkVersion.Contains("API-"))
            {
                int.TryParse(sdkVersion.Substring(sdkVersion.IndexOf("API-") + 4, 2), out version);
            }

            return version;
        }

        void askRequst() //call this function to ask request
        {
            if (UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
            {
                print("permission granted!!");
            }
            else
            {
                var callbacks = new UnityEngine.Android.PermissionCallbacks();
                callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;
                callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
                callbacks.PermissionDeniedAndDontAskAgain += PermissionCallbacks_PermissionDeniedAndDontAskAgain;
                UnityEngine.Android.Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS",
                    callbacks);
            }
        }

        internal void PermissionCallbacks_PermissionDeniedAndDontAskAgain(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionDeniedAndDontAskAgain");
        }

        internal void PermissionCallbacks_PermissionGranted(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionCallbacks_PermissionGranted");
        }

        internal void PermissionCallbacks_PermissionDenied(string permissionName)
        {
            Debug.Log($"{permissionName} PermissionCallbacks_PermissionDenied");
        }

        /// 2 message này để lắng nghe notify firebase
        public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
        {
            UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
        }

        public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
        {
            UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
        }

        #endregion

        #region Handle Remote Config

        private void InitRemoteConfig()
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>
            {
                { NEXT_INTER_MIN_TIME, 30 },
                { INTER_ADS_DELAY, 200 },
                { DESERTER_DELAY, 120 },
                { FIRST_TIME_SHOW_INTER_ADS_DELAY, 300 },
                { FIRST_TIME_SHOW_BANNER_DELAY, 5 },
                { REFRESH_BANNER_DELAY, 30 },
                { CAPPING_TIME, 60 },
                { PLAYER_SPEED, 100 },
                { SHOW_AOA, false },
                { SHOW_MREC_BATTLE, false },
                { SHOW_MREC_DAILY_QUEST_POPUP, false },
                { SHOW_MREC_GET_MONEY_POPUP, false },
                { SHOW_MREC_INTRO, false },
                { SHOW_MREC_NO_ADS_POPUP, false },
                { SHOW_MREC_SETTING_POPUP, false },
                { SHOW_MREC_SPEED_BOOSTER_POPUP, false },
                { SHOW_MREC_UPGRADE_POPUP, false },
                { SHOW_MREC_VIP_SOLIDER_POPUP, false },
                { SHOW_NATIVE_LOADING, false },
                { SHOW_NATIVE_BATTLE, false },
                { SHOW_NATIVE_BUILD_BEDROOM_POPUP, false },
                { SHOW_NATIVE_BUY_WORKER_POPUP, false },
                { SHOW_NATIVE_DAILY_QUEST_POPUP, false },
                { SHOW_NATIVE_GET_MONEY_POPUP, false },
                { SHOW_NATIVE_INTRO, false },
                { SHOW_NATIVE_NO_ADS_POPUP, false },
                { SHOW_NATIVE_SETTING_POPUP, false },
                { SHOW_NATIVE_SHOP_POPUP, false },
                { SHOW_NATIVE_SKIN_SHOP_POPUP, false },
                { SHOW_NATIVE_SPEED_BOOSTER_POPUP, false },
                { SHOW_NATIVE_TRAINING_ROOM_1_POPUP, false },
                { SHOW_NATIVE_UPGRADE_POPUP, false },
                { SHOW_NATIVE_VIP_SOLDIER_POPUP, false },
                { SHOW_NOEL, false },
                { SHOW_INTER, false },
                { SHOW_TAKE_A_BREAK, false },
            };
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
                .ContinueWithOnMainThread(_ => FetchDataAsync());
        }

        Task FetchDataAsync()
        {
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void DisplayData()
        {
            Debug.LogError($"Firebase Remote Config: {INTER_ADS_DELAY} {FirebaseRemoteData.InterAdsTime}");
        }

        public int nextInterMinTime = 30;
        public int firstTimeShowInterAdsDelay = 200;
        public int firstTimeShowBannerDelay = 5;
        public int refreshBannerDelay = 30;
        public int cappingTime = 60;
        public int playerSpeed = 100;
        public int interAdsDelay = 120;
        public int deserterDelay = 120;
        public bool showFirstAoa;
        public bool showAoa;
        public bool showMrecBattle;
        public bool showMrecDailyQuestPopup;
        public bool showMrecGetMoneyPopup;
        public bool showMrecIntro;
        public bool showMrecNoAdsPopup;
        public bool showMrecSettingPopup;
        public bool showMrecSpeedBoosterPopup;
        public bool showMrecUpgradePopup;
        public bool showMrecVipSoldierPopup;
        public bool showNativeLoading;
        public bool showNativeBattle;
        public bool showNativeBuildBedroomPopup;
        public bool showNativeBuyWorkerPopup;
        public bool showNativeDailyQuestPopup;
        public bool showNativeGetMoneyPopup;
        public bool showNativeIntro;
        public bool showNativeNoAdsPopup;
        public bool showNativeSettingPopup;
        public bool showNativeShopPopup;
        public bool showNativeSkinShopPopup;
        public bool showNativeSpeedBoosterPopup;
        public bool showNativeTrainingRoom1;
        public bool showNativeUpgradePopup;
        public bool showNativeVipSoldierPopup;
        public bool showNoel;
        public bool showInter;
        public bool showTakeABreak;

        private void FetchComplete(Task fetchTask)
        {
            var info = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    Debug.Log("FetchComplete success");
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(_ =>
                    {
                        // FirebaseRemoteData.SetInterAdsTime(FirebaseToInt(INTER_ADS_DELAY));
                        // FirebaseRemoteData.SetUseOpenAds(FirebaseToBool(USE_OPEN_ADS));
                        nextInterMinTime = (int)FirebaseRemoteConfig.DefaultInstance.GetValue(NEXT_INTER_MIN_TIME)
                            .LongValue;
                        
                        firstTimeShowInterAdsDelay = (int)FirebaseRemoteConfig.DefaultInstance
                            .GetValue(FIRST_TIME_SHOW_INTER_ADS_DELAY).LongValue;
                        
                        firstTimeShowBannerDelay = (int)FirebaseRemoteConfig.DefaultInstance
                            .GetValue(FIRST_TIME_SHOW_BANNER_DELAY).LongValue;
                        
                        refreshBannerDelay = (int)FirebaseRemoteConfig.DefaultInstance
                            .GetValue(REFRESH_BANNER_DELAY).LongValue;
                        
                        cappingTime = (int)FirebaseRemoteConfig.DefaultInstance
                            .GetValue(CAPPING_TIME).LongValue;
                        
                        playerSpeed = (int)FirebaseRemoteConfig.DefaultInstance
                            .GetValue(PLAYER_SPEED).LongValue;

                        playerSpeed = 100;
                        
                        interAdsDelay = (int)FirebaseRemoteConfig.DefaultInstance.GetValue(INTER_ADS_DELAY).LongValue;
                        
                        deserterDelay = (int)FirebaseRemoteConfig.DefaultInstance.GetValue(DESERTER_DELAY).LongValue;
                        
                        showAoa = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_AOA).BooleanValue;
                        
                        showMrecBattle = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_MREC_BATTLE).BooleanValue;
                        
                        showMrecDailyQuestPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_MREC_DAILY_QUEST_POPUP).BooleanValue;
                        
                        showMrecGetMoneyPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_MREC_GET_MONEY_POPUP).BooleanValue;
                        
                        showMrecIntro = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_MREC_INTRO).BooleanValue;
                        
                        showMrecNoAdsPopup = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_MREC_NO_ADS_POPUP)
                            .BooleanValue;
                        
                        showMrecSettingPopup = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_MREC_SETTING_POPUP)
                            .BooleanValue;
                        
                        showMrecSpeedBoosterPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_MREC_SPEED_BOOSTER_POPUP).BooleanValue;
                        
                        showMrecUpgradePopup = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_MREC_UPGRADE_POPUP)
                            .BooleanValue;
                        
                        showMrecVipSoldierPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_MREC_VIP_SOLIDER_POPUP).BooleanValue;
                        
                        showNativeLoading = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_NATIVE_LOADING)
                            .BooleanValue;
                        
                        showNativeBattle = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_NATIVE_BATTLE)
                            .BooleanValue;
                        
                        showNativeBuildBedroomPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_BUILD_BEDROOM_POPUP).BooleanValue;
                        
                        showNativeBuyWorkerPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_BUY_WORKER_POPUP).BooleanValue;
                        
                        showNativeDailyQuestPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_DAILY_QUEST_POPUP).BooleanValue;
                        
                        showNativeGetMoneyPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_GET_MONEY_POPUP).BooleanValue;
                        
                        showNativeIntro = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_NATIVE_INTRO).BooleanValue;
                        
                        showNativeNoAdsPopup = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_NATIVE_NO_ADS_POPUP)
                            .BooleanValue;
                        
                        showNativeSettingPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_SETTING_POPUP).BooleanValue;
                        
                        showNativeShopPopup = FirebaseRemoteConfig.DefaultInstance.GetValue(SHOW_NATIVE_SHOP_POPUP)
                            .BooleanValue;
                        
                        showNativeSkinShopPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_SKIN_SHOP_POPUP).BooleanValue;
                        
                        showNativeSpeedBoosterPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_SPEED_BOOSTER_POPUP).BooleanValue;
                        
                        showNativeTrainingRoom1 = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_TRAINING_ROOM_1_POPUP).BooleanValue;
                        
                        showNativeUpgradePopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_UPGRADE_POPUP).BooleanValue;
                        
                        showNativeVipSoldierPopup = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NATIVE_VIP_SOLDIER_POPUP).BooleanValue;
                        
                        showNoel = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_NOEL).BooleanValue;
                        
                        showNoel = false;
                        
                        showInter = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_INTER).BooleanValue;
                        
                        showTakeABreak = FirebaseRemoteConfig.DefaultInstance
                            .GetValue(SHOW_TAKE_A_BREAK).BooleanValue;

                        if (AdsManager.Instance && AdsManager.Instance.IsInitSuccess)
                        {
                            AdsManager.Instance.DelayToShowTakeABreakPopup();
                            AdsManager.Instance.DelayToShowTakeABreakPopupInFirstTime();
                        }

                        if (loadingScene && !showNativeLoading)
                            loadingScene.HideNativeAds();
                    });


                    break;
                case LastFetchStatus.Failure:
                    Debug.Log("FetchComplete Failure");
                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("FetchComplete Peding");
                    break;
            }
        }

        int FirebaseToInt(string key)
        {
            return int.Parse(FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue.Trim());
        }

        string FirebaseToString(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue.Trim();
        }

        bool FirebaseToBool(string key)
        {
            return bool.Parse(FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue.Trim());
        }

        #endregion

        public void LogEvent(string eventName, string paramName = "", int paramValue = 0)
        {
            // eventName = IdleGameData.GameVersion + "_" + eventName;

            if (paramName != "")
                FirebaseAnalytics.LogEvent(eventName, paramName, paramValue);
            else
                FirebaseAnalytics.LogEvent(eventName);

            if (FacilityManager.Instance && FacilityManager.Instance.testGameConfig.isShowLog)
            {
                DateTime currentTime = DateTime.Now;
                int hour = currentTime.Hour;     // Giờ (0-23)
                int minute = currentTime.Minute;
                var log = hour + ":" + minute + " - " + eventName;
                if(paramName != "")
                    log += ", " + paramName + ": " + paramValue;
                FacilityManager.Instance.AddLog(log);
            }
        }

        public void LogEventWithParameterArray(string eventName, Parameter[] param)
        {
            FirebaseAnalytics.LogEvent(eventName, param);
        }

        public void LogEventWithParameterString(string eventName, string paramKey1, string paramValue1,
            string paramKey2,
            string paramValue2)
        {
            Debug.Log("FIREBASE EVENT : " + eventName + " : " + Content(paramKey1, paramValue1) +
                      Content(paramKey2, paramValue2));

            Parameter[] param = { new(paramKey1, paramValue1), new(paramKey2, paramValue2) };
            FirebaseAnalytics.LogEvent(eventName, param);
        }

        public void LogEventWithParameterString(string eventName, string paramKey1, string paramValue1,
            string paramKey2,
            string paramValue2, string paramKey3, string paramValue3, string paramKey4, string paramValue4)
        {
            Debug.Log("FIREBASE EVENT : " + eventName + " : " + Content(paramKey1, paramValue1) +
                      Content(paramKey2, paramValue2) + Content(paramKey3, paramValue3) +
                      Content(paramKey4, paramValue4));

            Parameter[] param =
            {
                new(paramKey1, paramValue1), new(paramKey2, paramValue2),
                new(paramKey3, paramValue3), new(paramKey4, paramValue4)
            };
            FirebaseAnalytics.LogEvent(eventName, param);
        }

        public void LogEventWithParameterString(string eventName, string paramKey1, string paramValue1,
            string paramKey2,
            string paramValue2, string paramKey3, string paramValue3)
        {
            Debug.Log("FIREBASE EVENT : " + eventName + " : " + Content(paramKey1, paramValue1) +
                      Content(paramKey2, paramValue2) + Content(paramKey3, paramValue3));

            Parameter[] param =
            {
                new(paramKey1, paramValue1), new(paramKey2, paramValue2),
                new(paramKey3, paramValue3)
            };
            FirebaseAnalytics.LogEvent(eventName, param);
        }

        public void LogEventWithParameterString(string eventName, string paramKey, string paramValue)
        {
            Debug.Log("FIREBASE EVENT : " + eventName + " / paramKey:" + paramKey + " / paramValue:" + paramValue);
            FirebaseAnalytics.LogEvent(eventName, new Parameter(paramKey, paramValue));
        }

        public void LogEventWithString(string eventName)
        {
            Debug.Log("FIREBASE EVENT : " + eventName);

            FirebaseAnalytics.LogEvent(eventName);
        }

        private string Content(string arg1, string arg2)
        {
            return $"{arg1} : {arg2} || ";
        }
    }

    public abstract class FirebaseRemoteData
    {
        private static int _interAdsTime = 30;
        private static int _openAdsTime = 30;
        private static int _levelShowRateUs = 6;
        private static int _levelShowCollapseBanner = 3;
        private static int _levelShowInter = 6;
        private static int _maxCountToShowInter = 2;
        private static int _levelShowClaimX2 = 6;

        private static bool _offlinePlayable;
        private static bool _useOpenAds = true;
        private static bool _showRateUs = true;
        private static bool _showOpenAdsFirstOpen = true;

        #region Init Methods

        public static void SetInterAdsTime(int value)
        {
            _interAdsTime = value;
        }

        public static void SetUseOpenAds(bool value)
        {
            _useOpenAds = value;
        }

        public static void SetOpenAdsTime(int value)
        {
            _openAdsTime = value;
        }

        public static void CanShowRateUs(bool value)
        {
            _showRateUs = value;
        }

        public static void SetLevelShowRateUs(int value)
        {
            _levelShowRateUs = value;
        }

        public static void SetOfflinePlayable(bool value)
        {
            _offlinePlayable = value;
        }

        public static void SetLevelShowCollapseBanner(int value)
        {
            _levelShowCollapseBanner = value;
        }

        public static void CanShowOpenAdsFirstOpen(bool value)
        {
            _showOpenAdsFirstOpen = value;
        }

        public static void SetLevelStartShowInter(int value)
        {
            _levelShowInter = value;
        }

        public static void SetMaxCountToShowInter(int value)
        {
            _maxCountToShowInter = value;
        }

        public static void SetLevelShowClaimX2(int value)
        {
            _levelShowClaimX2 = value;
        }

        #endregion

        #region Numbers

        public static int InterAdsTime => _interAdsTime;
        public static int OpenAdsTime => _openAdsTime;
        public static int LevelShowRateUs => _levelShowRateUs;
        public static int LevelShowCollapseBanner => _levelShowCollapseBanner;
        public static int LevelShowInter => _levelShowInter;
        public static int MaxCountToShowInter => _maxCountToShowInter;
        public static int LevelShowClaimX2 => _levelShowClaimX2;

        #endregion

        #region Booleans

        public static bool ShowRateUs => _showRateUs;
        public static bool UseOpenAds => _useOpenAds;
        public static bool OfflinePlayable => _offlinePlayable;
        public static bool ShowOpenAdsFirstOpen => _showOpenAdsFirstOpen;

        #endregion
    }
}