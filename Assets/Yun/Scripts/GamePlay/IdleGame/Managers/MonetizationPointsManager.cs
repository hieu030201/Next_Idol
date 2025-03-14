using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

public class MonetizationPointsManager : MonoBehaviour
{
    [SerializeField] private GameObject moneyPointPrefab;
    [SerializeField] private GameObject speedPointPrefab;
    [SerializeField] private GameObject workerSpeedPointPrefab;

    private List<GameObject> _moneyPoints;
    private List<GameObject> _speedPoints;
    private List<GameObject> _workerSpeedPoints;

    // Start is called before the first frame update
    void Start()
    {
        _moneyPoints = new List<GameObject>();
        for (var i = 0; i < transform.Find("Money").childCount; i++)
        {
            _moneyPoints.Add(transform.Find("Money").GetChild(i).gameObject);
            transform.Find("Money").GetChild(i).gameObject.SetActive(false);
        }

        _speedPoints = new List<GameObject>();
        for (var i = 0; i < transform.Find("Speed").childCount; i++)
        {
            _speedPoints.Add(transform.Find("Speed").GetChild(i).gameObject);
            transform.Find("Speed").GetChild(i).gameObject.SetActive(false);
        }

        _workerSpeedPoints = new List<GameObject>();
        for (var i = 0; i < transform.Find("WorkerSpeed").childCount; i++)
        {
            _workerSpeedPoints.Add(transform.Find("WorkerSpeed").GetChild(i).gameObject);
            transform.Find("WorkerSpeed").GetChild(i).gameObject.SetActive(false);
        }
    }

    public void HideAllMoneyPoints()
    {
        foreach (var moneyPoint in _moneyPoints)
        {
            moneyPoint.SetActive(false);
        }

        _currentMoneyPoint = null;
    }

    public void HideAllSpeedPoints()
    {
        foreach (var speedPoint in _speedPoints)
        {
            speedPoint.SetActive(false);
        }

        _currentSpeedPoint = null;
    }

    public void HideAllWorkerSpeedPoints()
    {
        foreach (var speedPoint in _workerSpeedPoints)
        {
            speedPoint.SetActive(false);
        }

        _currentWorkerSpeedPoint = null;
    }

    public bool IsShowMoneyPoint { get; set; }

    public bool IsShowSpeedPoint { get; set; }

    public bool IsShowWorkerSpeedPoint { get; set; }

    private GameObject _player;
    private Coroutine _checkToShowPointsCoroutine;
    private GameObject _currentMoneyPoint;
    private GameObject _currentSpeedPoint;
    private GameObject _currentWorkerSpeedPoint;

    private GameObject _lastCheckedMoneyPoint;
    private GameObject _lastCheckedSpeedPoint;
    private GameObject _lastCheckedWorkerSpeedPoint;

    public void StartCheckToShowPoints(GameObject player)
    {
        _player = player;
        if (_checkToShowPointsCoroutine != null)
            StopCoroutine(_checkToShowPointsCoroutine);
        _checkToShowPointsCoroutine = StartCoroutine(CheckToShowPoints());
    }

    public void StopCheckToShowPoints()
    {
        if (_checkToShowPointsCoroutine != null)
            StopCoroutine(_checkToShowPointsCoroutine);
    }

