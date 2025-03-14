using System.Text;
using TMPro;
using UnityEngine;

namespace Yun.Scripts
{
    public class RamMonitor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private float updateInterval = 1f; // Cập nhật mỗi 1 giây
        private float timer = 0f;
        private string displayText = "";
        private GUIStyle guiStyle = new GUIStyle();

        void Start()
        {
            // Thiết lập style cho text
            guiStyle.fontSize = 20;
            guiStyle.normal.textColor = Color.white;
            guiStyle.padding = new RectOffset(10, 10, 10, 10);
            guiStyle.fontStyle = FontStyle.Bold;
            // Thêm outline cho text
            guiStyle.normal.background = CreateBackgroundTexture(new Color(0, 0, 0, 0.5f));
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                UpdateMemoryInfo();
                timer = 0f;
            }
        }

        void UpdateMemoryInfo()
        {
            StringBuilder info = new StringBuilder();

            // Game Memory Usage
            // long totalMemory = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024);
            // long reservedMemory = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / (1024 * 1024);
        
            // System Memory
            // long systemMemory = SystemInfo.systemMemorySize;
        
            // Android specific memory info
#if UNITY_ANDROID && !UNITY_EDITOR
        using (var activityManager = new AndroidJavaObject("android.app.ActivityManager.MemoryInfo"))
        using (var memoryInfo = new AndroidJavaObject("android.app.ActivityManager$MemoryInfo"))
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject activityManagerInstance = currentActivity.Call<AndroidJavaObject>("getSystemService", "activity"))
            {
                activityManagerInstance.Call("getMemoryInfo", memoryInfo);
                long availMem = memoryInfo.Get<long>("availMem") / (1024 * 1024);
                long totalMem = memoryInfo.Get<long>("totalMem") / (1024 * 1024);
                bool lowMemory = memoryInfo.Get<bool>("lowMemory");

                // info.AppendLine($"Game RAM Usage: {totalMemory}MB");
                // info.AppendLine($"Game RAM Reserved: {reservedMemory}MB");
                info.AppendLine("---System Memory---");
                info.AppendLine($"Total RAM: {totalMem}MB");
                info.AppendLine($"Available RAM: {availMem}MB");
                info.AppendLine($"Used RAM: {totalMem - availMem}MB");
                if (lowMemory)
                {
                    info.AppendLine("WARNING: Low Memory!");
                }
            }
        }
#else
            // Thông tin cơ bản khi không chạy trên Android
            // info.AppendLine($"Game RAM Usage: {totalMemory}MB");
            // info.AppendLine($"Game RAM Reserved: {reservedMemory}MB");
            // info.AppendLine($"System Memory: {systemMemory}MB");
#endif

            displayText = info.ToString();
        }

        void OnGUI()
        {
            // Hiển thị thông tin ở góc trên bên trái màn hình
            // GUI.Label(new Rect(10, 10, Screen.width/2, Screen.height/2), displayText, guiStyle);
            text.text = displayText;
        }

        private Texture2D CreateBackgroundTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        void OnDestroy()
        {
            // Cleanup
            if (guiStyle.normal.background != null)
            {
                Destroy(guiStyle.normal.background);
            }
        }
    }
}