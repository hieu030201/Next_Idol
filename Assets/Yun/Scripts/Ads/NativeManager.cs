using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Yun.Scripts.Ads
{
    public class NativeManager : MonoBehaviour
    {
        // public static NativeManager instance;

        [SerializeField] private GameObject _nativeLoading;

        private void Awake()
        {
            // instance = this;
            if(_nativeLoading.transform.Find("AdLoaded"))
                _nativeLoading.transform.Find("AdLoaded").gameObject.SetActive(false);
            if(_nativeLoading.transform.Find("AdLoading"))
                _nativeLoading.transform.Find("AdLoading").gameObject.SetActive(false);
        }

        private void Start()
        {
            ShowNativeLoading(true);
        }

        public void OnInstallBtnClick()
        {
            Debug.Log("OnInstallBtnClick");
        }

        [Button]
        public void ShowNativeLoading(bool isShow)
        {
            _nativeLoading.SetActive(isShow);
            if (!isShow) return;
            Advertisements.Instance.setNativeObject(_nativeLoading);
            Advertisements.Instance.SetTextureAndDetail();
        }
    }
}
