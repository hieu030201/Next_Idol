using UnityEngine;

namespace Yun.Scripts
{
    public class AndroidQualityManager : MonoBehaviour
    {
        [SerializeField]
        private bool applyOnStart = true;

        [SerializeField]
        private int targetFrameRate = 30;

        [SerializeField]
        private int lowQualityLevel = 0; // Lowest quality level

        void Start()
        {
            if (applyOnStart)
            {
                ApplyQualitySettings();
            }
        }

        public void ApplyQualitySettings()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                int sdkInt = version.GetStatic<int>("SDK_INT");
                
                // Android 11 là SDK version 30
                if (sdkInt <= 31)
                {
                    // Áp dụng cài đặt chất lượng thấp
                    // ApplyLowQualitySettings();
                    ApplyLowestQualitySettings();
                    Debug.Log($"Detected Android SDK {sdkInt} (Android 11 or lower). Applied low quality settings.");
                }
                else
                {
                    // Giữ nguyên cài đặt mặc định hoặc áp dụng cài đặt cao hơn
                    // ApplyDefaultQualitySettings();
                    Debug.Log($"Detected Android SDK {sdkInt} (Above Android 11). Keeping default quality settings.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error detecting Android version: {e.Message}");
            // Trong trường hợp lỗi, áp dụng cài đặt an toàn
            ApplyLowQualitySettings();
        }
#endif
        }
        
        private void ApplyLowestQualitySettings()
        {
            // Tắt hiệu ứng hậu kỳ (post-processing)
            if (Camera.main != null)
            {
                Camera.main.allowHDR = false;
                Camera.main.allowMSAA = false;
            }
            
            // Giảm số lượng particle
            QualitySettings.particleRaycastBudget = 4;
        
            // Tắt Anisotropic Filtering
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;

            // Đặt quality level thấp nhất
            QualitySettings.SetQualityLevel(lowQualityLevel, true);

            // Giảm độ phân giải texture
            QualitySettings.globalTextureMipmapLimit = 12; // 0 = full res, 1 = half res, 2 = quarter res

            // Tắt anti-aliasing
            QualitySettings.antiAliasing = 0;

            // Giảm shadow distance và quality
            QualitySettings.shadowDistance = 0;
            QualitySettings.shadowResolution = ShadowResolution.Low;
            QualitySettings.shadows = ShadowQuality.Disable;

            // Tắt các hiệu ứng hậu kỳ không cần thiết
            QualitySettings.softParticles = false;
            QualitySettings.softVegetation = false;
            QualitySettings.realtimeReflectionProbes = false;
            QualitySettings.billboardsFaceCameraPosition = false;

            // Tắt VSync
            QualitySettings.vSyncCount = 0;

            // Giảm LOD Bias
            QualitySettings.lodBias = 0.3f;

            // Giảm pixel light count
            QualitySettings.pixelLightCount = 0;

            // Áp dụng texture compression
            QualitySettings.globalTextureMipmapLimit = 1;
        }

        private void ApplyLowQualitySettings()
        {
            // Đặt quality level thấp nhất
            QualitySettings.SetQualityLevel(lowQualityLevel, true);

            // Giảm độ phân giải texture
            QualitySettings.globalTextureMipmapLimit = 1; // 0 = full res, 1 = half res, 2 = quarter res

            // Tắt anti-aliasing
            QualitySettings.antiAliasing = 0;

            // Giảm shadow distance và quality
            QualitySettings.shadowDistance = 15f;
            QualitySettings.shadowResolution = ShadowResolution.Low;
            QualitySettings.shadows = ShadowQuality.HardOnly;

            // Tắt các hiệu ứng hậu kỳ không cần thiết
            QualitySettings.softParticles = false;
            QualitySettings.softVegetation = false;
            QualitySettings.realtimeReflectionProbes = false;
            QualitySettings.billboardsFaceCameraPosition = false;

            // Giới hạn frame rate
            Application.targetFrameRate = targetFrameRate;

            // Tắt VSync
            QualitySettings.vSyncCount = 0;

            // Giảm LOD Bias
            QualitySettings.lodBias = 0.3f;

            // Giảm pixel light count
            QualitySettings.pixelLightCount = 0;

            // Áp dụng texture compression
            QualitySettings.globalTextureMipmapLimit = 1;
        }

        private void ApplyDefaultQualitySettings()
        {
            // Khôi phục các cài đặt mặc định hoặc áp dụng cài đặt cao hơn
            QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1, true);
            // Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 1;
            QualitySettings.globalTextureMipmapLimit = 0;
            QualitySettings.antiAliasing = 2;
            QualitySettings.shadowDistance = 40f;
            QualitySettings.shadowResolution = ShadowResolution.Medium;
            QualitySettings.shadows = ShadowQuality.All;
            QualitySettings.softParticles = true;
            QualitySettings.softVegetation = true;
            QualitySettings.realtimeReflectionProbes = true;
            QualitySettings.billboardsFaceCameraPosition = true;
            QualitySettings.lodBias = 1f;
            QualitySettings.pixelLightCount = 2;
            QualitySettings.globalTextureMipmapLimit = 0;
        }

        // Phương thức để kiểm tra phiên bản Android hiện tại
        public static int GetAndroidVersion()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return version.GetStatic<int>("SDK_INT");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error getting Android version: {e.Message}");
            return -1;
        }
#else
            return -1;
#endif
        }
    }
}