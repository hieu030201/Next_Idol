using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class StarPoint : YunBehaviour
    {
        [SerializeField] private GameObject starPrefab;

        [SerializeField] private GameObject points;

        private List<GameObject> _starList;
        private List<Transform> _starPointsList;
        private int _totalStar;

        protected override void Awake()
        {
            base.Awake();
            _totalStar = 0;
            _starList = new List<GameObject>();
            _starPointsList = new List<Transform>();

            _starList = new List<GameObject>();
            _starPointsList = new List<Transform>();
            for (var i = 0; i < points.transform.childCount; i++)
            {
                _starPointsList.Add(points.transform.GetChild(i));
            }
        }

        private int _starPerColumn = 10;
        public void SpawnStar(int star, Vector3 startPosition)
        {
            for (var i = 0; i < star; i++)
            {
                _totalStar++;
                if(_totalStar > _starPointsList.Count * _starPerColumn)
                    continue;
                var starObject = Instantiate(starPrefab);
                starObject.transform.position = startPosition;
                _starList.Add(starObject);
                var index1 = (int)Mathf.Floor((float)(_starList.Count - 1) / _starPerColumn);
                var index2 = (_starList.Count - 1) % _starPerColumn;
                var endPosition = _starPointsList[index1].position + new Vector3(0, index2 * 0.09f, 0);
                
                var midPoint = (startPosition + endPosition) / 2;
                midPoint.y += 1.5f;

                var path = new Vector3[3];
                path[0] = startPosition;
                path[1] = midPoint;
                path[2] = endPosition;
                
                SpawnOneStar(i * 0.1f, path, starObject);
            }
        }

        private void SpawnOneStar(float delay, Vector3[] path, GameObject star)
        {
            if (delay == 0)
            {
                star.transform.DOPath(path, 0.4f, PathType.CatmullRom)
                    .SetEase(Ease.OutQuad);
                return;
            }
            DOVirtual.DelayedCall(delay, (() =>
            {
                star.transform.DOPath(path, 0.4f, PathType.CatmullRom)
                    .SetEase(Ease.OutQuad).OnComplete((() =>
                    {
                        star.isStatic = true;
                    }));
            }));
        }

        public int WithdrawStar()
        {
            var starNumber = _totalStar;
            foreach (var star in _starList)
            {
                Destroy(star);
            }

            _starList = new List<GameObject>();
            _totalStar = 0;
            return starNumber;
        }
    }
}