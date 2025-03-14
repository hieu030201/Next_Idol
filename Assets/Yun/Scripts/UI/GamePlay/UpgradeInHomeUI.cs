using TMPro;
using UnityEngine;
using Yun.Scripts.Managers;

namespace Yun.Scripts.UI.GamePlay
{
    public class UpgradeInHomeUI : BaseUI
    {
        [SerializeField] private GameObject gatlingPrefab;
        [SerializeField] private GameObject rocketPrefab;
        [SerializeField] private GameObject tireSpikePrefab;
        [SerializeField] private GameObject tireSteelPrefab;
        [SerializeField] private GameObject alertTxt;
        [SerializeField] private SpeedBar speedBar;
        [SerializeField] private TextMeshProUGUI gunLevelTxt;
        [SerializeField] private TextMeshProUGUI tireLevelTxt;
        [SerializeField] private TextMeshProUGUI defenseLevelTxt;

        private int _gunLevel = 1;
        private int _tireLevel = 1;
        private int _defenseLevel = 1;
        
        protected override void Start()
        {
            Show();
            alertTxt.SetActive(false);
            gunLevelTxt.text = "LEVEL " + _gunLevel.ToString();
            tireLevelTxt.text = "LEVEL " + _tireLevel.ToString();
            defenseLevelTxt.text = "LEVEL " + _defenseLevel.ToString();
        }

        public override void Show()
        {
            gameObject.SetActive(true);
            speedBar.SetSpeed(50);
            GameManager.Instance.VehiceSpeed = 5;
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnLuckyBtnClick()
        {
            alertTxt.GetComponent<TextMeshProUGUI>().text = "Wish you luck next time";
            ShowEffect(YunEffectType.BubbleEffect, alertTxt.transform);
        }

        public void OnUpgradeDefenseBtnClick()
        {
            if(_defenseLevel == 2)
                return;
            _defenseLevel++;
            defenseLevelTxt.text = "LEVEL " + _defenseLevel.ToString();
            GameManager.Instance.UpgradeDefense();
        }

        public void OnUpgradeGunBtnClick()
        {
            if(_gunLevel == 2)
                return;
            _gunLevel++;
            gunLevelTxt.text = "LEVEL " + _gunLevel.ToString();
            GameManager.Instance.UpgradeGun(rocketPrefab);
        }

        private GameObject _currentTires;

        public void OnUpgradeTireBtnClick()
        {
            if(_tireLevel == 3)
                return;
            _tireLevel++;
            tireLevelTxt.text = "LEVEL " + _tireLevel.ToString();
            if (_currentTires == tireSteelPrefab)
            {
            }
            else if (_currentTires == tireSpikePrefab)
            {
                _currentTires = tireSteelPrefab;
                GameManager.Instance.UpgradeTires(_currentTires);
                speedBar.SetSpeed(150);
                GameManager.Instance.VehiceSpeed = 15;
            }
            else
            {
                _currentTires = tireSpikePrefab;
                GameManager.Instance.UpgradeTires(_currentTires);
                speedBar.SetSpeed(100);
                GameManager.Instance.VehiceSpeed = 10;
            }
        }

        public void OnPlayBtnClick()
        {
            Debug.Log("OnPlayBtnClick OnPlayBtnClick");
            gameObject.SetActive(false);
            GameManager.Instance.StartGame();
        }
    }
}