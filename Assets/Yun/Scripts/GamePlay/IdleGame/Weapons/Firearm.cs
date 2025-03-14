using System.Collections.Generic;
using DG.Tweening;
using Hovl_Studio.Toon_Projectiles_2.Scripts;
using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.Weapons;

namespace Yun.Scripts.GamePlay.IdleGame.Weapons
{
    public enum GunType
    {
        pistol = 0,
        shootGun = 1,
        thompson = 2,
        gatlingGun = 3,
    }
    public class Firearm : Weapon
    {
        protected List<GameObject> _positionList;
        protected Tween _shootTween;
        protected bool _isShooting;
        protected int _damageNumber;

        protected override void Awake()
        {
            _positionList = new List<GameObject>();
            for (var i = 0; i < transform.Find("Positions_List").childCount; i++)
            {
                // Debug.Log("Run Here");
                _positionList.Add(transform.Find("Positions_List").GetChild(i).gameObject);
                transform.Find("Positions_List").GetChild(i).gameObject.SetActive(false);
            }
        }
        
        public void SetDamageNumber(int damage)
        {
            _damageNumber = damage;
        }

        protected string _bulletLayer;
        public void SetBulletLayer(string bulletLayer)
        {
            _bulletLayer = bulletLayer;
        }

        protected int _shootIndex;
        public override void Shoot()
        {
            _shootIndex = 0;
            _shootTween?.Kill();
            OnShoot();    
        }
        
        public override void StopShoot()
        {
            _shootTween?.Kill();
        }

        protected void OnShoot()
        {
            spawnOneProjectile(_positionList[_shootIndex].transform.position,_positionList[_shootIndex].transform.rotation);
            _shootIndex++;
            if (_shootIndex > _positionList.Count - 1)
                _shootIndex = 0;
            // _shootTween = DOVirtual.DelayedCall(0.1f,
            //     OnShoot);
        }

        protected bool _isPass;
        public virtual void spawnOneProjectile(Vector3 position, Quaternion rotation)
        {
            _isPass = !_isPass;
            // var projectile = Instantiate(projectilePrefab);
            // // if(_isPass)
            // //     projectile.GetComponent<Collider>().enabled = false;
            // projectile.layer = LayerMask.NameToLayer(_bulletLayer);
            // projectile.GetComponent<ProjectileMover>().powerNumber =
            //     projectilePrefab.GetComponent<ProjectileMover>().powerNumber;
            // projectile.transform.position = position;
            // projectile.transform.rotation = rotation;
            // AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Gatling);
            
            var projectile = SimplePool.Spawn<ProjectileMover>(PoolType.BulletProjectile);
            projectile.gameObject.layer = LayerMask.NameToLayer(_bulletLayer);
            projectile.powerNumber = FacilityManager.Instance.testGameConfig.isTestWeakDevice ? projectilePrefab.GetComponent<ProjectileMover>().powerNumber*2 : projectilePrefab.GetComponent<ProjectileMover>().powerNumber;
            projectile.transform.SetPositionAndRotation(position, rotation);
            projectile.OnInit();

            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Gatling);
        }
    }
}


