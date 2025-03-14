using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using TwistedTangle.HieuUI;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class BuildBedRoomPopup : BasePopup
    {
        [SerializeField] public RawImage rawImage;
        [SerializeField] public GameObject border1;
        [SerializeField] public GameObject border2;
        [SerializeField] public GameObject border3;
        [SerializeField] public GameObject star1Txt;
        [SerializeField] public GameObject star2Txt;
        [SerializeField] public GameObject star3Txt;
        [SerializeField] public GameObject money1Txt;
        [SerializeField] public GameObject money2Txt;
        [SerializeField] public GameObject money3Txt;
        [SerializeField] private List<GameObject> imagesList;
        [SerializeField] private List<GameObject> imagesListMap2;
        [SerializeField] private RectTransform buttonContainer1;
        [SerializeField] private RectTransform buttonContainer2;
        [SerializeField] private RectTransform buttonContainer3;
        [SerializeField] private GameObject useGemBtn;

        public List<GameObject> bedRoomModelsList;
        public List<int> typeList;

        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> listBtnAdsReward;
        [SerializeField] private Image iconAdsReward;
        [SerializeField] private SticketMinusTxt sticketMinusTxt;

        public override void Show()
        {
            var content = transform.Find("Content");
            ShowEffect(YunEffectType.FlyUpEffect, content.transform);
        }

        protected override void Awake()
        {
            base.Awake();
            
            useGemBtn.transform.Find("Shadow_Text").GetComponent<YunTextShadow>()
                    .DisplayText =
                FacilityManager.Instance.PlayerConfig.GemCostForSkipAds.ToString();

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                iconAdsReward.sprite = listBtnAdsReward[1];
            }

        }

        public void OnUseGemBtnClick()
        {
            if (FacilityManager.Instance.IdleGameData.Gem < FacilityManager.Instance.PlayerConfig.GemCostForSkipAds)
            {
                FacilityManager.Instance.ShowNotEnoughGemPopup();
                return;
            }

            Close();
            FacilityManager.Instance.OnSelectBedRoomType(typeList[2], true);
            FacilityManager.Instance.IdleGameData.Gem -= FacilityManager.Instance.PlayerConfig.GemCostForSkipAds;
            FacilityManager.Instance.PlayerInfoUI.UpdateGem(FacilityManager.Instance.IdleGameData.Gem, false);
        }

        public void Init(BaseRoom room)
        {
            rawImage.gameObject.SetActive(true);
            foreach (var model in bedRoomModelsList)
            {
                model.SetActive(false);
                // model.transform.localPosition = new Vector3(15, 0, 0);
            }

            bedRoomModelsList[typeList[0]].transform.localPosition = new Vector3(3, 0, 0);
            bedRoomModelsList[typeList[0]].SetActive(true);

            HightlightView(0);

            // DOVirtual.DelayedCall(0.5f, (() => { HightlightView(0); })).SetAutoKill(true);

            foreach (var image in imagesList)
            {
                image.SetActive(false);
            }

            foreach (var image in imagesListMap2)
            {
                image.SetActive(false);
            }

            var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
            if (room.IsBuilt)
            {
                star1Txt.GetComponent<YunTextShadow>().DisplayText = "+" + room.stars[1].ToString();
                star2Txt.GetComponent<YunTextShadow>().DisplayText = "+" + room.stars[1].ToString();
                star3Txt.GetComponent<YunTextShadow>().DisplayText = "+" + (room.stars[1] * 2).ToString();

                money1Txt.GetComponent<YunTextShadow>().DisplayText = "x" + room.outcomes[1].ToString();
                money2Txt.GetComponent<YunTextShadow>().DisplayText = "x" + room.outcomes[1].ToString();
                money3Txt.GetComponent<YunTextShadow>().DisplayText = "x" + (room.outcomes[1] * 2).ToString();

                if (mapIndex == 1)
                {
                    imagesList[3].SetActive(true);
                    imagesList[4].SetActive(true);
                    imagesList[5].SetActive(true);
                }
                else if (mapIndex == 2)
                {
                    imagesListMap2[3].SetActive(true);
                    imagesListMap2[4].SetActive(true);
                    imagesListMap2[5].SetActive(true);
                }
            }
            else
            {
                star1Txt.GetComponent<YunTextShadow>().DisplayText = "+" + room.stars[0].ToString();
                star2Txt.GetComponent<YunTextShadow>().DisplayText = "+" + room.stars[0].ToString();
                star3Txt.GetComponent<YunTextShadow>().DisplayText = "+" + (room.stars[0] * 2).ToString();

                money1Txt.GetComponent<YunTextShadow>().DisplayText = "x" + room.outcomes[0].ToString();
                money2Txt.GetComponent<YunTextShadow>().DisplayText = "x" + room.outcomes[0].ToString();
                money3Txt.GetComponent<YunTextShadow>().DisplayText = "x" + (room.outcomes[0] * 2).ToString();

                if (mapIndex == 1)
                {
                    imagesList[0].SetActive(true);
                    imagesList[1].SetActive(true);
                    imagesList[2].SetActive(true);
                }
                else if (mapIndex == 2)
                {
                    imagesListMap2[0].SetActive(true);
                    imagesListMap2[1].SetActive(true);
                    imagesListMap2[2].SetActive(true);
                }
            }
        }

        private GameObject _currentModel;

        public void HightlightView(int viewType)
        {
            var duration = 1f;
            buttonContainer1.DOKill();
            buttonContainer2.DOKill();
            buttonContainer3.DOKill();

            border1.SetActive(false);
            border2.SetActive(false);
            border3.SetActive(false);

            foreach (var model in bedRoomModelsList)
            {
                model.transform.DOKill();
            }

            var duration2 = 1f;
            if (_currentModel)
            {
                var previousModel = _currentModel;
                previousModel.transform.DOLocalMoveX(-10f, duration2).SetEase(Ease.OutBack).OnComplete((() =>
                {
                    previousModel.SetActive(false);
                }));
            }

            _currentModel = bedRoomModelsList[typeList[viewType]];
            _currentModel.transform.localPosition = new Vector3(3, 0, 0);
            _currentModel.SetActive(true);
            _currentModel.transform.DOLocalMoveX(0, duration2).SetEase(Ease.OutBack);

            switch (viewType)
            {
                case 0:
                    // buttonContainer1.DOLocalMoveY(45, duration).SetEase(Ease.OutBack);
                    // buttonContainer2.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
                    // buttonContainer3.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
                    border1.SetActive(true);
                    break;
                case 1:
                    // buttonContainer1.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
                    // buttonContainer2.DOLocalMoveY(45, duration).SetEase(Ease.OutBack);
                    // buttonContainer3.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
                    border2.SetActive(true);
                    break;
                case 2:
                    // buttonContainer1.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
                    // buttonContainer2.DOLocalMoveY(0, duration).SetEase(Ease.OutBack);
                    // buttonContainer3.DOLocalMoveY(45, duration).SetEase(Ease.OutBack);
                    border3.SetActive(true);
                    break;
            }
        }

        public void Build1()
        {
            FacilityManager.Instance.OnSelectBedRoomType(typeList[0]);
            Close();
        }

        public void Build2()
        {
            FacilityManager.Instance.OnSelectBedRoomType(typeList[1]);
            Close();
        }

        public void Build3()
        {
            sticketMinusTxt.OnInit();
            FacilityManager.Instance.OnSelectBedRoomType(typeList[2]);
            Close();
        }

        public override void Close()
        {
            CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeToBottom);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
        }
    }
}