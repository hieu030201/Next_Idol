using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Yun.Scripts
{
    public class MemoryProfiler : MonoBehaviour
    {
        private Dictionary<string, long> previousMemorySnapshot = new Dictionary<string, long>();
        private float updateInterval = 5f; // Cập nhật mỗi 5 giây
        private float timer = 0f;

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                AnalyzeMemory();
                timer = 0f;
            }
        }

        void AnalyzeMemory()
        {
            Dictionary<string, long> currentSnapshot = new Dictionary<string, long>();

            // Lấy thông tin về memory của các loại tài nguyên chính
            currentSnapshot["Total Reserved"] = Profiler.GetTotalReservedMemoryLong();
            currentSnapshot["Total Allocated"] = Profiler.GetTotalAllocatedMemoryLong();
            currentSnapshot["Mono Used"] = Profiler.GetMonoUsedSizeLong();
            currentSnapshot["Texture Memory"] = Profiler.GetAllocatedMemoryForGraphicsDriver();

            // So sánh với snapshot trước đó
            foreach (var entry in currentSnapshot)
            {
                if (previousMemorySnapshot.ContainsKey(entry.Key))
                {
                    long difference = entry.Value - previousMemorySnapshot[entry.Key];
                    if (difference > 1024 * 1024) // Chỉ hiện những thay đổi lớn hơn 1MB
                    {
                        Debug.LogWarning($"Memory increase detected in {entry.Key}: " +
                                         $"+{FormatBytes(difference)} " +
                                         $"(Total: {FormatBytes(entry.Value)})");
                    
                        // Phân tích chi tiết nếu phát hiện tăng đáng kể
                        if (entry.Key == "Texture Memory")
                        {
                            AnalyzeTextures();
                        }
                        else if (entry.Key == "Mono Used")
                        {
                            AnalyzeGameObjects();
                        }
                    }
                }
            }

            previousMemorySnapshot = currentSnapshot;
        }

        void AnalyzeTextures()
        {
            // Tìm tất cả texture đang được load
            Texture[] allTextures = Resources.FindObjectsOfTypeAll<Texture>();
            var largeTextures = allTextures
                .Where(t => t.width * t.height * 4 > 1024 * 1024) // Texture lớn hơn 1MB
                .OrderByDescending(t => t.width * t.height)
                .Take(5);

            foreach (var texture in largeTextures)
            {
                Debug.LogWarning($"Large texture detected: {texture.name} " +
                                 $"Size: {texture.width}x{texture.height} " +
                                 $"({FormatBytes(texture.width * texture.height * 4)})");
            }
        }

        void AnalyzeGameObjects()
        {
            // Phân tích các GameObject trong scene
            var allObjects = FindObjectsOfType<GameObject>();
            var objectCounts = new Dictionary<string, int>();

            foreach (var obj in allObjects)
            {
                string key = obj.name.Split('(')[0]; // Bỏ qua suffix như (Clone)
                if (!objectCounts.ContainsKey(key))
                    objectCounts[key] = 0;
                objectCounts[key]++;
            }

            // Hiển thị các object xuất hiện nhiều lần
            foreach (var count in objectCounts.Where(c => c.Value > 10))
            {
                Debug.LogWarning($"Many instances of {count.Key}: {count.Value} objects");
            }
        }

        string FormatBytes(long bytes)
        {
            string[] suffixes = { "B", "KB", "MB", "GB" };
            int counter = 0;
            decimal number = bytes;
            while (number > 1024 && counter < suffixes.Length - 1)
            {
                number /= 1024;
                counter++;
            }
            return $"{number:n1} {suffixes[counter]}";
        }
    }
}