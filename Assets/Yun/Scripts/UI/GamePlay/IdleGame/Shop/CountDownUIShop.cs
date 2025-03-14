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

public class CountDownUIShop : YunBehaviour
{
        [SerializeField] protected TextMeshProUGUI countDownTxt;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> iconBackgroundList;
        public Image backgroundImage;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> iconImageList;
        public Image iconImage;
        [SerializeField] protected GameObject adsIcon;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> adsIconList;
        [SerializeField] protected List<GameObject> contentObj;

        [SerializeField] public TextMeshProUGUI activeAdsTurn;
        //[SerializeField] private Image timeCircle;
        protected Tween _delayTween;

        public virtual void Start()
        {
            SetStateIconAdsInPackage();
        }

        public void SetStateIconAdsInPackage()
        {
            if(FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                adsIcon.GetComponent<Image>().sprite = adsIconList[1];
            }else if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip &&
                      FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds <= 0)
            {
                adsIcon.GetComponent<Image>().sprite = adsIconList[0];
            }
        }
        public void SetBackGroundPack(int index)
        {
            backgroundImage.sprite = iconBackgroundList[index];
        }
        public void SetActiveAdsIcon(bool active)
        {
            adsIcon.SetActive(active);
        }

        public void SetActiveIconImage(int index)
        {
            iconImage.sprite = iconImageList[index];
            iconImage.SetNativeSize();
        }

        public void TurnOnCountDownContent()
        {
            contentObj[0].SetActive(false);
            contentObj[1].SetActive(true);
        }
        
        public void TurnOffCountDownContent()
        {
            contentObj[0].SetActive(true);
            contentObj[1].SetActive(false);
        }
        
        public void StartCountDown()
        {
            //timeCircle.fillAmount = 1;
            CountDown();
        }

        protected virtual void CountDown()
        {
            
        }
 
        
        public virtual void OnTimeToNextLifeChanged() {
  
        }
        
        protected string FormatRemainingTime(string time) {  
            if (time.Contains(":")) {  
                string[] timeParts = time.Split(':');  

                if (timeParts.Length == 3) { // Format: hours:minutes:seconds  
                    if (int.TryParse(timeParts[0], out int hours) &&   
                        int.TryParse(timeParts[1], out int minutes)) {  
                        return string.Format("{0}H {1}M", hours, minutes);  
                    }  
                }   
                if (timeParts.Length == 2) { // Format: minutes:seconds  
                    if (int.TryParse(timeParts[0], out int minutes) &&   
                        int.TryParse(timeParts[1], out int seconds)) {  
                        return string.Format("{0}M {1:D2}S", minutes, seconds);  
                    }  
                }  
            }  
            return time; // Return original if not in expected format  
        }  

        
        protected int ConvertStringToSeconds(string timeString)  
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
}
