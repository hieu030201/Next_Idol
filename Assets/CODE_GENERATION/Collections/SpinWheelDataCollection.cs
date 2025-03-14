using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "SpinWheelDataCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "SpinWheelData",
	    order = 120)]
	public class SpinWheelDataCollection : Collection<SpinWheelData>
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