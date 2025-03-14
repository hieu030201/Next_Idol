using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.Enemies;

namespace Yun.Scripts.GamePlay.Spawn
{
    public abstract class EnemySpawn : YunBehaviour
    {
        [SerializeField] protected bool IsSpawnWhenActive;
        [SerializeField] protected GameObject enemyPrefab;
        [SerializeField] protected GameObject player;

        private List<GameObject> _enemyList;

        protected override void Awake()
        {
            _enemyList = new List<GameObject>();
        }

        public abstract void StartSpawn();
        public abstract void StopSpawn();

        protected virtual void FixedUpdate()
        {
            if (player && !(player.transform.localPosition.z >= transform.localPosition.z + 20)) return;
            foreach (var enemy in _enemyList.Where(enemy => enemy != null))
            {
                Destroy(enemy);
                StopSpawn();
            }
        }

        protected void SpawnOneEnemy(Vector3 pos, float delayTime)
        {
            if (!player)
                return;
            var tween = DOVirtual.DelayedCall(delayTime, (() =>
            {
                var enemy = Instantiate(enemyPrefab, transform);
                enemy.transform.localPosition = pos;
                enemy.GetComponent<Enemy>().Active(player.transform);
                _enemyList.Add(enemy);
            }));
            AddTweenToTweenManager(tween);
        }
    }
}