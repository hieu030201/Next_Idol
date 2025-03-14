using DG.Tweening;
using UnityEngine;

namespace Yun.Scripts.GamePlay.Spawn
{
    public class BarrelSpawn : MonoBehaviour
    {
        [SerializeField] private bool isSpawnWhenActive;
        [SerializeField] private int spawnRound = 1;
        [SerializeField] private GameObject spawnPosition;
        [SerializeField] private GameObject barrelPrefab;
        [SerializeField] private float interstitialTime;
        [SerializeField] private float delayToStart;

        private int _countSpawn;
        // Start is called before the first frame update
        private void Start()
        {
            spawnPosition.SetActive(false);
            _countSpawn = 0;
            if (isSpawnWhenActive)
                StartSpawn();
        }

        public void StartSpawn()
        {
            if (delayToStart == 0)
            {
                SpawnBarrel();
            }
            else
            {
                DOVirtual.DelayedCall(delayToStart, (SpawnBarrel));
            }
        }

        private Tween _tweenToSpawn;

        private void SpawnBarrel()
        {
            _countSpawn++;
            if(_countSpawn > spawnRound)
                return;
            SpawnOneBarrel();
        }

        private void SpawnOneBarrel()
        {
            var barrel = Instantiate(barrelPrefab, transform);
            barrel.transform.localPosition = spawnPosition.transform.localPosition;
        }

        public void StopSpawn()
        {
            _tweenToSpawn?.Kill();
        }
    }
}