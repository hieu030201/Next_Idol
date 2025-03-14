using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Yun.Scripts.UI
{
    public class YunTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        private GameObject _selected;
        private GameObject _unselected;
        private YunTabGroup _tabGroup;

        private void Awake()
        {
            _selected = transform.Find("Selected").gameObject;
            _unselected = transform.Find("Unselected").gameObject;
        }

        public void SetTabGroup(YunTabGroup tabGroup)
        {
            _tabGroup = tabGroup;
        }

        public GameObject Active
        {
            get => _selected;
        }

        public GameObject Idle
        {
            get => _unselected;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // tabGroup.OnTabEnter(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_selected.activeSelf)
                return;
            // Debug.Log(gameObject.name + ", " + _tabGroup);
            _tabGroup.OnTabSelected(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // tabGroup.OnTabExit(this);
        }
    }
}
