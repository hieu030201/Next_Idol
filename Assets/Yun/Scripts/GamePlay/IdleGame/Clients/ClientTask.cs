using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yun.Scripts.GamePlay.IdleGame.Clients
{
    [Serializable]
    public class ClientTask
    {
        public WarBaseClient.ClientEmotionState emotionState;
        public float delayTime;
        public string strEnotionState;
        public void PopulateChangeEnumString()  
        {  
            strEnotionState = emotionState.ToString(); 
        }  
    }
}
