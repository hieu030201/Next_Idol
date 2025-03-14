using System;
using System.Collections.Generic;
using System.Linq;
using Adverstising_Integration.Scripts;
using Advertising;
using Gley.EasyIAP;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Logics;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class DailyGiftPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI nextGiftTimeLeftTxt;
        [SerializeField] private List<Gift> giftsList;

        [SerializeField] private GameObject day1to7Progress;
        [SerializeField] private GameObject day8to14Progress;
        [SerializeField] private GameObject day15to21Progress;
        [SerializeField] private GameObject day22to30Progress;

        [SerializeField] private GameObject circleDay8;
        [SerializeField] private GameObject circleDay15;
        [SerializeField] private GameObject circleDay22;
        [SerializeField] private GameObject circleDay30;
        
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> listBtnAdsReward;
        [SerializeField] private Image iconAdsReward;
        private DailyGiftData _data;

        protected override void Awake()
        {
            base.Awake();

            circleDay8.SetActive(false);
            circleDay15.SetActive(false);
            circleDay22.SetActive(false);
            circleDay30.SetActive(false);

            for (var i = 0; i < giftsList.Count; i++)
            {
                var gift = giftsList[i];
                gift.gameObject.SetActive(false);
                gift.dayTxt.text = "day " + (i + 1).ToString();
                gift.id = i + 1;
            }

            _data = IdleGameLogic.GetDailyGiftData();

            nextGiftTimeLeftTxt.text = _data.hoursLeftString + "H " + _data.minutesLeftString + "M";

            if (_data.currentDay == 7)
                circleDay8.SetActive(true);
            else if (_data.currentDay == 14)
                circleDay15.SetActive(true);
            else if (_data.currentDay == 21)
                circleDay22.SetActive(true);
            else if (_data.currentDay == 29)
                circleDay30.SetActive(true);

            var startIndex = 0;
            var endIndex = 0;
            if (_data.currentDay < 7)
            {
                endIndex = 7;
            }
            else if (_data.currentDay < 14)
            {
                startIndex = 7;
                endIndex = 14;
            }
            else if (_data.currentDay < 21)
            {
                startIndex = 14;
                endIndex = 21;
            }
            else
            {
                startIndex = 21;
                endIndex = 30;
            }

            for (var i = 0; i < giftsList.Count; i++)
            {
                var gift = giftsList[i];
                gift.rewardTxt.text = FacilityManager.Instance.DailyGiftConfig.giftList[i].reward.ToString();
                gift.moneyIcon.SetActive(false);
                gift.gemIcon.SetActive(false);
                gift.tokenIcon.SetActive(false);
                gift.starIcon.SetActive(false);
                switch (FacilityManager.Instance.DailyGiftConfig.giftList[i].rewardType)
                {
                    case DailyQuestDataConfig.RewardType.Money:
                        gift.moneyIcon.SetActive(true);
                        break;
                    case DailyQuestDataConfig.RewardType.Gem:
                        gift.gemIcon.SetActive(true);
                        break;
                    case DailyQuestDataConfig.RewardType.Star:
                        gift.starIcon.SetActive(true);
                        break;
                    case DailyQuestDataConfig.RewardType.Token:
                        gift.tokenIcon.SetActive(true);
                        break;
                }
                if (i >= startIndex && i < endIndex)
                {
                    gift.gameObject.SetActive(true);
                    if (i < _data.currentDay)
                    {
                        gift.overLayout.SetActive(true);
                        foreach (var receivedGiftDay in FacilityManager.Instance.GameSaveLoad.StableGameData
                            .receivedGiftDays)
                        {
                            if (gift.id == receivedGiftDay + 1)
                                gift.receivedIcon.SetActive(true);
                        }
                    }
                    else if (i == _data.currentDay)
                    {
                        gift.highlightIcon.SetActive(true);
                    }
                    else
                    {
                        gift.secretLayout.SetActive(true);
                    }
                }
                else
                {
                    gift.gameObject.SetActive(false);
                }
            }

            var distance = 144.5f;
            day8to14Progress.GetComponent<RectTransform>().localPosition =
                new Vector3(-distance, 0, 0);
            day15to21Progress.GetComponent<RectTransform>().localPosition =
                new Vector3(-distance, 0, 0);
            day22to30Progress.GetComponent<RectTransform>().localPosition =
                new Vector3(-distance, 0, 0);
            if (_data.currentDay < 8)
            {
                day1to7Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(-distance + distance * ((float) (_data.currentDay + 1) / 8), 0, 0);
            }
            else if (_data.currentDay < 15)
            {
                day1to7Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(0, 0, 0);
                day8to14Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(-distance + distance * ((float) (_data.currentDay + 1 - 7) / 8), 0, 0);
            }
            else if (_data.currentDay < 22)
            {
                day1to7Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(0, 0, 0);
                day8to14Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(0, 0, 0);
                day15to21Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(-distance + distance * ((float) (_data.currentDay + 1 - 14) / 8), 0, 0);
            }
            else
            {
                day1to7Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(0, 0, 0);
                day8to14Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(0, 0, 0);
                day15to21Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(0, 0, 0);
                day22to30Progress.GetComponent<RectTransform>().localPosition =
                    new Vector3(-distance + distance * ((float) (_data.currentDay + 1 - 21) / 9), 0, 0);
            }
            
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                iconAdsReward.sprite = listBtnAdsReward[1];
            }

        }

        public void ClickClaimBtn()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            FacilityManager.Instance.OnClaimDailyGift(_data.currentDay);
            Close();
        }

        public void ClickClaimX2Btn()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            FacilityManager.Instance.GameSaveLoad.StableGameData.lastReceivedGiftDay = _data.currentDay;
            AdsManager.Instance.ShowReward(FacilityManager.Instance.OnClaimX2DailyGift, AdsManager.RewardType.X2_DAILY_GIFT);
            Close();
        }
    }
}