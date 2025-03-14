using System;
using System.Collections.Generic;
using System.Linq;
using Adverstising_Integration.Scripts;
using Advertising;
using ExaGames.Common;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs
{
    public class SkinItem : MonoBehaviour
    {
        public enum State {Buy = 0, Bought = 1,Reward = 2 , Equipped = 3 }
        
        public int ID;

        public bool Unlock;
        public bool canRental;
        public int rentalMinus;

        private SkinPopup skin;
        public State state;
        public SellType sellType;
        public SkinType skinType;
        public NameType nameType;
        [HideInInspector] public float priceMoneyIAP;
        [HideInInspector] public float priceMoneyGem;
        [HideInInspector] public float priceMoneyToken;
        //[HideInInspector] public float priceMoneyToken;
        [HideInInspector] public string statusItem;
        [HideInInspector] public string hightLight;
        public SkinPopup skinPopup;
        
        [SerializeField] public GameObject[] stateObjects;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> bgListNormal;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> bgListNoel;
        [SerializeField] private List<Sprite> bgListMain = new List<Sprite>();
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> iconBuyTypeList;
        [SerializeField] private Image bgItem;
        [SerializeField] public Image iconImage;
        [SerializeField] private Image iconBuyType;
        [SerializeField] public GameObject SelectItem;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> lockBg;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> checkInBg;
        [SerializeField] private Image lockImage;
        [SerializeField] private Image checkInImage;
        [SerializeField] private GameObject hideBgItem;
        [SerializeField] private GameObject tutorialObject;
        [SerializeField] private GameObject tutorialHand;
        [SerializeField] private GameObject borderHightLight;
        [SerializeField] private Image overlay;
        private SkinItemRentalCountdownUI liveTimeItem;
        private bool uiState;
        public void Awake()
        {
            if (!FireBaseManager.Instance.showNoel)
            {
                lockImage.sprite = lockBg[0];
                checkInImage.sprite = checkInBg[0];
                bgListMain = bgListNormal;
            }
            else
            {
                lockImage.sprite = lockBg[1];
                checkInImage.sprite = checkInBg[1];
                bgListMain = bgListNoel;
            }

            liveTimeItem = GetComponent<SkinItemRentalCountdownUI>();
        }

        public void SetTutorialActive(bool active)
        {
            tutorialObject.SetActive(active);
            borderHightLight.SetActive(active);
            Color color = overlay.color;
            color.a = 0.1f;
            overlay.color = color;
        }

        public void SetTutorialTransform(Transform target)
        {
            if (tutorialHand == null) return;
            tutorialHand.transform.SetParent(target, worldPositionStays: true);
        }

        public void SetHideBg(bool active)
        {
            hideBgItem.SetActive(active);
        }

        public void SetData(int id, SkinPopup item)
        {
            this.ID = id;
            this.skinPopup = item;
        }
        public void SetState(State state)
        {
            for (int i = 0; i < stateObjects.Length; i++)
            {
                stateObjects[i].SetActive(false);
            }
        
            stateObjects[(int)state].SetActive(true);
        
            this.state = state;
        }

        public void SetBackGround()
        {
            switch (skinType)
            {
                case SkinType.Common:
                    bgItem.sprite = bgListMain[0];
                    ShowIconSellType();
                    break;
                case SkinType.Exclusive:
                    bgItem.sprite = bgListMain[2];
                    ShowIconSellType();
                    break;
                case SkinType.Rare:
                   bgItem.sprite = bgListMain[3];
                   ShowIconSellType();
                    break;
            } 
            if (Unlock && ID == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected)
            {
                bgItem.sprite = bgListMain[1];
                iconBuyType.gameObject.SetActive(false);
            }
        }

        public void ShowIconSellType()
        {
            if (sellType == SellType.Token)
            {
                iconBuyType.sprite = iconBuyTypeList[2];
            }else if (sellType == SellType.Gem)
            {
                iconBuyType.sprite = iconBuyTypeList[1];
            }else if (sellType == SellType.IAP)
            {
                iconBuyType.sprite = iconBuyTypeList[0];
            }else if (sellType == SellType.Reward || sellType == SellType.Get)
            {
                iconBuyType.gameObject.SetActive(false);
            }
            iconBuyType.SetNativeSize();
            iconBuyType.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); 
        }
        
        public void SelectButton()
        {
            skinPopup.SelectItem(this);
        }
        // [Button]
        // public void GetRetalSkin()
        // {
        //     AdsManager.Instance.ShowReward(RetalSkin, AdsManager.RewardType.RETAL_SKIN);
        // }
        //
        // public void RetalSkin()
        // {
        //     if (AdsManager.Instance.isCancelRewardedAd)
        //     {
        //         AdsManager.Instance.isCancelRewardedAd = false;
        //         return;
        //     }
        //     var data = FacilityManager.Instance.GameSaveLoad.StableGameData;
        //     data.idSkinRented = ID;
        //     data.isActiveRetal = true;
        // }

        public void ActiveItem(bool active)
        {
            SelectItem.SetActive(active);
        }
        public void UpdateTimer()
        {
           GetComponent<SkinItemRentalCountdownUI>().StartCountDown();
           FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Add(ID);
           SetStatusRentalUI(false);
        }

        public void SetStatusRentalUI(bool active)
        {
            if (!FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(ID))
            {
                stateObjects[0].SetActive(active);
            }
            stateObjects[3].SetActive(false);
        }

        public double GetTimeDownCurrent()
        {
            return liveTimeItem.CheckTimer();
        }

        public void OnStopCountDownWhenBuy()
        {
            FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Remove(ID);
            GetComponent<SkinItemRentalCountdownUI>().LivesManagerSkinItem.ResetPlayerPrefs();
            GetComponent<SkinItemRentalCountdownUI>().countDownObj.SetActive(false);
        }
    }

}

