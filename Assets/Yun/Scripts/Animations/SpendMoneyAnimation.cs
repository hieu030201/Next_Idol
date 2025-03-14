using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Yun.Scripts.Animations
{
    public class SpendMoneyAnimation : MonoBehaviour
    {
        private float _duration = 0.5f; // Thời gian di chuyển

        [Button]
        public void Test()
        {
            // FlyDown();
        }

        void Start()
        {
        }

        private Tween _flyDownTween;
        private List<Vector3> _positions;
        public void FlyDown(List<GameObject> pointList, float duration = 0)
        {
            if (duration != 0)
                _duration = duration;
            _positions = new List<Vector3>();
            if (pointList != null)
            {
                foreach (var point in pointList)
                {
                    _positions.Add(point.transform.position);
                }
            }

            if (_positions.Count > 0)
            {
                _flyDownTween = DOVirtual.Float(0, 1, _duration, t =>
                {
                    // Nội suy Bezier bậc 2 dựa trên ba điểm A, B, C
                    if (transform)
                    {
                        // Debug.Log("FlyDown: " + pointList);
                        var currentPos = CalculateQuadraticBezierPoint(t, _positions[0],
                            _positions[1], _positions[2]);
                        transform.position = currentPos;
                    }
                }).SetEase(Ease.Linear).OnComplete((() =>
                {
                    _flyDownTween?.Kill();
                    transform.DOKill();
                    Destroy(gameObject);
                }));

                transform.DORotate(new Vector3(0, 179, 0), _duration);
            }
            else
            {
                _flyDownTween?.Kill();
                transform.DOKill();
                Destroy(gameObject);
            }
            // Sử dụng DOVirtual.Float để thực hiện nội suy Bezier
            
        }

        private void OnDestroy()
        {
            _flyDownTween?.Kill();
        }

        // Hàm tính toán vị trí dựa trên công thức đường cong Bezier bậc 2
        Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            // Công thức Bezier bậc 2: B(t) = (1 - t)^2 * p0 + 2 * (1 - t) * t * p1 + t^2 * p2
            var u = 1 - t;
            var tt = t * t;
            var uu = u * u;

            var point = uu * p0; // (1 - t)^2 * p0
            point += 2 * u * t * p1; // 2 * (1 - t) * t * p1
            point += tt * p2; // t^2 * p2

            return point;
        }
    }
}