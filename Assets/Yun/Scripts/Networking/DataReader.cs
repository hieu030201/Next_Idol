using System.IO;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;

public class DataReader : MonoBehaviour
{
    public SkinDataCollection value;
    [Button]
    void ReadingData()  
    {  
        // Ensure the ScriptableObject is assigned  
        if (value != null)  
        {  
            // Convert the Scriptable Object to JSON  
            string jsonData = JsonUtility.ToJson(value, true); // 'true' for pretty printing  
            string filePath = Path.Combine(Application.streamingAssetsPath, "SkinDataCollectionConfig.json"); // Define the file path  
            File.WriteAllText(filePath, jsonData); // Write the JSON data to the text file  
        }  
        else  
        {  
            Debug.LogError("GameData ScriptableObject is not assigned!");  
        }  
    }  
}
