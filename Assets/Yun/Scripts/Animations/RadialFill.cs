using Sirenix.OdinInspector;
using UnityEngine;

namespace Yun.Scripts.Animations
{
    public class RadialFill : MonoBehaviour
    {
        private Material _material;
        [SerializeField] [Range(0, 1)] private float fillAmount = 0;

        private bool _isFill;

        private float _duration;

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        [Button]
        public void StartFill(float duration)
        {
            _duration = duration;
            fillAmount = 0;
            _isFill = true;
        }

        public void StopFill()
        {
            if(!_material)
                return;
            _isFill = false;
            fillAmount = 0;
            _material.SetFloat("_FillAmount", fillAmount);
        }

        public void ShowFullFill()
        {
            _isFill = false;
            fillAmount = 1;
            _material.SetFloat("_FillAmount", fillAmount);
        }

        private void Update()
        {
            if (!_isFill)
                return;
            // Cập nhật giá trị fill
            _material.SetFloat("_FillAmount", fillAmount);

            // Ví dụ fill dần trong 2 giây
            fillAmount = Mathf.Clamp01(fillAmount + Time.deltaTime / _duration);
            // Debug.Log("update radial fill: " + fillAmount);
            if (fillAmount >= 1)
                _isFill = false;
        }
    }
}