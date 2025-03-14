using System;
using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class LevelUpPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI levelTxt;
        [SerializeField] private TextMeshProUGUI rewardTxt;

        [SerializeField] private GameObject unlockPosition1;
        [SerializeField] private GameObject unlockPosition2;
        [SerializeField] private Image iconBtnRewardAds;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> listIconBtnRewardAds;
        [SerializeField] private SticketMinusTxt sticketMinusTxt;
        // Start is called before the first frame update
        protected override void Start()
        {
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
            }
        }

        protected override void Awake()
        {
            base.Awake();
            levelTxt.text = "";
            rewardTxt.text = "";
        }

        public void ClickNoThank()
        {
            FacilityManager.Instance.LevelUp();
            Close();
        }
        
        public override void Show()
        {
            base.Show();
            _isClickedAds = false;
        }
        
        private bool _isClickedAds;
        
        public void ClickClaimX2Btn()
        {
            if (_isClickedAds)
                return;
            _isClickedAds = true;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            sticketMinusTxt.OnInit();
            AdsManager.Instance.ShowReward(FacilityManager.Instance.OnClaimX2LevelUp, AdsManager.RewardType.CLAIM_X2_LEVEL_UP);
            Close();
        }

        private LevelDescription _levelDescription;
        private Action<string, string, bool> _actionWhenClose;
        public void SetData(LevelDescription levelDescription, Action<string, string, bool> actionWhenClose = null)
        {
            _levelDescription = levelDescription;
            _actionWhenClose = actionWhenClose;
            levelTxt.text = "level " + (levelDescription.id + 1);
            rewardTxt.text = levelDescription.reward.ToString();
            if (levelDescription.unlocks.Count > 0)
                Instantiate(levelDescription.unlocks[0], unlockPosition1.transform);
            if (levelDescription.unlocks.Count > 1)
                Instantiate(levelDescription.unlocks[1], unlockPosition2.transform);
        }

        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeIn);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
        }
    }
}
