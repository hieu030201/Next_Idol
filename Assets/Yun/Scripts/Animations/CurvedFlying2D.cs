using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Yun.Scripts.Animations
{
    public class CurvedFlying2D : MonoBehaviour
    {
        // Thay moveSpeed bằng moveDuration
        private float _moveDuration = 1f;
        private float elapsedTime = 0f;
        private bool isMoving = false;
        private List<Vector2> _pathList;

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
            var startPoint = new Vector2(transform.localPosition.x, transform.localPosition.y);
            var middleX = startPoint.x - (destination.x - startPoint.x) / 3;
            var middleY = startPoint.y + (destination.y - startPoint.y) * 3 / 4;
            var middlePosition = new Vector2(middleX, middleY);
            _pathList = new List<Vector2>() {startPoint, middlePosition, destination};

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
                var startPoint = new Vector2(transform.localPosition.x, transform.localPosition.y);
                var middleX = startPoint.x - (destination.x - startPoint.x) / 3;
                var middleY = startPoint.y + (destination.y - startPoint.y) * 3 / 4;
                var middlePosition = new Vector2(middleX, middleY);
                _pathList = new List<Vector2>() {startPoint, middlePosition, destination};
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

            // Tính toán vị trí mới
            var newPosition = CalculateQuadraticBezierPoint(easedT, _pathList[0], _pathList[1], _pathList[2]);
            transform.localPosition = newPosition;

            // Kiểm tra hoàn thành
            if (!(elapsedTime >= _moveDuration)) return;
            isMoving = false;
            transform.localPosition = _pathList[2]; // Đảm bảo đến đúng điểm đích
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

        private Vector2 CalculateQuadraticBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            var u = 1 - t;
            var tt = t * t;
            var uu = u * u;

            var point = uu * p0;
            point += 2 * u * t * p1;
            point += tt * p2;

            return point;
        }
    }
}