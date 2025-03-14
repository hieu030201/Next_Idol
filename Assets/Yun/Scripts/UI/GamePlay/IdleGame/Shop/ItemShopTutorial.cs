using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.GamePlay.IdleGame.Managers;

public class ItemShopTutorial : MonoBehaviour
{
    [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] protected List<Sprite> iconImageList;
    public Image iconImage;
    void Awake()
    {
        if (!FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip) return;
        iconImage.sprite = iconImageList[1];
    }

    public void OnclickInItemTutorial()
    {
        FacilityManager.Instance.GameSaveLoad.StableGameData.isActivatedTutorialItemShop = true;
        FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
    }
}
