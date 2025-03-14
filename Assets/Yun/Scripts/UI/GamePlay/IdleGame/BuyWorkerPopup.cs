using Advertising;
using TMPro;
using TwistedTangle.HieuUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yun.Scripts.Ads;
using Yun.Scripts.Audios;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Logics;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class BuyWorkerPopup : BasePopup
    {
        [SerializeField] private BuyWorkerButtonBG buyBedRoomStaffBg;
        [SerializeField] private BuyWorkerButtonBG buyBoxingStaffBg;
        [SerializeField] private BuyWorkerButtonBG buyTrainingStaffBg;
        [SerializeField] private BuyWorkerButtonBG buyDiningStaffBg;
        [SerializeField] private GameObject nativeAds;
        
        protected override void Start()
        {
            base.Start();
            
            buyBedRoomStaffBg.transform.Find("Bedroom_Staff_Icon").gameObject.SetActive(true);
            buyBedRoomStaffBg.transform.Find("Boxing_Staff_Icon").gameObject.SetActive(false);
            buyBedRoomStaffBg.transform.Find("Training_Staff_Icon").gameObject.SetActive(false);
            buyBedRoomStaffBg.transform.Find("Dining_Staff_Icon").gameObject.SetActive(false);
            buyBedRoomStaffBg.transform.Find("Name_Txt").GetComponent<TextMeshProUGUI>().text = "Bedroom";
            buyBedRoomStaffBg.transform.Find("Buy_Button").GetComponent<UIButton>().onClick.AddListener(OnBuyBedroomStaffClick);
            
            buyBoxingStaffBg.transform.Find("Bedroom_Staff_Icon").gameObject.SetActive(false);
            buyBoxingStaffBg.transform.Find("Boxing_Staff_Icon").gameObject.SetActive(true);
            buyBoxingStaffBg.transform.Find("Training_Staff_Icon").gameObject.SetActive(false);
            buyBoxingStaffBg.transform.Find("Dining_Staff_Icon").gameObject.SetActive(false);
            buyBoxingStaffBg.transform.Find("Name_Txt").GetComponent<TextMeshProUGUI>().text = "Boxing";
            buyBoxingStaffBg.transform.Find("Buy_Button").GetComponent<UIButton>().onClick.AddListener(OnBuyBoxingStaffClick);
            
            buyTrainingStaffBg.transform.Find("Bedroom_Staff_Icon").gameObject.SetActive(false);
            buyTrainingStaffBg.transform.Find("Boxing_Staff_Icon").gameObject.SetActive(false);
            buyTrainingStaffBg.transform.Find("Training_Staff_Icon").gameObject.SetActive(true);
            buyTrainingStaffBg.transform.Find("Dining_Staff_Icon").gameObject.SetActive(false);
            buyTrainingStaffBg.transform.Find("Name_Txt").GetComponent<TextMeshProUGUI>().text = "Training";
            buyTrainingStaffBg.transform.Find("Buy_Button").GetComponent<UIButton>().onClick.AddListener(OnBuyTrainingStaffClick);
            
            buyDiningStaffBg.transform.Find("Bedroom_Staff_Icon").gameObject.SetActive(false);
            buyDiningStaffBg.transform.Find("Boxing_Staff_Icon").gameObject.SetActive(false);
            buyDiningStaffBg.transform.Find("Training_Staff_Icon").gameObject.SetActive(false);
            buyDiningStaffBg.transform.Find("Dining_Staff_Icon").gameObject.SetActive(true);
            buyDiningStaffBg.transform.Find("Name_Txt").GetComponent<TextMeshProUGUI>().text = "Dining";
            buyDiningStaffBg.transform.Find("Buy_Button").GetComponent<UIButton>().onClick.AddListener(OnBuyDiningStaffClick);

            var bedroomStaffsList = FacilityManager.Instance.GameSaveLoad.GameData.buyBedRoomStaffsList;
            var quantity = 0;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey("Hire_Bedroom_Employee_Point_Area_1"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey("Hire_Bedroom_Employee_Point_Area_2"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey("Hire_Bedroom_Employee_Point_Area_3"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey("Hire_Bedroom_Employee_Point_Area_4"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey("Hire_Bedroom_Employee_Point_Area_5"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey("Hire_Bedroom_Employee_Point_Area_6"))
                quantity++;

            for (var i = 0; i < bedroomStaffsList.Count; i++)
            {
                var data = bedroomStaffsList[i];
                if (!data.isBought)
                {
                    buyBedRoomStaffBg.SetQuantity(quantity);
                    if (data.levelActive > FacilityManager.Instance.IdleGameData.Level)
                        buyBedRoomStaffBg.SetLevelActive(data.levelActive);
                    buyBedRoomStaffBg.SetPrice(data.price, FacilityManager.Instance.IdleGameData.Money);
                    break;
                }
                if (i == bedroomStaffsList.Count - 1)
                    buyBedRoomStaffBg.SetMax();

                quantity++;
            }


            var boxingStaffsList = FacilityManager.Instance.GameSaveLoad.GameData.buyBoxingStaffsList;
            quantity = 0;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey(
                    "Hire_Boxing_Worker_Point_Area_1"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey(
                    "Hire_Boxing_Worker_Point_Area_3"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey(
                    "Hire_Boxing_Worker_Point_Area_4"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey(
                    "Hire_Boxing_Worker_Point_Area_6"))
                quantity++;

            for (var i = 0; i < boxingStaffsList.Count; i++)
            {
                var data = boxingStaffsList[i];
                if (!data.isBought)
                {
                    buyBoxingStaffBg.SetQuantity(quantity);
                    if (data.levelActive > FacilityManager.Instance.IdleGameData.Level)
                        buyBoxingStaffBg.SetLevelActive(data.levelActive);
                    buyBoxingStaffBg.SetPrice(data.price, FacilityManager.Instance.IdleGameData.Money);
                    break;
                }
                
                if (i == boxingStaffsList.Count - 1)
                    buyBoxingStaffBg.SetMax();

                quantity++;
            }


            var trainingStaffsList = FacilityManager.Instance.GameSaveLoad.GameData.buyTrainingStaffsList;
            quantity = 0;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey(
                    "Hire_TrainingRoom_Employee_Point_Area_2"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey(
                    "Hire_TrainingRoom_Employee_Point_Area_3"))
                quantity++;

            for (var i = 0; i < trainingStaffsList.Count; i++)
            {
                var data = trainingStaffsList[i];
                if (!data.isBought)
                {
                    buyTrainingStaffBg.SetQuantity(quantity);
                    if (data.levelActive > FacilityManager.Instance.IdleGameData.Level)
                        buyTrainingStaffBg.SetLevelActive(data.levelActive);
                    buyTrainingStaffBg.SetPrice(data.price, FacilityManager.Instance.IdleGameData.Money);
                    break;
                }
                
                if (i == trainingStaffsList.Count - 1)
                    buyTrainingStaffBg.SetMax();

                quantity++;
            }


            var diningStaffsList = FacilityManager.Instance.GameSaveLoad.GameData.buyDiningStaffsList;
            quantity = 0;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey(
                    "Hire_DiningRoom_Employee_Point_Area_2"))
                quantity++;
            if (FacilityManager.Instance.GameSaveLoad.GameData.HireEmployeePointsDictionary.ContainsKey(
                    "Hire_DiningRoom_Employee_Point_Area_5"))
                quantity++;

            for (var i = 0; i < diningStaffsList.Count; i++)
            {
                var data = diningStaffsList[i];
                if (!data.isBought)
                {
                    buyDiningStaffBg.SetQuantity(quantity);
                    if (data.levelActive > FacilityManager.Instance.IdleGameData.Level)
                        buyDiningStaffBg.SetLevelActive(data.levelActive);
                    buyDiningStaffBg.SetPrice(data.price, FacilityManager.Instance.IdleGameData.Money);
                    break;
                }
                
                if (i == diningStaffsList.Count - 1)
                    buyDiningStaffBg.SetMax();

                quantity++;
            }
            
            if(nativeAds && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeBuyWorkerPopup)
                nativeAds.SetActive(false);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(nativeAds && nativeAds.activeSelf)
                nativeAds.GetComponent<NativeManager>().ShowNativeLoading(false);
        }

        public void OnBuyBedroomStaffClick()
        {
            FacilityManager.Instance.OnBuyBedroomStaff();
            Close();
        }
        
        public void OnBuyBoxingStaffClick()
        {
            FacilityManager.Instance.OnBuyBoxingStaff();
            Close();
        }

        public void OnBuyDiningStaffClick()
        {
            FacilityManager.Instance.OnBuyDiningStaff();
            Close();
        }

        public void OnBuyTrainingStaffClick()
        {
            FacilityManager.Instance.OnBuyTrainingStaff();
            Close();
        }
        
        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeIn);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
            FacilityManager.Instance.CheckCanBuyWorker();
        }
    }
}