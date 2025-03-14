using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "WorkerConfig", menuName = "GameConfig/Worker")]
    public class WorkerConfig : ScriptableObject
    {
        public float speed;
        public List<CapacityData> UpgradeWorker1CapacitysList = new List<CapacityData>();
        public List<CapacityData> UpgradeWorker2CapacitysList = new List<CapacityData>();
        public List<CapacityData> UpgradeWorker3CapacitysList = new List<CapacityData>();
        public List<CapacityData> UpgradeWorker4CapacitysList = new List<CapacityData>();
        
        public List<BuyWorkerData> buyBedRoomStaffsList = new List<BuyWorkerData>();
        public List<BuyWorkerData> buyBoxingStaffsList = new List<BuyWorkerData>();
        public List<BuyWorkerData> buyDiningStaffsList = new List<BuyWorkerData>();
        public List<BuyWorkerData> buyTrainingStaffsList = new List<BuyWorkerData>();
    }
}
