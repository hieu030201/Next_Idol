using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Yun.Scripts.Animations
{
    public class LinearFlying2D : MonoBehaviour
    {
        private float _moveDuration = 1f;
        private float elapsedTime = 0f;
        private bool isMoving = false;
        private Vector2 _startPosition;
        private Vector2 _destination;
        
        private Action _ONMovementComplete;

        public enum EaseType
        {
            EaseInOut,
            EaseIn,
            EaseOut
        }

        public EaseType easeType = EaseType.EaseInOut;
        public float scaleTime = 0.5f;

        public void StartFly(float duration, Vector2 destination, float destinationScale,
            Action ONMovementComplete = null)
        {
            transform.localScale = new Vector3(0, 0, 0);
            _startPosition = new Vector2(transform.localPosition.x, transform.localPosition.y);
            _destination = destination;

            _moveDuration = duration;
            transform.DOScale(1, scaleTime).SetEase(Ease.OutBack).OnComplete((() =>
            {
                transform.DOScale(destinationScale, _moveDuration - scaleTime + 0.2f).SetAutoKill(true);
            })).SetAutoKill(true);
            DOVirtual.DelayedCall(scaleTime - 0.2f, (() =>
            {
                isMoving = true;
                elapsedTime = 0f;
            })).SetAutoKill(true);

            _ONMovementComplete = ONMovementComplete;
        }
        
        public void StartFly2(float duration, Vector2 destination, float destinationScale,
            Action onMovementComplete = null)
        {
            transform.localScale = new Vector3(1, 1, 1);
            
            transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f).SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
            transform.DOMoveY(transform.localPosition.y + 100, scaleTime).OnComplete((() =>
            {
                _startPosition = new Vector2(transform.localPosition.x, transform.localPosition.y);
                _destination = destination;
                
                // Debug.Log("StartFly2 Dokill");
                transform.DOKill();
                transform.DOScale(destinationScale, _moveDuration - 0.4f);
                
                _moveDuration = duration;
                isMoving = true;
                elapsedTime = 0f;
            }));

            _ONMovementComplete = onMovementComplete;
        }

        private void Update()
        {
            if (!isMoving) return;
            // Tăng thời gian đã trôi qua
            elapsedTime += Time.deltaTime;

            // Tính toán t dựa trên tỉ lệ thời gian
            var t = Mathf.Clamp01(elapsedTime / _moveDuration);
            
            // Áp dụng easing
            var easedT = ApplyEasing(t);

            // Tính toán vị trí mới theo đường thẳng
            var newPosition = Vector2.Lerp(_startPosition, _destination, easedT);
            transform.localPosition = newPosition;

            // Kiểm tra hoàn thành
            if (!(elapsedTime >= _moveDuration)) return;
            isMoving = false;
            transform.localPosition = _destination; // Đảm bảo đến đúng điểm đích

            _ONMovementComplete?.Invoke();
            gameObject.transform.DOKill();
            // Debug.Log("Destroy Flying Object");
            Destroy(gameObject);
        }

        private float ApplyEasing(float t)
        {
            switch (easeType)
            {
                case EaseType.EaseInOut:
                    return EaseInOut(t);
                case EaseType.EaseIn:
                    return EaseIn(t);
                case EaseType.EaseOut:
                    return EaseOut(t);
                default:
                    return t;
            }
        }

        private float EaseInOut(float t)
        {
            return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }

        private float EaseIn(float t)
        {
            return t * t;
        }

        private float EaseOut(float t)
        {
            return 1 - (1 - t) * (1 - t);
        }
    }
}