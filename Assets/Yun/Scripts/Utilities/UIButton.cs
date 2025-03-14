using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace TwistedTangle.HieuUI
{
 
    public class UIButton : Button
    {
        private Vector2 buttonSize;
        private Vector3 vec3Default;
        private TypeButton type;
        protected override void Awake()
        {
            vec3Default = transform.localScale;
        }

        protected override void Start()
        {
            buttonSize = transform.GetComponent<RectTransform>().rect.size;
            
        }

        /*public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (IsInteractable() == false) return;

            if (buttonSize.x < 800) transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.1f);
            else transform.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.1f);
        }*/

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (IsInteractable() == false) return;
            transform.DOScale(vec3Default, 0.1f);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (transform.gameObject.GetComponent<EffectButtonActive>())
            {
                type = transform.gameObject.GetComponent<EffectButtonActive>().typebtn;
            }
            
            base.OnPointerDown(eventData);
            if (IsInteractable() == false) return;
            switch (type)
            {
                case TypeButton.NoBounce:
                    transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.1f);
                    break;
                case TypeButton.BounceButton:
                    transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.15f)  
                        .SetEase(Ease.Linear);  
                    break;
                case TypeButton.EslaticButton:
                    transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.15f)  
                        .SetEase(Ease.OutBack);  
                    break;
                case TypeButton.NoneEffect:
                    transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
                    break;
                default:
                    transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.1f);
                    break;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (IsInteractable() == false) return;
            switch (type)
            {
                case TypeButton.NoBounce:
                    transform.DOScale(vec3Default, 0.1f);
                    break;
                case TypeButton.BounceButton:
                    transform.DOScale(vec3Default, 0.1f)  
                        .SetEase(Ease.OutBounce); 
                    break;
                case TypeButton.EslaticButton:
                    transform.DOScale(vec3Default, 0.3f)  
                        .SetEase(Ease.OutBounce); 
                    break;
                case TypeButton.NoneEffect:
                    transform.DOScale(vec3Default, 0.1f);
                    break;
                default:
                    transform.DOScale(vec3Default, 0.1f);
                    break;
            }
        }
    }
}