    public void ShowNearestMoneyPoint()
    {
        var nearestDistance = float.MaxValue;
        GameObject nearestMoneyPoint = null;
        foreach (var moneyPoint in _moneyPoints)
        {
            if (Vector3.Distance(moneyPoint.transform.position, _player.transform.position) < nearestDistance)
            {
                nearestDistance = Vector3.Distance(moneyPoint.transform.position, _player.transform.position);
                nearestMoneyPoint = moneyPoint;
            }
        }

        if (nearestMoneyPoint == null)
            return;

        if (!nearestMoneyPoint.transform.Find("moneyPoint"))
        {
            var point = Instantiate(moneyPointPrefab, nearestMoneyPoint.transform);
            point.transform.localPosition = new Vector3(0, 0, 0);
            point.name = "moneyPoint";
        }

        var point2 = nearestMoneyPoint.transform.Find("moneyPoint").GetComponent<GetMoneyPoint>();
        DOVirtual.DelayedCall(0.5f, (() =>
        {
            var money = FacilityManager.Instance.LevelConfig
                .LevelRequirements[FacilityManager.Instance.IdleGameData.Level - 1]
                .AdsReward;
            point2.SetMoneyTxt(money);
        }));

        nearestMoneyPoint.transform.localScale = Vector3.zero;
        nearestMoneyPoint.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack);
        if (_currentMoneyPoint)
            _currentMoneyPoint.SetActive(false);
        nearestMoneyPoint.SetActive(true);
        _currentMoneyPoint = nearestMoneyPoint;
        _lastCheckedMoneyPoint = nearestMoneyPoint;
    }

    private float DistanceToPlayer(GameObject point)
    {
        return Vector3.Distance(point.transform.position, _player.transform.position);
    }

    private bool CheckCloserToLastPoint(GameObject point, GameObject lastPoint)
    {
        if (!lastPoint)
            return true;
        return Vector3.Distance(point.transform.position, _player.transform.position) < Vector3.Distance(lastPoint.transform.position, _player.transform.position);
    }

    private float _distanceToShow = 4f;
    private IEnumerator CheckToShowPoints()
    {
        while (true)
        {
            if(!gameObject.activeSelf)
                yield return new WaitForSeconds(0.3f);
            if (IsShowMoneyPoint)
            {
                foreach (var moneyPoint in _moneyPoints)
                {
                    if (moneyPoint != _lastCheckedMoneyPoint &&
                        DistanceToPlayer(moneyPoint) < _distanceToShow && CheckCloserToLastPoint(moneyPoint, _lastCheckedMoneyPoint))
                    {
                        _lastCheckedMoneyPoint = moneyPoint;
                        if (!moneyPoint.transform.Find("moneyPoint"))
                        {
                            var point = Instantiate(moneyPointPrefab, moneyPoint.transform);
                            point.transform.localPosition = new Vector3(0, 0, 0);
                            point.name = "moneyPoint";
                        }

                        var point2 = moneyPoint.transform.Find("moneyPoint").GetComponent<GetMoneyPoint>();
                        DOVirtual.DelayedCall(0.5f, (() =>
                        {
                            var money = FacilityManager.Instance.LevelConfig
                                .LevelRequirements[FacilityManager.Instance.IdleGameData.Level - 1]
                                .AdsReward;
                            point2.SetMoneyTxt(money);
                        }));

                        var showRate = new System.Random().Next(0, 3);
                        if (showRate == 0)
                        {
                            if (moneyPoint.gameObject.activeSelf)
                            {
                                moneyPoint.transform.localScale = Vector3.one;
                            }
                            else
                            {
                                moneyPoint.transform.localScale = Vector3.zero;
                                moneyPoint.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack);
                            }

                            if (_currentMoneyPoint)
                                _currentMoneyPoint.SetActive(false);
                            moneyPoint.SetActive(true);
                            _currentMoneyPoint = moneyPoint;
                        }
                    }
                }
            }

            if (IsShowSpeedPoint)
            {
                foreach (var speedPoint in _speedPoints)
                {
                    if (speedPoint != _lastCheckedSpeedPoint &&
                        DistanceToPlayer(speedPoint) < _distanceToShow && CheckCloserToLastPoint(speedPoint, _lastCheckedSpeedPoint))
                    {
                        _lastCheckedSpeedPoint = speedPoint;
                        if (!speedPoint.transform.Find("speedPoint"))
                        {
                            var point = Instantiate(speedPointPrefab, speedPoint.transform);
                            point.transform.localPosition = new Vector3(0, 0, 0);
                            point.name = "speedPoint";
                        }

                        var showRate = new System.Random().Next(0, 3);
                        if (showRate == 0)
                        {
                            if (speedPoint.gameObject.activeSelf)
                            {
                                speedPoint.transform.localScale = Vector3.one;
                            }
                            else
                            {
                                speedPoint.transform.localScale = Vector3.zero;
                                speedPoint.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack);
                            }

                            if (_currentSpeedPoint)
                                _currentSpeedPoint.SetActive(false);
                            speedPoint.SetActive(true);
                            _currentSpeedPoint = speedPoint;
                        }
                    }
                }
            }

            if (IsShowWorkerSpeedPoint)
            {
                foreach (var speedPoint in _workerSpeedPoints)
                {
                    if (speedPoint != _lastCheckedWorkerSpeedPoint &&
                        DistanceToPlayer(speedPoint) < _distanceToShow && CheckCloserToLastPoint(speedPoint, _lastCheckedWorkerSpeedPoint))
                    {
                        _lastCheckedWorkerSpeedPoint = speedPoint;
                        if (!speedPoint.transform.Find("speedPoint"))
                        {
                            var point = Instantiate(workerSpeedPointPrefab, speedPoint.transform);
                            point.transform.localPosition = new Vector3(0, 0, 0);
                            point.name = "speedPoint";
                        }

                        var showRate = new System.Random().Next(0, 3);
                        if (showRate == 0)
                        {
                            if (speedPoint.gameObject.activeSelf)
                            {
                                speedPoint.transform.localScale = Vector3.one;
                            }
                            else
                            {
                                speedPoint.transform.localScale = Vector3.zero;
                                speedPoint.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack);
                            }
                            
                            if (_currentWorkerSpeedPoint)
                                _currentWorkerSpeedPoint.SetActive(false);
                            speedPoint.SetActive(true);
                            speedPoint.transform.Find("speedPoint").GetComponent<GetWorkerSpeedBoosterPoint>()
                                .IsActive = true;
                            _currentWorkerSpeedPoint = speedPoint;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}