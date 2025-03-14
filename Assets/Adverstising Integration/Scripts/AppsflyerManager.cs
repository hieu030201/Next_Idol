using AppsFlyerSDK;
using System.Collections.Generic;
using UnityEngine;

namespace Advertising
{
    public abstract class AppsflyerParams
    {
        protected AppsflyerParams(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }
    }

    public class AppsflyerManager : Singleton<AppsflyerManager>
    {
        [SerializeField, Tooltip("For IOS Only")]
        private bool isIOSDebug;

        public const string AF_INTERS_CALL_SHOW = "af_inters_call_show";
        public const string AF_INTERS_PASSED_CAPPING_TIME = "af_inters_passed_capping_time";
        public const string AF_INTERS_AVAILABLE = "af_inters_available";
        public const string AF_INTERS_DISPLAYED = "af_inters_displayed";
        public const string AF_REWARDED_CALL_SHOW = "af_rewarded_call_show";
        public const string AF_REWARDED_AVAILABLE = "af_rewarded_available";
        public const string AF_REWARDED_DISPLAYED = "af_rewarded_ad_displayed";
        public const string AF_REWARDED_AD_COMPLETED = "af_rewarded_ad_completed";
        public const string AF_AD_REVENUE = "af_ad_revenue";

        public const string AF_PURCHASE = "af_purchase";

        private void Start()
        {
#if UNITY_IOS
        AppsFlyerAdRevenue.setIsDebug(isIOSDebug);
#endif
        }

        public void LogEventWithParam(string eventName, AppsflyerParams arg1)
        {
            Dictionary<string, string> afEvent =
                new Dictionary<string, string> { { arg1.Key, arg1.Value } };
            AppsFlyer.sendEvent(eventName, afEvent);
        }

        public void LogEventPurchase(string afRevenue, string afCurrency, string afQuantity, string afContentID)
        {
            Dictionary<string, string> afEvent = new Dictionary<string, string>
            {
                { "af_revenue", afRevenue },
                { "af_currency", afCurrency },
                { "af_quantity", afQuantity },
                { "af_content_id", afContentID }
            };
            AppsFlyer.sendEvent(AF_PURCHASE, afEvent);
        }

        public void LogEventWithName(string eventName)
        {
            Debug.Log("Appsflyer EVENT " + eventName);
            Dictionary<string, string> afEvent = new Dictionary<string, string>();
            AppsFlyer.sendEvent(eventName, afEvent);
        }

        public void LogAdRevenue(string monetizationNetwork, double eventRevenue, string revenueCurrency,
            Dictionary<string, string> dic)
        {
            AppsFlyerAdRevenue.logAdRevenue(monetizationNetwork,
                AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeGoogleAdMob, eventRevenue,
                revenueCurrency, dic);
        }
    }
}