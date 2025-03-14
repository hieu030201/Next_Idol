using System;
using Advertising;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yun.Scripts.Animations;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class GetMoneyPoint : YunBehaviour
    {
        [SerializeField] private CashPoint cashPoint;
        [SerializeField] private GameObject hideContent;
        [SerializeField] private TextMeshPro moneyTxt;

        public int Type;

        private GameObject _noel;
        private GameObject _moneyCase;
        private GameObject _moneyBag;
        private GameObject _moneySafe;

        protected override void Awake()
        {
            base.Awake();

            // var reward = FacilityManager.Instance.LevelConfig.LevelRequirements[FacilityManager.Instance.IdleGameData.Level - 1].AdsReward;
            moneyTxt.text = "";

            _moneyCase = hideContent.transform.Find("Money_Case").gameObject;
            _moneyBag = hideContent.transform.Find("Money_Bag").gameObject;
            _moneySafe = hideContent.transform.Find("Money_Safe").gameObject;

            if (hideContent.transform.Find("Noel"))
                _noel = hideContent.transform.Find("Noel").gameObject;

            if (_noel)
                _noel.SetActive(false);

            if (FireBaseManager.Instance.showNoel)
            {
                if (_noel)
                {
                    _noel.SetActive(true);
                    _moneyCase = _noel.transform.Find("Money_Case").gameObject;
                    _moneyBag = _noel.transform.Find("Money_Bag").gameObject;
                    _moneySafe = _noel.transform.Find("Money_Safe").gameObject;
                }
            }

            RandomShow();
        }

        public void RandomShow()
        {
            _moneyCase.SetActive(false);
            _moneyBag.SetActive(false);
            _moneySafe.SetActive(false);
            var rnd = new System.Random();
            var randomNumber = rnd.Next(0, 3);
            if (randomNumber == 0)
                _moneyCase.SetActive(true);
            else if (randomNumber == 1)
                _moneyBag.SetActive(true);
            else if (randomNumber == 2)
                _moneySafe.SetActive(true);
            // Debug.Log("RandomShow: " + gameObject.name + ", randomNumber: " + randomNumber);
            Type = randomNumber;
        }

        public void AddMoney(int money)
        {
            cashPoint.SpawnMoneyImmediate(money);
        }

        public void SetMoneyTxt(int money)
        {
            moneyTxt.text = "+ " + money.ToString();
        }

        public void WithDrawMoney(GameObject player)
        {
            if (cashPoint.TotalCash != 0)
            {
                cashPoint.WithdrawMoney(player);
            }
            IsActive = false;
            // GetComponent<Collider>().enabled = false;
            DOVirtual.DelayedCall(30, (() =>
            {
                RandomShow();
                IsActive = true;
                FacilityManager.Instance.IsWaitingToShowGetMoneyPoint = false;
                // GetComponent<Collider>().enabled = true;
            })).SetAutoKill(true);
        }

        public bool IsActive
        {
            get => hideContent.activeSelf;
            set => hideContent.SetActive(value);
        }
    }
}