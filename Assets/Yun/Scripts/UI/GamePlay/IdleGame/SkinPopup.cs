using System;
using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using ExaGames.Common;
using Sirenix.OdinInspector;
using TheraBytes.BetterUi;
using TMPro;
using TwistedTangle.HieuUI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.Ads;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.Managers;
using Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs;
using Sequence = DG.Tweening.Sequence;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class SkinPopup : BasePopup
    {
        public enum ButtonState {btnBuy = 0, btnBuyGem = 1, btnGet = 2 , btnSelect = 3, btnSelectLock = 4, btnSelected = 5}
        
        [SerializeField] public IdlePlayerActionStateMachine stateMachine;

        [SerializeField] private TextMeshProUGUI priceMoneyText;
        [SerializeField] private YunTextShadow priceGemMoneyText;
        [SerializeField] private TextMeshProUGUI priceTokenMoneyText;
        
        [SerializeField] private GameObject itemObject;
        [SerializeField] public List<GameObject> listSkin;
        [SerializeField] private GameObject[] stateButtons;
        [SerializeField] private Transform parentSpawn;
        [SerializeField] private GameObject labelContent;
        [SerializeField] private TextMeshProUGUI lableTextItem;
        [SerializeField] private GameObject nativeAds;
        
        [SerializeField] public RawImage rawImage;
        
        public GameObject renderModel;
        
        private SkinItem currentItem;
        private SkinItem currentItemSelected;
        private ButtonState buttonState;
        private NameType currentNameType;

        [SerializeField] public GameObject ModelCharacter;
        [SerializeField] public GameObject ModelCharacterSprunky;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;  
        // [SerializeField] private GameObject skinTutorial;
        [SerializeField] private GameObject buttonStateTutorial;
        [SerializeField] private GameObject borderGetHightLight;
        [SerializeField] private GameObject borderSelectHightLight;
        [SerializeField] private Transform targetViewPort;
        [SerializeField] private Button btnClose;

        [SerializeField] private List<RectTransform> listComponentUsingEffect = new List<RectTransform>();
        [SerializeField] private RectTransform bgListItem;
        [SerializeField] private Image bgBottom;
        [SerializeField] private GameObject btnRental;
        [SerializeField] private GameObject notActiveRentalObj;
        [SerializeField] private GameObject isActiveRentalObj;
        [SerializeField] private GameObject setActiveRentalAgainObj;
        [SerializeField] private YunTextShadow countRentalTime;
        [SerializeField] private YunTextShadow countRentedTimeAgain;
        [SerializeField] protected Image iconBtnRewardAds;

        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        protected List<Sprite> listIconBtnRewardAds;
        //List<LivesManagerSkinItem> livesManagers = new List<LivesManagerSkinItem>();
        private void Awake()
        {
            OnInit();
            float screenRatio = (float)Screen.height / (float)Screen.width;  
            //Vector3 currentPosition = tutorialrectTransform.anchoredPosition;  
            // Adjust the aspect ratio based on screen dimensions  

            if (screenRatio >= 2.0f) // For ultra-wide screens (e.g., 21:9)  iphone 13 promax
            {  
                aspectRatioFitter.aspectRatio = 1.4f; // Set to 2.0 
                //tutorialrectTransform.anchoredPosition = new Vector3(currentPosition.x, -660, currentPosition.z);  
            }  
            else if (screenRatio >= 1.78f) // ~16:9  (1080x1920)
            {  
                aspectRatioFitter.aspectRatio = 1.5f; // Set to 16:9  (1080x1920)
                //tutorialrectTransform.anchoredPosition = new Vector3(currentPosition.x, -660, currentPosition.z);  
            }  
            else if (screenRatio >= 1.5f) 
            {  
                aspectRatioFitter.aspectRatio = 2.5f; // Set to 18:9  
                //tutorialrectTransform.anchoredPosition = new Vector3(currentPosition.x, -660, currentPosition.z);  
            }  
            else if (screenRatio >= 1.33f) // ~4:3  
            {  
                aspectRatioFitter.aspectRatio = 6f; // Set to 4:3  
            }  
            else  
            {  
                aspectRatioFitter.aspectRatio = 1.0f; // Set to 1:1 for very narrow screens  
            }

            if(nativeAds && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeSkinShopPopup)
                nativeAds.SetActive(false);
        }
        
        private void OnEnable()
        {
            SkinItemRentalCountdownUI.SetTextAction += UpdateText;
            SkinItemRentalCountdownUI.setTimerDoneAction += GetSkinEquipped;
        }

        public override void Show()
        {
            var content = transform.Find("Content");
            // ShowEffect(YunEffectType., content.transform);
           
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if(nativeAds && nativeAds.activeSelf)
                nativeAds.GetComponent<NativeManager>().ShowNativeLoading(false);
        }

        private void Start()
        {
            foreach (Transform child in ModelCharacter.transform)
            {
                child.gameObject.SetActive(false);
            }

            foreach (Transform childSprunky in ModelCharacterSprunky.transform)
            {
                childSprunky.gameObject.SetActive(false);
            }
   
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;
            var skinId = data.isActiveRetal ? data.idSkinRented : data.idSkinSelected ;
            var skin = listSkin.Find(s => s.GetComponent<SkinItem>().ID == skinId)?.GetComponent<SkinItem>();

            if (skin != null)
            {
                skin.ActiveItem(true);
                skin.SetState(SkinItem.State.Equipped);
                currentItem = skin;
            }
            Debug.Log("currentItem: " + currentItem.ID);
            var model = currentItem.ID < 5 ? ModelCharacter.transform.Find(currentItem.nameType.ToString()) : ModelCharacterSprunky.transform.Find(currentItem.nameType.ToString());
            currentNameType = currentItem.nameType;
            model.gameObject.SetActive(true);
            if (!data.isActiveRetal)
            {
                stateButtons[5].SetActive(true);
            }
            else
            {
                SelectItem(currentItem);
            }

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(1))
            {
                foreach (var skinHide in listSkin)
                {
                    skinHide.GetComponent<SkinItem>().SetHideBg(false);
                }
                btnClose.interactable = true;
            }
            else
            {
                ChangeColorImageWhenActiveTutorial(true);
                btnClose.interactable = false;
            }
            
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip &&
                FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
            }
            
            ChangeAnimShowUp();
            AddEffectShow();
            AddEffectShowBackGround();
            AddEffectShowItem();
        }

        private void ChangeColorImageWhenActiveTutorial(bool active)
        {
            if (active)
            {
                bgListItem.gameObject.GetComponent<Image>().color = new Color(0.635f, 0.635f, 0.635f, 1f);
                bgBottom.color = new Color(0.635f, 0.635f, 0.635f, 1f);
            }
            else
            {
                bgListItem.gameObject.GetComponent<Image>().color = Color.white;
                bgBottom.color = Color.white;
            }
        }
        
        private void OnInit()
        {
            int countValues = FacilityManager.Instance.SkinDataCollection.Value.Count;  
            for (int i = 0; i < countValues; i++)
            {
                var obj = Instantiate(itemObject, parentSpawn);
                var valueIndex = FacilityManager.Instance.SkinDataCollection[i];
                SkinItemRentalCountdownUI lives = obj.GetComponent<SkinItemRentalCountdownUI>();
                lives.LivesManagerSkinItem.itemId = $"object_{i}";
                lives.LivesManagerSkinItem.MinutesToRecover = valueIndex.rentalMinutes;
                listSkin.Add(obj);
                var item = obj.GetComponent<SkinItem>();
                item.SetData(i, this);
                item.ID = valueIndex.id;
                item.canRental = valueIndex.canRental;
                item.rentalMinus = valueIndex.rentalMinutes;
                item.priceMoneyIAP = valueIndex.price;
                item.priceMoneyGem = valueIndex.price;
                item.priceMoneyToken = valueIndex.price;
                item.nameType = valueIndex.name;
                item.statusItem = valueIndex.status;
                item.hightLight = valueIndex.hightLight;
                item.iconImage.sprite = valueIndex.icon;
                item.iconImage.SetNativeSize();
                if (i >= 5)
                {
                    item.iconImage.transform.localScale = Vector3.one * 1.2f;
                }
                
                if (i == 0)
                {
                    RectTransform rectTransform = item.iconImage.GetComponent<RectTransform>();
                    Vector2 newAnchoredPosition = rectTransform.anchoredPosition;
                    newAnchoredPosition.y = 110; 
                    rectTransform.anchoredPosition = newAnchoredPosition; 
                }
                if (i == 1 && !FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(1))
                {
                    item.SetTutorialActive(true);
                    targetViewPort.GetComponent<RectMask2D>().enabled = false;
                    item.SetHideBg(false);
                }else if (i != 1 && !FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(1))
                {
                    item.GetComponent<UIButton>().interactable = false;
                }
                
                item.sellType = valueIndex.sellType;
                item.skinType = valueIndex.skinType;
                if (FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(item.ID))
                {
                    item.Unlock = true;
                }
                else
                {
                    item.Unlock = false;
                }
                
                GetStateItem(item);
                if (FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Contains(item.ID))
                {
                    item.SetStatusRentalUI(false);
                }
                
                // var livesManager = new LivesManagerSkinItem { itemId = $"item{item.ID}" };  
                // livesManagers.Add(livesManager);
            }
        }
       
        private void GetStateItem(SkinItem item)
        {
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;

            if (item.Unlock)
            {
                item.SetState(item.ID == data.idSkinSelected ? SkinItem.State.Equipped : SkinItem.State.Bought);
                item.SetBackGround();
                currentItemSelected = (item.ID == data.idSkinSelected) ? item : currentItemSelected;
            }
            else
            {
                item.SetState(item.sellType == SellType.Reward ? SkinItem.State.Reward : SkinItem.State.Buy);
                item.SetBackGround();

                if (item.canRental && data.isActiveRetal && data.idSkinRented == item.ID)
                {
                    item.SelectItem.SetActive(true);
                    item.stateObjects[3].SetActive(true);
                }
            }

            if (data.isActiveRetal && item.ID == data.idSkinSelected)
            {
                item.SetState(SkinItem.State.Bought);
                item.SetBackGround();
                item.stateObjects[3].SetActive(false);
                item.ActiveItem(false);
            }
        }

        private Transform modelCharacterOld;
        private Transform modelCharacter;
        internal void SelectItem(SkinItem item)
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            currentItem.ActiveItem(false);
            currentItem = item;
       
            if (currentItem.ID ==1 && !FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(1))
            {
                currentItem.SetTutorialActive(false);
                targetViewPort.GetComponent<RectMask2D>().enabled = true;
                buttonStateTutorial.SetActive(true);
                borderGetHightLight.SetActive(true);
                borderSelectHightLight.SetActive(true);
            }

            if (currentItem.canRental)
            {
                btnRental.SetActive(true);
                if (FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Contains(currentItem.ID))
                {
                    notActiveRentalObj.SetActive(false);
                    if (FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinRented == currentItem.ID)
                    {
                        setActiveRentalAgainObj.SetActive(false);
                        isActiveRentalObj.SetActive(true);
                        if (!FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveRetal)
                        {
                            setActiveRentalAgainObj.SetActive(true);
                            isActiveRentalObj.SetActive(false);
                        }
                        else
                        {
                            setActiveRentalAgainObj.SetActive(false);
                            isActiveRentalObj.SetActive(true);
                        }
                    }
                    else
                    {
                        setActiveRentalAgainObj.SetActive(true);
                        isActiveRentalObj.SetActive(false);
                    }
                }
                else
                {
                    ChangeStatusBtnRental(false);
                    setActiveRentalAgainObj.SetActive(false);
                }
            }
            else
                btnRental.SetActive(false);

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(currentItem.ID))
            {
                btnRental.SetActive(false);
            }
            
            if (currentItem.statusItem != "")
                labelContent.SetActive(true);
            else
            {
                labelContent.SetActive(false);
            }
            
            switch (currentItem.state)
            {
                case SkinItem.State.Buy:
                    if (currentItem.sellType == SellType.Gem)
                    {
                        SetButtonState(1);
                        priceGemMoneyText.DisplayText = item.priceMoneyGem.ToString();
                    }else if (currentItem.sellType == SellType.Token)
                    {
                        SetButtonState(6);
                        priceTokenMoneyText.text = item.priceMoneyToken.ToString();
                    }
                    else if (currentItem.sellType == SellType.Get)
                    {
                        SetButtonState(2);
                    }else if (currentItem.sellType == SellType.IAP)
                    {
                        SetButtonState(0);
                        priceMoneyText.text = item.priceMoneyIAP.ToString();
                    }
                    
                    break;
                case SkinItem.State.Reward:
                    SetButtonState(4);
    
                    break;
                case SkinItem.State.Bought:
                    SetButtonState(3);
                    break;
                case  SkinItem.State.Equipped:
                    if (FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Contains(currentItem.ID))
                    {
                        if (currentItem.sellType == SellType.Gem)
                        {
                            SetButtonState(1);
                            priceGemMoneyText.DisplayText = item.priceMoneyGem.ToString();
                        }else if (currentItem.sellType == SellType.Token)
                        {
                            SetButtonState(6);
                            priceTokenMoneyText.text = item.priceMoneyToken.ToString();
                        }
                        else if (currentItem.sellType == SellType.Get)
                        {
                            SetButtonState(2);
                        }else if (currentItem.sellType == SellType.IAP)
                        {
                            SetButtonState(0);
                            priceMoneyText.text = item.priceMoneyIAP.ToString();
                        }
                    }
                    else
                    {
                        SetButtonState(5);
                    }
                    break;
                default:
                    break;
            }
            string combinedText = MergeAndColorText(currentItem.statusItem, currentItem.hightLight);  
            lableTextItem.text = combinedText;
            currentItem.ActiveItem(true);
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveRetal)
            {
                if (currentItem.ID == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected)
                {
                    SetButtonState(3);
                }
            }

            ChangeModelPlayer();
            
            currentNameType = currentItem.nameType;
        }

        public void ChangeModelPlayer()
        {
            Debug.Log("currentNameType:" + currentNameType);
            if (ModelCharacter != null && ModelCharacterSprunky != null)
            {
                modelCharacterOld = ModelCharacter.transform.Find(currentNameType.ToString()) ?? 
                                    ModelCharacterSprunky.transform.Find(currentNameType.ToString());

                modelCharacter = ModelCharacter.transform.Find(currentItem.nameType.ToString()) ?? 
                                 ModelCharacterSprunky.transform.Find(currentItem.nameType.ToString());
            }

            if (modelCharacterOld != null)
            {
                modelCharacterOld.gameObject.SetActive(false);
                switch (currentNameType)
                {
                    case NameType.Rambo:
                        if (ModelCharacter.transform.Find("Minigun") != null)
                        {
                            ModelCharacter.transform.Find("Minigun").gameObject.SetActive(false);
                        }
                        break;
                    case NameType.Mafia:
                        if (ModelCharacter.transform.Find("Thompson") != null)
                        {
                            ModelCharacter.transform.Find("Thompson").gameObject.SetActive(false);
                        }
                        break;
                    case NameType.Terminator:
                        if (ModelCharacter.transform.Find("Shotgun") != null)
                        {
                            ModelCharacter.transform.Find("Shotgun").gameObject.SetActive(false);
                        }
                        break;
                    case NameType.Sprunki_FunBot:
                        if (ModelCharacterSprunky.transform.Find("AK_Funbot") != null)
                        {
                            ModelCharacterSprunky.transform.Find("AK_Funbot").gameObject.SetActive(false);
                        }
                        break;
                    case NameType.Sprunki_Oren:
                        if (ModelCharacterSprunky.transform.Find("AK_Oren") != null)
                        {
                            ModelCharacterSprunky.transform.Find("AK_Oren").gameObject.SetActive(false);
                        }
                        break;
                    case NameType.Sprunki_OWAKCX:
                        if (ModelCharacterSprunky.transform.Find("AK_OWAKCX") != null)
                        {
                            ModelCharacterSprunky.transform.Find("AK_OWAKCX").gameObject.SetActive(false);
                        }
                        break;
                    case NameType.Sprunki_Pinki:
                        if (ModelCharacterSprunky.transform.Find("AK_Pinki") != null)
                        {
                            ModelCharacterSprunky.transform.Find("AK_Pinki").gameObject.SetActive(false);
                        }
                        break;
                    case NameType.Sprunki_Wenda:
                        if (ModelCharacterSprunky.transform.Find("AK_Wenda") != null)
                        {
                            ModelCharacterSprunky.transform.Find("AK_Wenda").gameObject.SetActive(false);
                        }
                        break;
                }
            }

            if (modelCharacter != null)
            {
                modelCharacter.gameObject.SetActive(true);
                ChangeAnimShowUp();
            }
           
        }
        
        public void SetButtonState(int index)
        {
            // for (int i = 0; i < stateButtons.Length; i++)
            // {
            //     stateButtons[i].SetActive(false);
            // }
            //
            // stateButtons[(int)buttonState].SetActive(true);
            //
            // buttonState = btnState;
            for (int i = 0; i < stateButtons.Length; i++)
            {
                stateButtons[i].SetActive(false);
            }
            stateButtons[index].SetActive(true);
        }

        public void BuyButton()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            switch (currentItem.sellType)
            {
                case SellType.Gem:
                    if (FacilityManager.Instance.GameSaveLoad.GameData.gem >= currentItem.priceMoneyGem)
                    {
                        FacilityManager.Instance.GameSaveLoad.GameData.gem -= (int)currentItem.priceMoneyGem;
                        Debug.Log(FacilityManager.Instance.GameSaveLoad.GameData.gem);
                        FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Add(currentItem.ID);
                        currentItem.Unlock = true;
                        GetStateItem(currentItem);
                        SetButtonState(3);
                        if (currentItem.canRental)
                        {
                            if (FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Contains(currentItem.ID))
                            {
                                SetButtonState(5);
                            }
                            btnRental.SetActive(false);
                            currentItem.OnStopCountDownWhenBuy();
                            FacilityManager.Instance.PlayerInfoUI.ResetTimeFollowSkin();
                        }
                    }
                    else
                    {
                        FacilityManager.Instance.ShowNotEnoughGemPopup();
                        return;
                    }
                    break;
                case SellType.Token:
                    if (FacilityManager.Instance.GameSaveLoad.GameData.token >= currentItem.priceMoneyToken)
                    {
                        FacilityManager.Instance.GameSaveLoad.GameData.token -= (int)currentItem.priceMoneyToken;
                        FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Add(currentItem.ID);
                        currentItem.Unlock = true;
                        GetStateItem(currentItem);
                        SetButtonState(3);
                        if (currentItem.canRental)
                        {
                            if (FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinRetal.Contains(currentItem.ID))
                            {
                                SetButtonState(5);
                            }
                            btnRental.SetActive(false);
                            currentItem.OnStopCountDownWhenBuy();
                            FacilityManager.Instance.PlayerInfoUI.ResetTimeFollowSkin();
                        }
                    }
                    break;
            }
            FacilityManager.Instance.PlayerInfoUI.UpdateGem(FacilityManager.Instance.IdleGameData.Gem, false);
            FacilityManager.Instance.PlayerInfoUI.UpdateToken(FacilityManager.Instance.IdleGameData.Token, false);
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
        }

        public void SelectButton()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            if (currentItem.ID == 1 && FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(1))
            {
                buttonStateTutorial.SetActive(false);
                borderGetHightLight.SetActive(false);
                borderSelectHightLight.SetActive(false);
                ChangeColorImageWhenActiveTutorial(false);
                btnClose.interactable = true;
                foreach (var skin in listSkin)
                {
                    skin.GetComponent<SkinItem>().SetHideBg(false);
                    skin.GetComponent<UIButton>().interactable = true;
                }
            }
            
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveRetal)
            {
                var skinRental = listSkin.Find(s =>
                    s.GetComponent<SkinItem>().ID == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinRented);
                skinRental.GetComponent<SkinItem>().stateObjects[3].SetActive(false);
            }
            FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected = currentItem.ID;
            FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveRetal = false;
            FacilityManager.Instance.player.ChangeSkinPlayer(FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected);
            FacilityManager.Instance.PlayerInfoUI.SetActiveTimeFollowSkin(false);
            GetStateItem(currentItemSelected);
            GetStateItem(currentItem);
            
            SetButtonState(5);
            FacilityManager.Instance.player.GetComponent<PlayerShootingControl>().OnInit();
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
        }

        public void GetButton()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            if (currentItem.skinType == SkinType.Common)
            {
                FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Add(currentItem.ID);
                currentItem.Unlock = true;
                GetStateItem(currentItem);
                SetButtonState(3);
                FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            }

            if (currentItem.skinType == SkinType.Exclusive)
            {
                Close();
                FacilityManager.Instance.ShowBattleFund();
                FacilityManager.Instance.isShowBattleFundFromSkin = true;
            }
        }
        public void GetRentalSkin()
        {
            AdsManager.Instance.ShowReward(SkinRental, AdsManager.RewardType.RETAL_SKIN);
        }
        public void SkinRental()
        {
            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveRetal)
            {
                var skinBefore = listSkin.Find(s =>
                    s.GetComponent<SkinItem>().ID == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinRented);
                skinBefore.GetComponent<SkinItem>().stateObjects[3].SetActive(false);
            }
            else
            {
                var skinBefore = listSkin.Find(s =>
                    s.GetComponent<SkinItem>().ID == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected);
                skinBefore.GetComponent<SkinItem>().stateObjects[3].SetActive(false);
            }
            
          
            data.idSkinRented = currentItem.ID;
            data.minuteShowInIdleGameUI = currentItem.rentalMinus;
            
            ChangeStatusBtnRental(true);
           
            currentItem.UpdateTimer();
            double time = currentItem.GetTimeDownCurrent();
            FacilityManager.Instance.PlayerInfoUI.StartCountDownForSkinRetal(time);
            data.isActiveRetal = true;
            FacilityManager.Instance.player.ChangeSkinPlayer(data.idSkinRented);
            FacilityManager.Instance.player.GetComponent<PlayerShootingControl>().OnInit();
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            
            currentItem.stateObjects[3].SetActive(true);
        }

        public void SkinRentalAgain()
        {
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveRetal)
            {
                var skinBefore = listSkin.Find(s =>
                    s.GetComponent<SkinItem>().ID == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinRented);
                skinBefore.GetComponent<SkinItem>().stateObjects[3].SetActive(false);
            }
            else
            {
                var skinBefore = listSkin.Find(s =>
                    s.GetComponent<SkinItem>().ID == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected);
                skinBefore.GetComponent<SkinItem>().stateObjects[3].SetActive(false);
            }
            setActiveRentalAgainObj.SetActive(false);
            isActiveRentalObj.SetActive(true);
            currentItem.stateObjects[3].SetActive(true);
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;
            data.idSkinRented = currentItem.ID;
            data.isActiveRetal = true;
            double time = currentItem.GetTimeDownCurrent();
            Debug.Log("SkinRentalAgain Time:" + time);
            FacilityManager.Instance.PlayerInfoUI.ChangeTimeFollowSkin(time);
            FacilityManager.Instance.player.ChangeSkinPlayer(data.idSkinRented);
            FacilityManager.Instance.PlayerInfoUI.SetActiveTimeFollowSkin(true);
            FacilityManager.Instance.player.GetComponent<PlayerShootingControl>().OnInit();
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
        }

        public void ChangeStatusBtnRental(bool active)
        {
            notActiveRentalObj.SetActive(!active);
            isActiveRentalObj.SetActive(active);
            if (active != true)
            {
                if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds <= 0)
                {
                    iconBtnRewardAds.sprite = listIconBtnRewardAds[0];
                }else if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
                {
                    iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
                }
            }
        }
        public void ChangeStatusBtnRentalTwo(bool active, int id)
        {
            if (currentItem.ID == id)
            {
                notActiveRentalObj.SetActive(!active);
                isActiveRentalObj.SetActive(active);
                setActiveRentalAgainObj.SetActive(active);
                if (active != true)
                {
                    if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds <= 0)
                    {
                        iconBtnRewardAds.sprite = listIconBtnRewardAds[0];
                    }else if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
                    {
                        iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
                    }
                }
            }
        }

        public void GetSkinEquipped()
        {
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;
            var skinItem = listSkin
                .Find(s => s != null && s.GetComponent<SkinItem>()?.ID == data.idSkinSelected)
                ?.GetComponent<SkinItem>();

            if (skinItem != null)
            {
                skinItem.SetState(SkinItem.State.Equipped);
            }
        }

        // [SerializeField] private TextMeshProUGUI timerTxt;
        // public void SetTextAction(string remainingTime)
        // {
        //     timerTxt.text = remainingTime;
        // }

        private void OnDisable()
        {
            SkinItemRentalCountdownUI.SetTextAction -= UpdateText;
            SkinItemRentalCountdownUI.setTimerDoneAction -= GetSkinEquipped;
        }
        private void UpdateText(string newText, string idItem)
        {
            if (idItem == $"object_{currentItem.ID}")
            {
                string formattedText = newText.Replace("M", "<size=50>M</size>").Replace("S", "<size=50>S</size>");
                countRentalTime.DisplayText = formattedText;
                countRentedTimeAgain.DisplayText = formattedText;
            }
        }

        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeToBottom);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
            FacilityManager.Instance.PlayerInfoUI.HideSkinAnimation();
            FacilityManager.Instance.PlayerInfoUI.HideSkinPopupBg();
            if (renderModel)
                renderModel.SetActive(false);
        }

        private void ChangeAnimShowUp()
        {
            switch (currentItem.nameType)
            {
                case NameType.Player:
                    stateMachine.animator.Play("Breath");
                    break;
                case NameType.Rambo:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShowUpRampo;
                    if (ModelCharacter.transform.Find("Minigun") != null)
                    {
                        ModelCharacter.transform.Find("Minigun").gameObject.SetActive(true);
                    }
                    break;
                case NameType.Mafia:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShowUpMafia;
                    if (ModelCharacter.transform.Find("Thompson") != null)
                    {
                        ModelCharacter.transform.Find("Thompson").gameObject.SetActive(true);
                    }
                    break;
                case NameType.Terminator:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShowUpTerminal;
                    if (ModelCharacter.transform.Find("Shotgun") != null)
                    {
                        ModelCharacter.transform.Find("Shotgun").gameObject.SetActive(true);
                    }
                    break;
                case NameType.Sprunki_FunBot:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShowUpSprunky;
                    // if (ModelCharacterSprunky.transform.Find("AK_Funbot") != null)
                    // {
                    //     ModelCharacterSprunky.transform.Find("AK_Funbot").gameObject.SetActive(true);
                    // }
                    break;
                case NameType.Sprunki_Oren:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShowUpSprunky;
                    // if (ModelCharacterSprunky.transform.Find("AK_Oren") != null)
                    // {
                    //     ModelCharacterSprunky.transform.Find("AK_Oren").gameObject.SetActive(true);
                    // }
                    break;
                case NameType.Sprunki_OWAKCX:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShowUpSprunky;
                    // if (ModelCharacterSprunky.transform.Find("AK_OWAKCX") != null)
                    // {
                    //     ModelCharacterSprunky.transform.Find("AK_OWAKCX").gameObject.SetActive(true);
                    // }
                    break;
                case NameType.Sprunki_Pinki:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShowUpSprunky;
                    // if (ModelCharacterSprunky.transform.Find("AK_Pinki") != null)
                    // {
                    //     ModelCharacterSprunky.transform.Find("AK_Pinki").gameObject.SetActive(true);
                    // }
                    break;
                case NameType.Sprunki_Wenda:
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShowUpSprunky;
                    // if (ModelCharacterSprunky.transform.Find("AK_Wenda") != null)
                    // {
                    //     ModelCharacterSprunky.transform.Find("AK_Wenda").gameObject.SetActive(true);
                    // }
                    break;
           
                default:
                    stateMachine.animator.Play("Breath");
                    break;
            }
        }
        
        string MergeAndColorText(string strA, string strB)  
        {  
            // Tìm vị trí của strB trong strA  
            int index = strA.IndexOf(strB);  
            string result;  
            
            if (index != -1)  
            {  
                string beforeMatch = strA.Substring(0, index); 
                string match = strA.Substring(index, strB.Length); 
                string afterMatch = strA.Substring(index + strB.Length); 

                // Kết hợp các phần với màu  
                result = $"{beforeMatch}<color=#FBC80A>{match}</color>{afterMatch}"; // Sửa cú pháp tại đây  

            }  
            else  
            {  
                result = strA;   
            }  

            return result;  
        }  
        private float initialDelay = 0.1f; // Thời gian trễ giữa các item  
        private float effectDuration = 0.3f; // Thời gian hiệu ứng  
        private void AddEffectShow()
        {
            for (int i = 0; i < listComponentUsingEffect.Count; i++)
            {
                CanvasGroup canvasGroup = listComponentUsingEffect[i].GetComponent<CanvasGroup>();  
                if (canvasGroup == null)  
                {  
                    canvasGroup = listComponentUsingEffect[i].gameObject.AddComponent<CanvasGroup>();  
                } 
                
                canvasGroup.alpha = 0;  
                listComponentUsingEffect[i].localScale = Vector3.zero; 
            
                // Gọi phương thức animate từng item với hiệu ứng domino  
            }
            
            for (int i = 0; i < listComponentUsingEffect.Count; i++)
            {
                AnimateItem(listComponentUsingEffect[i], i);
            }
        }
        
        private void AddEffectShowItem()
        {
            for (int i = 0; i < listSkin.Count; i++)
            {
                CanvasGroup canvasGroup = listSkin[i].GetComponent<CanvasGroup>();  
                if (canvasGroup == null)  
                {  
                    canvasGroup = listSkin[i].AddComponent<CanvasGroup>();  
                } 
                
                canvasGroup.alpha = 0;  
                listSkin[i].GetComponent<RectTransform>().localScale = Vector3.zero; 
            
                // Gọi phương thức animate từng item với hiệu ứng domino  
                AnimateItem(listSkin[i].GetComponent<RectTransform>(), i);
            }

        }
        
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

        private void AddEffectShowBackGround()
        {
            if (!bgListItem.gameObject.GetComponent<CanvasGroup>())
                bgListItem.gameObject.AddComponent<CanvasGroup>();
            var canvasGroup = bgListItem.GetComponent<CanvasGroup>();
            //bgListItem
            RectTransform rectTransformFullFly = bgListItem.GetComponent<RectTransform>();
                    
            Vector3 originalPositionFullFly = rectTransformFullFly.anchoredPosition;
                    
            Vector3 startPositionFullFly = originalPositionFullFly - new Vector3(0, Screen.height / 4, 0);
            rectTransformFullFly.anchoredPosition = startPositionFullFly;
                    
            bgListItem.gameObject.SetActive(true);
                    
            Sequence showSequenceFullFly = DOTween.Sequence();
                    
            showSequenceFullFly.Append(canvasGroup.DOFade(1f, 0.2f).SetEase(Ease.OutQuad)); 
            showSequenceFullFly.Join(rectTransformFullFly.DOAnchorPosY(originalPositionFullFly.y, 0.2f).SetEase(Ease.OutQuad));

        }
        
    }
}

