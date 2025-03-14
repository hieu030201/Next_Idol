using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "AbilitiesDataCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "AbilitiesData",
	    order = 120)]
	public class AbilitiesDataCollection : Collection<AbilitiesData>
	{
		[Button]
		private void fillData()
		{
			for (var i = 0; i < Value.Count; i++)
			{
				Value[i].id = i;
			}
		}
		
		public AbilitiesData GetItem()
		{
			return Value.Find(s => s.id == FacilityManager.Instance.GameSaveLoad.StableGameData.abilityLevel);
		}
	}
}