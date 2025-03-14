using System;
using System.Collections.Generic;
using Gley.EasyIAP;
using Sirenix.OdinInspector;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class IapData
    {
        public ShopProductNames productNames;
        public string strProductNames;
        public int value;
        public string valueString;
        
        public void PopulateChangeEnumString()  
        {  
            strProductNames = productNames.ToString(); 
        }  
    }
}
