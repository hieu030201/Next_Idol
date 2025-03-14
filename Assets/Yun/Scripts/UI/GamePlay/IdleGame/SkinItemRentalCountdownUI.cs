using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using ExaGames.Common;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class SkinItemRentalCountdownUI : YunBehaviour
    {
        public LivesManagerSkinItem LivesManagerSkinItem;
        [SerializeField] public TextMeshProUGUI countDownTxt;
        //[SerializeField] private Image timeCircle;
        [SerializeField] public GameObject countDownObj;
        private Tween _delayTween;
        public static Action<string, string> SetTextAction;
        public static Action setTimerDoneAction;
        private string lastSentValue;
        
        public void StartCountDown()
        {
            CountDown();
        }

        private void CountDown()
        {
            LivesManagerSkinItem.AddLifeSlots(1);
            countDownObj.SetActive(true);
            //timeCircle.DOFillAmount(1, _startCountDown).OnComplete(OnCountDownComplete); 
        }
        
        
        public void OnTimeToNextLifeChanged() {
            string remainingTime = LivesManagerSkinItem.RemainingTimeString;  
            
            string formattedTime = FormatRemainingTime(remainingTime); 
            countDownTxt.text = formattedTime;  
            
            SetTextAction?.Invoke(formattedTime, LivesManagerSkinItem.itemId);
            lastSentValue = remainingTime;
            if (remainingTime != lastSentValue) 
            {
                lastSentValue = remainingTime;
            }

            if (remainingTime == "00:0")
            {
                Debug.Log("OnTimeToNextLifeChanged");
            }
            
            if (remainingTime == "Done")
            {
                countDownObj.SetActive(false);
                var item = GetComponent<SkinItem>();
                if (item.canRental && FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Contains(item.ID))
                {
                    item.skinPopup.ChangeStatusBtnRentalTwo(false, item.ID);
                    item.SetStatusRentalUI(true);
                    FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Remove(item.ID);
                    if (item.ID == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinRented)
                    {
                        //item.skinPopup.GetSkinEquipped().SetState(SkinItem.State.Equipped);
                        setTimerDoneAction?.Invoke();
                    }
                }
           
            }
   
            //int remainingMinutes = ConvertStringToSeconds(remainingTime);
            //var startTime = 43200;
            //timeCircle.DOFillAmount((float) (remainingMinutes) / startTime, 1f);
        }
        
        private string FormatRemainingTime(string time) {  
            if (time.Contains(":")) {  
                string[] timeParts = time.Split(':');  

                if (timeParts.Length == 3) { // Format: hours:minutes:seconds  
                    if (int.TryParse(timeParts[0], out int hours) &&   
                        int.TryParse(timeParts[1], out int minutes)) {  
                        return string.Format("{0}<size=24>H</size> {1}<size=24>M</size>", hours, minutes);  
                    }  
                }   
                if (timeParts.Length == 2) { // Format: minutes:seconds  
                    if (int.TryParse(timeParts[0], out int minutes) &&   
                        int.TryParse(timeParts[1], out int seconds)) {  
                        return string.Format("{0}<size=24>M</size> {1:D2}<size=24>S</size>", minutes, seconds);  
                    }  
                }  
            }  
            return time; // Return original if not in expected format  
        }  
        
        private int ConvertStringToSeconds(string timeString)  
        {  
            if (timeString.Contains(":"))  
            {  
                string[] timeParts = timeString.Split(':');  
        
                if (timeParts.Length == 3) 
                {  
                    if (int.TryParse(timeParts[0], out int hours) &&   
                        int.TryParse(timeParts[1], out int minutes) &&   
                        int.TryParse(timeParts[2], out int seconds))  
                    {  
                        return (hours * 3600) + (minutes * 60) + seconds;  
                    }  
                }  
                else if (timeParts.Length == 2)
                {  
                    if (int.TryParse(timeParts[0], out int minutes) &&   
                        int.TryParse(timeParts[1], out int seconds))  
                    {  
                        return (minutes * 60) + seconds;  
                    }  
                }  
            }  
            else if (timeString.Contains("hr")) 
            {  
                string[] parts = timeString.Split(' ');  
                if (parts.Length > 0 && int.TryParse(parts[0], out int hours))  
                {  
                    return hours * 3600; 
                }  
            }  
    
            return 0; 
        }  
        
        public double CheckTimer()
        {
            return LivesManagerSkinItem.GetRemainingSeconds();
        }
    }
}