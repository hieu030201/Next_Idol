using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "ClientTypeConfig", menuName = "GameConfig/Client")]
    public class ClientTypeConfig : ScriptableObject
    {
        public List<ClientTask> TasksList = new List<ClientTask>();
        public float Speed = 2;
        
        [Button]
        public void ClickToFillData()  
        {  
            foreach (var task in TasksList)  
            {  
                task.PopulateChangeEnumString(); 
            }  
        } 
    }
}
