using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Animations;
using Yun.Scripts.Audios;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Configs;

namespace Yun.Scripts.GamePlay.Vehicles.BattleService
{
    public abstract class BattleVehicle : YunBehaviour
    {
        [SerializeField] protected GameObject[] levelPrefabs;
        [SerializeField] public GunRecoil _gunRecoil;
     
        protected int _level;
        protected float durationShow = 0.5f;
        protected float initialScale = 0.1f;

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                GetPropertyForPrefabs(Level);
            }
        }

        public bool IsActive { get; set; }

        public abstract void GetPropertyForPrefabs(int index);

        [Button]
        public virtual void Show()
        {
            gameObject.SetActive(true);
            IsActive = true;
        }

        [Button]
        public void UpdateLevel()
        {
            levelPrefabs[Level].SetActive(false);
            Level++;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Upgrade_Vehicle);
        }

        [Button]
        public void IsLose()
        {
            var randomDelay = Random.Range(0f, 1.5f);
            DOVirtual.DelayedCall(randomDelay, () =>
            {
                ParticlePool.Play(ParticleType.ExplosionAbility, transform.position);
                gameObject.GetComponent<ExploseAnimation>().Explode();
                IsActive = false;
            });
        }

        public void ResetData()
        {
            Level = 0;
        }
        // public Vector3 ExplosionZone(int index)
        // {
        //     Vector3 vectorExplosionZone = Vector3.zero;
        //     switch (index)
        //     {
        //         case 1:
        //             vectorExplosionZone = CalculationExplosionZone(_explosionZoneEnemy);
        //             break;
        //         case 2:
        //             vectorExplosionZone = CalculationExplosionZone(_explosionZoneClient);
        //             break;
        //         default:
        //             Debug.LogWarning("Invalid index for ExplosionZone");
        //             break;
        //     }
        //
        //     return vectorExplosionZone;
        // }
        //
        // public Vector3 CalculationExplosionZone(Transform[] target)
        // {
        //     float minX = Mathf.Min(target[0].position.x, target[1].position.x,
        //         target[2].position.x, target[3].position.x);
        //     float maxX = Mathf.Max(target[0].position.x, target[1].position.x,
        //         target[2].position.x, target[3].position.x);
        //
        //     float minY = Mathf.Min(target[0].position.y, target[1].position.y,
        //         target[2].position.y, target[3].position.y);
        //     float maxY = Mathf.Max(target[0].position.y, target[1].position.y,
        //         target[2].position.y, target[3].position.y);
        //
        //     float minZ = Mathf.Min(target[0].position.z, target[1].position.z,
        //         target[2].position.z, target[3].position.z);
        //     float maxZ = Mathf.Max(target[0].position.z, target[1].position.z,
        //         target[2].position.z, target[3].position.z);
        //
        //     float randomX = Random.Range(minX, maxX);
        //     float randomY = Random.Range(minY, maxY);
        //     float randomZ = Random.Range(minZ, maxZ);
        //     return new Vector3(randomX, randomY, randomZ);
        // }
    }
}