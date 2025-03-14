using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "WeaponDataCollection.asset",
	    menuName = SOArchitecture_Utility.COLLECTION_SUBMENU + "WeaponData",
	    order = 120)]
	public class WeaponDataCollection : Collection<WeaponData>
	{
		[Button]
		private void fillData()
		{
			for (var i = 0; i < Value.Count; i++)
			{
				Value[i].id = i;
			}
		}
		public int GetIdByName(string name)  
		{  
			var skinData = Value.Find(s => s.nameType.ToString() == name);
			return skinData.id; // Trả về ID nếu tìm thấy  
		}   
	}
}