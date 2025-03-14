using System.Collections.Generic;
using UnityEngine;

namespace Yun.Scripts.UI
{
    public class InventoryPage : MonoBehaviour
    {
        private List<GameObject> _itemList;
        
        // Start is called before the first frame update
        private void Start()
        {
            _itemList = new List<GameObject>();
            for (var i = 0; i < transform.childCount; i++)
            {
                _itemList.Add(transform.GetChild(i).gameObject);
                if(i != 0)
                    transform.GetChild(i).gameObject.transform.Find("border").gameObject.SetActive(false);
            }
        }

        public void OnClickItem(GameObject item)
        {
            SelectItem(item);
        }

        protected virtual void SelectItem(GameObject item)
        {
            foreach (var _item in _itemList)
            {
                if (_item == item)
                {
                    _item.transform.Find("border").gameObject.SetActive(true);
                }
                else
                {
                    _item.transform.Find("border").gameObject.SetActive(false);
                }
            }
        }
    }
}
