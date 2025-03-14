using System.Collections;
using System.Collections.Generic;
using Advertising;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class IntroUI : BaseUI
    {
        [SerializeField] private GameObject dialogBox1;
        [SerializeField] private GameObject dialogBox2;
        [SerializeField] private GameObject dialogBox3;
        [SerializeField] private GameObject dialogBox4;
        [SerializeField] private GameObject moveElement;
        [SerializeField] private GameObject closeBtn;
        [SerializeField] private TextMeshProUGUI dialogTxt;
        [SerializeField] private GameObject nativeAds;


        protected override void Awake()
        {
            base.Awake();
            dialogBox1.SetActive(false);
            dialogBox2.SetActive(false);
            dialogBox3.SetActive(false);
            dialogBox4.SetActive(false);
            moveElement.SetActive(false);
            closeBtn.SetActive(false);
            moveElement.transform.localPosition = new Vector3(0, -1339, 0);

            if (nativeAds)
                nativeAds.SetActive(false);
        }

        private Tween _tweenToContinue;
        private bool _isSkipable;

        public override void Show()
        {
        }

        private bool _isShowed;

        public void StartShow()
        {
            if (_isShowed)
                return;
            _isShowed = true;
            moveElement.SetActive(true);
            closeBtn.SetActive(true);
            dialogBox1.SetActive(true);
            moveElement.transform.DOLocalMove(new Vector3(0, -862, 0), 1);
            DOVirtual.DelayedCall(4, (() =>
            {
                moveElement.transform.DOLocalMove(new Vector3(0, -558, 0), 1);
                dialogBox2.SetActive(true);
            }));
            DOVirtual.DelayedCall(8, (() =>
            {
                moveElement.transform.DOLocalMove(new Vector3(0, -90, 0), 1);
                dialogBox3.SetActive(true);
                _isSkipable = true;
            }));
            DOVirtual.DelayedCall(10, (() =>
            {
                moveElement.transform.DOLocalMove(new Vector3(0, 22, 0), 1);
                dialogBox4.SetActive(true);
            }));

            _tweenToContinue = DOVirtual.DelayedCall(20, (OnTouchToContinue));

            if (nativeAds && FireBaseManager.Instance && FireBaseManager.Instance.showNativeIntro)
                nativeAds.SetActive(true);
        }

        private bool _isShowDialog;
        private Tween _dialogTween;

        public void ShowDialog(string content, float delay = 5)
        {
            _isShowDialog = true;
            dialogTxt.text = content;

            dialogBox1.SetActive(false);
            dialogBox2.SetActive(false);
            dialogBox4.SetActive(false);
            moveElement.transform.localPosition = new Vector3(0, -558, 0);
            moveElement.SetActive(true);
            // closeBtn.SetActive(true);

            moveElement.transform.DOLocalMove(new Vector3(0, -90, 0), 1);
            dialogBox3.SetActive(true);
            _isSkipable = true;

            _dialogTween?.Kill();
            _dialogTween = DOVirtual.DelayedCall(delay, (() =>
            {
                if (nativeAds)
                    nativeAds.SetActive(false);
                moveElement.SetActive(false);
            }));
        }

        public void OnTouchToContinue()
        {
            if (!_isSkipable)
                return;
            _tweenToContinue?.Kill();
            moveElement.SetActive(false);
            closeBtn.SetActive(false);
            if (!_isShowDialog)
                FacilityManager.Instance.StartIntro();
        }

        public void HideNativeAds()
        {
            if (nativeAds)
                nativeAds.SetActive(false);
        }

        public override void Hide()
        {
        }
    }
}