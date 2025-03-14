using UnityEngine;

namespace Yun.Scripts.Utilities
{
    public static class UtilitiesFunction
    {
        public static bool Chance(int rand, int max = 100)
        {
            return Random.Range(0, max) < rand;
        }
        
        public static string FormatNumber(int value)
        {
            if(value < 1000)
                return value.ToString();
            var result = value / 1000f;
            if (Mathf.Approximately(result, (int)result))
            {
                // Nếu kết quả là số nguyên (không có phần thập phân)
                return ((int)result) + "k";
            }
            // Nếu có phần thập phân
            return result.ToString("0.0") + "k";
        }
    }
}