using System;
using System.Collections.Generic;
using System.Linq;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using Gley.EasyIAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using DG.Tweening;
using TwistedTangle.HieuUI;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class DailyQuestPopup : BasePopup
    {
        [SerializeField] private List<DailyQuest> questList;
        [SerializeField] public YunTextShadow rewardTxt;
        [SerializeField] public YunTextShadow timeLeftTxt;
        [SerializeField] public YunTextShadow questCompleteTxt;
        [SerializeField] public YunProgressBar completedProgressBar;
        [SerializeField] public UIButton claimBtn;
        [SerializeField] private GameObject moneyIcon;
        [SerializeField] private GameObject starIcon;
        [SerializeField] private GameObject gemIcon;
        [SerializeField] private GameObject tokenIcon;
        [SerializeField] private DailyQuest dailyQuestPrefab;
        [SerializeField] private GameObject questContainer;
        [SerializeField] private List<RectTransform> listComponentEffect = new List<RectTransform>();

        protected override void Awake()
        {
            base.Awake();
            claimBtn.interactable = false;
            questCompleteTxt.DisplayText = "";

            moneyIcon.SetActive(false);
            starIcon.SetActive(false);
            gemIcon.SetActive(false);
            tokenIcon.SetActive(false);
        }

        public override void Show()
        {
            var content = transform.Find("Content");
            ShowEffect(YunEffectType.FlyUpEffect2, content.transform);
        }

        public void SetConfig(DailyQuestData dailyQuestConfig)
        {
            questList = new List<DailyQuest>();
            for (var i = 0; i < dailyQuestConfig.questList.Count; i++)
            {
                var dailyQuest = Instantiate(dailyQuestPrefab, questContainer.transform);
                questList.Add(dailyQuest);
            }

            switch (dailyQuestConfig.rewardType)
            {
                case DailyQuestDataConfig.RewardType.Money:
                    moneyIcon.SetActive(true);
                    break;
                case DailyQuestDataConfig.RewardType.Star:
                    starIcon.SetActive(true);
                    break;
                case DailyQuestDataConfig.RewardType.Token:
                    tokenIcon.SetActive(true);
                    break;
                case DailyQuestDataConfig.RewardType.Gem:
                    gemIcon.SetActive(true);
                    break;
            }

            var arrangeList = new List<DailyQuestDataConfig>();

            for (var i = 0; i < dailyQuestConfig.questList.Count; i++)
            {
                var questConfig = dailyQuestConfig.questList[i];
                if (questConfig.countProgress >= questConfig.quantity)
                {
                    if (!questConfig.isReceived)
                    {
                        arrangeList.Add(questConfig);
                        // Debug.Log("AAAA: " + questConfig.id);
                    }
                }
            }

            for (var i = 0; i < dailyQuestConfig.questList.Count; i++)
            {
                var questConfig = dailyQuestConfig.questList[i];
                if (questConfig.countProgress < questConfig.quantity)
                {
                    arrangeList.Add(questConfig);
                    // Debug.Log("BBBB: " + questConfig.id);
                }
            }

            for (var i = 0; i < dailyQuestConfig.questList.Count; i++)
            {
                var questConfig = dailyQuestConfig.questList[i];
                if (questConfig.isReceived)
                {
                    arrangeList.Add(questConfig);
                    // Debug.Log("CCCC: " + questConfig.id);
                }
            }

            claimBtn.gameObject.SetActive(!dailyQuestConfig.isReceived);
            var countComplete = 0;
            for (var i = 0; i < arrangeList.Count; i++)
            {
                var questConfig = arrangeList[i];
                questList[i].id = questConfig.id;
                questList[i].SetRewardType(questConfig.rewardType);
                questList[i].rewardTxt.DisplayText = questConfig.reward.ToString();
                questList[i].startDescriptionTxt.DisplayText = questConfig.startDescription;
                questList[i].endDescriptionTxt.DisplayText = questConfig.endDescription;
                if (questConfig.countProgress >= questConfig.quantity)
                    questConfig.countProgress = questConfig.quantity;
                if (questConfig.endDescription != "")
                    questList[i].quantityTxt.DisplayText = questConfig.quantity.ToString();
                questList[i].questCompleteTxt.DisplayText =
                    questConfig.countProgress + " / " + questConfig.quantity;
                var questProgress = (float) questConfig.countProgress / questConfig.quantity;
                questList[i].completedProgressBar.UpdateProgress(questProgress);
                if (questConfig.countProgress >= questConfig.quantity)
                {
                    countComplete++;
                    if (!questConfig.isReceived)
                        questList[i].EnableClaim();
                }

                if (questConfig.isReceived)
                {
                    questList[i].ShowCompleted();
                }


                // Nếu cần, bạn có thể gọi LayoutRebuilder
                LayoutRebuilder.ForceRebuildLayoutImmediate(questList[i].layoutContent.GetComponent<RectTransform>());
            }

            questCompleteTxt.DisplayText = countComplete + " / " + dailyQuestConfig.questList.Count;
            
            var progress = (float) countComplete / dailyQuestConfig.questList.Count;
            completedProgressBar.UpdateProgress(progress);
            rewardTxt.DisplayText = dailyQuestConfig.reward.ToString();
            if (countComplete >= dailyQuestConfig.questList.Count)
                claimBtn.interactable = true;

            for (int i = 0; i < questList.Count; i++)
            {
                listComponentEffect.Add(questList[i].GetComponent<RectTransform>());
            }
            
            AddEffect();
        }

        public void SetData(DailyQuestData data)
        {
            timeLeftTxt.DisplayText = data.hoursLeftString + "H" + data.minutesLeftString;
        }

        public void OnClickClaimBtn()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            FacilityManager.Instance.OnClaimCompleteAllDailyQuest();
            claimBtn.gameObject.SetActive(false);
        }

        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeToBottom);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
        }

        private void AddEffect()
        {
            for (int i = 0; i < listComponentEffect.Count; i++)
            {
                CanvasGroup canvasGroup = listComponentEffect[i].GetComponent<CanvasGroup>();  
                if (canvasGroup == null)  
                {  
                    canvasGroup = listComponentEffect[i].gameObject.AddComponent<CanvasGroup>();  
                } 
                
                canvasGroup.alpha = 0;  
                listComponentEffect[i].localScale = Vector3.zero; 
            
                // Gọi phương thức animate từng item với hiệu ứng domino  
            }
            
            for (int i = 0; i < listComponentEffect.Count; i++)
            {
                AnimateItem(listComponentEffect[i], i);
            }
        }
        private float initialDelay = 0.1f; // Thời gian trễ giữa các item  
        private float effectDuration = 0.3f; // Thời gian hiệu ứng  
        private void AnimateItem(RectTransform rectTransform, int index)
        {
            float delay = index * initialDelay;
            CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
            Sequence itemSequence = DOTween.Sequence();
            itemSequence.Append(
                rectTransform.DOScale(1f, effectDuration)
                    .SetDelay(delay)
                    .SetEase(Ease.OutBack, 1.4f)
            ).Join(
                canvasGroup.DOFade(1, effectDuration)
                    .SetEase(Ease.Linear) 
            );
        }  

    }
}