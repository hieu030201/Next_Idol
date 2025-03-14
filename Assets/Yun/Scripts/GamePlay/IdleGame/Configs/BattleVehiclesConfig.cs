using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "BattleVehicleConfig", menuName = "GameConfig/BattleVehicleConfig")]
    public class BattleVehiclesConfig : ScriptableObject
    {
        [Header("Tank Configuration")] [Header("Tank 1")]
        public int TankDamageLevel1 = 1;

        public float TankSpeedShootingLv1 = 5f;

        [Header("Tank 2")] public int TankDamageLevel2 = 2;

        public float TankSpeedShootingLv2 = 5f;

        [Header("Tank 3")] public int TankDamageLevel3 = 3;

        public float TankSpeedShootingLv3 = 5f;

        [Space(10)] [Header("Missile Configuration")] [Header("Missile 1")]
        public int MissileDamageLevel1 = 1;

        public float MissileSpeedShootingLv1 = 6f;
        public int MisslileBulletNumberLv1 = 4;

        [Header("Missile 2")] public int MissileDamageLevel2 = 2;

        public float MissileSpeedShootingLv2 = 5f;
        public int MisslileBulletNumberLv2 = 5;

        [Header("Missile 3")] public int MissileDamageLevel3 = 3;

        public float MissileSpeedShootingLv3 = 5f;
        public int MisslileBulletNumberLv3 = 3;

        [Space(10)] [Header("Armored Configuration")] [Header("Armored 1")]
        public int ArmoredDamageLevel1 = 1;

        public float ArmoredSpeedShootingLv1 = 0.3f;

        [Header("Armored 2")] public int ArmoredDamageLevel2 = 1;

        public float ArmoredSpeedShootingLv2 = 0.25f;

        [Header("Armored 3")] public int ArmoredDamageLevel3 = 1;

        public float ArmoredSpeedShootingLv3 = 0.2f;
    }
}