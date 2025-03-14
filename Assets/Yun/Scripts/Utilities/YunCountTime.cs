using UnityEngine;

namespace Yun.Scripts.Utilities
{
    public class YunCountTime : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            gameObject.SetActive(false);
        }

        private YunCountTimeData _countTimeData;
        public void Init(string countTimeName, int countNumber, int saveInterval, bool isCountingWhenGameOff)
        {
            if (!PlayerPrefs.HasKey(countTimeName))
            {
                
            }
            PlayerPrefs.GetInt("mapIndex", 1);
            PlayerPrefs.SetInt(countTimeName, countNumber);

            var data = new YunCountTimeData
            {
                countTime = countNumber,
                saveInterval = saveInterval,
                isCountingWhenGameOff = isCountingWhenGameOff
            };
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(countTimeName, json);
        }

        public void StartCounting()
        {
            
        }
    }
}
