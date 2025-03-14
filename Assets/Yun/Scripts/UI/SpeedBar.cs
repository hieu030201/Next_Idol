using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Yun.Scripts.UI
{
    public class SpeedBar : MonoBehaviour
    {
        [SerializeField] private GameObject child;

        [SerializeField] private TextMeshProUGUI speedTxt;
        // Start is called before the first frame update
        private void Start()
        {
            speedTxt.text = "";
            child.GetComponent<Image>().fillAmount = 0;
        }

        private int _startSpeed;
        private int _currentSpeed;
        private int _nextSpeed;
        public void SetSpeed(int speed)
        {
            _startSpeed = _currentSpeed;
            _nextSpeed = speed;
            DOVirtual.DelayedCall(1 / (float)(_nextSpeed - _startSpeed), (OnSpeedUp));
        }

        private void OnSpeedUp()
        {
            _currentSpeed++;
            speedTxt.text = _currentSpeed + "KM/H";
            child.GetComponent<Image>().fillAmount = ((float)_currentSpeed / 200);
            if (_currentSpeed < _nextSpeed)
            {
                DOVirtual.DelayedCall(1 / (float)(_nextSpeed - _startSpeed), (OnSpeedUp));
            }
        }
    }
}
