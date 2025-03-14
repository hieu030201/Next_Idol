using System;
using System.Collections.Generic;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class IdlePlayerData
    {
        public IdlePlayerData(int money, int capacity, float speed)
        {
            _money = money;
            _capacity = capacity;
            _speed = speed;
        }
        
        private float _speed;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }
        
        private int _speedMultiplier;

        public int SpeedMultiplier
        {
            get => _speedMultiplier;
            set => _speedMultiplier = value;
        }
        
        private int _money;

        public int Money
        {
            get => _money;
            set => _money = value;
        }

        private int _levelProgress;

        public int LevelProgress
        {
            get => _levelProgress;
            set => _levelProgress = value;
        }
        
        private int _level = 1;

        public int Level
        {
            get => _level;
            set => _level = value;
        }
        
        private int _capacity;

        public int Capacity
        {
            get => _capacity;
            set => _capacity = value;
        }
    }
}
