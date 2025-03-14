using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class UpgradeButtonBg : YunBehaviour
    {
        [SerializeField] private YunTextShadow priceTxt;
        [SerializeField] private TextMeshProUGUI levelTxt;
        [SerializeField] private TextMeshProUGUI titleTxt;
        [SerializeField] private TextMeshProUGUI levelActiveTxt;
        [SerializeField] private GameObject levelActive;
        [SerializeField] private GameObject lockedIcon;
        [SerializeField] private GameObject dollarDeactivateIcon;
        [SerializeField] private Button buyBtn;
        [SerializeField] private GameObject deactivateButton;
        [SerializeField] private YunTextShadow priceDeactivateTxt;

        protected override void Awake()
        {
            base.Awake();
            levelActive.SetActive(false);
            lockedIcon.SetActive(false);
        }

        public void SetLevel(int level)
        {
            levelTxt.text = "LV " + level.ToString();
        }

        public void SetPrice(int price, int playerMoney)
        {
            // dolarIcon.SetActive(playerMoney >= price);
            buyBtn.gameObject.SetActive(playerMoney >= price);
            deactivateButton.gameObject.SetActive(!(playerMoney >= price));
            if (playerMoney >= price)
            {
                priceTxt.DisplayText = price.ToString();
            }
            else
            {
                FacilityManager.Instance.ShowGetMoneyPoints();
                dollarDeactivateIcon.SetActive(true);
                priceDeactivateTxt.DisplayText = price.ToString();
            }
            
            buyBtn.interactable = playerMoney >= price;
        }

        public void SetLevelActive(int level)
        {
            // dolarIcon.SetActive(false);
            buyBtn.gameObject.SetActive(false);
            // deactivateButton.gameObject.SetActive(true);
            lockedIcon.SetActive(true);
            priceDeactivateTxt.DisplayText = "Unlock";
            levelActiveTxt.text = level.ToString();
            levelActive.SetActive(true);
            dollarDeactivateIcon.SetActive(false);
            // buyBtn.interactable = false;
        }
    }
}