using System.Collections.Generic;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "HireEmployeeConfig", menuName = "GameConfig/HireEmployeeConfig")]
    public class HireEmployeeConfig : ScriptableObject
    {
        public List<HireEmployeeDataConfig> hireEmployeeConfigs = new ();
    }
}
