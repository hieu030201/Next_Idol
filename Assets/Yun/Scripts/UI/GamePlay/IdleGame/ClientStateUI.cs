using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Cores;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class ClientStateUI : YunBehaviour
    {
        private GameObject _connectedBg;
        private GameObject _notConnectedBg;
        // [SerializeField] private GameObject bg;

        private const int LoopCount = 2;
        private float _bgStartScale;

        protected override void Awake()
        {
            base.Awake();

            if (transform.Find("Connected_Bg"))
                _connectedBg = transform.Find("Connected_Bg").gameObject;
            if (transform.Find("Not_Connected_Bg"))
                _notConnectedBg = transform.Find("Not_Connected_Bg").gameObject;
            if (_connectedBg)
                _connectedBg.SetActive(false);
            if (_connectedBg)
                _bgStartScale = _connectedBg.transform.localScale.x;
        }

        private Tween _connectAnimTween;

        public void ShowConnectAnim(float duration = 1)
        {
            if (!_connectedBg) return;
            _connectedBg.GetComponent<Image>().fillAmount = 0f;
            _connectedBg.gameObject.SetActive(true);
            _connectAnimTween?.Kill();
            _connectAnimTween = _connectedBg.GetComponent<Image>().DOFillAmount(1f, duration);
        }

        public void Show()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 0.7f).SetEase(Ease.OutBack);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            transform.DOKill();
        }

        private const float ScaleDuration = 0.2f;
        private const float ScaleAmount = 1.3f;

        public void ShowConnectedStatus()
        {
            if (_connectedBg)
            {
                _connectedBg.GetComponent<Image>().fillAmount = 1f;
                _connectedBg.gameObject.SetActive(true);
                _notConnectedBg.gameObject.SetActive(false);

                // Tạo sequence để quản lý chuỗi hoạt ảnh
                var scaleSequence = DOTween.Sequence();

                // Thêm hoạt ảnh scale ra
                scaleSequence.Append(
                    _connectedBg.transform.DOScale(Vector3.one * ScaleAmount * _bgStartScale, ScaleDuration));

                // Thêm hoạt ảnh scale vào
                scaleSequence.Append(_connectedBg.transform.DOScale(Vector3.one * _bgStartScale, ScaleDuration));

                // Đặt số lần lặp lại
                scaleSequence.SetLoops(LoopCount, LoopType.Restart);

                // Bắt đầu chuỗi hoạt ảnh
                scaleSequence.Play();
            }
        }

        public void HideConnectAnim()
        {
            if (!_connectedBg) return;
            _connectedBg.gameObject.SetActive(false);
            _connectedBg.GetComponent<Image>().DOKill();
        }
    }
}