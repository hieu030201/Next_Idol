using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI
{
    public class BasePopup : BaseUI
    {
        public YunEffectType yunEffectType = YunEffectType.FlyUpEffect;
        public override void Show()
        {
            // var content = transform.Find("Content");
            // ShowEffect(YunEffectType.BubblePopupEffect, content.transform);
            
            var content = transform.Find("Content");
            ShowEffect(yunEffectType, content.transform);
        }

        public override void Hide()
        {
        }
        
        public virtual void Close()
        {
            CanvasManager.Instance.HidePopup(UIName);
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
        }
    }
}