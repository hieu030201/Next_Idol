using System.Collections.Generic;
using Gley.EasyIAP;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "IAPConfig", menuName = "GameConfig/IAP")]
    public class IAPConfig : ScriptableObject
    {
        public List<IapData> ProductsList;
        
        [Button]
        public void ClickToFillData()  
        {  
            foreach (var product in ProductsList)  
            {  
                product.PopulateChangeEnumString(); 
            }  
        } 
    }
}
