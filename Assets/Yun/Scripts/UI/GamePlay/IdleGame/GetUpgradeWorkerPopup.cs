using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.GamePlay.IdleGame.Employees;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class GetUpgradeWorkerPopup : MonetizationPopup
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void ShowUp()
        {
            var value = RenderModel.transform.Find("BaseModel/base").GetComponent<Animator>();
            if (value != null)
            {
                value.Play("Female_Soldier_Showup");
            }
        }
    }
}