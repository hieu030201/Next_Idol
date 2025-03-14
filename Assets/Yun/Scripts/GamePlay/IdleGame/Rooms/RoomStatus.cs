using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Animations;

namespace Yun.Scripts.GamePlay.IdleGame.Clients
{
    public class RoomStatus : MonoBehaviour
    {
        [SerializeField] private GameObject desertStatus;
        [SerializeField] private RadialFill radialFill;
        [SerializeField] private GameObject statusActive;

        private float _bgStartScale;

        private void Awake()
        {
            gameObject.SetActive(false);
            // desertStatus.SetActive(false);
            statusActive.SetActive(false);

            _bgStartScale = transform.localScale.x;
            
            HideConnectAnimation();
        }

        private Vector3 _startDistance;
        private Quaternion _startRotation;
        private Transform _parentTransform;

        public void InitTransform(Transform parentTransform)
        {
            _parentTransform = parentTransform;
            var statusTransform = transform;
            _startRotation = statusTransform.rotation;
            _startDistance = _parentTransform.position - statusTransform.position;
        }

        public void Reposition()
        {
            transform.rotation = _startRotation;
            transform.position = _parentTransform.position - _startDistance;
        }

        private bool _isReposition;

        public void ActiveReposition()
        {
            _isReposition = true;
        }

        public void DeactivateReposition()
        {
            _isReposition = false;
        }

        private void Update()
        {
            if (!_isReposition)
                return;
            Reposition();
        }

        private void HideAllStatus()
        {
            gameObject.SetActive(false);
            desertStatus.SetActive(false);
        }

        public void ShowConnectedStatus()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            statusActive.SetActive(true);
            radialFill.ShowFullFill();
            // Tạo sequence để quản lý chuỗi hoạt ảnh
            var scaleSequence = DOTween.Sequence();

            // Thêm hoạt ảnh scale ra
            scaleSequence.Append(
                transform.DOScale(Vector3.one * 1.3f * _bgStartScale, 0.3f));

            // Thêm hoạt ảnh scale vào
            scaleSequence.Append(transform.DOScale(Vector3.one * _bgStartScale, 0.3f));

            // Đặt số lần lặp lại
            scaleSequence.SetLoops(2, LoopType.Restart);

            // Bắt đầu chuỗi hoạt ảnh
            scaleSequence.Play();
        }

        [Button]
        public void ShowConnectAnimation(float duration)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            statusActive.SetActive(false);
            radialFill.StartFill(duration);
        }

        public void HideConnectAnimation()
        {
            radialFill.StopFill();
            GetComponent<SpriteRenderer>().enabled = false;
            statusActive.SetActive(false);
        }

        private void ScaleUp()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(_bgStartScale, 1).SetEase(Ease.OutBack);
        }
    }
}