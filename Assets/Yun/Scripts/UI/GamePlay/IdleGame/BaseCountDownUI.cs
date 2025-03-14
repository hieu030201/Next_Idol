using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Cores;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class BaseCountDownUI : YunBehaviour
    {
        private TextMeshProUGUI _countDownTxt;
        private Image _timeCircle;
        private GameObject _textBg;

        private int _startCountDown;
        private int _countDownNumber;
        private Tween _delayTween;

        protected override void Awake()
        {
            base.Awake();
            
            var obj = transform.Find("GameObject");
            _textBg = obj.transform.Find("Text_BG").gameObject;
            _countDownTxt = _textBg.transform.Find("Count_Down_Txt").GetComponent<TextMeshProUGUI>();
            // Debug.Log(name);
            _timeCircle = transform.Find("Time_Circle").GetComponent<Image>();
            
            _timeCircle.fillAmount = 0;
            _countDownTxt.text = "";
            _textBg.SetActive(false);
        }

        public bool isCountingDown;
        private Action _onCountDownFinished;
        public void StartCountDown(int countDownNumber, Action onCountDownFinished = null)
        {
            _onCountDownFinished = onCountDownFinished;
            isCountingDown = true;
            _startCountDown = countDownNumber;
            _countDownNumber = countDownNumber;
            _timeCircle.gameObject.SetActive(true);
            _timeCircle.fillAmount = 0;
            _countDownTxt.text = "";
            CountDown();
            // StartCoroutine(CountDownCoroutine());
            _textBg.SetActive(true);
        }
        
        public void StopCountDown(Action actionWhenStopCountDown = null)
        {
            _delayTween?.Kill();
            actionWhenStopCountDown?.Invoke();
            gameObject.SetActive(false);
        }

        public void HideTimeCircle()
        {
            _timeCircle.gameObject.SetActive(false);
        }

        private bool _isPaused;

        private IEnumerator CountDownCoroutine()
        {
            while (_countDownNumber > 0)
            {
                if (!_isPaused)
                {
                    var minutes = (_countDownNumber % 3600) / 60;
                    var seconds = _countDownNumber % 60;

                    _countDownTxt.text = $"{minutes:D2}:{seconds:D2}";
                    
                    _countDownNumber--;
                }
                yield return new WaitForSeconds(1f);
            }

            _onCountDownFinished?.Invoke();
            _timeCircle.gameObject.SetActive(false);
            _timeCircle.fillAmount = 0;
            _countDownTxt.text = "";
            isCountingDown = false;
            _textBg.SetActive(false);
        }

        private void CountDown()
        {
            var minutes = (_countDownNumber % 3600) / 60;
            var seconds = _countDownNumber % 60;

            _countDownTxt.text = $"{minutes:D2}:{seconds:D2}";

            _countDownNumber--;

            if (_countDownNumber <= 0)
            {
                _onCountDownFinished?.Invoke();
                _timeCircle.gameObject.SetActive(false);
                _timeCircle.fillAmount = 0;
                _countDownTxt.text = "";
                isCountingDown = false;
                _textBg.SetActive(false);
                return;
            }

            _delayTween?.Kill();
            _delayTween = DOVirtual.DelayedCall(1, CountDown);
            AddTweenToTweenManager(_delayTween);
            
            _timeCircle.DOFillAmount((float) (_countDownNumber) / _startCountDown, 1f);
        }
    }
}