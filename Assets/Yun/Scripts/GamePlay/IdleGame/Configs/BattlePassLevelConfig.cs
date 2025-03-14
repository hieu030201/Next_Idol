using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;

[CreateAssetMenu(fileName = "BattlePassLevelConfig", menuName = "GameConfig/BattlePassLevelConfig")]
public class BattlePassLevelConfig : ScriptableObject
{
    public List<BattlePassDataLevelConfig> battlePassLevelList = new List<BattlePassDataLevelConfig>();
    
    [Button]
    private void fillData()
    {
        for (var i = 0; i < battlePassLevelList.Count; i++)
        {
            battlePassLevelList[i].id = i;
        }
    }
}
[Serializable]
public class BattlePassDataLevelConfig
{
    public int id;
    public int MetalTagNumber;
}
