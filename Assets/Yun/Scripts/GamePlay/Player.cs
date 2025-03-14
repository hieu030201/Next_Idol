using DG.Tweening;
using Hovl_Studio.Toon_Projectiles_2.Scripts;
using UnityEngine;
using Yun.Scripts.GamePlay.Items;
using Yun.Scripts.Managers;
using Yun.Scripts.UI;

namespace Yun.Scripts.GamePlay
{
    public class Player : Entity
    {
        [SerializeField] protected GameObject projectilePrefab;
        [SerializeField] protected GameObject healthBarPos;
        [SerializeField] protected GameObject healthBarPrefab;
    }
}