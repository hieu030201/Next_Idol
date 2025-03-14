using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.UI.GamePlay.IdleGame.AbilityItemCs;

public class AbilityNormalItem : AbilityItem
{
    
    [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)]
    protected List<Sprite> dashList;
    [SerializeField] protected Image dashImg;

    public override void SetItemProperty()
    {
        base.SetItemProperty();
        SetDashImage();
    }

    public void SetDashImage()
    {
        dashImg.sprite = isUnLock ? dashList[1] : dashList[0];
    }
}
