using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.Spawn
{
    public class AroundSpawn : EnemySpawn
    {
        [SerializeField] private int spawnNumberPerRound;
        [SerializeField] private int spawnRound;
        [SerializeField] private float interstitialTime;
        [SerializeField] private float delayToStart;

        public List<Transform> _posList;
        private List<float> _timeListToSpawn;
        private bool _isCheckPlayerNearby = true;

        protected override void Start()
        {
            // if (IsSpawnWhenActive)
            //     StartSpawn();
            
            for (var i = 0; i < transform.Find("positionList").childCount; i++)
            {
                var position = transform.Find("positionList").GetChild(i);
                if(position)
                    position.gameObject.SetActive(false);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (player == null)
                return;
            if (!_isCheckPlayerNearby ||
                !(Mathf.Abs(player.transform.localPosition.z - transform.localPosition.z) < 90)) return;
            _isCheckPlayerNearby = false;
            StartSpawn();
        }

        public override void StartSpawn()
        {
            _posList = new List<Transform>();
            for (var i = 0; i < transform.Find("positionList").childCount; i++)
            {
                var position = transform.Find("positionList").GetChild(i);
                _posList.Add(position);
            }

            _timeListToSpawn = new List<float>();
            for (var i = 0; i < spawnRound - 1; i++)
            {
                _timeListToSpawn.Add(interstitialTime);
            }

            SpawnEnemy();
        }

        private Tween _tweenToSpawn;

        private void SpawnEnemy()
        {
            for (var i = 0; i < spawnNumberPerRound; i++)
            {
                // Debug.Log("SpawnEnemy " + (0.2f * i));
                if (i < _posList.Count)
                    SpawnOneEnemy(_posList[i].transform.localPosition, 0.2f * i);
                else
                    SpawnOneEnemy(_posList[i % _posList.Count].transform.localPosition, 0.2f * i);
            }

            if (_timeListToSpawn.Count <= 0) return;
            _tweenToSpawn = DOVirtual.DelayedCall(_timeListToSpawn[0], (SpawnEnemy));
            AddTweenToTweenManager(_tweenToSpawn);
            _timeListToSpawn.RemoveAt(0);
        }

        public override void StopSpawn()
        {
            _tweenToSpawn?.Kill();
        }
    }
}