using System;
using System.Collections.Generic;
using Advertising;
using DG.Tweening;
using Joystick_Pack.Scripts.Joysticks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Core;
using Yun.Scripts.UI;

namespace Yun.Scripts.Managers
{
    public enum TypeCloseEffect
    {
        None,
        FadeOut,
        FadeIn,
        FadeToBottom,
    }

    public class CanvasManager : MonoSingleton<CanvasManager>
    {
        [SerializeField] public Canvas canvas;
        [SerializeField] public GameObject popupUILayer;
        [SerializeField] public GameObject overlayUILayer;
        [SerializeField] public GameObject gamePlayUILayer;
        [SerializeField] public FloatingJoystick floatingJoystick;

        [FoldoutGroup("UI_Prefab_Normal_Theme")] [SerializeField]
        private GameObject idleGamePlayerInfoUIPrefab,
            levelUpPopupPrefab,
            settingPopupPrefab,
            shopPopupPrefab,
            dailyQuestPopupPrefab,
            buildBedRoomPopupPrefab,
            dailyGiftPopupPrefab,
            upgradePopupPrefab,
            getBoosterPopupPrefab,
            getMoneyPopupPrefab,
            getVipSoldierPopupPrefab,
            getUpgradeWorkerPopupPrefab,
            buyWorkerPopupPrefab,
            introUIPrefab,
            battleStartPopupPrefab,
            battleWarningPopupPrefab,
            alertPopupPrefab,
            skinPopupPrefab,
            takeABreakPopupPrefab,
            buyNoAdsPopupPrefab,
            battlePassPopupPrefab,
            spinWheelPopupPrefab,
            battleFundPopupPrefab,
            abilitiesPopupPrefab,
            changeMapPopupPrefab,
            getWorkerSpeedBoosterPopupPrefab,
            notEnoughCurrencyPopupPrefab,
            repairDamagedRoomPopupPrefab;

        [FoldoutGroup("UI_Prefab_Noel_Theme")] [SerializeField]
        private GameObject levelUpPopupNoelPrefab,
            idleGamePlayerInfoUINoelPrefab,
            settingPopupNoelPrefab,
            shopPopupNoelPrefab,
            dailyQuestPopupNoelPrefab,
            dailyGiftPopupNoelPrefab,
            buildBedRoomPopupNoelPrefab,
            upgradePopupNoelPrefab,
            getBoosterPopupNoelPrefab,
            getMoneyPopupNoelPrefab,
            getVipSoldierPopupNoelPrefab,
            getUpgradeWorkerPopupNoelPrefab,
            buyWorkerPopupNoelPrefab,
            alertPopupNoelPrefab,
            skinPopupNoelPrefab,
            takeABreakPopupNoelPrefab,
            buyNoAdsPopupNoelPrefab,
            battlePassPopupNoelPrefab,
            spinWheelPopupNoelPrefab,
            battleFundPopupNoelPrefab,
            abilitiesPopupNoelPrefab,
            changeMapPopupNoelPrefab,
            getWorkerSpeedBoosterPopupNoelPrefab,
            notEnoughCurrencyPopupNoelPrefab,
            repairDamagedRoomPopupNoelPrefab;

        private Dictionary<UIName.Name, GameObject> _UIsList;

        private Camera _mainCamera;

        public enum EffectType
        {
            InOutEffect,
            BubbleEffect,
            FlyUpEffect,

            // InOutEffectShort,
            InOutEffectShort,
            MovingBottomToMiddle,
        }

        private Vector2 _sizeDelta;

