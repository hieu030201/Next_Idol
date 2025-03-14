using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace Yun.Scripts.UI
{
    public class HealthBar : GameUnit
    {
        // [SerializeField] private GameObject health;
        //
        // public float Health
        // {
        //     set
        //     {
        //         // health.transform.localScale = new Vector3(value, 1, 1);
        //         health.transform.DOScaleX(value,0.3f);
        //     }
        // }
        [SerializeField] private GameObject health;
        private Transform target;
        private Vector3 viewPoint;
        [SerializeField] RectTransform rect;
        [SerializeField] private Image progressBar;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] private float duration =0.5f;
        Vector3 screenHalf = new Vector2(Screen.width, Screen.height) / 2;
        public float Health
        {
            set
            {
                // health.transform.localScale = new Vector3(value, 1, 1);
                health.transform.DOScaleX(value,0.3f);
            }
        }

        public void OnInit()
        {
            progressBar.fillAmount = 1;
        }

        private void Update()
        {
            if (target == null) return;
            viewPoint = MainCamera.WorldToViewportPoint(target.position);
            Vector3 targetSPoint = MainCamera.ViewportToScreenPoint(viewPoint) - screenHalf;
            rect.anchoredPosition = targetSPoint / (Screen.width / 1080f);
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
            OnInit();
        }
        public void SetAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }
        
        public void SetHealth(int current, int full)  
        {  
            // Ensure full is not zero to avoid division by zero error  
            if (full <= 0)  
            {  
                Debug.LogWarning("Full health must be greater than zero.");  
                return;  
            }  

            // Calculate health percentage using integers  
            float healthPercentage = Mathf.Clamp((float)current / full, 0f, 1f);  
    
            // Update the progress bar  
            progressBar.fillAmount = healthPercentage;  
            progressBar.DOFillAmount(healthPercentage, duration);  
        }

        public void OnDestroy()
        {
            SimplePool.Despawn(this);
        }
    }
}
