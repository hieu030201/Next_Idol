using System.Collections;
using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using I2.MiniGames;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using TMPro;
using TwistedTangle.HieuUI;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;
using Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class SpinWheelPopup : BasePopup
    {
        [SerializeField] private GameObject itemObject;
        // [SerializeField] private GameObject result;
        // [SerializeField] private GameObject containerResult;
        [SerializeField] private Transform wheelElement;
        //[SerializeField] private List<GameObject> prizePart;
        [SerializeField] private List<GameObject> listSeparators = new();
        [SerializeField] private GameObject btnTicket;
        
        [SerializeField] [CanBeNull] private MiniGame_Controller miniGame;
        private readonly List<GameObject> listItem = new();
        private SkinData _skinItemReward;
        [SerializeField] private PrizeWheel prizeWheel;
        [SerializeField] private GameObject freeBtn;
        [SerializeField] private GameObject adsBtn;
        [SerializeField] private GameObject gemBtn;
        [SerializeField] private GameObject textChange;

        [SerializeField] private GameObject rewardPopup;
        [SerializeField] private GameObject ContentReward;
        [SerializeField] private GameObject containerResult;

        [SerializeField] private GameObject freeGroup;
        [SerializeField] private UIButton btnClose;

        [SerializeField] private GameObject notFreeGroup;
        [SerializeField] private ParticleSystem lightWarm;
        
        [SerializeField] private Image iconBtnRewardAds;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> listIconBtnRewardAds;
        [SerializeField] private SticketMinusTxt sticketMinusTxt;
        //private ulong checkedTime;
        private Image iconSkin;
        
        private Tween tween;

        public void Awake()
        {
            OnInit();
        }
        private void OnEnable()
        {
            prizeWheel._OnFinishSpinning.AddListener(ReferencesRewardSpinWheel);
            rewardPopup.SetActive(false);
            ContentReward.GetComponent<CanvasGroup>().alpha = 0;
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.freeSpinAmount <= 0)
            {
                ShowBtn(freeBtn,false);
                ShowBtn(adsBtn,true);
                ShowGemBtn();
                ShowBtn(textChange,true);
            }
            else
            {
                freeBtn.SetActive(true);
                ShowBtn(adsBtn,false);
                ShowBtn(gemBtn, false);
                ShowBtn(textChange,false);
            }

            if(FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
            }else if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip &&
                      FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds <= 0)
            {
                iconBtnRewardAds.sprite = listIconBtnRewardAds[0];
            }
        }

        public void OnInit()
        {
            if (listItem == null || listItem.Count > 0) return;
            var quantity = FacilityManager.Instance.SpinWheelDataCollection.Count;
            for (int i = 0; i < quantity; i++)
            {
                var indexObj = FacilityManager.Instance.SpinWheelDataCollection[i];
                
                var obj = Instantiate(itemObject, wheelElement, true);
                obj.transform.localScale = Vector3.one;
                listItem.Add(obj);
                obj.name = indexObj.name;
                var item = obj.GetComponent<SpinWheelItem>();
                
                item.ID = indexObj.id;
                item.name = indexObj.name;
                item.spinRewardType = indexObj.spinRewardType;
                item.icon.sprite = indexObj.icon;
               
                if (item.spinRewardType == SpinRewardType.Skin)
                {
                    //FacilityManager.Instance.GameSaveLoad.GameData.idSkinInSpin;
                    //RandomItemSkin();
                    var idItem = FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinInSpin;
                    var itemSkin =  FacilityManager.Instance.SkinDataCollection.Value.Find(s => s.id == idItem);
                    item.icon.sprite = itemSkin.icon;
                    item.icon.transform.localScale = new Vector3(0.75f, 0.75f, 1);  
                }
                if (item.spinRewardType == SpinRewardType.Gem)
                {
                    item.icon.transform.localScale = new Vector3(0.75f, 0.75f, 1);  
                }
                if (item.spinRewardType == SpinRewardType.Token)
                {
                    item.icon.transform.localScale = new Vector3(1f, 1f, 1);  
                }
                item.icon.SetNativeSize();
                //item.bg.color = GetColorFromString(indexObj.hexCode);
                item.amount.text = indexObj.amount.ToString();
                if (i % 2 == 1)
                {
                    if (ColorUtility.TryParseHtmlString("#D52027", out Color colorNew))  
                    {  
                        item.amount.color = colorNew;  
                    } 
                }
                var reward = obj.GetComponent<PrizeWheel_Reward>();
                reward._Separator = listSeparators[i].GetComponent<RectTransform>();
                reward.Probability = indexObj.probability;
            }
        }

        // public void RandomItemSkin()
        // {
        //     _skinItemReward = FacilityManager.Instance.SkinDataCollection.GetItemRewardNotUnlock();
        //     FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinInSpin = _skinItemReward.id;
        // }

        private void ReferencesRewardSpinWheel(int id)
        {
            var item = FacilityManager.Instance.SpinWheelDataCollection[id];
            DOVirtual.DelayedCall(1.0f, () =>
            {
                ShowUIReward(item);
            });
        }

        private void ShowUIReward(SpinWheelData itemReward)
        {
            freeGroup.SetActive(false);
            notFreeGroup.SetActive(false);
            rewardPopup.SetActive(true);
            DOVirtual.DelayedCall(0.5f,()=> lightWarm.Play());
            rewardPopup.GetComponent<SpinWheelReward>().SetData(itemReward);
            ContentReward.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
            containerResult.GetComponent<RectTransform>().DOScale(1f, 1.1f).SetEase(Ease.OutBack);
        }

        #region BUTTON STATE
        public void OnClickBtnFree()
        {
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.freeSpinAmount <=0) return;
            
            FireBaseManager.Instance.LogEvent(FireBaseManager.SPIN_WHEEL_FREE);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            SetInnactive(freeBtn);
            StartValidateAround();
            FacilityManager.Instance.GameSaveLoad.StableGameData.freeSpinAmount--;
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.freeSpinAmount <=0)
            {
                FacilityManager.Instance.PlayerInfoUI.HideSpinNotification();
                SetInnactive(adsBtn);
                SetInnactive(gemBtn);
                ShowNoFreeGroup();
                FacilityManager.Instance.PlayerInfoUI.StartCountDownForSpinWheel();
            }
        }
        private CanvasGroup _canvasFree;
        public void ShowNoFreeGroup()
        {
            freeBtn.SetActive(false);
            //_canvasFree = freeGroupBtn.GetComponent<CanvasGroup>();
            ShowBtn(adsBtn, true);
            ShowBtn(textChange, true);
            ShowGemBtn();
        }

        public void GetStateButtonSpin()
        {
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.freeSpinAmount <=0) 
                notFreeGroup.SetActive(true);
            else
            {
                freeGroup.SetActive(true);
            }
        }

        public void ShowBtn(GameObject obj, bool active)
        {
            obj.SetActive(active);
        }
        
        public void ShowGemBtn()
        {
            gemBtn.SetActive(true);
            if (FacilityManager.Instance.GameSaveLoad.GameData.gem < 3)
            {
                var _canvasGem = gemBtn.GetComponent<CanvasGroup>();
                _canvasGem.alpha = 0.7f;
                _canvasGem.interactable = false;
            }else if (FacilityManager.Instance.GameSaveLoad.GameData.gem >= 3)
            {
                var _canvasGem = gemBtn.GetComponent<CanvasGroup>();
                _canvasGem.alpha = 1f;
                _canvasGem.interactable = true;
            }
        }

        public void OnOutOfTurnTicket()
        {
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds <= 0)
            {
                iconBtnRewardAds.sprite = listIconBtnRewardAds[0];
            }
        }
        
        public void OnClickBtnAdsReward()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
            sticketMinusTxt.OnInit();
            AdsManager.Instance.ShowReward(StartValidateAround, AdsManager.RewardType.SPIN_WHEEL);
            OnOutOfTurnTicket();
            rewardPopup.GetComponent<SpinWheelReward>().OnOutOfTurnTicket();
        }

        public void OnClickBtnGameReward()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
           
            var countGem = FacilityManager.Instance.GameSaveLoad.GameData.gem;
            if (countGem >= 3)
            {
         
                StartValidateAround();
                FacilityManager.Instance.GameSaveLoad.GameData.gem -= 3;
                FacilityManager.Instance.PlayerInfoUI.UpdateGem(FacilityManager.Instance.IdleGameData.Gem, false);
                FireBaseManager.Instance.LogEvent(FireBaseManager.SPIN_WHEEL_GEM);
            }
            else
            {
                //TODO: Show popup Shop hoặc thiếu gem
            }
        }

        public void StartValidateAround()
        {
            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }
            SetInnactive(gemBtn);
            SetInnactive(adsBtn);
            btnClose.interactable = false;
            miniGame.ValidateRound();
            DOVirtual.DelayedCall(0f, () =>
            {
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Spin_Wheel_2);
            });
            
            DOVirtual.DelayedCall(8f, () =>   
            {  
                btnClose.interactable = true;   
            });  
        }

        #endregion

        #region CHANGE COLOR
        private int HexToDec(string hex) {
            int dec = System.Convert.ToInt32(hex, 16);
            return dec;
        }

        private string DecToHex(int value) {
            return value.ToString("X2");
        }

        private string FloatNormalizedToHex(float value) {
            return DecToHex(Mathf.RoundToInt(value * 255f));
        }

        private float HexToFloatNormalized(string hex) {
            return HexToDec(hex) / 255f;
        }

        public Color GetColorFromString(string hexString) {
            float red = HexToFloatNormalized(hexString.Substring(0, 2));
            float green = HexToFloatNormalized(hexString.Substring(2, 2));
            float blue = HexToFloatNormalized(hexString.Substring(4, 2));
            float alpha = 1f;
            if (hexString.Length >= 8) {
                alpha = HexToFloatNormalized(hexString.Substring(6, 2));
            }
            return new Color(red, green, blue, alpha);
        }

        public string GetStringFromColor(Color color, bool useAlpha = false) {
            string red = FloatNormalizedToHex(color.r);
            string green = FloatNormalizedToHex(color.g);
            string blue = FloatNormalizedToHex(color.b);
            if (!useAlpha) {
                return red + green + blue;
            } else {
                string alpha = FloatNormalizedToHex(color.a);
                return red + green + blue + alpha;
            }
        }
        
        #endregion
        
        private void OnDisable()
        {
            if (prizeWheel != null)
            {
                prizeWheel._OnFinishSpinning.RemoveListener(ReferencesRewardSpinWheel);
            }
        }

        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeIn);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
            FacilityManager.Instance.PlayerInfoUI.HideSpinWheelPopup();
        }

        public void SetInnactive(GameObject obj)
        {
            var button = obj.GetComponent<Button>();
            button.interactable = false;
            DOVirtual.DelayedCall(8.0f, ()=>
            {
                button.interactable = true;
            });
        }
    }

}