        protected override void Awake()
        {
            base.Awake();
            _mainCamera = Camera.main;
            _UIsList = new Dictionary<UIName.Name, GameObject>();
            _sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;

            if (FireBaseManager.Instance.showNoel)
            {
                RegisterUI(UIName.Name.IDLE_GAME_PLAYER_INFO_UI, idleGamePlayerInfoUINoelPrefab);
                RegisterUI(UIName.Name.LEVEL_UP_POPUP, levelUpPopupNoelPrefab);
                RegisterUI(UIName.Name.BUY_WORKER_POPUP, buyWorkerPopupNoelPrefab);
                RegisterUI(UIName.Name.SETTING_POPUP, settingPopupNoelPrefab);
                RegisterUI(UIName.Name.SHOP_POPUP, shopPopupNoelPrefab);
                RegisterUI(UIName.Name.DAILY_QUEST_POPUP, dailyQuestPopupNoelPrefab);
                RegisterUI(UIName.Name.BUILD_BED_ROOM_POPUP, buildBedRoomPopupNoelPrefab);
                RegisterUI(UIName.Name.DAILY_GIFT_POPUP, dailyGiftPopupNoelPrefab);
                RegisterUI(UIName.Name.UPGRADE_POPUP, upgradePopupNoelPrefab);
                RegisterUI(UIName.Name.GET_MONEY_POPUP, getMoneyPopupNoelPrefab);
                RegisterUI(UIName.Name.GET_VIP_SOLDIER_POPUP, getVipSoldierPopupNoelPrefab);
                RegisterUI(UIName.Name.GET_UPGRADE_WORKER_POPUP,
                    getUpgradeWorkerPopupNoelPrefab);
                RegisterUI(UIName.Name.GET_BOOSTER_POPUP, getBoosterPopupNoelPrefab);
                RegisterUI(UIName.Name.INTRO_UI, introUIPrefab);
                RegisterUI(UIName.Name.BATTLE_START_POPUP, battleStartPopupPrefab);
                RegisterUI(UIName.Name.BATTLE_WARNING_POPUP, battleWarningPopupPrefab);
                RegisterUI(UIName.Name.ALERT_POPUP, alertPopupNoelPrefab);
                RegisterUI(UIName.Name.SKIN_POPUP, skinPopupNoelPrefab);
                RegisterUI(UIName.Name.TAKE_A_BREAK_POPUP, takeABreakPopupNoelPrefab);
                RegisterUI(UIName.Name.BUY_NO_ADS_POPUP, buyNoAdsPopupNoelPrefab);
                RegisterUI(UIName.Name.BATTLE_PASS_POPUP, battlePassPopupNoelPrefab);
                RegisterUI(UIName.Name.SPIN_WHEEL_POPUP, spinWheelPopupNoelPrefab);
                RegisterUI(UIName.Name.BATTLE_PASS_BUY_PACK_POPUP, battleFundPopupNoelPrefab);
                RegisterUI(UIName.Name.ABILITIES_POPUP, abilitiesPopupNoelPrefab);
                RegisterUI(UIName.Name.CHANGE_MAP_POPUP, changeMapPopupNoelPrefab);
                RegisterUI(UIName.Name.GET_WORKER_SPEED_BOOSTER_POPUP, getWorkerSpeedBoosterPopupNoelPrefab);
                RegisterUI(UIName.Name.NOT_ENOUGH_CURRENCY_POPUP, notEnoughCurrencyPopupNoelPrefab);
                RegisterUI(UIName.Name.REPAIR_DAMAGED_ROOM_POPUP, repairDamagedRoomPopupPrefab);
            }
            else if (!FireBaseManager.Instance.showNoel)
            {
                RegisterUI(UIName.Name.IDLE_GAME_PLAYER_INFO_UI, idleGamePlayerInfoUIPrefab);
                RegisterUI(UIName.Name.LEVEL_UP_POPUP, levelUpPopupPrefab);
                RegisterUI(UIName.Name.BUY_WORKER_POPUP, buyWorkerPopupPrefab);
                RegisterUI(UIName.Name.SETTING_POPUP, settingPopupPrefab);
                RegisterUI(UIName.Name.SHOP_POPUP, shopPopupPrefab);
                RegisterUI(UIName.Name.DAILY_QUEST_POPUP, dailyQuestPopupPrefab);
                RegisterUI(UIName.Name.BUILD_BED_ROOM_POPUP, buildBedRoomPopupPrefab);
                RegisterUI(UIName.Name.DAILY_GIFT_POPUP, dailyGiftPopupPrefab);
                RegisterUI(UIName.Name.UPGRADE_POPUP, upgradePopupPrefab);
                RegisterUI(UIName.Name.GET_MONEY_POPUP, getMoneyPopupPrefab);
                RegisterUI(UIName.Name.GET_VIP_SOLDIER_POPUP, getVipSoldierPopupPrefab);
                RegisterUI(UIName.Name.GET_UPGRADE_WORKER_POPUP, getUpgradeWorkerPopupPrefab);
                RegisterUI(UIName.Name.GET_BOOSTER_POPUP, getBoosterPopupPrefab);
                RegisterUI(UIName.Name.INTRO_UI, introUIPrefab);
                RegisterUI(UIName.Name.BATTLE_START_POPUP, battleStartPopupPrefab);
                RegisterUI(UIName.Name.BATTLE_WARNING_POPUP, battleWarningPopupPrefab);
                RegisterUI(UIName.Name.ALERT_POPUP, alertPopupPrefab);
                RegisterUI(UIName.Name.SKIN_POPUP, skinPopupPrefab);
                RegisterUI(UIName.Name.TAKE_A_BREAK_POPUP, takeABreakPopupPrefab);
                RegisterUI(UIName.Name.BUY_NO_ADS_POPUP, buyNoAdsPopupPrefab);
                RegisterUI(UIName.Name.BATTLE_PASS_POPUP, battlePassPopupPrefab);
                RegisterUI(UIName.Name.SPIN_WHEEL_POPUP, spinWheelPopupPrefab);
                RegisterUI(UIName.Name.BATTLE_PASS_BUY_PACK_POPUP, battleFundPopupPrefab);
                RegisterUI(UIName.Name.ABILITIES_POPUP, abilitiesPopupPrefab);
                RegisterUI(UIName.Name.CHANGE_MAP_POPUP, changeMapPopupPrefab);
                RegisterUI(UIName.Name.GET_WORKER_SPEED_BOOSTER_POPUP, getWorkerSpeedBoosterPopupPrefab);
                RegisterUI(UIName.Name.NOT_ENOUGH_CURRENCY_POPUP, notEnoughCurrencyPopupPrefab);
                RegisterUI(UIName.Name.REPAIR_DAMAGED_ROOM_POPUP, repairDamagedRoomPopupNoelPrefab);
            }
        }

