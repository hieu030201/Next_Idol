using Advertising;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Utilities;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "SkinDataCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "SkinData",
	    order = 120)]
	public class SkinDataCollection : Collection<SkinData>
	{
		[Button]
		private void fillData()
		{
			for (var i = 0; i < Value.Count; i++)
			{
				Value[i].id = i;
			}
		}
		
		[Button]
		private void resetData()
		{
			for (var i = 1; i < Value.Count; i++) Value[i].unlock = false;
		}
		
		// [Button]
		// public void FillEnumToSing()  
		// {  
		// 	for (var i = 0; i < Value.Count; i++)
		// 	{
		// 		Value[i].PopulateChangeEnumString();
		// 	}
		// }  
		
		public SkinData GetItem()
		{
			return Value.Find(s => s.id == FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected);
		}
		
		public SkinData GetItem(int id)
		{
			return Value.Find(s => s.id == id);
		}

		public string GetNameItem(int id)  
		{  
			var skinData = Value.Find(s => s.id == id);  
			return skinData != null ? skinData.name.ToString() : null; 
		}

		public void ChangeDefaulSkin(NameType nameType, Sprite iconChange)
		{
			var defaultSkin = Value.Find(s => s.id == 0);  
			if (defaultSkin != null)  
			{  
				defaultSkin.name = nameType;
				defaultSkin.icon = iconChange;
			}  
		}
		public SkinData GetItemRewardNotUnlock()
		{
			var _itemReward = Value.FindAll(s => s.sellType != SellType.IAP &&  s.sellType != SellType.Get);
			var _itemNotUnlock =
				_itemReward.FindAll(s => !FacilityManager.Instance.GameSaveLoad.StableGameData.listIDSkinUnlock.Contains(s.id));
			if (_itemNotUnlock.Count > 0)
			{
				_itemNotUnlock.Shuffle();
				return _itemNotUnlock[0];
			}

			return null;
		}
	
	}
}