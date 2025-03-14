using System.Collections;
using System.Collections.Generic;
using Advertising;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs
{
    public class BattlePassHeroItem : BattlePassItem
    {
        public GameObject lockIcon;
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> lockIconList;
        public bool canClaim;
        
        public void Awake()
        {
            base.Awake();
            if (!FireBaseManager.Instance.showNoel)
            {
                lockIcon.GetComponent<Image>().sprite = lockIconList[0];
            }
            else
            {
                lockIcon.GetComponent<Image>().sprite = lockIconList[1];
            }
        }
        public override void ClaimItem()
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Button_Click);
            if (canClaim)
            {
                GetRewardItem();
                Claimed = true;
                SetBackGround();
                FacilityManager.Instance.GameSaveLoad.GameData.listIDBattlePassHero.Add(ID);
                FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
            }
        }
        public void HideLock()
        {
            lockIcon.SetActive(false);
            canClaim = true;
        }
    }
}
