using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [Serializable]
    public class LevelDescription
    {
        public int id;
        public int totalProgress = 2;
        public int reward = 30;
        public int AdsReward = 30;
        public List<GameObject> unlocks = new ();
        public List<string> UnlockString = new();
        public void PopulateUnlockString()  
        {  
            UnlockString.Clear(); // Clear existing names if any  
            foreach (GameObject obj in unlocks)  
            {  
                if (obj != null)  
                {  
                    UnlockString.Add(obj.name); // Add the GameObject's name to the list  
                }  
            }  
        }  
    }  
}
