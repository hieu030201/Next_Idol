using System;
using System.Collections.Generic;
using DG.Tweening;
using Hovl_Studio.Toon_Projectiles_2.Scripts;
using UnityEngine;
using Yun.Scripts.Audios;

namespace Yun.Scripts.GamePlay.Weapons
{
    public class Rocket : Weapon
    {
        private List<GameObject> _positionList;
        private List<GameObject> _positionList2;
        private Tween _shootTween;
        private Tween _shootTween2;
        private bool _isShooting;

        protected override void Awake()
        {
            _positionList = new List<GameObject>();
            for (var i = 0; i < transform.Find("Positions_List").childCount; i++)
            {
                _positionList.Add(transform.Find("Positions_List").GetChild(i).gameObject);
                transform.Find("Positions_List").GetChild(i).gameObject.SetActive(false);
            }
            
            _positionList2 = new List<GameObject>();
            for (var i = 0; i < transform.Find("Positions_List_2").childCount; i++)
            {
                _positionList2.Add(transform.Find("Positions_List_2").GetChild(i).gameObject);
                transform.Find("Positions_List_2").GetChild(i).gameObject.SetActive(false);
            }
        }

        private int _shootIndex;
        private int _shootIndex2;
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

        private void OnShoot()
        {
            spawnOneProjectile(_positionList[_shootIndex].transform.position,_positionList[_shootIndex].transform.rotation);
            spawnOneProjectile(_positionList2[_shootIndex].transform.position,_positionList2[_shootIndex].transform.rotation);
            _shootIndex++;
            if (_shootIndex > _positionList.Count - 1)
                _shootIndex = 0;
            _shootTween = DOVirtual.DelayedCall(0.1f,
                OnShoot);
        }

        private void spawnOneProjectile(Vector3 position, Quaternion rotation)
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<ProjectileMover>().powerNumber =
                projectilePrefab.GetComponent<ProjectileMover>().powerNumber;
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Rocket);
        }
    }
}
