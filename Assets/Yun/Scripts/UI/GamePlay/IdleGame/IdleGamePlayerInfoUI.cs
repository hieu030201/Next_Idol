using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using ExaGames.Common;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.Ads;
using Yun.Scripts.Animations;
using Yun.Scripts.Audios;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;
using Yun.Scripts.Utilities;
using SkinRentalCountdownUI = Yun.Scripts.UI.GamePlay.IdleGame.SkinRentalCountdownUI;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class IdleGamePlayerInfoUI : BaseUI
    {
        [SerializeField] private TextMeshProUGUI androidVersionText;

        [SerializeField] private TextMeshProUGUI moneyTxt,
            tokenTxt,
            gemTxt,
            sticketTxt,
            battleHpTxt,
            levelTxt,
            guideTxt,
            levelPassTxt,
            metaTagTxt;

        [SerializeField] private Image iconBattlePassFill;
        [SerializeField] private YunTextShadow levelProgressTxt;
        [SerializeField] private YunProgressBar levelProgressChild;
        [SerializeField] private GameObject riffleEffectPrefab;
        [SerializeField] private GameObject cashEffectPrefab;
        [SerializeField] private GameObject starEffectPrefab;
        [SerializeField] private GameObject gemEffectPrefab;
        [SerializeField] private GameObject tokenEffectPrefab;
        [SerializeField] private List<GameObject> cashPositionList;
        [SerializeField] private GameObject cashEffectDestination;
        [SerializeField] private GameObject starEffectDestination;
        [SerializeField] private GameObject gemEffectDestination;
        [SerializeField] private GameObject tokenEffectDestination;
        [SerializeField] private GameObject riffleEffectDestination;
        [SerializeField] private GameObject battleResult;
        [SerializeField] private GameObject dailyQuestBtn;
        [SerializeField] private GameObject navigationIcon;
        [SerializeField] private DesertCountdown desertCountdown;
        [SerializeField] public GameObject shopIconObj;
        [SerializeField] public GameObject battlePassObj;
        [SerializeField] public GameObject skinIconObj;
        [SerializeField] public GameObject spinIconObj;
        [SerializeField] public GameObject sticketIconObj;
        [SerializeField] private FlutterAnimation navigationChild;
        [SerializeField] public RectTransform arrowRectTransform;
        [SerializeField] public GameObject navigationArrow;
        [SerializeField] public GameObject shopNotification;
        [SerializeField] public GameObject dailyQuestRedNotification;
        [SerializeField] public GameObject battlePassNotification;
        [SerializeField] public GameObject spinNotification;
        [SerializeField] public GameObject skinNotification;
        [SerializeField] private GameObject navigationNotification;
        [SerializeField] private TextMeshProUGUI navigationTxt;
        [SerializeField] private YunProgressBar battleProgressBar;
        [SerializeField] private Button startBattleBtn;
        [SerializeField] private GameObject topContainer;


        [SerializeField] private SpinWheelCountDownUI spinWheelCountDown;
        [SerializeField] private SkinRentalCountdownUI skinRetalCountDown;
        [SerializeField] private LivesManagerSkin timerSkinInIdleGame;
        [SerializeField] private GameObject battleMilestoneIconPrefab;
        [SerializeField] private GameObject bubbleCashPrefab;
        [SerializeField] private SoldierMoving2D movingSoldierIcon;
        [SerializeField] private GameObject shopBg;
        [SerializeField] private GameObject skinPopupBg;
        [SerializeField] private GameObject battlePassPopupBg;
        [SerializeField] private GameObject spinWheelPopupBg;
        [SerializeField] private GameObject battleFundPopupBg;
        [SerializeField] private GameObject abilitiesPopupBg;
        [SerializeField] private GameObject shopHeaderBg;
        [SerializeField] private GameObject countSoldierTutorial;
        [SerializeField] private GameObject changeMapTutorial;
        [SerializeField] private GameObject SpinWheelTutorial;
        [SerializeField] private GameObject BattleFundTutorial;
        [SerializeField] private GameObject startBattleTutorial;
        [SerializeField] private GameObject playerSkinTutorial;
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject soldierCountBtn;
        [SerializeField] private FadeInOutAnimation battleProgressHighlight;
        [SerializeField] private GameObject logContainer;
        [SerializeField] private GameObject logScroll;
        [SerializeField] private GameObject logBtn;

        [SerializeField] private GameObject logPrefab;

        //[SerializeField] private GameObject noAdsBtn;
        [SerializeField] private GameObject nativeLoading;
        [SerializeField] private GameObject testBtn;
        [SerializeField] private GameObject changeMapBtn;
        [SerializeField] private GameObject moneyBtn;

        private BaseCountDownUI _speedBoosterCountdown;
        private BaseCountDownUI _workerSpeedBoosterCountdown;
        private BaseCountDownUI _upgradeWorkerCountdown;
        private BaseCountDownUI _vipSoldierCountdown;

        public UnityAction SettingButtonClick;

        public bool IsLocked { get; set; }

        [Button]
        public void TestBatlleProgressBar()
        {
            battleProgressBar.UpdateProgress(0.5f);
        }

        protected override void Awake()
        {
            base.Awake();

            var top = GameObject.Find("Top_Container");
            var countDown = top.transform.Find("Count_Down");
            _speedBoosterCountdown =
                countDown.transform.Find("Speed_Booster_Count_Down_BG").GetComponent<BaseCountDownUI>();
            _workerSpeedBoosterCountdown = countDown.transform.Find("Worker_Speed_Booster_Count_Down_BG")
                .GetComponent<BaseCountDownUI>();
            _upgradeWorkerCountdown =
                countDown.transform.Find("Upgrade_Worker_Count_Down_BG").GetComponent<BaseCountDownUI>();
            _vipSoldierCountdown =
                countDown.transform.Find("Vip_Soldier_Count_Down_BG").GetComponent<BaseCountDownUI>();

            var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
            if (mapIndex == 1)
                HideChangeMapBtn();

            logBtn.SetActive(FacilityManager.Instance.testGameConfig.isShowLog);
            logBtn.SetActive(FacilityManager.Instance.testGameConfig.isShowLogPrivate);
            logContainer.SetActive(false);
            logPrefab.SetActive(false);
            battleProgressHighlight.gameObject.SetActive(false);
            battleResult.SetActive(false);
            startBattleBtn.gameObject.SetActive(false);
            _speedBoosterCountdown.gameObject.SetActive(false);
            _workerSpeedBoosterCountdown.gameObject.SetActive(false);
            _upgradeWorkerCountdown.gameObject.SetActive(false);
            _vipSoldierCountdown.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            countSoldierTutorial.SetActive(false);
            changeMapTutorial.SetActive(false);
            SpinWheelTutorial.SetActive(false);
            BattleFundTutorial.SetActive(false);
            startBattleTutorial.SetActive(false);
            playerSkinTutorial.SetActive(false);
            shopIconObj.SetActive(false);
            battlePassObj.SetActive(false);
            skinIconObj.SetActive(false);
            spinIconObj.SetActive(false);

            HideMoneyBtn();
            HideSpeedBoosterBtn();
            HideWorkerSpeedBoosterBtn();
            HideUpgradeWorkerBtn();
            HideVipSoldierBtn();

            HideShopBg();
            HideSkinPopupBg();
            HideBattlePassPopupBg();
            HideBattleFundPopupBg();
            HideAbilitiesPopupBg();
            HideSpinWheelPopup();

            battleHpTxt.text = "";

            moneyTxt.text = "";
            levelTxt.text = "";
            levelProgressTxt.DisplayText = "";

            guideTxt.gameObject.SetActive(false);

            dailyQuestRedNotification.SetActive(false);
            shopNotification.SetActive(false);
            battlePassNotification.SetActive(false);
            spinNotification.SetActive(false);
            skinNotification.SetActive(false);
            HideNavigation();

            // if(nativeLoading && FireBaseManager.Instance && !FireBaseManager.Instance.showNativeGetMoneyPopup)
            nativeLoading.SetActive(false);

            testBtn.SetActive(false);
        }

        public void ShowMoneyBtn()
        {
            moneyBtn.SetActive(true);
        }

        public void HideMoneyBtn()
        {
            moneyBtn.SetActive(false);
        }

        public void HideChangeMapBtn()
        {
            changeMapBtn.SetActive(false);
        }

        public void ShowChangeMapBtn()
        {
            changeMapBtn.SetActive(true);
        }

        public void Start()
        {
            // Debug.Log("StableGameData.isBoughtNoAdsVip: " + FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip);
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip != false)
            {
                sticketIconObj.SetActive(true);
                UpdateSticketRemoveAdsReward();
            }
            else
                sticketIconObj.SetActive(false);

            if (FacilityManager.Instance.GameSaveLoad.GameData.battlePassLevel > 0)
            {
                int level = FacilityManager.Instance.GameSaveLoad.GameData.battlePassLevel;
                float levelPercentage = FacilityManager.Instance.GameSaveLoad.GameData.levelPercentageBattlePass;
                battlePassObj.SetActive(true);
                UpdateLevelBattlePassProgress(level, levelPercentage);
            }

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveSkinShop)
                skinIconObj.SetActive(true);

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveSpin)
                spinIconObj.SetActive(true);

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveBattlePass)
            {
                battlePassObj.SetActive(true);
            }

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveShop)
            {
                shopIconObj.SetActive(true);
            }

            if (!FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveRetal)
            {
                skinRetalCountDown.countDownObj.SetActive(false);
            }

            shopNotification.SetActive(FacilityManager.Instance.GameSaveLoad.StableGameData.freeInShopAmount > 0);
            spinNotification.SetActive(FacilityManager.Instance.GameSaveLoad.StableGameData.freeSpinAmount > 0);
            var gameData = FacilityManager.Instance.GameSaveLoad.GameData;
            //var stableGameData = FacilityManager.Instance.GameSaveLoad.StableGameData;  

            int levelCurrent = gameData.battlePassLevel;
            bool isFreePassAvailable = false;
            bool isHeroPassAvailable = false;
            bool isLegendPassAvailable = false;

            if (gameData.listIDBattlePassFree.Count > 0)
            {
                isFreePassAvailable = gameData.listIDBattlePassFree.Max() - 1 < levelCurrent;
            }

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassHero)
            {
                if (gameData.listIDBattlePassHero.Count > 0)
                {
                    isHeroPassAvailable = gameData.listIDBattlePassHero.Max() - 1 < levelCurrent;
                }

                if (gameData.listIDBattlePassHero.Count < 2)
                {
                    isHeroPassAvailable = true;
                }
            }

            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassLegend)
            {
                if (gameData.listIDBattlePassLegend.Count > 0)
                {
                    isLegendPassAvailable = gameData.listIDBattlePassLegend.Max() - 1 < levelCurrent;
                }

                if (gameData.listIDBattlePassLegend.Count < 2)
                {
                    isLegendPassAvailable = true;
                }
            }

            battlePassNotification.SetActive(isFreePassAvailable || isHeroPassAvailable || isLegendPassAvailable);

            androidVersionText.text = SystemInfo.operatingSystem;
            for (var i = FacilityManager.Instance.GameSaveLoad.GameData.logList.Count - 1; i >= 0; i--)
            {
                if (FacilityManager.Instance.testGameConfig.isShowLog)
                    AddLog(FacilityManager.Instance.GameSaveLoad.GameData.logList[i], false);
            }

            timerSkinInIdleGame.MinutesToRecover =
                FacilityManager.Instance.GameSaveLoad.StableGameData.minuteShowInIdleGameUI;
        }

        public void ShowTicketRemoveAdsVipInHeader()
        {
            sticketIconObj.SetActive(true);
            UpdateSticketRemoveAdsReward();
        }

        public GameObject content; // GameObject Content của ScrollRect
        public GameObject itemPrefab; // Prefab của phần tử cần thêm
        private List<GameObject> items = new(); // Danh sách lưu trữ các phần tử
        private const int maxItems = 30; // Số lượng tối đa các phần tử

        public void AddLog(string logStr, bool isSaveData = true)
        {
            var log = Instantiate(logPrefab, logScroll.transform);
            log.SetActive(true);
            log.transform.Find("Log_Txt").GetComponent<TextMeshProUGUI>().text = logStr;

            // Thêm vào đầu danh sách
            items.Insert(0, log);
            if (isSaveData)
                FacilityManager.Instance.GameSaveLoad.GameData.logList.Insert(0, logStr);

            // Đảm bảo phần tử mới nhất nằm trên cùng
            log.transform.SetAsFirstSibling();

            // Xóa phần tử cũ nhất nếu danh sách vượt quá 30
            if (items.Count > maxItems)
            {
                var oldestItem = items[^1];
                items.RemoveAt(items.Count - 1);
                if (isSaveData)
                    FacilityManager.Instance.GameSaveLoad.GameData.logList.RemoveAt(items.Count - 1);
                Destroy(oldestItem);
            }

            FacilityManager.Instance.GameSaveLoad.OrderToSaveData();
        }

        public void HideSoldierCountBtn()
        {
            soldierCountBtn.gameObject.SetActive(false);
        }

        public void ShowSoldierCountBtn()
        {
            soldierCountBtn.gameObject.SetActive(true);
        }

        private bool IsShowingTutorial { get; set; }

        public void ShowCountSoldierTutorial()
        {
            if (IsShowingTutorial)
                return;
            IsShowingTutorial = true;
            soldierCountBtn.gameObject.SetActive(true);
            countSoldierTutorial.SetActive(true);
            background.gameObject.SetActive(true);
        }

        public void ShowChangeMapTutorial()
        {
            if (IsShowingTutorial)
                return;
            IsShowingTutorial = true;
            changeMapTutorial.SetActive(true);
            background.gameObject.SetActive(true);
        }

        [Button]
        public void ShowSpinWheelTutorial()
        {
            if (IsShowingTutorial)
                return;
            IsShowingTutorial = true;
            SpinWheelTutorial.SetActive(true);
            background.gameObject.SetActive(true);
        }

        [Button]
        public void ShowBattleFundTutorial()
        {
            if (IsShowingTutorial)
                return;
            IsShowingTutorial = true;
            BattleFundTutorial.SetActive(true);
            background.gameObject.SetActive(true);
        }

        [Button]
        public void ShowPlayerSkinTutorial()
        {
            if (IsShowingTutorial)
                return;
            IsShowingTutorial = true;
            playerSkinTutorial.SetActive(true);
            background.gameObject.SetActive(true);
            ShowSkinRequestAnimation();
        }

        public void ShowStartBattleTutorial()
        {
            if (IsShowingTutorial)
                return;
            IsShowingTutorial = true;
            if (FireBaseManager.Instance.showNativeBattle)
                nativeLoading.SetActive(true);
            startBattleTutorial.SetActive(true);
            background.gameObject.SetActive(true);
            Advertisements.Instance.HideBanner();
            if (FireBaseManager.Instance.showMrecBattle)
                AdsManager.Instance.ShowMrec();
        }

        public void ShowShopBg()
        {
            shopBg.SetActive(true);
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(shopBg);
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                1,
                0.15f
            ).SetEase(Ease.Linear);
            shopHeaderBg.SetActive(true);
        }

        public void HideShopBg()
        {
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(shopBg);
            DOTween.To(
                    () => canvasGroup.alpha,
                    x => canvasGroup.alpha = x,
                    0f,
                    0.15f
                ).SetEase(Ease.Linear)
                .OnComplete(() => { shopBg.SetActive(false); });
            shopHeaderBg.SetActive(false);
        }

        public void ShowSkinPopupBg()
        {
            skinPopupBg.SetActive(true);
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(skinPopupBg);
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                1,
                0.15f
            ).SetEase(Ease.Linear);
            shopHeaderBg.SetActive(true);
        }

        public void HideSkinPopupBg()
        {
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(skinPopupBg);
            DOTween.To(
                    () => canvasGroup.alpha,
                    x => canvasGroup.alpha = x,
                    0f,
                    0.15f
                ).SetEase(Ease.Linear)
                .OnComplete(() => { skinPopupBg.SetActive(false); });

            shopHeaderBg.SetActive(false);
        }

        public void ShowBattlePassPopupBg()
        {
            battlePassPopupBg.SetActive(true);
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(battlePassPopupBg);
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                1,
                0.15f
            ).SetEase(Ease.Linear);
            shopHeaderBg.SetActive(true);
        }

        public void HideBattlePassPopupBg()
        {
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(battlePassPopupBg);
            DOTween.To(
                    () => canvasGroup.alpha,
                    x => canvasGroup.alpha = x,
                    0f,
                    0.15f
                ).SetEase(Ease.Linear)
                .OnComplete(() => { battlePassPopupBg.SetActive(false); });
            shopHeaderBg.SetActive(false);
        }

        public void ShowSpinWheelPopupBg()
        {
            spinWheelPopupBg.SetActive(true);
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(spinWheelPopupBg);
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                1,
                0.15f
            ).SetEase(Ease.Linear);
            shopHeaderBg.SetActive(true);
        }

        public void HideSpinWheelPopup()
        {
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(spinWheelPopupBg);
            DOTween.To(
                    () => canvasGroup.alpha,
                    x => canvasGroup.alpha = x,
                    0f,
                    0.15f
                ).SetEase(Ease.Linear)
                .OnComplete(() => { spinWheelPopupBg.SetActive(false); });
            shopHeaderBg.SetActive(false);
        }

        public void ShowBattleFundPopupBg()
        {
            battleFundPopupBg.SetActive(true);
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(battleFundPopupBg);
            DOTween.To(
                () => canvasGroup.alpha,
                x => canvasGroup.alpha = x,
                1,
                0.15f
            ).SetEase(Ease.Linear);
            battleFundPopupBg.SetActive(true);
            shopHeaderBg.SetActive(true);
        }

        public void ShowAbilitiesPopupBg()
        {
            abilitiesPopupBg.SetActive(true);
            shopHeaderBg.SetActive(true);
        }

        public void HideAbilitiesPopupBg()
        {
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(abilitiesPopupBg);
            DOTween.To(
                    () => canvasGroup.alpha,
                    x => canvasGroup.alpha = x,
                    0f,
                    0.15f
                ).SetEase(Ease.Linear)
                .OnComplete(() => { battlePassPopupBg.SetActive(false); });
            shopHeaderBg.SetActive(false);
        }

        public void HideBattleFundPopupBg()
        {
            CanvasGroup canvasGroup = AddCanvasGroupInBackground(battleFundPopupBg);
            DOTween.To(
                    () => canvasGroup.alpha,
                    x => canvasGroup.alpha = x,
                    0f,
                    0.15f
                ).SetEase(Ease.Linear)
                .OnComplete(() => { battleFundPopupBg.SetActive(false); });
            shopHeaderBg.SetActive(false);
        }

        public CanvasGroup AddCanvasGroupInBackground(GameObject obj)
        {
            if (!obj.GetComponent<CanvasGroup>())
            {
                obj.AddComponent<CanvasGroup>();
            }

            return obj.GetComponent<CanvasGroup>();
        }

        public void ShowMovingSoldierIcon()
        {
            battleProgressHighlight.gameObject.SetActive(true);
            battleProgressHighlight.StartFading();
            movingSoldierIcon.StartMoving();
        }

        public void HideMovingSoldierIcon()
        {
            battleProgressHighlight.gameObject.SetActive(false);
            battleProgressHighlight.StopFading();
            movingSoldierIcon.StopMoving();
        }

        public void ShowSpeedBoosterBtn()
        {
            _speedBoosterCountdown.gameObject.SetActive(true);
        }

        public void HideSpeedBoosterBtn()
        {
            _speedBoosterCountdown.gameObject.SetActive(false);
        }

        public void ShowWorkerSpeedBoosterBtn()
        {
            _workerSpeedBoosterCountdown.gameObject.SetActive(true);
        }

        public void HideWorkerSpeedBoosterBtn()
        {
            _workerSpeedBoosterCountdown.gameObject.SetActive(false);
        }

        public void ShowUpgradeWorkerBtn()
        {
            _upgradeWorkerCountdown.gameObject.SetActive(true);
        }

        public void HideUpgradeWorkerBtn()
        {
            _upgradeWorkerCountdown.gameObject.SetActive(false);
        }

        public void ShowVipSoldierBtn()
        {
            _vipSoldierCountdown.gameObject.SetActive(true);
        }

        public void HideVipSoldierBtn()
        {
            _vipSoldierCountdown.gameObject.SetActive(false);
        }

        public void ShowStartBattleButton()
        {
            var currentBattleRoom = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
            if (currentBattleRoom && currentBattleRoom.ClientsList.Count == 0)
            {
                startBattleBtn.interactable = false;
            }
            else
            {
                startBattleBtn.interactable = true;
                var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
                if (!FacilityManager.Instance.GameSaveLoad.GameData.isShowedStartBattleTutorial && mapIndex == 1)
                {
                    FacilityManager.Instance.GameSaveLoad.GameData.isShowedStartBattleTutorial = true;
                    ShowStartBattleTutorial();

                    if (FireBaseManager.Instance)
                    {
                        FireBaseManager.Instance.LogEvent(FireBaseManager.FINISH_TUTORIAL);
                    }
                }
            }

            startBattleBtn.gameObject.transform.localScale = Vector3.zero;
            startBattleBtn.gameObject.SetActive(true);
            startBattleBtn.gameObject.transform.DOScale(1, 0.7f).SetEase(Ease.OutBounce);
        }

        public void HideStartBattleButton()
        {
            startBattleBtn.gameObject.SetActive(false);
        }

        public void StartCountDownForSpeedBooster(int countDownNumber)
        {
            _speedBoosterCountdown.gameObject.SetActive(true);
            _speedBoosterCountdown.StartCountDown(countDownNumber, FacilityManager.Instance.StopBooster);
        }

        public void StartCountDownForWorkerSpeedBooster(int countDownNumber)
        {
            _workerSpeedBoosterCountdown.gameObject.SetActive(true);
            _workerSpeedBoosterCountdown.StartCountDown(countDownNumber,
                FacilityManager.Instance.StopWorkerSpeedBooster);
        }

        public void StartCountDownForUpgradeWorker(int countDownNumber)
        {
            _upgradeWorkerCountdown.gameObject.SetActive(true);
            _upgradeWorkerCountdown.StartCountDown(countDownNumber);
        }

        public void StartCountDownForVipSoldier(int countDownNumber)
        {
            _vipSoldierCountdown.gameObject.SetActive(true);
            _vipSoldierCountdown.StartCountDown(countDownNumber, ActionWhenCountDownVipSoldierFinished);
            _vipSoldierCountdown.HideTimeCircle();
        }

        public void ActionWhenCountDownVipSoldierFinished()
        {
            FacilityManager.Instance.OnHideVipSoldierPoint();
            _vipSoldierCountdown.gameObject.SetActive(false);
        }

        public void StartCountDownForSpinWheel()
        {
            spinWheelCountDown.gameObject.SetActive(true);
            spinWheelCountDown.StartCountDown();
        }

        public void StartCountDownForSkinRetal(double second)
        {
            skinRetalCountDown.StartCountDown(second);
        }

        public void ChangeTimeFollowSkin(double timer)
        {
            skinRetalCountDown.ChangeTimer(timer);
        }

        public void ResetTimeFollowSkin()
        {
            //skinRetalCountDown.LivesManagerSkin.ResetPlayerPrefs();
            skinRetalCountDown.countDownObj.SetActive(false);
        }

        public void SetActiveTimeFollowSkin(bool active)
        {
            skinRetalCountDown.countDownObj.SetActive(active);
        }

        public void StopCountDownVipSoldier()
        {
            _vipSoldierCountdown.StopCountDown(FacilityManager.Instance.OnHideVipSoldierPoint);
        }

        public void OnLogBtnClick()
        {
            logContainer.SetActive(!logContainer.activeSelf);
        }

        private bool _isNearDesertRoom;
        public bool IsFocusingOnDesertRoom { get; set; }

        public void OnClickDeserterIcon()
        {
            if (IsFocusingOnDesertRoom)
                return;
            if (_isNearDesertRoom)
                return;
            FacilityManager.Instance.FocusCameraToDesertRoom();
        }

        public void OnClickMoneyBtn()
        {
            if (IsLocked)
                return;
            FacilityManager.Instance.ShowGetMoneyPopup();

            FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BOOSTER_CASH_ICON);
        }

        public void OnClickSpeedBoosterBtn()
        {
            if (IsLocked)
                return;
            if (_speedBoosterCountdown.isCountingDown)
                return;
            FacilityManager.Instance.ShowGetBoosterPopup();

            FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BOOSTER_SPEED_ICON);
        }

        public void OnClickWorkerSpeedBoosterBtn()
        {
            if (IsLocked)
                return;
            if (_workerSpeedBoosterCountdown.isCountingDown)
                return;
            FacilityManager.Instance.ShowGetWorkerSpeedBoosterPopup();

            FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BOOSTER_SPEED_WORKER_ICON);
        }

        public void OnClickUpgradeWorkerBtn()
        {
            if (IsLocked)
                return;
            if (_upgradeWorkerCountdown.isCountingDown)
                return;
            FacilityManager.Instance.OnShowUpgradeWorkerPopup();

            FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_LADY_ICON);
        }

        public void OnClickVipSoldierBtn()
        {
            if (IsLocked)
                return;
            // if(vipSoldierCountdown.isCountingDown)
            //     return;
            FacilityManager.Instance.OnShowVipSoldierPopup();

            FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_DOG_ICON);
        }

        public void OnStartBattleButtonClick()
        {
            if (IsLocked)
                return;
            Advertisements.Instance.ShowBanner();
            AdsManager.Instance.HideMrec();
            nativeLoading.SetActive(false);
            IsShowingTutorial = false;
            startBattleTutorial.SetActive(false);
            background.gameObject.SetActive(false);
            HideStartBattleButton();
            var battleRoom = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
            if (battleRoom.TotalHp >= battleRoom.Data.minSoldierToStart)
                FacilityManager.Instance.BattleManager.OnStartBattle();
            else
                FacilityManager.Instance.ShowBattleWarningPopup();
        }

        public void OnClickShopBtn()
        {
            if (IsLocked)
                return;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            FacilityManager.Instance.ShowShopPopup();
        }

        public void OnClickSpinWheel()
        {
            if (IsLocked)
                return;
            IsShowingTutorial = false;
            SpinWheelTutorial.SetActive(false);
            background.SetActive(false);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            FacilityManager.Instance.ShowSpinWheelPopup();
        }

        public void OnClickAbilities()
        {
            if (IsLocked)
                return;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            FacilityManager.Instance.ShowAbilitiesPopup();
        }

        public void OnClickBuyNoAdsBtn()
        {
            if (IsLocked)
                return;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            FacilityManager.Instance.ShowBuyNoAdsPopup();
        }

        public void OnClickChangeMapBtn()
        {
            if (IsLocked)
                return;
            IsShowingTutorial = false;
            changeMapTutorial.SetActive(false);
            background.SetActive(false);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            FacilityManager.Instance.ShowChangeMapPopup();
        }

        public void OnClickSkinBtn()
        {
            if (IsLocked)
                return;
            IsShowingTutorial = false;
            playerSkinTutorial.SetActive(false);
            background.SetActive(false);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            FacilityManager.Instance.ShowSkinPopup();
        }

        public void OnClickBattlePassBtn()
        {
            if (IsLocked)
                return;
            IsShowingTutorial = false;
            BattleFundTutorial.SetActive(false);
            background.SetActive(false);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            FacilityManager.Instance.ShowBattlePass();
            FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_BATTLE_FUND);
        }

        // public void OnClickBattleFunBtn()
        // {
        //     AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
        //     FacilityManager.Instance.OpenBattleFund();
        // }

        public void OnClickDailyQuestBtn()
        {
            if (IsLocked)
                return;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            FacilityManager.Instance.ShowDailyQuestPopup();
        }

        public void OnClickCountSoldierBtn()
        {
            if (IsLocked)
                return;
            IsShowingTutorial = false;
            countSoldierTutorial.SetActive(false);
            background.SetActive(false);
            FacilityManager.Instance.BattleManager.ViewBattle();
            if (FireBaseManager.Instance)
            {
                FireBaseManager.Instance.LogEvent(FireBaseManager.CLICK_CHECK_CAMERA);
            }
        }

        public void ClickSettingBtn()
        {
            if (IsLocked)
                return;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            SettingButtonClick();
        }

        private List<GameObject> _milestonesList;

        public void InitBattleMilestone(BattleData battleData)
        {
            // Debug.Log("InitBattleMilestone");
            if (_milestonesList != null)
            {
                foreach (var milestone in _milestonesList.Where(milestone => milestone))
                {
                    Destroy(milestone);
                }
            }

            _milestonesList = new List<GameObject>();

            var startBattlePosition = battleResult.transform.Find("Start_Battle_Position");
            var endBattlePosition = battleResult.transform.Find("End_Battle_Position");
            foreach (var milestoneData in battleData.milestonesList)
            {
                var milestoneIcon = Instantiate(battleMilestoneIconPrefab, battleResult.transform);
                milestoneIcon.transform.Find("blue").gameObject.SetActive(false);
                var yPos = startBattlePosition.localPosition.y;
                var xPos = startBattlePosition.localPosition.x +
                           (endBattlePosition.localPosition.x - startBattlePosition.localPosition.x) *
                           milestoneData.progress / 100;
                if (milestoneData.progress <= battleProgressBar.Progress * 100)
                {
                    milestoneIcon.transform.Find("blue").gameObject.SetActive(true);
                    milestoneIcon.transform.Find("red").gameObject.SetActive(false);
                }

                milestoneIcon.transform.localPosition = new Vector3(xPos, yPos);
                _milestonesList.Add(milestoneIcon);
            }
        }

        [Button]
        public void ArchiveMilestones(int index, int reward)
        {
            if (_milestonesList != null && _milestonesList.Count > index)
            {
                _milestonesList[index].transform.Find("red").gameObject.SetActive(false);
                _milestonesList[index].transform.Find("blue").gameObject.SetActive(true);
                var bubbleCash = Instantiate(bubbleCashPrefab, battleResult.transform);
                bubbleCash.transform.localPosition = _milestonesList[index].transform.localPosition;
                bubbleCash.GetComponent<BubbleFlyingCash2D>().SetValue(reward);
                bubbleCash.GetComponent<BubbleFlyingCash2D>().StartFly();
            }
        }

        public void UpdateBattleProgress(float progress)
        {
            // Debug.Log("UpdateBattleProgress");
            battleProgressBar.UpdateProgress(progress, false);
        }

        public void ShowDailyQuestAnimation()
        {
            dailyQuestRedNotification.SetActive(true);
            // dailyQuestBtn.GetComponent<FlutterAnimation>().StartFlutter(1.2f, 0.7f);
        }

        public void ShowShopQuestAnimation()
        {
            shopNotification.SetActive(true);
        }

        public void ShowBattlePassRequestAnimation()
        {
            battlePassNotification.SetActive(true);
        }

        public void ShowSkinRequestAnimation()
        {
            skinNotification.SetActive(true);
        }

        public void ShowSpinRequestAnimation()
        {
            spinNotification.SetActive(true);
        }

        public void HideSkinAnimation()
        {
            skinNotification.SetActive(false);
        }

        public void HideShopAnimation()
        {
            shopNotification.SetActive(false);
        }

        public void HideDailyQuestAnimation()
        {
            dailyQuestRedNotification.SetActive(false);
            // dailyQuestBtn.GetComponent<FlutterAnimation>().StopFlutter();
        }

        public void HideBattlePassRequestAnimation()
        {
            battlePassNotification.SetActive(false);
        }

        public void HideSpinNotification()
        {
            spinNotification.SetActive(false);
        }

        public void ShowAnimBattleResult()
        {
            var position = battleResult.transform.localPosition;
            battleResult.transform.localPosition = new Vector3(position.x, position.y + 300, 0);
            battleResult.SetActive(true);
            battleResult.transform.DOLocalMove(position, 2.5f).SetEase(Ease.OutBack);
        }

        public void ShowBattleResult()
        {
            battleResult.SetActive(true);
        }

        public void HideBattleResult()
        {
            battleResult.SetActive(false);
        }

        public void HideGuideTxt()
        {
            guideTxt.gameObject.SetActive(false);
            guideTxt.GetComponent<FlutterAnimation>().StopFlutter();
        }

        private int _moneyNumber;
        private int _startMoneyNumber;

        private Transform _parentTransform;

        public void UpdateMoney(int money, bool isShowMoneyEffect = false, Vector3 effectPosition = new(),
            bool isShow = false, Transform parentTransform = null)
        {
            _parentTransform = parentTransform;
            if (!_parentTransform)
                _parentTransform = transform.parent;
            _startMoneyNumber = _moneyNumber;
            _moneyNumber = money;
            if (!isShowMoneyEffect)
            {
                string formattedNumber = UtilitiesFunction.FormatNumber(money);
                moneyTxt.text = formattedNumber;
            }
            else
            {
                if (money > _startMoneyNumber)
                    ShowAddMoneyEffect(money - _startMoneyNumber, effectPosition, isShow);
            }
        }

        /*string FormatNumber(int value)
        {
            if (value >= 1000)
            {
                return (value / 1000f).ToString("0.0") + "k";
            }

            return value.ToString();
        }*/

        private int _currentMoneyEffectIndex;
        private int _addMoneyValue;

        private bool _isShow;
        private bool _isPassSound;

        private List<Vector2> _spawnedPositions = new List<Vector2>();

        // Thêm hàm này để kiểm tra khoảng cách giữa các điểm
        private bool IsTooClose(Vector2 position, float minDistance)
        {
            foreach (Vector2 existingPosition in _spawnedPositions)
            {
                if (Vector2.Distance(position, existingPosition) < minDistance)
                {
                    return true;
                }
            }

            return false;
        }

        // Thêm hàm này để reset danh sách vị trí khi bắt đầu hiệu ứng mới
        public void ResetSpawnedPositions()
        {
            _spawnedPositions.Clear();
        }

        public void ShowAddMoneyEffect(int money, Vector3 rootPosition, bool isShow = false)
        {
            _isShow = isShow;
            _currentMoneyEffectIndex = 0;
            var localPosition = cashEffectDestination.transform.localPosition;
            var destination = new Vector2(localPosition.x,
                localPosition.y);

            var delay = 0.05f;

            var animCount = money switch
            {
                < 350 => 6,
                <= 500 => 7,
                _ => 8
            };

            _addMoneyValue = 1;
            if (money > 20)
            {
                _addMoneyValue = (int)Math.Ceiling((double)money / animCount);
            }

            if (animCount * delay > 1.5f)
                delay = (float)1.5f / animCount;

            // Reset danh sách vị trí đã tạo khi bắt đầu hiệu ứng mới
            ResetSpawnedPositions();

            for (var i = 0; i < animCount; i++)
            {
                var addPosition = new Vector2(cashPositionList[_currentMoneyEffectIndex].transform.localPosition.x,
                    cashPositionList[_currentMoneyEffectIndex].transform.localPosition.y);

                _currentMoneyEffectIndex++;
                if (_currentMoneyEffectIndex > cashPositionList.Count - 1)
                    _currentMoneyEffectIndex = 0;
                if (delay == 0)
                {
                    StartCoroutine(SpawnOneCash(0, rootPosition, addPosition, destination, isShow));
                }
                else
                {
                    StartCoroutine(SpawnOneCash(delay * i, rootPosition, addPosition, destination, isShow));
                }
            }
        }

        private IEnumerator SpawnOneCash(float delay, Vector3 rootsStartPosition, Vector2 addPosition,
            Vector2 destination, bool isShow = false)
        {
            yield return new WaitForSeconds(delay);
            var sizeDelta = CanvasManager.Instance.canvas.GetComponent<RectTransform>().sizeDelta;
            Vector2 viewportPosition = MainCamera.WorldToViewportPoint(rootsStartPosition);
            var screenPoint = new Vector2(
                ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));

            if (rootsStartPosition == Vector3.zero)
                screenPoint = Vector2.zero;

            Vector2 finalPosition;
            int maxAttempts = 10;
            float minDistance = 40f;

            do
            {
                float randomRadius = UnityEngine.Random.Range(60f, 120f);
                float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
                Vector2 randomOffset = new Vector2(
                    randomRadius * Mathf.Cos(randomAngle),
                    randomRadius * Mathf.Sin(randomAngle)
                );

                finalPosition = screenPoint + randomOffset;
                maxAttempts--;
            } while (IsTooClose(finalPosition, minDistance) && maxAttempts > 0);

            _spawnedPositions.Add(finalPosition);

            if (isShow)
            {
                var cash = Instantiate(cashEffectPrefab, _parentTransform);
                cash.transform.localPosition = screenPoint;
                cash.transform.localScale = Vector3.zero;
                var randomSize = UnityEngine.Random.Range(0.8f, 1f);
                Sequence appearSequence = DOTween.Sequence();
                appearSequence.Join(cash.transform.DOScale(randomSize, 0.3f).SetEase(Ease.OutBack));
                appearSequence.Join(cash.transform.DOLocalMove(finalPosition, 0.3f).SetEase(Ease.OutBack));
                appearSequence.OnComplete(() =>
                {
                    StartCoroutine(MoveToDestinationWithDelay(cash, finalPosition, destination, delay));
                });
            }
            else
            {
                OnMoneyFlyComplete();
            }
        }

        private IEnumerator MoveToDestinationWithDelay(GameObject cash, Vector2 startPos, Vector2 destination,
            float delay)
        {
            yield return new WaitForSeconds(delay);

            float randomDelay = UnityEngine.Random.Range(0f, 0.1f);
            yield return new WaitForSeconds(randomDelay);

            if (cash != null)
            {
                if (cash.GetComponent<LinearFlying2D>() == null)
                {
                    var flying = cash.AddComponent<LinearFlying2D>();
                    flying.scaleTime = _delaySpawn;
                }
                else
                {
                    cash.GetComponent<LinearFlying2D>().scaleTime = _delaySpawn;
                }

                cash.GetComponent<LinearFlying2D>().StartFly(0.4f, destination,
                    cashEffectDestination.transform.localScale.x, OnMoneyFlyComplete);
            }
            else
            {
                OnMoneyFlyComplete();
            }
        }

        private IEnumerator MoveToDestinationTokenWithDelay(GameObject obj, Vector2 startPos, Vector2 destination,
            float delay)
        {
            yield return new WaitForSeconds(delay);

            float randomDelay = UnityEngine.Random.Range(0f, 0.1f);
            yield return new WaitForSeconds(randomDelay);

            if (obj != null)
            {
                if (obj.GetComponent<LinearFlying2D>() == null)
                {
                    var flying = obj.AddComponent<LinearFlying2D>();
                    flying.scaleTime = _delaySpawn;
                }
                else
                {
                    obj.GetComponent<LinearFlying2D>().scaleTime = _delaySpawn;
                }

                obj.GetComponent<LinearFlying2D>().StartFly(0.4f, destination,
                    cashEffectDestination.transform.localScale.x, OnTokenFlyComplete);
            }
            else
            {
                OnTokenFlyComplete();
            }
        }

        private IEnumerator MoveToDestinationGemWithDelay(GameObject obj, Vector2 startPos, Vector2 destination,
            float delay)
        {
            yield return new WaitForSeconds(delay);

            float randomDelay = UnityEngine.Random.Range(0f, 0.1f);
            yield return new WaitForSeconds(randomDelay);

            if (obj != null)
            {
                if (obj.GetComponent<LinearFlying2D>() == null)
                {
                    var flying = obj.AddComponent<LinearFlying2D>();
                    flying.scaleTime = _delaySpawn;
                }
                else
                {
                    obj.GetComponent<LinearFlying2D>().scaleTime = _delaySpawn;
                }

                obj.GetComponent<LinearFlying2D>().StartFly(0.4f, destination,
                    cashEffectDestination.transform.localScale.x, OnGemFlyComplete);
            }
            else
            {
                OnGemFlyComplete();
            }
        }

        private void OnMoneyFlyComplete()
        {
            _startMoneyNumber += _addMoneyValue;
            if (_startMoneyNumber > _moneyNumber)
                _startMoneyNumber = _moneyNumber;

            string formattedNumber = UtilitiesFunction.FormatNumber(_startMoneyNumber);
            moneyTxt.text = formattedNumber;
            _isPassSound = !_isPassSound;
            if (!_isPassSound && _isShow)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Money);
        }

        public void UpdateLevel(int level)
        {
            levelTxt.text = level.ToString();
        }

        public void UpdateLevelProgress(int currentProgress, int totalProgress)
        {
            levelProgressTxt.DisplayText = currentProgress + " / " + totalProgress;
            var progress = (float)currentProgress / totalProgress;
            // Debug.Log("UpdateLevelProgress: " + progress);
            levelProgressChild.UpdateProgress(progress);
        }

        private int _currentStarEffectIndex;

        public void ShowAddStarEffect(int star, Vector2 rootPosition, Transform parentTransform = null)
        {
            _parentTransform = parentTransform;
            if (!_parentTransform)
                _parentTransform = transform;
            // _currentStarEffectIndex = 0;
            var localPosition = starEffectDestination.transform.localPosition;
            var destination = new Vector2(localPosition.x,
                localPosition.y);
            for (var i = 0; i < star; i++)
            {
                var addPosition = new Vector2(cashPositionList[_currentStarEffectIndex].transform.localPosition.x,
                    cashPositionList[_currentStarEffectIndex].transform.localPosition.y);
                var startPosition = rootPosition + addPosition;
                _currentStarEffectIndex++;
                if (_currentStarEffectIndex > cashPositionList.Count - 1)
                    _currentStarEffectIndex = 0;
                spawnOneStar(0.2f * i, startPosition, destination);
            }
        }

        private void spawnOneStar(float delay, Vector2 startPosition, Vector2 destination)
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Collect_Star);
            var star = Instantiate(starEffectPrefab, _parentTransform);
            star.transform.localScale = new Vector3(0, 0, 0);
            star.transform.localPosition = startPosition;

            if (delay == 0)
            {
                star.GetComponent<CurvedFlying2D>().scaleTime = 1;
                star.GetComponent<CurvedFlying2D>().StartFly(0.5f, destination,
                    starEffectDestination.transform.localScale.x, OnMoneyFlyComplete);
                return;
            }

            DOVirtual.DelayedCall(delay, (() =>
            {
                star.GetComponent<CurvedFlying2D>().scaleTime = 1;
                star.GetComponent<CurvedFlying2D>().StartFly(0.5f, destination,
                    starEffectDestination.transform.localScale.x, OnMoneyFlyComplete);
            })).SetAutoKill(true);
        }

        public override void Show()
        {
        }

        public override void Hide()
        {
        }

        private int _battleHpNumber;
        private int _startBattleHpNumber;

        public void UpdateBattleHp(int battleHp, bool isShowEffect = false, Vector3 effectPosition = new())
        {
            _startBattleHpNumber = _battleHpNumber;
            _battleHpNumber = battleHp;
            if (battleHp == 0)
            {
                battleHpTxt.text = 0.ToString();
                _startBattleHpNumber = 0;
            }

            if (!isShowEffect)
            {
                battleHpTxt.text = battleHp.ToString();
            }
            else
            {
                if (battleHp > _startBattleHpNumber)
                    ShowAddBattleHpEffect(battleHp - _startBattleHpNumber, effectPosition);
            }
        }

        private int _currentRiffleEffectIndex;

        public void ShowAddBattleHpEffect(int riffle, Vector3 rootPosition)
        {
            _currentRiffleEffectIndex = 0;
            var localPosition = riffleEffectDestination.transform.localPosition;
            var destination = new Vector2(localPosition.x,
                localPosition.y);

            var delay = 0.3f;
            if (riffle * 0.3f > 2)
                delay = (float)2 / riffle;
            for (var i = 0; i < riffle; i++)
            {
                var addPosition = new Vector2(cashPositionList[_currentRiffleEffectIndex].transform.localPosition.x,
                    cashPositionList[_currentRiffleEffectIndex].transform.localPosition.y);

                _currentRiffleEffectIndex++;
                if (_currentRiffleEffectIndex > cashPositionList.Count - 1)
                    _currentRiffleEffectIndex = 0;
                if (delay == 0)
                {
                    StartCoroutine(SpawnOneRiffle(0, rootPosition, addPosition, destination));
                }
                else
                {
                    StartCoroutine(SpawnOneRiffle(delay * i, rootPosition, addPosition, destination));
                }
            }
        }

        private IEnumerator SpawnOneRiffle(float delay, Vector3 rootsStartPosition, Vector2 addPosition,
            Vector2 destination)
        {
            yield return new WaitForSeconds(delay);
            var sizeDelta = CanvasManager.Instance.canvas.GetComponent<RectTransform>().sizeDelta;
            Vector2 viewportPosition = MainCamera.WorldToViewportPoint(rootsStartPosition);
            var screenPoint = new Vector2(
                ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));

            var startPosition = screenPoint + addPosition;

            var cash = Instantiate(riffleEffectPrefab, transform.parent);
            // cash.SetActive(false);
            cash.transform.localScale = new Vector3(0, 0, 0);
            cash.transform.localPosition = startPosition;

            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Battle_CheckPoint_MGB);

            cash.transform.DOScale(new Vector3(1, 1, 1), 0.3f)
                .SetEase(Ease.OutBack).OnComplete((() =>
                {
                    cash.transform.DOScale(riffleEffectDestination.transform.localScale.x, 0.9f);
                    cash.transform.DOLocalMove(destination, 1f).OnComplete((() =>
                    {
                        _startBattleHpNumber++;
                        battleHpTxt.text = _startBattleHpNumber.ToString();
                        cash.transform.DOKill();
                        Destroy(cash);
                    })).SetEase(Ease.OutQuad);
                }));
        }

        private bool _isShowNavi;

        public void ShowNavigation()
        {
            _isNearDesertRoom = false;
            navigationArrow.gameObject.SetActive(true);
            arrowRectTransform.gameObject.SetActive(true);
            desertCountdown.gameObject.SetActive(true);
            navigationChild.gameObject.SetActive(true);
            _isShowNavi = true;
        }

        public void CountDownNavigation(int time)
        {
            desertCountdown.StartCountdown(time);
        }

        public void HideNavigation()
        {
            _isNearDesertRoom = true;
            navigationArrow.gameObject.SetActive(false);
            arrowRectTransform.gameObject.SetActive(false);
            navigationChild.gameObject.SetActive(false);
            navigationNotification.gameObject.SetActive(false);
            desertCountdown.gameObject.SetActive(false);
            _isShowNavi = false;
        }

        public void UpdateNavigation(GameObject target, int quantity)
        {
            _currentNavigationTarget = target;
            /*arrowRectTransform.gameObject.SetActive(true);
            navigationChild.gameObject.SetActive(true);
            navigationNotification.gameObject.SetActive(true);*/
            navigationTxt.text = quantity.ToString();
            // navigationChild.StartFlutter(1.2f, 0.7f);
            // _isShowNavi = true;
        }

        public void AddNavigationTarget(GameObject target)
        {
            _navigationTargetsList.Add(target);
        }

        public void RemoveNavigationTarget(GameObject target)
        {
            for (var i = 0; i < _navigationTargetsList.Count; i++)
            {
                if (_navigationTargetsList[i] != target) continue;
                _navigationTargetsList.RemoveAt(i);
                break;
            }

            if (target == _currentNavigationTarget)
            {
                _currentNavigationTarget = null;
                if (_navigationTargetsList.Count > 0)
                    _currentNavigationTarget = _navigationTargetsList[0];
            }
        }

        private List<GameObject> _navigationTargetsList = new();

        private GameObject _currentNavigationTarget;

        private void Update()
        {
            if (!_isShowNavi)
                return;
            if (!_currentNavigationTarget) return;
            // Chuyển đổi vị trí của object B từ World Space sang Viewport Space
            var targetViewportPos =
                MainCamera.WorldToViewportPoint(_currentNavigationTarget.transform.position);

            // Kiểm tra xem object B có nằm trong tầm nhìn của camera không
            // if (targetViewportPos is { z: > 0, x: >= 0 and <= 1, y: >= 0 and <= 1 })
            // Debug.Log(Vector3.Distance(FacilityManager.Instance.player.transform.position,
            //     _currentNavigationTarget.transform.position));
            if (Vector3.Distance(FacilityManager.Instance.player.transform.position,
                    _currentNavigationTarget.transform.position) < 10)
            {
                // Object B nằm trong tầm nhìn, ẩn mũi tên
                _isNearDesertRoom = true;
                navigationArrow.gameObject.SetActive(false);
                // arrowRectTransform.gameObject.SetActive(false);
                // navigationChild.gameObject.SetActive(false);
                // navigationNotification.gameObject.SetActive(false);
                // desertCountdown.gameObject.SetActive(false);
            }
            else
            {
                _isNearDesertRoom = false;
                // Object B nằm ngoài tầm nhìn, hiển thị và xoay mũi tên
                navigationArrow.gameObject.SetActive(true);
                arrowRectTransform.gameObject.SetActive(true);
                navigationChild.gameObject.SetActive(true);
                navigationNotification.gameObject.SetActive(true);
                desertCountdown.gameObject.SetActive(true);

                var sizeDelta = CanvasManager.Instance.canvas.GetComponent<RectTransform>().sizeDelta;

                // Chuyển đổi vị trí viewport sang canvas
                var targetCanvasPos = new Vector2(
                    ((targetViewportPos.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                    ((targetViewportPos.y * sizeDelta.y) - (sizeDelta.y * 0.5f))
                );

                // Tính toán hướng từ trung tâm canvas đến vị trí đích
                var direction = (targetCanvasPos - Vector2.zero).normalized;

                // Xoay mũi tên
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrowRectTransform.localRotation = Quaternion.Euler(0, 0, angle); // -90 để điều chỉnh hướng mũi tên
            }
        }

        public void Navigate(GameObject target)
        {
            // Chuyển đổi vị trí của object B từ World Space sang Viewport Space
            Vector3 targetViewportPos = MainCamera.WorldToViewportPoint(target.transform.position);

            // Kiểm tra xem object B có nằm trong tầm nhìn của camera không
            if (targetViewportPos.z > 0 &&
                targetViewportPos.x >= 0 && targetViewportPos.x <= 1 &&
                targetViewportPos.y >= 0 && targetViewportPos.y <= 1)
            {
                // Object B nằm trong tầm nhìn, ẩn mũi tên
                arrowRectTransform.gameObject.SetActive(false);
            }
            else
            {
                // Object B nằm ngoài tầm nhìn, hiển thị và xoay mũi tên
                arrowRectTransform.gameObject.SetActive(true);

                var sizeDelta = CanvasManager.Instance.canvas.GetComponent<RectTransform>().sizeDelta;

                // Chuyển đổi vị trí viewport sang canvas
                Vector2 targetCanvasPos = new Vector2(
                    ((targetViewportPos.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                    ((targetViewportPos.y * sizeDelta.y) - (sizeDelta.y * 0.5f))
                );

                // Tính toán hướng từ trung tâm canvas đến vị trí đích
                Vector2 direction = (targetCanvasPos - Vector2.zero).normalized;

                // Đặt vị trí mũi tên ở mép canvas
                // Vector2 arrowPosition = direction * (sizeDelta.x * 0.45f); // 0.45f để tránh mũi tên nằm sát mép
                // arrowRectTransform.anchoredPosition = arrowPosition;

                // Xoay mũi tên
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrowRectTransform.localRotation = Quaternion.Euler(0, 0, angle); // -90 để điều chỉnh hướng mũi tên
            }
        }

        public void UpdateToken(int token, bool isShowEffect = false, Vector3 effectPosition = new(),
            bool isShow = false, Transform parentTransform = null)
        {
            _parentTransform = parentTransform;
            if (!_parentTransform)
                _parentTransform = transform.parent;
            _startTokenNumber = _tokenNumber;
            _tokenNumber = token;
            if (!isShowEffect)
            {
                string formattedNumber = UtilitiesFunction.FormatNumber(token);
                tokenTxt.text = formattedNumber;
            }
            else
            {
                if (token > _startTokenNumber)
                    ShowAddTokenEffect(token - _startTokenNumber, effectPosition, isShow);
            }
        }

        private int _currentTokenEffectIndex;
        private int _addTokenValue;
        private int _tokenNumber;
        private int _startTokenNumber;

        public void ShowAddTokenEffect(int token, Vector3 rootPosition, bool isShow = false)
        {
            _isShow = isShow;
            _currentTokenEffectIndex = 0;
            var localPosition = tokenEffectDestination.transform.localPosition;
            var destination = new Vector2(localPosition.x,
                localPosition.y);

            var delay = 0.05f;

            var animCount = token;
            _addTokenValue = 1;
            if (token > 7)
            {
                animCount = 7;
                _addTokenValue = (int)Math.Ceiling((double)token / animCount);
            }

            if (animCount * delay > 1.5f)
                delay = 1.5f / animCount;
            ResetSpawnedPositions();
            for (var i = 0; i < animCount; i++)
            {
                var addPosition = new Vector2(cashPositionList[_currentTokenEffectIndex].transform.localPosition.x,
                    cashPositionList[_currentTokenEffectIndex].transform.localPosition.y);

                _currentTokenEffectIndex++;
                if (_currentTokenEffectIndex > cashPositionList.Count - 1)
                    _currentTokenEffectIndex = 0;
                if (delay == 0)
                {
                    StartCoroutine(SpawnOneToken(0, rootPosition, addPosition, destination, isShow));
                }
                else
                {
                    StartCoroutine(SpawnOneToken(delay * i, rootPosition, addPosition, destination, isShow));
                }
            }
        }

        private IEnumerator SpawnOneToken(float delay, Vector3 rootsStartPosition, Vector2 addPosition,
            Vector2 destination, bool isShow = false)
        {
            yield return new WaitForSeconds(delay);
            var sizeDelta = CanvasManager.Instance.canvas.GetComponent<RectTransform>().sizeDelta;
            Vector2 viewportPosition = MainCamera.WorldToViewportPoint(rootsStartPosition);
            var screenPoint = new Vector2(
                ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));

            if (rootsStartPosition == Vector3.zero)
                screenPoint = Vector2.zero;

            Vector2 finalPosition;
            int maxAttempts = 10;
            float minDistance = 40f;

            do
            {
                float randomRadius = UnityEngine.Random.Range(60f, 120f);
                float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
                Vector2 randomOffset = new Vector2(
                    randomRadius * Mathf.Cos(randomAngle),
                    randomRadius * Mathf.Sin(randomAngle)
                );

                finalPosition = screenPoint + randomOffset;
                maxAttempts--;
            } while (IsTooClose(finalPosition, minDistance) && maxAttempts > 0);

            _spawnedPositions.Add(finalPosition);

            if (isShow)
            {
                var token = Instantiate(tokenEffectPrefab, _parentTransform);
                token.transform.localPosition = screenPoint;
                token.transform.localScale = Vector3.zero;
                var randomSize = UnityEngine.Random.Range(0.9f, 1.2f);
                Sequence appearSequence = DOTween.Sequence();
                appearSequence.Join(token.transform.DOScale(randomSize, 0.3f).SetEase(Ease.OutBack));
                appearSequence.Join(token.transform.DOLocalMove(finalPosition, 0.3f).SetEase(Ease.OutBack));
                appearSequence.OnComplete(() =>
                {
                    StartCoroutine(MoveToDestinationTokenWithDelay(token, finalPosition, destination, delay));
                });
            }
            else
            {
                OnTokenFlyComplete();
            }
        }

        private void OnTokenFlyComplete()
        {
            _startTokenNumber += _addTokenValue;
            if (_startTokenNumber > _tokenNumber)
                _startTokenNumber = _tokenNumber;
            string formattedNumber = UtilitiesFunction.FormatNumber(_startTokenNumber);
            tokenTxt.text = formattedNumber;
            if (!_isPassSound && _isShow)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Collect_Star);
        }

        private int _currentGemEffectIndex;
        private int _addGemValue;
        private int _gemNumber;
        private int _startGemNumber;

        public void UpdateSticketRemoveAdsReward()
        {
            sticketTxt.text = FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds.ToString();
        }

        public void UpdateGem(int gem, bool isShowEffect = false, Vector3 effectPosition = new(),
            bool isShow = false, Transform parentTransform = null)
        {
            _startGemNumber = _gemNumber;
            _gemNumber = gem;

            _parentTransform = parentTransform;
            if (!_parentTransform)
                _parentTransform = transform.parent;

            if (!isShowEffect)
            {
                string formattedNumber = UtilitiesFunction.FormatNumber(gem);
                gemTxt.text = formattedNumber;
            }
            else
            {
                if (gem > _startGemNumber)
                    ShowAddGemEffect(gem - _startGemNumber, effectPosition, isShow);
            }
        }

        public void UpdateLevelBattlePass()
        {
            gemTxt.text = FacilityManager.Instance.GameSaveLoad.GameData.battlePassLevel.ToString();
        }

        private float _delaySpawn;

        public void ShowAddGemEffect(int gem, Vector3 rootPosition, bool isShow = false)
        {
            _isShow = isShow;
            _currentGemEffectIndex = 0;
            var localPosition = gemEffectDestination.transform.localPosition;
            var destination = new Vector2(localPosition.x,
                localPosition.y);

            var delay = 0.05f;

            var animCount = gem;
            _addGemValue = 1;
            if (gem > 7)
            {
                animCount = 7;
                _addTokenValue = (int)Math.Ceiling((double)gem / animCount);
            }

            if (animCount * delay > 1.5f)
                delay = (float)1.5f / animCount;
            ResetSpawnedPositions();
            for (var i = 0; i < animCount; i++)
            {
                var addPosition = new Vector2(cashPositionList[_currentGemEffectIndex].transform.localPosition.x,
                    cashPositionList[_currentGemEffectIndex].transform.localPosition.y);

                _currentGemEffectIndex++;
                if (_currentGemEffectIndex > cashPositionList.Count - 1)
                    _currentGemEffectIndex = 0;
                if (delay == 0)
                {
                    StartCoroutine(SpawnOneGem(0, rootPosition, addPosition, destination, isShow));
                }
                else
                {
                    StartCoroutine(SpawnOneGem(delay * i, rootPosition, addPosition, destination, isShow));
                }
            }
        }

        private IEnumerator SpawnOneGem(float delay, Vector3 rootsStartPosition, Vector2 addPosition,
            Vector2 destination, bool isShow = false)
        {
            yield return new WaitForSeconds(delay);
            var sizeDelta = CanvasManager.Instance.canvas.GetComponent<RectTransform>().sizeDelta;
            Vector2 viewportPosition = MainCamera.WorldToViewportPoint(rootsStartPosition);
            var screenPoint = new Vector2(
                ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));

            if (rootsStartPosition == Vector3.zero)
                screenPoint = Vector2.zero;

            Vector2 finalPosition;
            int maxAttempts = 10;
            float minDistance = 40f;
            do
            {
                float randomRadius = UnityEngine.Random.Range(60f, 120f);
                float randomAngle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
                Vector2 randomOffset = new Vector2(
                    randomRadius * Mathf.Cos(randomAngle),
                    randomRadius * Mathf.Sin(randomAngle)
                );

                finalPosition = screenPoint + randomOffset;
                maxAttempts--;
            } while (IsTooClose(finalPosition, minDistance) && maxAttempts > 0);

            _spawnedPositions.Add(finalPosition);

            if (isShow)
            {
                var gem = Instantiate(gemEffectPrefab, _parentTransform);
                gem.transform.localPosition = screenPoint;
                gem.transform.localScale = Vector3.zero;
                var randomSize = UnityEngine.Random.Range(0.7f, 1f);
                Sequence appearSequence = DOTween.Sequence();
                appearSequence.Join(gem.transform.DOScale(randomSize, 0.3f).SetEase(Ease.OutBack));
                appearSequence.Join(gem.transform.DOLocalMove(finalPosition, 0.3f).SetEase(Ease.OutBack));
                appearSequence.OnComplete(() =>
                {
                    StartCoroutine(MoveToDestinationGemWithDelay(gem, finalPosition, destination, delay));
                });
            }
            else
            {
                OnGemFlyComplete();
            }
        }

        public void HideInfoWhenStartBattle()
        {
            topContainer.SetActive(false);
        }

        public void ShowTestBtn()
        {
            testBtn.SetActive(true);
        }

        public void ShowInfoAfterBattle()
        {
            if (FacilityManager.Instance.testGameConfig.isForVideoRecording)
            {
                return;
            }

            topContainer.SetActive(true);
        }

        private void OnGemFlyComplete()
        {
            _startGemNumber += _addGemValue;
            if (_startGemNumber > _gemNumber)
                _startGemNumber = _gemNumber;
            string formattedNumber = UtilitiesFunction.FormatNumber(_startGemNumber);
            gemTxt.text = formattedNumber;
            _isPassSound = !_isPassSound;
            if (!_isPassSound && _isShow)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Money);
        }

        public void LevelBattlePassProgress()
        {
            int totalMetalTag = FacilityManager.Instance.GameSaveLoad.GameData.metalTagNumber;
            var countIndex = FacilityManager.Instance.BattlePassLevelConfig.battlePassLevelList;
            int level = 0;
            int remainingMetalTag = totalMetalTag;

            while (level < countIndex.Count && remainingMetalTag >= countIndex[level].MetalTagNumber)
            {
                remainingMetalTag -= countIndex[level].MetalTagNumber;
                level++;
            }

            float levelPercentage = (level < countIndex.Count)
                ? (float)remainingMetalTag / countIndex[level].MetalTagNumber
                : 1f;
            UpdateLevelBattlePassProgress(level, levelPercentage);
            FacilityManager.Instance.GameSaveLoad.GameData.levelPercentageBattlePass = levelPercentage;
            FacilityManager.Instance.GameSaveLoad.GameData.battlePassLevel = level;
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
        }

        private void UpdateLevelBattlePassProgress(int level, float percentage)
        {
            levelPassTxt.text = level.ToString();
            if (level > FacilityManager.Instance.GameSaveLoad.GameData.battlePassLevel)
            {
                iconBattlePassFill.fillAmount = 0f;
                iconBattlePassFill.DOFillAmount(1f, 0.5f)
                    .OnComplete(() =>
                    {
                        levelPassTxt.text = level.ToString();
                        iconBattlePassFill.DOFillAmount(percentage, 2f)
                            .SetEase(Ease.OutExpo);
                        FacilityManager.Instance.PlayerInfoUI.ShowBattlePassRequestAnimation();
                        FacilityManager.Instance.BattlePassPopupUI.GetComponent<BattlePassPopup>().OnInit();
                    })
                    .SetEase(Ease.OutExpo);
            }
            else
            {
                iconBattlePassFill.DOFillAmount(percentage, 2f)
                    .SetEase(Ease.OutExpo);
            }
        }

        public void SetActiveShopObj(bool active)
        {
            shopIconObj.SetActive(active);
            spinIconObj.transform.localScale = Vector3.zero;
            spinIconObj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        public void SetActiveBattlePassObj(bool active)
        {
            FacilityManager.Instance.SetDataBeginBattlePass();
            battlePassObj.SetActive(active);
            battlePassObj.transform.localScale = Vector3.zero;
            battlePassObj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        
        [Button]
        public void SetActiveSkinIconObj(bool active)
        {
            if (active)
            {
                var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
                if (!FacilityManager.Instance.GameSaveLoad.GameData.isShowedPlayerSkinTutorial && mapIndex == 1)
                {
                    ShowPlayerSkinTutorial();
                    FacilityManager.Instance.GameSaveLoad.GameData.isShowedPlayerSkinTutorial = true;
                }
            }

            skinIconObj.SetActive(active);
            skinIconObj.transform.localScale = Vector3.zero;
            skinIconObj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        public void SetActiveSpinIconObj(bool active)
        {
            if (active && !spinIconObj.activeSelf)
            {
                spinIconObj.transform.localScale = Vector3.zero;
                spinIconObj.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            }

            spinIconObj.SetActive(active);
        }

        [Button]
        public void ShowMetalTagTurnNumber(int number)
        {
            metaTagTxt.rectTransform.localPosition = new Vector3(metaTagTxt.rectTransform.localPosition.x, 0,
                metaTagTxt.rectTransform.localPosition.z);

            metaTagTxt.gameObject.SetActive(true);
            metaTagTxt.text = "+" + number.ToString();

            metaTagTxt.color = new Color(metaTagTxt.color.r, metaTagTxt.color.g, metaTagTxt.color.b, 0);

            metaTagTxt.rectTransform.localPosition = new Vector3(metaTagTxt.rectTransform.localPosition.x,
                metaTagTxt.rectTransform.localPosition.y - 50,
                metaTagTxt.rectTransform.localPosition.z);

            metaTagTxt.DOFade(1, 1.0f)
                .OnStart(() =>
                {
                    metaTagTxt.rectTransform.DOLocalMoveY(metaTagTxt.rectTransform.localPosition.y + 50, 0.8f)
                        .SetEase(Ease.OutCubic);
                })
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1.5f, () =>
                    {
                        metaTagTxt.rectTransform.DOKill();

                        metaTagTxt.DOFade(0, 1.0f).OnStart(() =>
                        {
                            metaTagTxt.rectTransform.DOLocalMoveY(metaTagTxt.rectTransform.localPosition.y + 50, 0.8f)
                                .SetEase(Ease.InCubic);
                        }).OnComplete(() => { metaTagTxt.gameObject.SetActive(false); });
                    });
                });
        }
    }
}