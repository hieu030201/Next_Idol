using Advertising;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Joystick_Pack.Scripts.Joysticks
{
    public class FloatingJoystick : Joystick
    {
        public UnityAction OnPointerUpAction;
        protected override void Start()
        {
            base.Start();
            background.gameObject.SetActive(false);
        }

        private Tween _tweenToShowIntersAd;
        public bool IsPointerDown;
        public override void OnPointerDown(PointerEventData eventData)
        {
            // Debug.Log("OnPointerDown");
            _tweenToShowIntersAd?.Kill();
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            // background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
            IsPointerDown = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            // Debug.Log("OnPointerUp");
            _tweenToShowIntersAd?.Kill();
            // _tweenToShowIntersAd = DOVirtual.DelayedCall(15, AdsManager.Instance.ShowInterstitialAd);
            OnPointerUpAction();
            background.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
            IsPointerDown = false;
        }
    }
}