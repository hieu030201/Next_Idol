using UnityEngine;

namespace _Game.Scripts
{
    public class FPSDisplay : MonoBehaviour
    {
        private float _deltaTime;
        private int _count;
        private float _totalFps;
        private float _saveFps;
        private bool isShow = true;

        private void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            if(!isShow)
                return;
            var fps = 1.0f / _deltaTime;

            _count++;
            _totalFps += fps;

            int w = Screen.width, h = Screen.height;
            var style = new GUIStyle();
            const int size = 80;
            var rect = new Rect(100, 200, w, h * 2 / size);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / size;
            style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            var text = "FPS: " + _saveFps;
            GUI.Label(rect, text, style);

            if (_count < 60)
                return;
            
            var averageFps = _totalFps / _count;
            _saveFps = averageFps;
            _count = 0;
            _totalFps = 0;
        }
    }
}