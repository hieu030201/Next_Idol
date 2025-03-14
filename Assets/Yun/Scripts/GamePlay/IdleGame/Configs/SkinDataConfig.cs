using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "SkinDataConfig", menuName = "GameConfig/SkinDataConfig")]

    public class SkinDataConfig : ScriptableObject
    {
        public List<SkinData> SkinDatas = new List<SkinData>();
    }

    [Serializable]
    public class SkinData
    {
        public int id;
        public NameType name;
        public SellType sellType;
        public SkinType skinType;
        public SkinModelType skinModelType;
        public float price;
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public Sprite icon;
        public string status;
        public string hightLight;
        public bool canRental;
        public int rentalMinutes;
        public bool unlock;
        
        //public string strSelltype;
         // public void PopulateChangeEnumString()  
         // {  
         //     strSelltype = sellType.ToString();
         //     strName = name.ToString();
         // } 
    }

    public class HireSkinData
    {
        public bool hire;
        public int minutes;
    }
    public enum SellType
    {
        Reward,
        Get,
        Gem,
        IAP,
        Token,
    }

    public enum NameType
    {
        Player = 0,
        Skin_Santa = 1,
        Mafia = 2,
        Rambo = 3,
        Terminator = 4,
        Sprunki_FunBot = 5,
        Sprunki_Oren = 6,
        Sprunki_OWAKCX = 7,
        Sprunki_Pinki = 8,
        Sprunki_Wenda = 9,
    }
    
    public enum SkinType
    {
        Common = 0,
        Exclusive = 1,
        Rare = 2,
    }

    public enum SkinModelType
    {
        BaseModel = 0,
        SprunkyModel = 1,
    }

}