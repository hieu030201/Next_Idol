using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "BattleFundConfig", menuName = "GameConfig/BattleFundConfig")]
    public class BattleFundConfig : ScriptableObject
    {
        [Header("Hero Pack")] 
        public int heroCash;
        public int heroToken;
        public int heroGem;
        
        [Header("Legend Pack")] 
        public int legendCash;
        public int legendToken;
        public int legendGem;
        
        [Header("HeroLegend Pack")] 
        public int heroLegendCash;
        public int heroLegendToken;
        public int heroLegendGem;
    }

}
