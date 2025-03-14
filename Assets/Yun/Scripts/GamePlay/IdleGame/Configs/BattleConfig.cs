using System.Collections.Generic;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "BattleConfig", menuName = "GameConfig/BattleConfig")]
    public class BattleConfig : ScriptableObject
    {
        public List<BattleData> battleList = new List<BattleData>();
    }
}
