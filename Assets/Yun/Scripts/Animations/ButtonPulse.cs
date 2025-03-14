using System;
using DG.Tweening;
using UnityEngine;

namespace Yun.Scripts.Animations
{
    public class ButtonPulse : MonoBehaviour
    {
        [SerializeField] private float minScale = 0.95f;
        [SerializeField] private float maxScale = 1.05f;
        [SerializeField] private float pulseSpeed = 2f;
        [SerializeField] private AnimationCurve pulseCurve;

        private RectTransform _rectTransform;
        private Vector3 _originalScale;
        private Tween _tween;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
            _rectTransform.transform.localScale = _originalScale * minScale;

            // Sử dụng DOTween để tạo animation mượt
            _tween = DOTween.Sequence()
                .SetLoops(-1) // Loop vô hạn
                .Append(_rectTransform.DOScale(_originalScale * maxScale, pulseSpeed / 2)
                    .SetEase(Ease.InOutSine))
                .Append(_rectTransform.DOScale(_originalScale * minScale, pulseSpeed / 2)
                    .SetEase(Ease.InOutSine));
        }

        private void OnDestroy()
        {
            _rectTransform.DOKill();
            _tween?.Kill();
        }
    }
}