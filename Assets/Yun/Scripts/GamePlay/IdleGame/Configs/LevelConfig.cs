using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "GameConfig/Level")]
    public class LevelConfig : ScriptableObject
    {
        public List<LevelDescription> LevelRequirements = new List<LevelDescription>();

        [Button("Click to Fill Data")]  
        public void ClickToFillData()  
        {  
            foreach (var level in LevelRequirements)  
            {  
                level.PopulateUnlockString(); 
            }  
        } 
    }
}
