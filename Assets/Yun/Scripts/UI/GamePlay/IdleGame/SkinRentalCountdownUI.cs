using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using ExaGames.Common;
using Sirenix.OdinInspector;
using Yun.Scripts.GamePlay.IdleGame.Players;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class SkinRentalCountdownUI : YunBehaviour
    {
        public LivesManagerSkin LivesManagerSkin;
        [SerializeField] private TextMeshProUGUI countDownTxt;
        //[SerializeField] private Image timeCircle;
        [SerializeField] public GameObject countDownObj;
        private Tween _delayTween;
        public static Action<string> SetTextAction;
        private string lastSentValue;
        public void StartCountDown(double seconds)
        {
            LivesManagerSkin.MinutesToRecover = seconds/60;
            CountDown();
            ChangeTimer(seconds);
        }

        private void CountDown()
        {
            LivesManagerSkin.AddLifeSlots(1);
            countDownObj.SetActive(true);
            //timeCircle.DOFillAmount(1, _startCountDown).OnComplete(OnCountDownComplete); 
        }
        
        
        public void OnTimeToNextLifeChanged()
        {
            string remainingTime = LivesManagerSkin.RemainingTimeString;
            Debug.Log("remainingTime:" + remainingTime);
            string formattedTime = FormatRemainingTime(remainingTime);
            countDownTxt.text = formattedTime;
        
            if (remainingTime != lastSentValue)
            {
                SetTextAction?.Invoke(formattedTime);
                lastSentValue = remainingTime;

                if (remainingTime == "00:00" || remainingTime == "Done")
                {
                    HandleTimeUp(remainingTime);
                }
            }

            // Code bị comment có thể bỏ đi nếu không cần thiết
            // int remainingMinutes = ConvertStringToSeconds(remainingTime);
            // var startTime = 43200;
            // timeCircle.DOFillAmount((float) (remainingMinutes) / startTime, 1f);
        }

        private void HandleTimeUp(string remainingTime)
        {
            var gameData = FacilityManager.Instance.GameSaveLoad.StableGameData;
    
            // if (remainingTime == "00:00")
            // {
            //     LivesManagerSkin.ResetPlayerPrefs();
            // }

            if (gameData.isActiveRetal || remainingTime == "00:00")
            {
                gameData.isActiveRetal = false;
                FacilityManager.Instance.player.ChangeSkinPlayer(gameData.idSkinSelected);
                FacilityManager.Instance.player.GetComponent<PlayerShootingControl>().OnInit();
                FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            }

            countDownObj.SetActive(false);
        }
        
        private string FormatRemainingTime(string time) {  
            if (time.Contains(":")) {  
                string[] timeParts = time.Split(':');  

                if (timeParts.Length == 3) { // Format: hours:minutes:seconds  
                    if (int.TryParse(timeParts[0], out int hours) &&   
                        int.TryParse(timeParts[1], out int minutes)) {  
                        return string.Format("{0}<size=21>H</size> {1}<size=21>M</size>", hours, minutes);  
                    }  
                }   
                if (timeParts.Length == 2) { // Format: minutes:seconds  
                    if (int.TryParse(timeParts[0], out int minutes) &&   
                        int.TryParse(timeParts[1], out int seconds)) {  
                        return string.Format("{0}<size=21>M</size> {1:D2}<size=21>S</size>", minutes, seconds);  
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

        [Button]
        public void ChangeTimer(double timer)
        {
            LivesManagerSkin.ChangeRecoveryTime(timer);
        }
    }
}