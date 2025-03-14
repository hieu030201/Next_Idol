using UnityEngine;
using Yun.Scripts.Audios;

namespace Yun.Scripts.Datas
{
    public class SettingGameData
    {
        public void Init()
        {
            _isSoundOn = PlayerPrefs.GetInt("IsSoundOn", 1) == 1;
            _isMusicOn = PlayerPrefs.GetInt("IsMusicOn", 1) == 1;
            _isVibrationOn = PlayerPrefs.GetInt("IsVibrationOn", 1) == 1;
            AudioManager.Instance.IsSoundOn = _isSoundOn;
            AudioManager.Instance.IsMusicOn = _isMusicOn;
        }
        
        private bool _isSoundOn;

        public bool IsSoundOn
        {
            get => _isSoundOn;
            set
            {
                _isSoundOn = value;
                if(value)
                    PlayerPrefs.SetInt("IsSoundOn", 1);
                else
                    PlayerPrefs.SetInt("IsSoundOn", 0);
                AudioManager.Instance.IsSoundOn = value;
            }
        }
        
        private bool _isMusicOn;

        public bool IsMusicOn
        {
            get => _isMusicOn;
            set
            {
                _isMusicOn = value;
                if(value)
                    PlayerPrefs.SetInt("IsMusicOn", 1);
                else
                    PlayerPrefs.SetInt("IsMusicOn", 0);
                AudioManager.Instance.IsMusicOn = value;
            }
        }
        
        private bool _isVibrationOn;

        public bool IsVibrationOn
        {
            get => _isVibrationOn;
            set
            {
                _isVibrationOn = value;
                if(value)
                    PlayerPrefs.SetInt("IsVibrationOn", 1);
                else
                    PlayerPrefs.SetInt("IsVibrationOn", 0);
            }
        }
    }
}
