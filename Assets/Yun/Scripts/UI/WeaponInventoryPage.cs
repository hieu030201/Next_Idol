using UnityEngine;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI
{
    public class WeaponInventoryPage : InventoryPage
    {
        protected override void SelectItem(GameObject item)
        {
            base.SelectItem(item);
            GameManager.Instance.OnChangeWeapon(item.GetComponent<InventoryItem>().ItemPrefab);
        }
    }
}
