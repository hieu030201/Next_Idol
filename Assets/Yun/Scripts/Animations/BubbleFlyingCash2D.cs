using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.UI.GamePlay.IdleGame;
using Random = UnityEngine.Random;

namespace Yun.Scripts.Animations
{
    public class BubbleFlyingCash2D : MonoBehaviour
    {
        [SerializeField] private YunTextShadow textShadow;

        private void Awake()
        {
            textShadow.DisplayText = "";
        }

        private int _value;
        private int _increaseValue;
        private int _currentValue;
        public void SetValue(int value)
        {
            _value = value;
            _increaseValue = value / 20;
            _currentValue = 0;
            textShadow.DisplayText = "+" + _currentValue.ToString();
            InvokeRepeating(nameof(IncreaseToValue), 0.025f, 0.05f);
        }

        private void IncreaseToValue()
        {
            _currentValue += _increaseValue;
            if (_currentValue >= _value)
            {
                _currentValue = _value;
                textShadow.DisplayText = "+" + _currentValue.ToString();
                CancelInvoke(nameof(IncreaseToValue));
                return;
            }
            textShadow.DisplayText = "+" + _currentValue.ToString();
        }

        public void StartFly()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1, 1).SetEase(Ease.OutBack);
            transform.DOLocalMoveY(transform.localPosition.y + 100f, 1f);

            DOVirtual.DelayedCall(3, (() => { Destroy(gameObject); })).SetAutoKill(true);
        }
    }
}