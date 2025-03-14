using TMPro;
using UnityEngine;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class AlertPopup : BasePopup
    {
        [SerializeField] private TextMeshProUGUI txt;
        
        public void SetContent(string content)
        {
            txt.text = content;    
        }
    }
}