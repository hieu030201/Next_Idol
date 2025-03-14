using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "BossDataCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "BossData",
	    order = 120)]
	public class BossDataCollection : Collection<BossData>
	{
		[Button]
		private void fillData()
		{
			for (var i = 0; i < Value.Count; i++)
			{
				Value[i].id = i;
			}
		}
	}
}