using UnityEngine;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI
{
    public class ProjectileInventoryPage : InventoryPage
    {
        protected override void SelectItem(GameObject item)
        {
            base.SelectItem(item);
            GameManager.Instance.OnChangeProjectile(item.GetComponent<InventoryItem>().ItemPrefab);
        }
    }
}
