using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class ScaleBorderOnPoint : YunBehaviour
    {
        [SerializeField] private float moveDuration = 0.5f;

        private float _startScale;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            var transform1 = transform;
            _startScale = transform1.localScale.x;
            var sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(_startScale * 1.05f, moveDuration));
            sequence.Append(transform.DOScale(_startScale, moveDuration));
            sequence.SetLoops(-1);
            AddTweenToTweenManager(sequence);
        }
    }
}