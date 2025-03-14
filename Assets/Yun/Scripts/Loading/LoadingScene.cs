using System.Collections;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.Ads;
using Yun.Scripts.UI;
using Yun.Scripts.UI.GamePlay.IdleGame;

namespace Yun.Scripts.Loading
{
    public class LoadingScene : Singleton<LoadingScene>
    {
        [SerializeField] private GameObject content;
        [SerializeField] private YunProgressBar progressBar;

        [SerializeField] private float minimumLoadingTime = 6f;
        [SerializeField] private string mainSceneName = "Idle_Game_Scene";
        [SerializeField] private YunTextShadow textShadow;
        [SerializeField] private GameObject nativeLoading;
        [SerializeField] private TextMeshProUGUI txt1;
        [SerializeField] private TextMeshProUGUI txt2;
        [SerializeField] private TextMeshProUGUI txt3;
        [SerializeField] public TextMeshProUGUI txt4;

        private bool _isLoadNativeAdsCompleted;
        private float _loadingProgress;
        private float _timeElapsed;
        private bool _isLoadingComplete;

        public bool _isConsentPopupClosed;
        private bool _isLoadingCompleted;

        private AsyncOperation _asyncOperation;

        private void Start()
        {
#if UNITY_EDITOR
            minimumLoadingTime = 2;
#endif
            txt1.text = "";
            txt2.text = "";
            txt3.text = "";
            progressBar.UpdateProgress(0);
            textShadow.DisplayText = "0%";

            var isFirstTimeLoading = PlayerPrefs.GetInt("isFirstTimeLoading3", 1);
            var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
            switch (mapIndex)
            {
                case 1:
                    mainSceneName = "Idle_Game_Scene";
                    break;
                case 2:
                    mainSceneName = "Idle_Game_Scene_Map_2";
                    break;
            }

            // if (isFirstTimeLoading == 1)
            //     nativeLoading.SetActive(false);
            Debug.Log("isFirstTimeLoading: " + isFirstTimeLoading);
            if (isFirstTimeLoading == 1)
            {
                txt1.text = "isFirstTimeLoading TRUE";
                UmpManager.Instance.InitUMP(() =>
                {
                    PlayerPrefs.SetInt("isFirstTimeLoading3", 0);
                    StartCoroutine(LoadSceneAsync());
                }, () => { Time.timeScale = 0; }, () => { Time.timeScale = 1; });
            }
            else
            {
                txt1.text = "isFirstTimeLoading FALSE";
                StartCoroutine(LoadSceneAsync());
            }
        }

        private AsyncOperation asyncOperation;

        IEnumerator LoadSceneAsync()
        {
            // Bắt đầu load scene nhưng không active
            asyncOperation = SceneManager.LoadSceneAsync(mainSceneName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;

            float elapsedTime = 0f;

            while (elapsedTime < minimumLoadingTime)
            {
                // Tính % loading
                float percentComplete = Mathf.Clamp01(elapsedTime / minimumLoadingTime);

                // Cập nhật slider
                progressBar.UpdateProgress(percentComplete);

                // Cập nhật text
                textShadow.DisplayText = Mathf.RoundToInt(percentComplete * 100) + "%";

                // Đợi frame tiếp theo
                yield return null;

                // Tăng thời gian
                elapsedTime += Time.deltaTime;
            }

            // Đảm bảo slider và text ở 100%
            progressBar.UpdateProgress(1);
            textShadow.DisplayText = "100%";

            _isLoadingCompleted = true;
            // Cho phép kích hoạt scene
            // if (_isConsentPopupClosed)
            // asyncOperation.allowSceneActivation = true;

            if (FireBaseManager.Instance)
            {
                txt1.text = "Firebase Ok";
            }
            else
            {
                txt1.text = "Firebase null";
            }

            if (AdsManager.Instance)
            {
                txt2.text = "AdsManager Ok";
                txt3.text = "showAoa false";
                if (FireBaseManager.Instance.showAoa)
                    txt3.text = "showAoa true";
            }
            else
            {
                txt2.text = "AdsManager null";
            }
            
            if (FireBaseManager.Instance && AdsManager.Instance && FireBaseManager.Instance.showAoa)
                AdsManager.Instance.FirstShowAppOpenAd(OnAppOpenAdHide, txt4);
            else
                asyncOperation.allowSceneActivation = true;
        }

        private void OnAppOpenAdHide()
        {
            asyncOperation.allowSceneActivation = true;
        }

        public void OnConsentPopupClosed()
        {
            _isConsentPopupClosed = true;
            if (_isLoadingCompleted)
                asyncOperation.allowSceneActivation = true;
        }

        public void HideNativeAds()
        {
            nativeLoading.SetActive(false);
        }

        public void ShowNativeAds()
        {
            return;
            if (!nativeLoading.activeSelf)
                return;
            if (_isLoadNativeAdsCompleted) return;
            _isLoadNativeAdsCompleted = true;
            if (FireBaseManager.Instance && Advertisements.Instance && FireBaseManager.Instance.showNativeLoading)
            {
                Advertisements.Instance.setNativeObject(nativeLoading);
                Advertisements.Instance.SetTextureAndDetail();
            }
        }
    }
}