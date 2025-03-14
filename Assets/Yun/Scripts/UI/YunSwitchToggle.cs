using UnityEngine;
using UnityEngine.Events;
using Yun.Scripts.Cores;

namespace Yun.Scripts.UI
{
    public class YunSwitchToggle : YunBehaviour
    {
        [SerializeField] private GameObject onStatus;
        [SerializeField] private GameObject offStatus;

        public UnityAction ONActive;
        public UnityAction ONDeactivate;

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                onStatus.SetActive(value);
                offStatus.SetActive(!value);
            }
        }

        private void Active()
        {
            IsActive = true;
            ONActive();
        }
        
        private void Deactivate()
        {
            IsActive = false;
            ONDeactivate();
        }

        public void OnPointerDown()
        {
            if(onStatus.activeSelf)
                Deactivate();
            else
                Active();
        }
    }
}
