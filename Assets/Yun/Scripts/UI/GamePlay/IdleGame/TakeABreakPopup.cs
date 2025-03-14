using System.Collections;
using Adverstising_Integration.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class TakeABreakPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI countDownTxt;
        [SerializeField] private TextMeshProUGUI rewardTxt;
        [SerializeField] private Image circle;

        private int countNumber = 3;
        protected override void Start()
        {
            base.Start();
            
            countDownTxt.text = countNumber.ToString();

            StartCoroutine(CountDown());
            
            circle.DOFillAmount(0, 4f);

            if (FacilityManager.Instance)
            {
                var reward = FacilityManager.Instance.LevelConfig.LevelRequirements[FacilityManager.Instance.IdleGameData.Level - 1].AdsReward / 2;
                rewardTxt.text = "you will receive " + reward;
            }
        }

        private IEnumerator CountDown()
        {
            while (countNumber >= 0)
            {
                countDownTxt.text = countNumber.ToString();
                countNumber--;
                yield return new WaitForSeconds(1f);
            }

            AdsManager.Instance.ShowInterstitialAd();
            Close();
        }
    }
}