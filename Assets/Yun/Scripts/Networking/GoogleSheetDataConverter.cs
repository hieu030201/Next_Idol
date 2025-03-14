using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Yun.Scripts.Networking
{
    public class GoogleSheetDataConverter
    {
        [Serializable]
        private class GoogleSheetResponse
        {
            public string range;
            public string majorDimension;
            public List<List<string>> values;
        }

        public static List<List<string>> ConvertGoogleSheetData(string jsonResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<GoogleSheetResponse>(jsonResponse);

                if (response?.values == null || response.values.Count < 1)
                {
                    Debug.LogError("Invalid or empty Google Sheet data");
                    return null;
                }

                var result = new List<List<string>>();

                for (var i = 1; i < response.values.Count; i++)
                {
                    var currentRow = response.values[i];
                    result.Add(currentRow);
                }

                return result;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error converting Google Sheet data: {e.Message}");
                return null;
            }
        }
    }
}