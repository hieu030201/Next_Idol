using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Yun.Scripts.Animations;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Utilities;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class BuildPoint : YunBehaviour
    {
        [SerializeField] private bool isShowGuide;
        [SerializeField] private GameObject cashPrefab;
        [SerializeField] private GameObject tokenPrefab;

        private TextMeshPro _priceTxt;
        private TextMeshPro _levelTxt;
        private GameObject _priceGameObject;
        private GameObject _levelGameObject;
        private GameObject _activeBorder;
        private GameObject _spriteMask;
        private List<GameObject> _spendMoneyPositionsList;

        [Button]
        public void Test()
        {
            ShowSpendMoneyAnimation();
        }

        private float _startMaskZPosition;

        protected override void Awake()
        {
            base.Awake();

            if(transform.Find("Guide_Arrow"))
                transform.Find("Guide_Arrow").gameObject.SetActive(false);
            
            if (_spriteMask)
                _spriteMask.gameObject.SetActive(false);
            // Debug.Log("Awake: " + gameObject.name);
            _priceTxt = transform.Find("Build_Point_Background_2").Find("price").Find("priceTxt")
                .GetComponent<TextMeshPro>();
            _levelTxt = transform.Find("Build_Point_Background_2").Find("level").Find("levelTxt")
                .GetComponent<TextMeshPro>();

            _priceGameObject = transform.Find("Build_Point_Background_2").Find("price").gameObject;
            _levelGameObject = transform.Find("Build_Point_Background_2").Find("level").gameObject;

            _activeBorder = transform.Find("Build_Point_Background_2").Find("Active_Border").gameObject;
            _spriteMask = transform.Find("Build_Point_Background_2").Find("Sprite_Mask").gameObject;

            _spendMoneyPositionsList = new List<GameObject>();
            _spendMoneyPositionsList.Add(transform.Find("Build_Point_Background_2").Find("point1").gameObject);
            _spendMoneyPositionsList.Add(transform.Find("Build_Point_Background_2").Find("point2").gameObject);
            _spendMoneyPositionsList.Add(transform.Find("Build_Point_Background_2").Find("point3").gameObject);

            if (_spriteMask)
                _startMaskZPosition = _spriteMask.transform.localPosition.z;
            if (_levelGameObject)
                _levelGameObject.SetActive(false);
            if (_activeBorder)
                _activeBorder.SetActive(false);

            // _priceTxt.text = Price.ToString();
            _priceTxt.text = UtilitiesFunction.FormatNumber(Price);
        }

        private int _price;

        public int Price
        {
            get => _price;
            set
            {
                _price = value;
                if (_priceTxt)
                    _priceTxt.text = UtilitiesFunction.FormatNumber(value);
            }
        }

        private int _depositNumber;

        public int DepositNumber
        {
            get => _depositNumber;
            set
            {
                _depositNumber = value;
                // _priceTxt.text = (_price - value).ToString();
                _priceTxt.text = UtilitiesFunction.FormatNumber(_price - value);;
                if (!_spriteMask) return;
                if (_spriteMask)
                    _spriteMask.gameObject.SetActive(true);
                var percent = (float)_depositNumber / (float)_price;
                var newZ = percent * _startMaskZPosition;
                _spriteMask.transform.DOLocalMoveZ(_startMaskZPosition - newZ, 0.1f);
            }
        }

        public void ShowSpendMoneyAnimation()
        {
            var cash = Instantiate(cashPrefab);
            cash.transform.position = _spendMoneyPositionsList[0].transform.position;
            cash.GetComponent<SpendMoneyAnimation>().FlyDown(_spendMoneyPositionsList);
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                GetComponent<Collider>().enabled = value;
                if (_priceGameObject)
                    _priceGameObject.SetActive(value);
                if (_activeBorder)
                    _activeBorder.SetActive(value);
                if (_levelGameObject)
                    _levelGameObject.SetActive(!value);
            }
        }

        public void SetLevelToActive(int level)
        {
            _levelTxt.text = level.ToString();
        }

        public void Show()
        {
            gameObject.transform.localScale = Vector3.zero;
            DOVirtual.DelayedCall(1f, (() => { gameObject.transform.DOScale(1, 1f).SetEase(Ease.OutBack); }))
                .SetAutoKill(true);
            gameObject.SetActive(true);
            if (isShowGuide && GetComponent<GuidePoint>())
            {
                FacilityManager.Instance.GuidesManager.OnPointShowGuide(GetComponent<GuidePoint>());
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        // public void Reset()
        // {
        //     gameObject.SetActive(true);
        // }
    }
}