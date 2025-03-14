using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms.Others
{
    public class ColorOverlay : MonoBehaviour
    {
        private Outlinable outlinable;
        [SerializeField] private float scaleTarget = 1.02f;
        private Vector3 originalScale;

        [SerializeField] private float duration = 0.2f;
        private void Start()
        {
            outlinable = GetComponent<Outlinable>();
            if (outlinable != null)
            {
                outlinable.enabled = false;
            }
       
            originalScale = transform.localScale;
        }
        [Button]
        public void VictimHit()
        {
            //outlinable.OutlineParameters.Color = Color.blue;
            outlinable.enabled = true; 
            
            transform.DOScale(originalScale * scaleTarget, duration) 
                .SetEase(Ease.OutBack) 
                .OnComplete(() =>  
                {  
                    outlinable.enabled = false;  
                });  

            DOVirtual.DelayedCall(0.1f, () =>  
            {  
                transform.DOScale(originalScale, 0.1f).SetEase(Ease.OutBack);  
            });  
        }

    }
}

