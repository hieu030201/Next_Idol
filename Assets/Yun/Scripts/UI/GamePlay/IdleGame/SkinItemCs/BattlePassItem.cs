using System;
using System.Collections.Generic;
using Advertising;
using Sirenix.OdinInspector;
using TMPro;
using TwistedTangle.HieuUI;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using UnityEngine.UI;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Utilities;

namespace Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs
{
    public class BattlePassItem : MonoBehaviour
    {
        public enum ItemState {Claim = 0, Claimed = 1}
        public int ID;
        public int Amount;
        public RewardBattlePassType RewardType;
        public bool Claimed;
        
        public TextMeshProUGUI AmountTxt;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> iconList;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> bgListNomal;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> bgNoelList;
        [ShowInInspector] protected List<Sprite> bgList;
        [SerializeField] protected Image iconItem;
        [SerializeField] protected Image bgItem;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> overlayList;
        [SerializeField] protected GameObject bgOverlay;
        protected BattlePassPopup battlePassPopup;

        protected virtual void Awake()
        {
            bgList = FireBaseManager.Instance.showNoel ? bgNoelList : bgListNomal;
            bgOverlay.GetComponent<Image>().sprite = FireBaseManager.Instance.showNoel ? overlayList[1] : overlayList[0];
        }

        public void SetData(int id, BattlePassPopup item)
        {
            ID = id;
            battlePassPopup = item;
        }
        public virtual void ClaimItem()
        {
     
        }

        public virtual void SetHight(bool active)
        {
            
        }
        
        public void SetBackGround()
        {
            if (!Claimed)
            {
                bgItem.sprite = bgList[0];
                bgOverlay.SetActive(false);
            }
            else
            {
                bgItem.sprite = bgList[1];
                bgOverlay.SetActive(true);
            }
            
            bgItem.SetNativeSize();
        }

        public void SetIconItem()
        {
            switch (RewardType)
            {
                case RewardBattlePassType.None:
                    GetComponent<UIButton>().enabled = false;
                    foreach (Transform child in gameObject.transform)  
                    {  
                        child.gameObject.SetActive(false); 
                    }
                    break;
                case RewardBattlePassType.Cash:
                    iconItem.sprite = iconList[0];
                    break;
                case RewardBattlePassType.Token:
                    //iconItem.sprite = iconList
                    break;
                case RewardBattlePassType.Diamon:
                    iconItem.sprite = iconList[1];
                    break;
                case RewardBattlePassType.RemoveAd:
                    iconItem.sprite = iconList[2];
                    break;
                case RewardBattlePassType.Skin:
                    break;
            }
        }

        public void GetRewardItem()
        {
            if (!Claimed)
            {
                switch (RewardType)
                {
                    case RewardBattlePassType.Cash:
                        FacilityManager.Instance.AddMoney(Amount, gameObject.transform.position, true, battlePassPopup.transform);
                        break;
                    case RewardBattlePassType.Diamon:
                        FacilityManager.Instance.AddGem(Amount, gameObject.transform.position, true,battlePassPopup.transform);
                        break;
                    case RewardBattlePassType.Token:
                        FacilityManager.Instance.AddToken(Amount, gameObject.transform.position, true,battlePassPopup.transform);
                        break;
                    case RewardBattlePassType.RemoveAd:
                        FacilityManager.Instance.GameSaveLoad.StableGameData.isPurchasedRemoveAds = true;
                        break;
                    case RewardBattlePassType.Skin:
                        FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Add(3);
                        break;
                }
                //FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            }
        }

        public void SetAmount(int amount)
        {
            if (amount == 1 || RewardType== RewardBattlePassType.RemoveAd)
            {
                AmountTxt.gameObject.SetActive(false);
            }
            AmountTxt.text = UtilitiesFunction.FormatNumber(amount);
        }
        
        
    }
}


