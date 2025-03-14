using System;
using System.Collections;
using System.Collections.Generic;
using Hovl_Studio.Toon_Projectiles_2.Scripts;
using UnityEngine;
using Yun.Scripts.Audios;

namespace Yun.Scripts.GamePlay.IdleGame.Weapons.Gun
{
    public class ShotGun : Firearm
    {
        public override void spawnOneProjectile(Vector3 position, Quaternion rotation)
        {
            _isPass = !_isPass;
            var projectile = Instantiate(projectilePrefab);
            // if(_isPass)
            //     projectile.GetComponent<Collider>().enabled = false;
            projectile.layer = LayerMask.NameToLayer(_bulletLayer);
            projectile.GetComponent<ProjectileMover>().powerNumber = _damageNumber;
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Gatling);
        }
    }
}


