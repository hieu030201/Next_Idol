using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Logics;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.Datas.IdleGame
{
    public class IdleGameData
    {
        public const string GameVersion = "V108";
        private IdleGameSaveLoad _gameSaveLoad;

        public void SetGameSaveLoad(IdleGameSaveLoad idleGameSaveLoad)
        {
            _gameSaveLoad = idleGameSaveLoad;
        }

        public int Money
        {
            get => _gameSaveLoad.GameData.money;
            set
            {
                _gameSaveLoad.GameData.money = value;
                // _gameSaveLoad.SaveData();
            }
        }
        
        public int Gem
        {
            get => _gameSaveLoad.GameData.gem;
            set
            {
                _gameSaveLoad.GameData.gem = value;
                // _gameSaveLoad.SaveData();
            }
        }
        
        public int Token
        {
            get => _gameSaveLoad.GameData.token;
            set
            {
                _gameSaveLoad.GameData.token = value;
                // _gameSaveLoad.SaveData();
            }
        }

        public int LevelProgress
        {
            get => _gameSaveLoad.GameData.levelProgress;
            set
            {
                _gameSaveLoad.GameData.levelProgress = value;
                // _gameSaveLoad.SaveData();
            }
        }

        public int Level
        {
            get => _gameSaveLoad.GameData.level;
            set
            {
                _gameSaveLoad.GameData.level = value;
                // _gameSaveLoad.SaveData();
            }
        }

        public int Capacity
        {
            get => _gameSaveLoad.GameData.capacity;
            set
            {
                _gameSaveLoad.GameData.capacity = value;
                // _gameSaveLoad.SaveData();
            }
        }
        
        private int _worker1Capacity;

        public int Worker1Capacity
        {
            get => _gameSaveLoad.GameData.worker1Capacity;
            set
            {
                _gameSaveLoad.GameData.worker1Capacity = value;
                // _gameSaveLoad.SaveData();
            }
        }
        
        private int _worker2Capacity;

        public int Worker2Capacity
        {
            get => _gameSaveLoad.GameData.worker2Capacity;
            set
            {
                _gameSaveLoad.GameData.worker2Capacity = value;
                // _gameSaveLoad.SaveData();
            }
        }
        
        private int _worker3Capacity;

        public int Worker3Capacity
        {
            get => _gameSaveLoad.GameData.worker3Capacity;
            set
            {
                _gameSaveLoad.GameData.worker3Capacity = value;
                // _gameSaveLoad.SaveData();
            }
        }
        
        private int _worker4Capacity;

        public int Worker4Capacity
        {
            get => _gameSaveLoad.GameData.worker4Capacity;
            set
            {
                _gameSaveLoad.GameData.worker4Capacity = value;
                // _gameSaveLoad.SaveData();
            }
        }

        public float RealSpeed
        {
            get
            {
                var speed = FacilityManager.Instance.IdleGameData.Speed *
                            FacilityManager.Instance.IdleGameData.SpeedMultiplier;
                if (speed > 8)
                    speed = 8;
                if (FacilityManager.Instance.testGameConfig.IsFastPlay)
                    return 7;
                return speed;
            }
        }

        public float Speed
        {
            get => _gameSaveLoad.GameData.speed;
            set
            {
                _gameSaveLoad.GameData.speed = value;
                // _gameSaveLoad.SaveData();
                FacilityManager.Instance.OnPlayerUpdateSpeed();
            }
        }

        private float _speedMultiplier = 1;
        public float SpeedMultiplier
        {
            get => _speedMultiplier;
            set
            {
                _speedMultiplier = value;
                FacilityManager.Instance.OnPlayerUpdateSpeed();
            }
        }
        
        private float _workerSpeedMultiplier = 1;
        public float WorkerSpeedMultiplier
        {
            get => _workerSpeedMultiplier;
            set
            {
                _workerSpeedMultiplier = value;
                FacilityManager.Instance.OnAllWorkersUpdateSpeed();
            }
        }
    }
}