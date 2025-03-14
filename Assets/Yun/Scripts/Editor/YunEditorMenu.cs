using UnityEditor;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.Editor
{
    public static class YunEditorMenu
    {
        [MenuItem("YunGameData/ClearData")]
        public static void ClearData()
        {
            PlayerPrefs.DeleteAll();
            ES3.DeleteFile();
        }
    }
}