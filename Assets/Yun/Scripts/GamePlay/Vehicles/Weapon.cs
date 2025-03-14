using UnityEngine;

namespace Yun.Scripts.GamePlay.Vehicles
{
    public class Weapon : MonoBehaviour
    {
        private string _weaponType;

        public string WeaponType
        {
            get => _weaponType;
            set
            {
                _weaponType = value;
            }
        }

        private int _power;

        public int Power
        {
            get => _power;
            set
            {
                _power = value;
            }
        }
        
        private float _powerRange;

        public float PowerRange
        {
            get => _powerRange;
            set
            {
                _powerRange = value;
            }
        }

        private int _bulletsQuantity;

        public int BulletsQuantity
        {
            get => _bulletsQuantity;
            set
            {
                _bulletsQuantity = value;
            }
        }

        private int _firedBullets;

        public int FiredBullets
        {
            get => _firedBullets;
            set
            {
                _firedBullets = value;
            }
        }

        public void Shoot()
        {
            
        }

        public void Reload()
        {
            
        }
    }
}
