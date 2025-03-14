using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class SkinSaveDataEvent : UnityEvent<SkinSaveData> { }

	[CreateAssetMenu(
	    fileName = "SkinSaveDataVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "SkinSaveData",
	    order = 120)]
	public class SkinSaveDataVariable : BaseVariable<SkinSaveData, SkinSaveDataEvent>
	{
		[Button]
		public void resetData()
		{
			Value = new SkinSaveData();
		}
		
		
	}
}