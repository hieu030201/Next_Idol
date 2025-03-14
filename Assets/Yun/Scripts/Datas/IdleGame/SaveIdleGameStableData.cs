using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class SaveIdleGameStableData
    {
        public Dictionary<string, bool> RetentionDictionary = new ();
        public string firstPlayDate;
        public bool isPurchasedRemoveAds;
        public bool isBoughtNoAdsVip = false;
        public int amountFreeRewardAds = 0;
        public long firstPlayTime;
        public int lastReceivedGiftDay = -1;
        public DateTime lastCheckedDate;
        public DateTime lastCheckedRewardDailyShop;
        public List<int> receivedGiftDays = new ();
        public int addDay = 0;
        public bool isActivatedTutorialItemShop;

        public bool isFirstTimeUseSpeedBooster = true;
        public bool isFirstTimeUseUpgradeWorker = true;
        public bool isFirstTimeUseVipSoldier = true;
        public bool isFirstTimeUseWorkerSpeedBooster = true;

        public int idSkinRented;
        public int idSkinSelected = 0;
        public int idSkinInSpin;
        public int freeSpinAmount = 1;
        public int freeInShopAmount = 1;
        public int rewardAdsPackOne = 2;
        public int rewardAdsPackTwo = 3;
        public int rewardAdsPackThree = 3;
        [TitleGroup("ListDataUnlock")] public List<int> listIDSkinUnlock = new() { 0};
        [TitleGroup("ListDataRental")] public List<int> listIDSkinRetal = new();
        
        public bool isBoughtBattlePassHero = false;
        public bool isBoughtBattlePassLegend = false;
        
        public bool isActiveSkinShop = false;
        public bool isActiveSpin = false;
        public bool isActiveBattlePass = false;
        public bool isActiveShop = false;

        public bool isActiveRetal;
        public int minuteShowInIdleGameUI;
        
        public int abilityLevel = 0;
        public int abilitySpeed = 0;
        public int abilityIncomeCheckIn = 0;
        public int abilityCashRadius = 0;
        public int abilityWorkerSpeed = 0;
        public int abilityIncomeVipSoldier = 0;
        public int abilityCapacity = 0;

        public bool isActiveSkinSpunky;
    }
}
