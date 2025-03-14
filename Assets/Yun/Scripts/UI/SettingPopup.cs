using Adverstising_Integration.Scripts;
using Advertising;
using UnityEngine;
using Yun.Scripts.Ads;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI
{
    public class SettingPopup : BasePopup
    {
        [SerializeField] private YunSwitchToggle soundToggle;
        [SerializeField] private YunSwitchToggle musicToggle;
        [SerializeField] private YunSwitchToggle vibrationToggle;
        [SerializeField] private GameObject nativeAds;

        [SerializeField] private GameObject test;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            soundToggle.IsActive = FacilityManager.Instance.SettingGameData.IsSoundOn;
            musicToggle.IsActive = FacilityManager.Instance.SettingGameData.IsMusicOn;
            vibrationToggle.IsActive = FacilityManager.Instance.SettingGameData.IsVibrationOn;
            
            soundToggle.ONActive = ActiveSound;
            soundToggle.ONDeactivate = DeactivateSound;
            musicToggle.ONActive = ActiveMusic;
            musicToggle.ONDeactivate = DeactivateMusic;
            vibrationToggle.ONActive = ActiveVibration;
            vibrationToggle.ONDeactivate = DeactivateVibration;
            
            test.SetActive(FacilityManager.Instance.testGameConfig.IsShowTestInSettingPopup);
            
            if(nativeAds && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeSettingPopup)
                nativeAds.SetActive(false);

            if (FireBaseManager.Instance && FireBaseManager.Instance.showMrecSettingPopup)
            {
                Advertisements.Instance.HideBanner();
                AdsManager.Instance.ShowMrec();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(nativeAds && nativeAds.activeSelf)
                nativeAds.GetComponent<NativeManager>().ShowNativeLoading(false);
            
            Advertisements.Instance.ShowBanner();
            AdsManager.Instance.HideMrec();
        }

        public void ClickFakeLevelUp()
        {
            FacilityManager.Instance.FakeLevelUp();
        }

        public void ClickShowRateUs()
        {
            RateUs.Instance.ShowRatePopup();
        }
        
        public void ClickAddMoney()
        {
            FacilityManager.Instance.GetMoney();
        }

        public void ClickAddDeserter()
        {
            FacilityManager.Instance.ActiveDesertRoom();
            Close();
        }

        public void ClickShowLady()
        {
            Close();
            FacilityManager.Instance.ShowUpgradeWorkerPoint();
        }
        
        public void ClickAddToken()
        {
            FacilityManager.Instance.AddTokenWhenUpgrade(10);
            // Close();
        }

        public void ClickAddGem()
        {
            FacilityManager.Instance.AddGemWhenInAppPurchase(100, gameObject.transform);
            Close();
        }
        
        public void ClickShowVipSoldier()
        {   
            FacilityManager.Instance.ShowVipSoldier();
            Close();
        }
        
        public void ClickAddDay()
        {
            FacilityManager.Instance.GameSaveLoad.StableGameData.addDay++;
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData();
        }
        
        public void ClickAddClient(int level)
        {
            FacilityManager.Instance.FakeAddClient(level);
        }

        public void ClickFight()
        {
            FacilityManager.Instance.BattleManager.OnStartBattle();
        }

        private void ActiveSound()
        {
            FacilityManager.Instance.SettingGameData.IsSoundOn = true;
        }

        private void DeactivateSound()
        {
            FacilityManager.Instance.SettingGameData.IsSoundOn = false;
        }
        
        private void ActiveMusic()
        {
            FacilityManager.Instance.SettingGameData.IsMusicOn = true;
        }

        private void DeactivateMusic()
        {
            FacilityManager.Instance.SettingGameData.IsMusicOn = false;
        }
        
        private void ActiveVibration()
        {
            FacilityManager.Instance.SettingGameData.IsVibrationOn = true;
        }

        private void DeactivateVibration()
        {
            FacilityManager.Instance.SettingGameData.IsVibrationOn = false;
        }

        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeIn);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
        }
    }
}
