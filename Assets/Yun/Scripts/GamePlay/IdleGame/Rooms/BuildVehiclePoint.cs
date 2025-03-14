using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Animations;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Employees;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class BuildVehiclePoint : YunBehaviour
    {
        [SerializeField] private TextMeshPro priceTxt;
        [SerializeField] private int priceNumber;
        [SerializeField] private GameObject spriteMask;
        [SerializeField] private GameObject cashPrefab;

        public enum VehicleType
        {
            Tank,
            Missile,
            Armored
        }

        private List<GameObject> _spendMoneyPositionsList;

        private int _price;

        public int Price
        {
            get => _price;
            set
            {
                _price = value;
                priceTxt.text = value.ToString();
            }
        }

        private int _depositNumber;

        public int DepositNumber
        {
            get => _depositNumber;
            set
            {
                if (_depositNumber != value)
                {
                    _depositNumber = value;
                    priceTxt.text = (_price - value).ToString();
                    if (!spriteMask) return;
                    if (spriteMask)
                        spriteMask.gameObject.SetActive(true);
                    var percent = (float) _depositNumber / (float) _price;
                    var newZ = percent * _startMaskZPosition;
                    var tween = spriteMask.transform.DOLocalMoveZ(_startMaskZPosition - newZ, 0.1f);
                    AddTweenToTweenManager(tween);
                }
            }
        }

        public void ShowSpendMoneyAnimation(float duration = 0)
        {
            var cash = Instantiate(cashPrefab);
            cash.transform.position = _spendMoneyPositionsList[0].transform.position;
            cash.GetComponent<SpendMoneyAnimation>().FlyDown(_spendMoneyPositionsList, duration);
        }

        public int GetDepositLeft()
        {
            return _price - _depositNumber;
        }

        private float _startMaskZPosition;

        protected override void Awake()
        {
            base.Awake();
            
            if(transform.Find("Guide_Arrow"))
                transform.Find("Guide_Arrow").gameObject.SetActive(false);

            _spendMoneyPositionsList = new List<GameObject>
            {
                transform.Find("Build_Point_Background_3").Find("point1").gameObject,
                transform.Find("Build_Point_Background_3").Find("point2").gameObject,
                transform.Find("Build_Point_Background_3").Find("point3").gameObject
            };

            if (spriteMask)
                spriteMask.gameObject.SetActive(false);
        }

        public void Init()
        {
            Price = priceNumber;

            if (spriteMask)
                _startMaskZPosition = spriteMask.transform.localPosition.z;
            IsActive = true;
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                GetComponent<Collider>().enabled = value;
            }
        }
    }
}