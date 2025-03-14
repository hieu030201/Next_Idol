using System;
using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using TwistedTangle.HieuUI;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Ads;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class MonetizationPopup : BasePopup
    {
        protected GameObject NativeAds;
        private RawImage _rawImage;
        protected Transform RootTransform;
        [SerializeField] protected SticketMinusTxt sticketMinusTxt;
        [SerializeField] protected Image iconBtnRewardAds;

        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        protected List<Sprite> listIconBtnRewardAds;

        private Vector3 _initialScaleRawImg;

        private GameObject _useGemBtn;
        private GameObject _viewAdsBtn;
        private GameObject _freeBtn;

        protected override void Awake()
        {
            base.Awake();

            RootTransform = transform.Find("Content").transform;

            if (RootTransform.Find("NativeAdsLoading"))
                NativeAds = RootTransform.Find("NativeAdsLoading").gameObject;

            if (RootTransform.Find("Content").Find("RawImage"))
                _rawImage = RootTransform.Find("Content").Find("RawImage").GetComponent<RawImage>();
            
            if (RootTransform.Find("Content").Find("Use_Money_Btn"))
            {
                RootTransform.Find("Content").Find("Use_Money_Btn").GetComponent<UIButton>().onClick
                    .AddListener(OnUseMoneyBtnClick);
            }

            if (RootTransform.Find("Content").Find("Use_Gem_Btn"))
            {
                _useGemBtn = RootTransform.Find("Content").Find("Use_Gem_Btn").gameObject;
                RootTransform.Find("Content").Find("Use_Gem_Btn").GetComponent<UIButton>().onClick
                    .AddListener(OnUseGemBtnClick);
                RootTransform.Find("Content").Find("Use_Gem_Btn").transform.Find("Shadow_Text")
                        .GetComponent<YunTextShadow>()
                        .DisplayText =
                    FacilityManager.Instance.PlayerConfig.GemCostForSkipAds.ToString();
            }

            if (RootTransform.Find("Content").Find("View_Ads_Btn"))
            {
                _viewAdsBtn = RootTransform.Find("Content").Find("View_Ads_Btn").gameObject;
                RootTransform.Find("Content").Find("View_Ads_Btn").GetComponent<UIButton>().onClick
                    .AddListener(OnViewAdsBtnClick);
            }

            if (RootTransform.Find("Content").Find("Free_Btn"))
            {
                _freeBtn = RootTransform.Find("Content").Find("Free_Btn").gameObject;
                RootTransform.Find("Content").Find("Free_Btn").GetComponent<UIButton>().onClick
                    .AddListener(OnFreeBtnClick);
            }

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip &&
                FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
            }

            if (_rawImage)
            {
                _initialScaleRawImg = _rawImage.GetComponent<RectTransform>().localScale;
                _rawImage.gameObject.SetActive(true);
                _rawImage.gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
            }
        }

        public override void Show()
        {
            base.Show();
            _isClickedAds = false;
            if (!_rawImage) return;
            var uiElement = _rawImage.gameObject.GetComponent<RectTransform>();
            DOVirtual.DelayedCall(0.15f, () =>
            {
                const float scaleDuration = 0.25f;
                const float bounceDuration = 0.2f;
                uiElement.DOScale(_initialScaleRawImg, scaleDuration)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() =>
                    {
                        var scaleSequence = DOTween.Sequence();

                        scaleSequence
                            .Append(uiElement.DOScale(_initialScaleRawImg, bounceDuration)
                                .SetEase(Ease.InOutQuad));
                    });
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (NativeAds && NativeAds.activeSelf)
                NativeAds.GetComponent<NativeManager>().ShowNativeLoading(false);

            Advertisements.Instance.ShowBanner();
            AdsManager.Instance.HideMrec();
        }

        private Action _callback;
        private AdsManager.RewardType _rewardType;

        public void SetData(Action callBack, AdsManager.RewardType rewardType, bool isShowMrec, bool isShowNativeAds,
            bool isFirstTime)
        {
            _callback = callBack;
            _rewardType = rewardType;

            if (_viewAdsBtn)
                _viewAdsBtn.SetActive(!isFirstTime);
            if (_useGemBtn)
                _useGemBtn.SetActive(!isFirstTime);
            if (_freeBtn)
                _freeBtn.SetActive(isFirstTime);

            if (NativeAds)
                NativeAds.SetActive(isShowNativeAds);

            if (!isShowMrec) return;
            Advertisements.Instance.HideBanner();
            AdsManager.Instance.ShowMrec();
        }

        private void OnUseGemBtnClick()
        {
            if (FacilityManager.Instance.IdleGameData.Gem < FacilityManager.Instance.PlayerConfig.GemCostForSkipAds)
            {
                FacilityManager.Instance.ShowNotEnoughGemPopup();
                return;
            }

            Close();
            _callback?.Invoke();
            FacilityManager.Instance.IdleGameData.Gem -= FacilityManager.Instance.PlayerConfig.GemCostForSkipAds;
            FacilityManager.Instance.PlayerInfoUI.UpdateGem(FacilityManager.Instance.IdleGameData.Gem);
        }

        protected int MoneyCostForSkipAds;

        private void OnUseMoneyBtnClick()
        {
            if (FacilityManager.Instance.IdleGameData.Money < MoneyCostForSkipAds)
            {
                FacilityManager.Instance.ShowNotEnoughCurrencyToRepairPopup(MoneyCostForSkipAds - FacilityManager.Instance.IdleGameData.Money);
                Close();
                return;
            }

            Close();
            _callback?.Invoke();
            FacilityManager.Instance.IdleGameData.Money -= MoneyCostForSkipAds;
            FacilityManager.Instance.PlayerInfoUI.UpdateMoney(FacilityManager.Instance.IdleGameData.Money);
        }

        private bool _isClickedAds;

        protected virtual void OnViewAdsBtnClick()
        {
            if (_isClickedAds)
                return;

            if (_callback != null)
            {
                sticketMinusTxt.OnInit();
                AdsManager.Instance.ShowReward(_callback, _rewardType);
            }

            _isClickedAds = true;
            Close();
        }

        private void OnFreeBtnClick()
        {
            _callback?.Invoke();
            Close();
        }

        protected GameObject RenderModel;

        public void SetRenderModel(GameObject renderModel)
        {
            if (!_rawImage) return;
            RenderModel = renderModel;
            RenderModel.SetActive(true);
            RenderModel.GetComponent<Model3DRenderer>().StartRender(_rawImage);
        }

        public override void Close()
        {
            if (RenderModel)
                RenderModel.SetActive(false);
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeToBottom);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
        }
    }
}