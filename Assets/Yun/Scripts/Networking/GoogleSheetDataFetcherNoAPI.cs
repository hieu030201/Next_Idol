using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Yun.Scripts.Networking
{
    public class GoogleSheetDataFetcherNoAPI : MonoBehaviour
    {
        [SerializeField] 
        private string googleSheetId = "YOUR_GOOGLE_SHEET_ID";
    
        [SerializeField] 
        private string sheetId = "0"; // Thường là 0 cho sheet đầu tiên

        void Start()
        {
            StartCoroutine(FetchGoogleSheetData());
        }

        IEnumerator FetchGoogleSheetData()
        {
            string url = $"https://docs.google.com/spreadsheets/d/{googleSheetId}/export?format=csv&gid={sheetId}";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error: {webRequest.error}");
                }
                else
                {
                    string csvData = webRequest.downloadHandler.text;
                    string jsonData = ConvertCsvToJson(csvData);
                    ProcessJsonData(jsonData);
                }
            }
        }

        string ConvertCsvToJson(string csv)
        {
            Debug.Log("ConvertCsvToJson: " + csv);
            var lines = csv.Split('\n');
            var headers = lines[0].Split(',');
            var jsonResult = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                if (values.Length != headers.Length) continue;

                var entry = new Dictionary<string, string>();
                for (int j = 0; j < headers.Length; j++)
                {
                    entry[headers[j].Trim()] = values[j].Trim();
                }
                jsonResult.Add(entry);
            }

            return JsonUtility.ToJson(new { data = jsonResult });
        }

        void ProcessJsonData(string jsonData)
        {
            Debug.Log(jsonData);
            // Xử lý dữ liệu JSON ở đây
        }
    }
}