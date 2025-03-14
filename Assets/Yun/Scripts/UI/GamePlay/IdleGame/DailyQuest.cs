using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class DailyQuest : MonoBehaviour
    {
        [SerializeField] public YunTextShadow startDescriptionTxt;
        [SerializeField] public YunTextShadow endDescriptionTxt;
        [SerializeField] public YunTextShadow quantityTxt;
        [SerializeField] public TextMeshProUGUI textComponentsTitle;
        [SerializeField] public YunTextShadow rewardTxt;
        [SerializeField] public YunTextShadow questCompleteTxt;
        [SerializeField] public TextMeshProUGUI questCompletedTxt;
        [SerializeField] public GameObject completedBarStatus;
        [SerializeField] public GameObject receivedBarStatus;
        [SerializeField] public GameObject moneyIcon;
        [SerializeField] public GameObject tokenIcon;
        [SerializeField] public GameObject starIcon;
        [SerializeField] public GameObject gemIcon;
        [SerializeField] public GameObject completedTxt;
        [SerializeField] public GameObject border;
        [SerializeField] public YunProgressBar completedProgressBar;
        [SerializeField] public Button claimBtn;
        [SerializeField] public HorizontalLayoutGroup layoutContent;

        private GameObject _currentCurrencyIcon;
        private void Awake()
        {
            completedBarStatus.SetActive(false);
            receivedBarStatus.SetActive(false);
            completedTxt.SetActive(false);
            questCompletedTxt.gameObject.SetActive(false);
            border.SetActive(false);
            completedBarStatus.SetActive(false);
            claimBtn.interactable = false;
            rewardTxt.DisplayText = "";
            startDescriptionTxt.DisplayText = "";
            endDescriptionTxt.DisplayText = "";
            questCompleteTxt.DisplayText = "";
            questCompletedTxt.text = "";
            quantityTxt.DisplayText = "";
            
            moneyIcon.SetActive(false);
            tokenIcon.SetActive(false);
            starIcon.SetActive(false);
            gemIcon.SetActive(false);
        }

        public void SetRewardType(DailyQuestDataConfig.RewardType type)
        {
            switch (type)
            {
                case DailyQuestDataConfig.RewardType.Money:
                    moneyIcon.SetActive(true);
                    _currentCurrencyIcon = moneyIcon;
                    break;
                case DailyQuestDataConfig.RewardType.Token:
                    tokenIcon.SetActive(true);
                    _currentCurrencyIcon = tokenIcon;
                    break;
                case DailyQuestDataConfig.RewardType.Star:
                    starIcon.SetActive(true);
                    _currentCurrencyIcon = starIcon;
                    break;
                case DailyQuestDataConfig.RewardType.Gem:
                    gemIcon.SetActive(true);
                    _currentCurrencyIcon = gemIcon;
                    break;
            }
        }

        public DailyQuestDataConfig.QuestId id;

        public void OnClickClaimBtn()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            FacilityManager.Instance.OnClaimDailyQuest(id);
            ShowCompleted();
        }

        public void EnableClaim()
        {
            claimBtn.interactable = true;
            completedBarStatus.SetActive(true);
            border.SetActive(true);
            completedProgressBar.HideChildBar();
        }

        public void ShowCompleted()
        {
            _currentCurrencyIcon.SetActive(false);
            rewardTxt.gameObject.SetActive(false);
            completedTxt.SetActive(true);
            claimBtn.gameObject.SetActive(false);
            completedBarStatus.SetActive(false);
            receivedBarStatus.SetActive(true);
            questCompletedTxt.text = questCompleteTxt.DisplayText;
            questCompleteTxt.gameObject.SetActive(false);
            questCompletedTxt.gameObject.SetActive(true);
            textComponentsTitle.text = startDescriptionTxt.DisplayText + " ";
            textComponentsTitle.text += " " + quantityTxt.DisplayText + "  ";
            textComponentsTitle.text += " " + endDescriptionTxt.DisplayText;
            textComponentsTitle.gameObject.SetActive(true);
            startDescriptionTxt.gameObject.SetActive(false);
            endDescriptionTxt.gameObject.SetActive(false);
            quantityTxt.gameObject.SetActive(false);
     
            border.SetActive(false);
            completedProgressBar.HideChildBar();
        }
    }
}