        public void RegisterUI(UIName.Name uiName, GameObject uiPrefab)
        {
            _UIsList[uiName] = uiPrefab;
        }

        public GameObject ShowPopup(UIName.Name uiName)
        {
            if (!_UIsList.ContainsKey(uiName))
            {
                ShowErrorLog(uiName.ToString() + " IS NOT REGISTERED YET");
                return null;
            }

            var popup = Instantiate(_UIsList[uiName], popupUILayer.transform);
            popup.name = uiName.ToString();
            popup.GetComponent<BaseUI>().Show();
            popup.GetComponent<BaseUI>().UIName = uiName;
            return popup;
        }

        public void HidePopup(UIName.Name uiName)
        {
            if (!_UIsList.ContainsKey(uiName))
            {
                ShowErrorLog(uiName.ToString() + " IS NOT REGISTERED YET");
                return;
            }

            var popup = popupUILayer.transform.Find(uiName.ToString());

            if (popup)
            {
                Destroy(popup.gameObject);
            }
            else
            {
                ShowErrorLog(uiName.ToString() + " IS NOT EXIST");
            }
        }

        public void HidePopup(UIName.Name uiName, TypeCloseEffect closeEffect)
        {
            if (!_UIsList.ContainsKey(uiName))
            {
                ShowErrorLog(uiName.ToString() + " IS NOT REGISTERED YET");
                return;
            }

            var popup = popupUILayer.transform.Find(uiName.ToString());

            if (popup)
            {
                // Find the child named "Content"  
                Transform content = popup.Find("Content");
                var bg = popup.Find("Background")?.GetComponent<Image>();
                ;
                if (content != null)
                {
                    CanvasGroup canvasGroupContent = content.GetComponent<CanvasGroup>();
                    if (canvasGroupContent == null)
                    {
                        canvasGroupContent = content.gameObject.AddComponent<CanvasGroup>();
                    }

                    RectTransform contentRect = content.GetComponent<RectTransform>();

                    switch (closeEffect)
                    {
                        case TypeCloseEffect.FadeToBottom:
                            Sequence fadeSequence = DOTween.Sequence();

                            fadeSequence.Append(canvasGroupContent.DOFade(0f, 0.2f).SetEase(Ease.Linear));
                            fadeSequence.Join(contentRect
                                .DOAnchorPosY(contentRect.anchoredPosition.y - Screen.height / 8, 0.3f)
                                .SetEase(Ease.Linear)); // Move Down 50px

                            if (bg != null)
                            {
                                fadeSequence.Join(DOTween.To(
                                    () => bg.color.a,
                                    x => bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, x),
                                    0f,
                                    0.2f
                                ).SetEase(Ease.Linear));
                            }

                            fadeSequence.OnComplete(() => { Destroy(popup.gameObject); });
                            break;
                        case TypeCloseEffect.FadeIn:
                            Sequence fadeInSequence = DOTween.Sequence();
                            // Then scale down and fade out with a smoother curve
                            fadeInSequence.Append(contentRect.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.3f)
                                .SetEase(Ease.OutCubic));
                            fadeInSequence.Join(canvasGroupContent.DOFade(0f, 0.3f).SetEase(Ease.OutCubic));

                            // Fade background with a slightly different timing for better visual harmony
                            if (bg != null)
                            {
                                fadeInSequence.Join(DOTween.To(
                                    () => bg.color.a,
                                    x => bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, x),
                                    0f,
                                    0.35f // Slightly longer fade for background
                                ).SetEase(Ease.InOutQuad));
                            }

                            fadeInSequence.OnComplete(() => { Destroy(popup.gameObject); });
                            break;
                    }
                }
                else
                {
                    ShowErrorLog("Content child is not found in " + uiName.ToString());
                }
            }
            else
            {
                ShowErrorLog(uiName.ToString() + " IS NOT EXIST");
            }
        }

        public void ZoomInAndFade(Transform popup, Transform content)
        {
            // Assuming the child has a CanvasGroup component  
            var bg = content.Find("Background");
            CanvasGroup canvasGroup = content.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                // If no CanvasGroup, add one for fading effect  
                canvasGroup = content.gameObject.AddComponent<CanvasGroup>();
            }

            RectTransform contentRect = content.GetComponent<RectTransform>();
            // Set the initial scale to 1  
            content.localScale = Vector3.one;
            Sequence sequence = DOTween.Sequence();

            sequence.Append(canvasGroup.DOFade(0.5f, 5f)); // Fades to 0 over 0.5 seconds  
            sequence.Join(contentRect.DOAnchorPosY(contentRect.anchoredPosition.y - 500f, 5f)
                .SetEase(Ease.Linear)); // Move Down 50px
            sequence.Join(content.DOScale(0.8f, 5f)); // Scales down to 80% over 0.5 seconds  

            sequence.OnComplete(() =>
            {
                Destroy(popup.gameObject); // Destroy the whole popup after animations complete  
            });
        }

        public GameObject ShowUI(UIName.Name uiName)
        {
            if (!_UIsList.ContainsKey(uiName))
            {
                ShowErrorLog(uiName.ToString() + " IS NOT REGISTERED YET");
                return null;
            }

            var ui = Instantiate(_UIsList[uiName], overlayUILayer.transform);
            ui.name = uiName.ToString();
            ui.GetComponent<BaseUI>().Show();
            ui.GetComponent<BaseUI>().UIName = uiName;
            return ui;
        }

        public void HideUI(UIName.Name uiName)
        {
            if (!_UIsList.ContainsKey(uiName))
            {
                ShowErrorLog(uiName.ToString() + " IS NOT REGISTERED YET");
                return;
            }

            var ui = overlayUILayer.transform.Find(uiName.ToString());

            if (ui)
            {
                Destroy(ui.gameObject);
            }
            else
            {
                ShowErrorLog(uiName.ToString() + " IS NOT EXIST");
            }
        }

        public GameObject GetUI(UIName.Name uiName)
        {
            var ui = overlayUILayer.transform.Find(uiName.ToString());

            if (ui)
            {
                return ui.gameObject;
            }
            else
            {
                ShowErrorLog(uiName.ToString() + " IS NOT EXIST");
                return null;
            }
        }

        public GameObject GetPopup(UIName.Name uiName)
        {
            var popup = popupUILayer.transform.Find(uiName.ToString());

            if (popup)
            {
                return popup.gameObject;
            }
            else
            {
                ShowErrorLog(uiName.ToString() + " IS NOT EXIST");
                return null;
            }
        }

        public void ShowEffect(EffectType effectName, Transform target)
        {
            if (!target.gameObject.GetComponent<CanvasGroup>())
                target.gameObject.AddComponent<CanvasGroup>();
            var canvasGroup = target.GetComponent<CanvasGroup>();
            switch (effectName)
            {
                case EffectType.InOutEffect:
                    target.transform.localScale = Vector3.zero;
                    target.gameObject.SetActive(true);
                    target.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
                    canvasGroup.alpha = 0;
                    canvasGroup.DOFade(1, 1);
                    Debug.Log("CanvasManager InOutEffect");
                    break;
                case EffectType.BubbleEffect:
                    target.transform.localScale = Vector3.zero;
                    target.gameObject.SetActive(true);
                    target.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack).SetUpdate(true);
                    DOVirtual.DelayedCall(2f, (() => target.gameObject.SetActive(false)));
                    Debug.Log("CanvasManager BubbleEffect");
                    break;
                case EffectType.FlyUpEffect:
                    break;
                case EffectType.InOutEffectShort:
                    break;
                case EffectType.MovingBottomToMiddle:
                    Vector3 originalPosition = target.position;
                    Vector3 targetPosition =
                        new Vector3(originalPosition.x, Screen.height / 2,
                            originalPosition.z); // Căn giữa theo chiều Y  

                    var movingCanvasGroup = target.gameObject.GetComponent<CanvasGroup>();
                    if (movingCanvasGroup == null)
                    {
                        movingCanvasGroup = target.gameObject.AddComponent<CanvasGroup>();
                    }

                    movingCanvasGroup.alpha = 0;
                    target.gameObject.SetActive(true);

                    target.DOMoveY(targetPosition.y, 0.5f).SetEase(Ease.OutBack).SetUpdate(true).OnStart(() =>
                    {
                        target.position =
                            new Vector3(originalPosition.x, -Screen.height / 2,
                                originalPosition.z); // Đưa đối tượng ra dưới màn hình  
                    });

                    // Tăng alpha lên từ 0 đến 1, cùng lúc di chuyển  
                    movingCanvasGroup.DOFade(1f, 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);

                    Debug.Log("CanvasManager MovingBottomToMiddle");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effectName), effectName, null);
            }
        }

        public Vector2 ConvertWorldPositionToScreenPosition(Vector3 worldPosition)
        {
            Vector2 viewportPosition = _mainCamera.WorldToViewportPoint(worldPosition);
            var screenPoint = new Vector2(
                ((viewportPosition.x * _sizeDelta.x) - (_sizeDelta.x * 0.5f)),
                ((viewportPosition.y * _sizeDelta.y) - (_sizeDelta.y * 0.5f)));
            return screenPoint;
        }

        private void ShowErrorLog(string message)
        {
            message = ": " + message;
            Debug.LogWarning($"[{this.GetType().Name}] {message}");
        }
    }
}