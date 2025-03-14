using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class YunTextShadowEffect : MonoBehaviour 
    {
        [Button]
        public void SetShadow()
        {
            // Tùy chỉnh shadow thông qua material
            text.fontSharedMaterial.EnableKeyword("UNDERLAY_ON");
            text.fontSharedMaterial.SetFloat("_UnderlayOffsetX", 1); // Offset X
            text.fontSharedMaterial.SetFloat("_UnderlayOffsetY", -1); // Offset Y
            text.fontSharedMaterial.SetFloat("_UnderlayDilate", 2);  // Độ dày
            text.fontSharedMaterial.SetFloat("_UnderlaySoftness", 0); // Độ mềm
            text.fontSharedMaterial.SetColor("_UnderlayColor", new Color(0,0,0,0.5f)); // Màu
        }
        
        public TextMeshProUGUI text;
    
        void Start()
        {
            // Tùy chỉnh shadow thông qua material
            text.fontSharedMaterial.EnableKeyword("UNDERLAY_ON");
            text.fontSharedMaterial.SetFloat("_UnderlayOffsetX", 1); // Offset X
            text.fontSharedMaterial.SetFloat("_UnderlayOffsetY", -1); // Offset Y
            text.fontSharedMaterial.SetFloat("_UnderlayDilate", 10);  // Độ dày
            text.fontSharedMaterial.SetFloat("_UnderlaySoftness", 0); // Độ mềm
            text.fontSharedMaterial.SetColor("_UnderlayColor", new Color(0,0,0,0.5f)); // Màu
        }
    }
}