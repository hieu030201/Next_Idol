using UnityEngine;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI
{
    public class VehicleInventoryPage : InventoryPage
    {
        protected override void SelectItem(GameObject item)
        {
            base.SelectItem(item);
            GameManager.Instance.OnChangeVehicle(item.GetComponent<InventoryItem>().ItemPrefab);
        }
    }
}
