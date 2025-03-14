using System;
using Advertising;
using UnityEngine;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class GetBoosterPoint : YunBehaviour
    {
        [SerializeField] private GameObject hideContent;

        protected override void Awake()
        {
            base.Awake();

            hideContent.transform.Find("Mini_Tank").gameObject.SetActive(true);
            hideContent.transform.Find("Sleight_Base").gameObject.SetActive(false);
            if (FireBaseManager.Instance.showNoel)
            {
                hideContent.transform.Find("Mini_Tank").gameObject.SetActive(false);
                hideContent.transform.Find("Sleight_Base").gameObject.SetActive(true);
            }
        }

        public bool IsActive
        {
            get => hideContent.activeSelf;
            set => hideContent.SetActive(value);
        }
    }
}