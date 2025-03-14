using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Random = UnityEngine.Random;

namespace Yun.Scripts.Animations
{
    public class FlutterAnimation : MonoBehaviour
    {
        [SerializeField] private bool isStartFlutterWhenAwake;
        [SerializeField] private float scale = 1.1f;
        [SerializeField] private float duration = 1;
        private float _startScale;

        private void Start()
        {
        }

        private void Awake()
        {
            _startScale = transform.localScale.x;
            if (isStartFlutterWhenAwake)
                StartFlutter(scale, duration);
        }

        private bool _isFluttering;
        private Sequence _scaleSequence;

        public void StartFlutter(float scaleAmount, float scaleDuration)
        {
            if (_isFluttering)
                return;
            _isFluttering = true;
            // Debug.Log("StartFlutter: " + gameObject.name);
            transform.localScale = new Vector3(_startScale, _startScale, _startScale);
            _scaleSequence?.Kill();
            _scaleSequence = DOTween.Sequence();
            _scaleSequence.Append(transform.DOScale(Vector3.one * scaleAmount * _startScale, scaleDuration));
            _scaleSequence.Append(transform.DOScale(Vector3.one * _startScale, scaleDuration));
            _scaleSequence.SetLoops(-1, LoopType.Restart);
            _scaleSequence.Play();
        }

        public void StopFlutter()
        {
            transform.localScale = new Vector3(_startScale, _startScale, _startScale);
            _isFluttering = false;
            _scaleSequence?.Kill();
        }

        private void OnDestroy()
        {
            StopFlutter();
            transform.DOKill();
        }
    }
}