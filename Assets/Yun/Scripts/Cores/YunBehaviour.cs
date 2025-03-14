using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.Cores
{
    public class YunBehaviour : MonoBehaviour
    {
        protected Camera MainCamera;

        protected virtual void Start()
        {
        }

        protected virtual void Awake()
        {
            MainCamera = Camera.main;
            InitProperties();
        }

        protected virtual void InitProperties()
        {
        }

        public enum YunEffectType
        {
            InOutEffect,
            BubbleEffect,
            BubblePopupEffect,
            FlyUpEffect,
            InOutEffectShort,
            FullFlyUpEffect,
            FlyUpEffect2,
        }

        protected virtual void OnDestroy()
        {
            KillAllTween();
        }

        private void KillAllTween()
        {
            foreach (var variable in TweenManager)
            {
                variable.Value?.Kill();
            }
        }
        
        private int _countTween;

        private Dictionary<string, Tween> TweenManager { get; } = new();

        protected void AddTweenToTweenManager(Tween tween, string tweenName = "")
        {
            if (tweenName == "")
            {
                tweenName = "tween" + _countTween;
                _countTween++;
            }

            TweenManager[tweenName] = tween;
        }

        protected void ShowEffect(YunEffectType effectName, Transform target)
        {
            if (!target.gameObject.GetComponent<CanvasGroup>())
                target.gameObject.AddComponent<CanvasGroup>();
            var canvasGroup = target.GetComponent<CanvasGroup>();
            switch (effectName)
            {
                case YunEffectType.InOutEffect:
                    target.transform.localScale = Vector3.zero;
                    target.gameObject.SetActive(true);
                    var tween = target.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
                    AddTweenToTweenManager(tween);
                    canvasGroup.alpha = 0.7f;
                    var inOutEffectTween = canvasGroup.DOFade(1, 1);
                    AddTweenToTweenManager(inOutEffectTween);
                    break;
                case YunEffectType.BubbleEffect:
                    target.transform.localScale = Vector3.zero;
                    target.gameObject.SetActive(true);
                    var bubbleEffectTween = target.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
                    AddTweenToTweenManager(bubbleEffectTween);
                    var bubbleEffectTween2 =
                        DOVirtual.DelayedCall(2, (() => target.gameObject.SetActive(false)));
                    AddTweenToTweenManager(bubbleEffectTween2);
                    break;
                case YunEffectType.BubblePopupEffect:
                    var originalScale = target.transform.localScale.x;
                    target.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    target.gameObject.SetActive(true);
                    var bubblePopupEffectTween = target.transform.DOScale(originalScale, 0.7f).SetEase(Ease.OutBack)
                        .SetUpdate(true);
                    AddTweenToTweenManager(bubblePopupEffectTween);
                    break;
                case YunEffectType.FlyUpEffect:
                    RectTransform rectTransform = target.GetComponent<RectTransform>();
                    
                    Vector3 originalPosition = rectTransform.anchoredPosition;
                    
                    Vector3 startPosition = originalPosition - new Vector3(0, Screen.height / 8, 0);
                    rectTransform.anchoredPosition = startPosition;

                    canvasGroup.alpha = 0f;
                    
                    target.gameObject.SetActive(true);
                    
                    Sequence showSequence = DOTween.Sequence();
                    
                    showSequence.Append(canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad)); // Fade in over 0.4 seconds with smooth easing
                    showSequence.Join(rectTransform.DOAnchorPosY(originalPosition.y, 0.25f).SetEase(Ease.OutQuad)); // Move up to the original position with smooth easing

                    float bounceHeight = 15f; 
                    float bounceDuration = 0.2f; 

                    showSequence.Append(rectTransform.DOAnchorPosY(originalPosition.y + bounceHeight, bounceDuration).SetEase(Ease.OutQuad));
                    showSequence.Append(rectTransform.DOAnchorPosY(originalPosition.y, bounceDuration).SetEase(Ease.OutBack)); 
                    break;
                case YunEffectType.FullFlyUpEffect:
                    RectTransform rectTransformFullFly = target.GetComponent<RectTransform>();
                    
                    Vector3 originalPositionFullFly = rectTransformFullFly.anchoredPosition;
                    
                    Vector3 startPositionFullFly = originalPositionFullFly - new Vector3(0, Screen.height / 4, 0);
                    rectTransformFullFly.anchoredPosition = startPositionFullFly;
                    
                    target.gameObject.SetActive(true);
                    
                    Sequence showSequenceFullFly = DOTween.Sequence();
                    
                    showSequenceFullFly.Append(canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad)); 
                    showSequenceFullFly.Join(rectTransformFullFly.DOAnchorPosY(originalPositionFullFly.y, 0.25f).SetEase(Ease.OutQuad));
                    break;
                case YunEffectType.InOutEffectShort:
                    break;
                case YunEffectType.FlyUpEffect2:
                    RectTransform rectTransform2 = target.GetComponent<RectTransform>();
                    
                    Vector3 originalPosition2 = rectTransform2.anchoredPosition;
                    
                    Vector3 startPosition2 = originalPosition2 - new Vector3(0, Screen.height / 8, 0);
                    rectTransform2.anchoredPosition = startPosition2;

                    canvasGroup.alpha = 0f;
                    
                    target.gameObject.SetActive(true);
                    
                    Sequence showSequence2 = DOTween.Sequence();
                    
                    showSequence2.Append(canvasGroup.DOFade(1f, 0.35f).SetEase(Ease.OutQuad)); // Fade in over 0.4 seconds with smooth easing
                    showSequence2.Join(rectTransform2.DOAnchorPosY(originalPosition2.y, 0.35f).SetEase(Ease.OutQuad)); // Move up to the original position with smooth easing

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effectName), effectName, null);
            }
        }

        protected bool _isShowErrorLog = true;

        protected void ShowErrorLog(string message)
        {
            if (!_isShowErrorLog)
                return;
            message = ": " + message;
            Debug.LogError($"[{this.GetType().Name}] {message}");
        }

        protected SaveIdleGameData GameData
        {
            get => FacilityManager.Instance.GameSaveLoad.GameData;
        }
    }
}