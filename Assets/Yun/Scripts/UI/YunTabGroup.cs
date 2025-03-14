using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;

namespace Yun.Scripts.UI
{
    public class YunTabGroup : YunBehaviour
    {
        private List<YunTabButton> _tabButtonsList;
        
        private List<GameObject> _pagesList;

        protected override void Awake()
        {
            base.Awake();
            _tabButtonsList = new List<YunTabButton>();
            var tabButtonsListContainer = transform.Find("Tab_Buttons_List");
            for (var i = 0; i < tabButtonsListContainer.childCount; i++)
            {
                // Debug.Log(tabButtonsListContainer.GetChild(i).gameObject.name);
                _tabButtonsList.Add(tabButtonsListContainer.GetChild(i).GetComponent<YunTabButton>());
                tabButtonsListContainer.GetChild(i).GetComponent<YunTabButton>().SetTabGroup(this);
            }
            _pagesList = new List<GameObject>();
            var pagesListContainer = transform.Find("Pages_List");
            for (var i = 0; i < pagesListContainer.childCount; i++)
            {
                _pagesList.Add(pagesListContainer.GetChild(i).gameObject);
            }
        }

        protected override void Start()
        {
            base.Start();
            OnTabSelected(_tabButtonsList[0]);
        }

        public void OnTabSelected(YunTabButton button)
        {
            foreach (var t in _tabButtonsList)
            {
                t.Idle.SetActive(true);
                t.Active.SetActive(false);
            }
            
            button.Idle.SetActive(false);
            button.Active.SetActive(true);
            var index = button.transform.GetSiblingIndex();
            for (var i = 0; i < _pagesList.Count; i++)
            {
                _pagesList[i].SetActive(i == index);
            }
        }
    }
}