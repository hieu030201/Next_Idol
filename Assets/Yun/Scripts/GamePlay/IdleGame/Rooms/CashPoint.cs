using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Animations;
using Yun.Scripts.Audios;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Players;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class CashPoint : YunBehaviour
    {
        [SerializeField] private GameObject cashPrefab;

        [SerializeField] private GameObject points;

        private List<GameObject> _cashList;
        private List<Transform> _cashPointsList;
        private IdlePlayer _player;

        public int TotalCash
        {
            get => _totalCash;
        }

        private int _totalCash;
        private int _currentCash;

        protected override void Awake()
        {
            base.Awake();
            _totalCash = 0;
            _cashList = new List<GameObject>();
            _cashPointsList = new List<Transform>();

            _cashList = new List<GameObject>();
            _cashPointsList = new List<Transform>();
            for (var i = 0; i < points.transform.childCount; i++)
            {
                _cashPointsList.Add(points.transform.GetChild(i));
            }
        }

        protected override void Start()
        {
            base.Start();
            if (FacilityManager.Instance.player)
                _player = FacilityManager.Instance.player;
        }

        private const int MoneyPerColumn = 10;

        private Coroutine _checkWithdrawMoneyCoroutine;

        public void SpawnMoney(int money, Vector3 startPosition)
        {
            for (var i = 0; i < money; i++)
            {
                _totalCash++;
                if (_totalCash > _cashPointsList.Count * MoneyPerColumn)
                    continue;
                var cash = Instantiate(cashPrefab);
                cash.transform.position = startPosition;
                _cashList.Add(cash);
                var index1 = (int)Mathf.Floor((float)(_cashList.Count - 1) / MoneyPerColumn);
                var index2 = (_cashList.Count - 1) % MoneyPerColumn;
                var endPosition = _cashPointsList[index1].position + new Vector3(0, index2 * 0.09f, 0);

                var midPoint = (startPosition + endPosition) / 2;
                midPoint.y += 1.5f;

                var path = new Vector3[3];
                path[0] = startPosition;
                path[1] = midPoint;
                path[2] = endPosition;

                cash.transform.rotation = _cashPointsList[index1].rotation;
                SpawnOneMoney(i * 0.1f, path, cash);
            }

            if (_checkWithdrawMoneyCoroutine != null)
                StopCoroutine(_checkWithdrawMoneyCoroutine);
            _checkWithdrawMoneyCoroutine = StartCoroutine(CheckWithdrawMoneyCoroutine());
        }

        private IEnumerator CheckWithdrawMoneyCoroutine()
        {
            while (!IsPlayerInWithdrawRange)
            {
                yield return new WaitForSeconds(0.5f);
            }

            if (_checkWithdrawMoneyCoroutine != null)
                StopCoroutine(_checkWithdrawMoneyCoroutine);
            if (_player)
            {
                if (_totalCash == 0) yield break;
                var money = WithdrawMoney(_player.gameObject);
                FacilityManager.Instance.AddMoney(money, transform.position);
            }
            else
            {
                StopCoroutine(_checkWithdrawMoneyCoroutine);
            }
        }

        private float _withdrawRange = 1f;

        private bool IsPlayerInWithdrawRange
        {
            get
            {
                if (_player == null)
                    return false;
                return (Vector3.Distance(transform.position, _player.transform.position) <= _player.CollectMoneyRadius);
            }
        }

        public void SpawnMoneyImmediate(int money)
        {
            for (var i = 0; i < money; i++)
            {
                _totalCash++;
                if (_totalCash > _cashPointsList.Count * MoneyPerColumn)
                    continue;
                var cash = Instantiate(cashPrefab);
                _cashList.Add(cash);
                var index1 = (int)Mathf.Floor((float)(_cashList.Count - 1) / MoneyPerColumn);
                var index2 = (_cashList.Count - 1) % MoneyPerColumn;
                var endPosition = _cashPointsList[index1].position + new Vector3(0, index2 * 0.12f, 0);
                cash.transform.position = endPosition;
                cash.transform.rotation = _cashPointsList[index1].rotation;
            }

            /*if (_checkWithdrawMoneyCoroutine != null)
                StopCoroutine(_checkWithdrawMoneyCoroutine);
            _checkWithdrawMoneyCoroutine = StartCoroutine(CheckWithdrawMoneyCoroutine());*/
        }

        private static void SpawnOneMoney(float delay, Vector3[] path, GameObject cash)
        {
            if (delay == 0)
            {
                cash.transform.DOPath(path, 0.4f, PathType.CatmullRom)
                    .SetEase(Ease.OutQuad);
                return;
            }

            DOVirtual.DelayedCall(delay, (() =>
            {
                if (cash)
                {
                    cash.transform.DOPath(path, 0.4f, PathType.CatmullRom)
                        .SetEase(Ease.OutQuad).OnComplete((() => { cash.isStatic = true; }));
                }
            }));
        }

        public int WithdrawMoney(GameObject player)
        {
            if (_checkWithdrawMoneyCoroutine != null)
                StopCoroutine(_checkWithdrawMoneyCoroutine);
            var money = _totalCash;

            var delay = 0.05f;
            if (money * delay > 2)
                delay = (float)2 / money;
            var count = 0;
            if (_cashList.Count > 0)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Collect_Money_2);
            for (var i = _cashList.Count - 1; i >= 0; i--)
            {
                count++;

                var cash = _cashList[i];
                if (count == 1)
                {
                    cash.GetComponent<FlyingMoney3D>().DelayToFlyToPlayer(0, player);
                }
                else
                {
                    if (cash)
                    {
                        cash.GetComponent<FlyingMoney3D>().DelayToFlyToPlayer(count * delay, player);
                    }
                }
            }

            _cashList = new List<GameObject>();
            _totalCash = 0;
            return money;
        }
    }
}