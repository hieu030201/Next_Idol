using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.Networking
{
    public class GoogleSheetDataFetcher : MonoBehaviour
    {
        [SerializeField] private string googleSheetId = "1lqRLCJykqaRYu1EFqqrOBJm4AUQg73mDqij4FR6batY";

        [SerializeField] private string roomConfigSheetName = "Room_Config";
        [SerializeField] private string levelConfigSheetName = "Level_Config";

        private const string BaseURL = "https://sheets.googleapis.com/v4/spreadsheets/";
        private const string URLSuffix = "/values/";
        private const string APIKey = "AIzaSyCP-OWnZMBSGpCmcB6WcMf4UPb_ImRxYBQ";

        [Button]
        private void TestReadData()
        {
            
        }
        [Button]
        private void RunStart()
        {
            StartCoroutine(FetchLevelConfigGoogleSheetData());
        }

        private IEnumerator FetchLevelConfigGoogleSheetData()
        {
            Debug.Log("FetchLevelConfigGoogleSheetData");
            var url = $"{BaseURL}{googleSheetId}{URLSuffix}{levelConfigSheetName}?key={APIKey}&alt=json";

            using var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                var jsonResult = webRequest.downloadHandler.text;
                // ProcessJsonData(jsonResult);
                //Debug.Log("Level Config Data: " + jsonResult); 
                
                //Hàm ConvertGoogleSheetData có thể sẽ phân tích JSON này và tạo ra một danh sách các đối tượng.Kết quả trả ra từ ConvertGoogleSheetData sẽ là một danh sách các đối tượng LevelConfig, mà bạn có thể dễ dàng sử dụng trong mã của mình để cập nhật giao diện hoặc thực hiện các logic game.
                var result = GoogleSheetDataConverter.ConvertGoogleSheetData(jsonResult);
                Debug.Log(result);
                //FacilityManager.Instance.UpdateLevelConfigList(result);
                //StartCoroutine(FetchRoomConfigGoogleSheetData());
            }
        }
        
        private IEnumerator FetchRoomConfigGoogleSheetData()
        {
            Debug.Log("FetchRoomConfigGoogleSheetData");
            var url = $"{BaseURL}{googleSheetId}{URLSuffix}{roomConfigSheetName}?key={APIKey}&alt=json";

            using var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
            else
            {
                var jsonResult = webRequest.downloadHandler.text;
                // ProcessJsonData(jsonResult);
                Debug.Log("Room Config Data: " + jsonResult);  

                //var result = GoogleSheetDataConverter.ConvertGoogleSheetData(jsonResult);
                //FacilityManager.Instance.OnGetDataFromSheetComplete(result);
            }
        }
    }
}