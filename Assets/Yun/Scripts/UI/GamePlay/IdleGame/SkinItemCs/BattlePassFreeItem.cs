using System;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;
namespace Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs
{
    public class BattlePassFreeItem : BattlePassItem
    {
        public GameObject hightLight;
        public GameObject hightLightBorder;
        public void Start()
        {
            if (ID == 2)
            {
                var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
                if (!FacilityManager.Instance.GameSaveLoad.GameData.listIDBattlePassFree.Contains(2) && mapIndex == 1)
                {
                    DOVirtual.DelayedCall(0.5f, () =>
                    {
                        SetHight(true);
                        battlePassPopup.ActiveOverLayHeader(true);
                    });
                }
                else
                {
                    SetHight(false);
                }
            }
            else
            {
                SetHight(false);
            }
            
        }

        public override void ClaimItem()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            GetRewardItem();
            Claimed = true;
            SetBackGround();
            if (ID == 2)
            {
                SetHight(false);
                battlePassPopup.ActiveOverLayHeader(false);
            }
            FacilityManager.Instance.GameSaveLoad.GameData.listIDBattlePassFree.Add(ID);
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
        }

        public override void SetHight(bool active)
        {
            hightLight.SetActive(active);
            hightLightBorder.SetActive(active);
        }
    }
}
