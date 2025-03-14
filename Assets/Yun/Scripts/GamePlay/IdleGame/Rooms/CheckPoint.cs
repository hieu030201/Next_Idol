using System;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Animations;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class CheckPoint : YunBehaviour
    {
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject activeIcon;
        [SerializeField] private GameObject deactivateIcon;

        private Tween _scaleUpTween;
        private Tween _scaleDownTween;
        private bool _isStepIn;

        public virtual void StepIn()
        {
            _isStepIn = true;
            if (activeIcon)
                activeIcon.SetActive(true);
            if (deactivateIcon)
                deactivateIcon.SetActive(false);
            _scaleDownTween?.Kill();
            _scaleUpTween?.Kill();
            // _scaleUpTween = background.transform.DOScale(1.15f, 0.4f).SetEase(Ease.OutBack);
        }

        public virtual void StepOut()
        {
            _isStepIn = false;
            _scaleDownTween?.Kill();
            _scaleUpTween?.Kill();
            // _scaleDownTween = background.transform.DOScale(1, 0.4f);
            if (activeIcon)
                activeIcon.SetActive(false);
            if (deactivateIcon)
                deactivateIcon.SetActive(true);
        }

        public void Active()
        {
            if (activeIcon)
                activeIcon.SetActive(true);
            if (deactivateIcon)
                deactivateIcon.SetActive(false);
            if (background)
                background.GetComponent<FlutterAnimation>().StartFlutter(1.15f, 0.8f);
        }

        public void Deactivate()
        {
            if (activeIcon)
                activeIcon.SetActive(false);
            if (deactivateIcon)
                deactivateIcon.SetActive(true);
            if (background)
                background.GetComponent<FlutterAnimation>().StopFlutter();
        }

        private float _startBackgroundAlpha = 0.39f;
        private float _startIconAlpha = 0.3f;
        private GameObject _bgCircle;

        protected override void Awake()
        {
            base.Awake();
            
            if(transform.Find("Guide_Arrow"))
                transform.Find("Guide_Arrow").gameObject.SetActive(false);
            
            if (activeIcon)
                activeIcon.SetActive(false);
            if (deactivateIcon)
                deactivateIcon.SetActive(true);
            
            if(!_isInitColor)
                InitColor();
        }

        private bool _isInitColor;
        private void InitColor()
        {
            _isInitColor = true;
            
            if (deactivateIcon)
                _startIconAlpha = deactivateIcon.GetComponent<SpriteRenderer>().color.a;
            
            if (background && background.transform.Find("Bg_Circle"))
            {
                _bgCircle = background.transform.Find("Bg_Circle").gameObject;
                // _startBackgroundAlpha = _bgCircle.GetComponent<SpriteRenderer>().color.a;
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if(!_isInitColor)
                    InitColor();
                
                _isActive = value;
                if(_isStepIn)
                    return;
                if (value)
                {
                    var r = deactivateIcon.GetComponent<SpriteRenderer>().color.r;
                    var g = deactivateIcon.GetComponent<SpriteRenderer>().color.g;
                    var b = deactivateIcon.GetComponent<SpriteRenderer>().color.b;
                    if (deactivateIcon)
                        deactivateIcon.GetComponent<SpriteRenderer>().color = new Color(r, g, b, _startIconAlpha);

                    var r2 = deactivateIcon.GetComponent<SpriteRenderer>().color.r;
                    var g2 = deactivateIcon.GetComponent<SpriteRenderer>().color.g;
                    var b2 = deactivateIcon.GetComponent<SpriteRenderer>().color.b;
                    if (_bgCircle)
                        _bgCircle.GetComponent<SpriteRenderer>().color = new Color(r2, g2, b2, _startBackgroundAlpha);
                }
                else
                {
                    if(_startIconAlpha == 0)
                        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                    var r = deactivateIcon.GetComponent<SpriteRenderer>().color.r;
                    var g = deactivateIcon.GetComponent<SpriteRenderer>().color.g;
                    var b = deactivateIcon.GetComponent<SpriteRenderer>().color.b;
                    if (deactivateIcon)
                    {
                        deactivateIcon.GetComponent<SpriteRenderer>().color =
                            new Color(r, g, b, _startIconAlpha * 0.3f);
                    }

                    var r2 = deactivateIcon.GetComponent<SpriteRenderer>().color.r;
                    var g2 = deactivateIcon.GetComponent<SpriteRenderer>().color.g;
                    var b2 = deactivateIcon.GetComponent<SpriteRenderer>().color.b;
                    if (_bgCircle)
                    {
                        _bgCircle.GetComponent<SpriteRenderer>().color =
                            new Color(r2, g2, b2, _startBackgroundAlpha * 0.3f);
                    }
                }
            }
        }
    }
}