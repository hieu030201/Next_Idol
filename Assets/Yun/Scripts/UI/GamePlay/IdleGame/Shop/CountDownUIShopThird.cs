using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExaGames.Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;

public class CountDownUIShopThird : CountDownUIShop
{
        [SerializeField] private LivesManagerShopThird livesManager;
        public override void Start()
        {
                base.Start();
                livesManager.CounterCompleted += OnCounterCompleted;  
                activeAdsTurn.text = FacilityManager.Instance.GameSaveLoad.StableGameData.rewardAdsPackThree.ToString();
        }
        
        protected override void CountDown()
        { 
                livesManager.AddLifeSlots(1);
        }
        
        public override void OnTimeToNextLifeChanged() {
                string remainingTime = livesManager.RemainingTimeString;
                countDownTxt.text = remainingTime;
                string formattedTime = FormatRemainingTime(remainingTime); 
                countDownTxt.text = formattedTime;  
                if (remainingTime == "Done")
                {
                        countDownTxt.text = "Free";
                }
        }
                
        public void OnCounterCompleted() {  
                FacilityManager.Instance.GameSaveLoad.StableGameData.rewardAdsPackThree = 3;
                activeAdsTurn.text = FacilityManager.Instance.GameSaveLoad.StableGameData.rewardAdsPackThree.ToString();
                SetBackGroundPack(0);
                SetActiveAdsIcon(true);
                SetActiveIconImage(0);
                TurnOffCountDownContent();
            
                FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true); 
        } 
}
