using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "BattlePassDataCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "BattlePassData",
	    order = 120)]
	public class BattlePassDataCollection : Collection<BattlePassData>
	{
		[Button]
		private void fillData()
		{
			for (var i = 0; i < Value.Count; i++)
			{
				Value[i].id = i;
			}
		}
		
		public BattlePassFree GetItemBattlePassFree(int id)  
		{  
			foreach (var battlePassFree in Value)  
			{  
				if (battlePassFree.id == id)  
				{  
					return battlePassFree.battlePassFree; 
				}  
			}  
			return null; 
		}  
		
		public BattlePassHero GetItemBattleHero(int id)  
		{  
			foreach (var battlePassHero in Value)  
			{  
				if (battlePassHero.id == id)  
				{  
					return battlePassHero.battlePassHero; 
				}  
			}  
			return null; 
		}  
		
		public BattlePassLegend GetItemBattleLegend(int id)  
		{  
			foreach (var battPassLegend in Value)  
			{  
				if (battPassLegend.id == id)  
				{  
					return battPassLegend.battlePassLengend; 
				}  
			}  
			return null; 
		} 
	}
}