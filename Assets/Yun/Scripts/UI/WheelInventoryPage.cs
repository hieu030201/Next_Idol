using UnityEngine;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI
{
    public class WheelInventoryPage : InventoryPage
    {
        protected override void SelectItem(GameObject item)
        {
            base.SelectItem(item);
            GameManager.Instance.OnChangeWheel(item.GetComponent<InventoryItem>().ItemPrefab);
        }
    }
}
