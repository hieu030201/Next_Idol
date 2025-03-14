using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Yun.Scripts.Ads;
using Yun.Scripts.Audios;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Logics;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class UpgradePopup : BasePopup
    {
        [SerializeField] private UpgradeButtonBg upgradePlayerSpeedButtonBg;
        [SerializeField] private UpgradeButtonBg upgradePlayerCapacityButtonBg;
        [SerializeField] private ParticleSystem upgradeSpeedEffect;
        [SerializeField] private ParticleSystem upgradeCapacityEffect;
        //[SerializeField] private GameObject nativeAds;
        
        /*[SerializeField] private UpgradeButtonBg upgradeWorker1CapacityButtonBg;
        [SerializeField] private UpgradeButtonBg upgradeWorker2CapacityButtonBg;
        [SerializeField] private UpgradeButtonBg upgradeWorker3CapacityButtonBg;
        [SerializeField] private UpgradeButtonBg upgradeWorker4CapacityButtonBg;*/

        public UnityAction<int> OnUpgradePlayerSpeedAction;
        public UnityAction<int> OnUpgradePlayerCapacityAction;
        /*public UnityAction<int> OnUpgradeWorker1CapacityAction;
        public UnityAction<int> OnUpgradeWorker2CapacityAction;
        public UnityAction<int> OnUpgradeWorker3CapacityAction;
        public UnityAction<int> OnUpgradeWorker4CapacityAction;*/

        protected override void Start()
        {
            base.Start();
            // if(nativeAds && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeUpgradePopup)
            //     nativeAds.SetActive(false);
            
            if (FireBaseManager.Instance && FireBaseManager.Instance.showMrecUpgradePopup)
            {
                Advertisements.Instance.HideBanner();
                AdsManager.Instance.ShowMrec();
            }
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            // if(nativeAds && nativeAds.activeSelf)
            //     nativeAds.GetComponent<NativeManager>().ShowNativeLoading(false);
            
            Advertisements.Instance.ShowBanner();
            AdsManager.Instance.HideMrec();
        }

        public void OnUpgradePlayerSpeedBtnClick()
        {
            OnUpgradePlayerSpeedAction(_data.UpgradePlayerSpeedLevel);
            upgradeSpeedEffect.Play();
            upgradeSpeedEffect.gameObject.SetActive(true);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Upgrade_Speed);
            DOVirtual.DelayedCall(1.5f, (() =>
            {
                upgradeSpeedEffect.gameObject.SetActive(false);
            })).SetAutoKill(true);
            // Close();
        }

        public void OnUpgradePlayerCapacityClick()
        {
            OnUpgradePlayerCapacityAction(_data.UpgradePlayerCapacityLevel);
            upgradeCapacityEffect.Play();
            upgradeCapacityEffect.gameObject.SetActive(true);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Upgrade_Capacity);
            DOVirtual.DelayedCall(1.5f, (() =>
            {
                upgradeCapacityEffect.gameObject.SetActive(false);
            })).SetAutoKill(true);
            // Close();
        }

        
        /*public void OnUpgradeWorker1CapacityClick()
        {
            OnUpgradeWorker1CapacityAction(_data.UpgradeWorker1CapacityLevel);
            // Close();
        }
        
        public void OnUpgradeWorker2CapacityClick()
        {
            OnUpgradeWorker2CapacityAction(_data.UpgradeWorker2CapacityLevel);
            // Close();
        }
        
        public void OnUpgradeWorker3CapacityClick()
        {
            OnUpgradeWorker3CapacityAction(_data.UpgradeWorker3CapacityLevel);
            // Close();
        }
        
        public void OnUpgradeWorker4CapacityClick()
        {
            OnUpgradeWorker4CapacityAction(_data.UpgradeWorker4CapacityLevel);
            // Close();
        }*/
        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeIn);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
        }
        private UpgradePopupData _data;
        public void UpdateView()
        {
            var idleGameData = FacilityManager.Instance.IdleGameData;
            var playerConfig = FacilityManager.Instance.PlayerConfig;
            var workerConfig = FacilityManager.Instance.WorkerConfig;
            var data = IdleGameLogic.CreatePlayerUpgradePopupData(idleGameData.Level, idleGameData.Money,
                idleGameData.Speed, idleGameData.Capacity, playerConfig, idleGameData.Worker1Capacity,
                idleGameData.Worker2Capacity, idleGameData.Worker3Capacity, idleGameData.Worker4Capacity, workerConfig);
            _data = data;
            if (data.UpgradePlayerSpeedPrice == 0)
            {
                upgradePlayerSpeedButtonBg.gameObject.SetActive(false);
            }
            else
            {
                upgradePlayerSpeedButtonBg.SetLevel(data.UpgradePlayerSpeedLevel);
                upgradePlayerSpeedButtonBg.SetPrice(data.UpgradePlayerSpeedPrice, data.PlayerMoney);
                if (data.UpgradePlayerSpeedLevelActive != 0)
                    upgradePlayerSpeedButtonBg.SetLevelActive(data.UpgradePlayerSpeedLevelActive);
            }

            if (data.UpgradePlayerCapacityPrice == 0)
            {
                upgradePlayerCapacityButtonBg.gameObject.SetActive(false);
            }
            else
            {
                upgradePlayerCapacityButtonBg.SetLevel(data.UpgradePlayerCapacityLevel);
                upgradePlayerCapacityButtonBg.SetPrice(data.UpgradePlayerCapacityPrice, data.PlayerMoney);
                if (data.UpgradePlayerCapacityLevelActive != 0)
                    upgradePlayerCapacityButtonBg.SetLevelActive(data.UpgradePlayerCapacityLevelActive);
            }
            
            
        }
    }
}