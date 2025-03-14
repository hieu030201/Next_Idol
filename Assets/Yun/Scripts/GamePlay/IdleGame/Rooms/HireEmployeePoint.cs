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
    public class HireEmployeePoint : YunBehaviour
    {
        [SerializeField] private TextMeshPro priceTxt;
        [SerializeField] private TextMeshPro levelTxt;
        [SerializeField] private int levelActive;
        [SerializeField] private int priceNumber;
        [SerializeField] public Employee employee;
        [SerializeField] private GameObject price;
        [SerializeField] private GameObject level;
        [SerializeField] private GameObject activeBorder;
        [SerializeField] private GameObject spriteMask;
        [SerializeField] private GameObject cameraPoint;
        [SerializeField] private GameObject cashPrefab;
        [SerializeField] public BaseRoom relatedRoom;

        public GameObject CameraPoint => cameraPoint;
        
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
                _depositNumber = value;
                priceTxt.text = (_price - value).ToString();
                if (!spriteMask) return;
                spriteMask.SetActive(true);
                var percent = (float) _depositNumber / (float) _price;
                var newZ = percent * _startMaskZPosition;
                var tween = spriteMask.transform.DOLocalMoveZ(_startMaskZPosition - newZ, 0.1f);
                AddTweenToTweenManager(tween);
            }
        }
        
        public void ShowSpendMoneyAnimation()
        {
            var cash = Instantiate(cashPrefab);
            cash.transform.position = _spendMoneyPositionsList[0].transform.position;
            cash.GetComponent<SpendMoneyAnimation>().FlyDown(_spendMoneyPositionsList);
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
            
            level.SetActive(false);
            
            _spendMoneyPositionsList = new List<GameObject>();
            _spendMoneyPositionsList.Add(transform.Find("Hire_Employee_Point_Background").Find("point1").gameObject);
            _spendMoneyPositionsList.Add(transform.Find("Hire_Employee_Point_Background").Find("point2").gameObject);
            _spendMoneyPositionsList.Add(transform.Find("Hire_Employee_Point_Background").Find("point3").gameObject);
            
            spriteMask.SetActive(false);
        }

        public void Init()
        {
            Price = priceNumber;
            if (employee)
            {
                employee.gameObject.SetActive(false);
            }
            if (spriteMask)
                _startMaskZPosition = spriteMask.transform.localPosition.z;
            if(level)
                level.SetActive(false);
            if(activeBorder)
                activeBorder.SetActive(false);
            IsActive = true;
        }

        public void ActiveEmployee()
        {
            if (!employee) return;
            employee.gameObject.SetActive(true);
            employee.ShowStartEffect();
            employee.StartWorking();
        }
        
        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                GetComponent<Collider>().enabled = value;
                if (price)
                    price.SetActive(value);
                if (activeBorder)
                    activeBorder.SetActive(value);
                if (level)
                    level.SetActive(!value);
            }
        }

        public void SetLevelToActive(int levelNumber)
        {
            levelTxt.text = levelNumber.ToString();
        }
    }
}