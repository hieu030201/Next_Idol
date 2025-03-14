using TMPro;
using UnityEditor;
using UnityEngine;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    // Để sử dụng SerializedProperty

#if UNITY_EDITOR
    [ExecuteInEditMode] // Cho phép script chạy trong Editor
#endif
    public class YunTextShadow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text1;
        [SerializeField] private TextMeshProUGUI text2;

        [SerializeField] private string displayText;

        // Tạo property để tự động cập nhật text khi thay đổi giá trị
        public string DisplayText
        {
            get { return displayText; }
            set
            {
                displayText = value;
                UpdateTexts();
            }
        }

        private void OnValidate() // Được gọi khi có thay đổi trong Inspector
        {
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            if (text1 != null)
                text1.text = displayText;
        
            if (text2 != null)
                text2.text = displayText;
        }
    }

#if UNITY_EDITOR
// Custom Editor để tạo giao diện Inspector đẹp hơn
    [CustomEditor(typeof(YunTextShadow))]
    public class TextControllerEditor : Editor
    {
        SerializedProperty text1Prop;
        SerializedProperty text2Prop;
        SerializedProperty displayTextProp;

        private void OnEnable()
        {
            // Lấy reference đến các property
            text1Prop = serializedObject.FindProperty("text1");
            text2Prop = serializedObject.FindProperty("text2");
            displayTextProp = serializedObject.FindProperty("displayText");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(text1Prop, new GUIContent("Text 1"));
            EditorGUILayout.PropertyField(text2Prop, new GUIContent("Text 2"));
        
            EditorGUILayout.Space(10);
        
            EditorGUILayout.PropertyField(displayTextProp, new GUIContent("Display Text"));

            if (serializedObject.hasModifiedProperties)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
#endif
}