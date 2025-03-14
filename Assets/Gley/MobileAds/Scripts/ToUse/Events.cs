namespace Gley.MobileAds
{
    public class Events
    {
        public delegate void Initialized();

        public static event Initialized onInitialized;

        public void TriggerOnInitialized()
        {
            if (onInitialized != null)
            {
                onInitialized();
            }
        }


        public delegate void BannerLoadSuccess();

        public static event BannerLoadSuccess onBannerLoadSuccess;

        public void TriggerBannerLoadSuccess()
        {
            if (onBannerLoadSuccess != null)
            {
                onBannerLoadSuccess();
            }
        }


        public delegate void BannerLoadFailed(string message);

        public static event BannerLoadFailed onBannerLoadFailed;

        public void TriggerBannerLoadFailed(string message)
        {
            if (onBannerLoadFailed != null)
            {
                onBannerLoadFailed(message);
            }
        }

        public delegate void BannerDisplaySuccess();

        public static event BannerDisplaySuccess onBannerDisplaySuccess;

        public void TriggerBannerDisplaySuccess()
        {
            if (onBannerDisplaySuccess != null)
            {
                onBannerDisplaySuccess();
            }
        }

        public delegate void BannerClicked();

        public static event BannerClicked onBannerClicked;

        public void TriggerBannerClicked()
        {
            if (onBannerClicked != null)
            {
                onBannerClicked();
            }
        }

        public delegate void BannerRevenue(long value, string currencyCode);

        public static event BannerRevenue onBannerRevenue;

        public void TriggerBannerRevenue(long value, string currencyCode)
        {
            if (onBannerRevenue != null)
            {
                onBannerRevenue(value, currencyCode);
            }
        }

        public delegate void InterstitialLoadSuccess();

        public static event InterstitialLoadSuccess onInterstitialLoadSuccess;

        public void TriggerInterstitialLoadSuccess()
        {
            if (onInterstitialLoadSuccess != null)
            {
                onInterstitialLoadSuccess();
            }
        }


        public delegate void InterstitialLoadFailed(string message);

        public static event InterstitialLoadFailed onInterstitialLoadFailed;

        public void TriggerInterstitialLoadFailed(string message)
        {
            if (onInterstitialLoadFailed != null)
            {
                onInterstitialLoadFailed(message);
            }
        }

        public delegate void InterstitialDisplaySuccess();

        public static event InterstitialDisplaySuccess onInterstitialDisplaySuccess;

        public void TriggerInterstitialDisplaySuccess()
        {
            if (onInterstitialDisplaySuccess != null)
            {
                onInterstitialDisplaySuccess();
            }
        }


        public delegate void InterstitialDisplayFailed(string message);

        public static event InterstitialDisplayFailed onInterstitialDisplayFailed;

        public void TriggerInterstitialDisplayFailed(string message)
        {
            if (onInterstitialDisplayFailed != null)
            {
                onInterstitialDisplayFailed(message);
            }
        }


        public delegate void InterstitialClicked();

        public static event InterstitialClicked onInterstitialClicked;

        public void TriggerInterstitialClicked()
        {
            if (onInterstitialClicked != null)
            {
                onInterstitialClicked();
            }
        }

        public delegate void InterstitialRevenue(long value, string currencyCode);

        public static event InterstitialRevenue onInterstitialRevenue;

        public void TriggerInterstitialRevenue(long value, string currencyCode)
        {
            if (onInterstitialRevenue != null)
            {
                onInterstitialRevenue(value, currencyCode);
            }
        }

        public delegate void AppOpenLoadSuccess();

        public static event AppOpenLoadSuccess onAppOpenLoadSuccess;

        public void TriggerAppOpenLoadSuccess()
        {
            if (onAppOpenLoadSuccess != null)
            {
                onAppOpenLoadSuccess();
            }
        }


        public delegate void AppOpenLoadFailed(string message);

        public static event AppOpenLoadFailed onAppOpenLoadFailed;

        public void TriggerAppOpenLoadFailed(string message)
        {
            if (onAppOpenLoadFailed != null)
            {
                onAppOpenLoadFailed(message);
            }
        }

        public delegate void AppOpenDisplaySuccess();

        public static event AppOpenDisplaySuccess onAppOpenDisplaySuccess;

        public void TriggerAppOpenDisplaySuccess()
        {
            if (onAppOpenDisplaySuccess != null)
            {
                onAppOpenDisplaySuccess();
            }
        }


        public delegate void AppOpenDisplayFailed(string message);

        public static event AppOpenDisplayFailed onAppOpenDisplayFailed;

        public void TriggerAppOpenDisplayFailed(string message)
        {
            if (onAppOpenDisplayFailed != null)
            {
                onAppOpenDisplayFailed(message);
            }
        }

        public delegate void AppOpenClicked();

        public static event AppOpenClicked onAppOpenClicked;

        public void TriggerAppOpenClicked()
        {
            if (onAppOpenClicked != null)
            {
                onAppOpenClicked();
            }
        }

        public delegate void AppOpenRevenue(long value, string currencyCode);

        public static event AppOpenRevenue onAppOpenRevenue;

        public void TriggerAppOpenRevenue(long value, string currencyCode)
        {
            if (onAppOpenRevenue != null)
            {
                onAppOpenRevenue(value, currencyCode);
            }
        }

        public delegate void RewardedVideoLoadSuccess();

        public static event RewardedVideoLoadSuccess onRewardedVideoLoadSuccess;

        public void TriggerRewardedVideoLoadSuccess()
        {
            if (onRewardedVideoLoadSuccess != null)
            {
                onRewardedVideoLoadSuccess();
            }
        }


        public delegate void RewardedVideoLoadFailed(string message);

        public static event RewardedVideoLoadFailed onRewardedVideoLoadFailed;

        public void TriggerRewardedVideoLoadFailed(string message)
        {
            if (onRewardedVideoLoadFailed != null)
            {
                onRewardedVideoLoadFailed(message);
            }
        }

        public delegate void RewardedDisplaySuccess();

        public static event RewardedDisplaySuccess onRewardedDisplaySuccess;

        public void TriggerRewardedDisplaySuccess()
        {
            if (onRewardedDisplaySuccess != null)
            {
                onRewardedDisplaySuccess();
            }
        }


        public delegate void RewardedDisplayFailed(string message);

        public static event RewardedDisplayFailed onRewardedDisplayFailed;

        public void TriggerRewardedDisplayFailed(string message)
        {
            if (onRewardedDisplayFailed != null)
            {
                onRewardedDisplayFailed(message);
            }
        }

        public delegate void RewardedVideoClicked();

        public static event RewardedVideoClicked onRewardedVideoClicked;

        public void TriggerRewardedVideoClicked()
        {
            if (onRewardedVideoClicked != null)
            {
                onRewardedVideoClicked();
            }
        }

        public delegate void RewardedRevenue(long value, string currencyCode);

        public static event RewardedRevenue onRewardedRevenue;

        public void TriggerRewardedRevenue(long value, string currencyCode)
        {
            if (onRewardedRevenue != null)
            {
                onRewardedRevenue(value, currencyCode);
            }
        }

        public delegate void RewardedInterstitialLoadSuccess();

        public static event RewardedInterstitialLoadSuccess onRewardedInterstitialLoadSuccess;

        public void TriggerRewardedInterstitialLoadSuccess()
        {
            if (onRewardedInterstitialLoadSuccess != null)
            {
                onRewardedInterstitialLoadSuccess();
            }
        }


        public delegate void RewardedInterstitialLoadFailed(string message);

        public static event RewardedInterstitialLoadFailed onRewardedInterstitialLoadFailed;

        public void TriggerRewardedInterstitialLoadFailed(string message)
        {
            if (onRewardedInterstitialLoadFailed != null)
            {
                onRewardedInterstitialLoadFailed(message);
            }
        }


        public delegate void RewardedInterstitialClicked();

        public static event RewardedInterstitialClicked onRewardedInterstitialClicked;

        public void TriggerRewardedInterstitialClicked()
        {
            if (onRewardedInterstitialClicked != null)
            {
                onRewardedInterstitialClicked();
            }
        }
    }
}