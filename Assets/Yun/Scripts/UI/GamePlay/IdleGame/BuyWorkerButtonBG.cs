using TMPro;
using TwistedTangle.HieuUI;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class BuyWorkerButtonBG : YunBehaviour
    {
        [SerializeField] private YunTextShadow priceTxt;
        [SerializeField] private TextMeshProUGUI quantityTxt;
        [SerializeField] private TextMeshProUGUI levelActiveTxt;
        [SerializeField] private GameObject levelActive;
        [SerializeField] private GameObject maxIcon;
        [SerializeField] private GameObject overLayer;
        [SerializeField] private UIButton buyBtn;
        [SerializeField] private GameObject deactivateButton;
        [SerializeField] private YunTextShadow priceDeactivateTxt;

        protected override void Awake()
        {
            base.Awake();
            levelActive.SetActive(false);
            maxIcon.SetActive(false);
            overLayer.SetActive(false);
        }

        public void SetQuantity(int quantity)
        {
            quantityTxt.text = "Quantity: " + quantity.ToString();
        }

        public void SetMax()
        {
            maxIcon.SetActive(true);
            buyBtn.gameObject.SetActive(false);
            deactivateButton.SetActive(false);
            levelActive.SetActive(false);
        }

        public void SetPrice(int price, int playerMoney)
        {
            buyBtn.gameObject.SetActive(playerMoney >= price);
            deactivateButton.gameObject.SetActive(!(playerMoney >= price));
            priceTxt.DisplayText = price.ToString();
            priceDeactivateTxt.DisplayText = price.ToString();
            
            if(playerMoney < price)
                FacilityManager.Instance.ShowGetMoneyPoints();
        }

        public void SetLevelActive(int level)
        {
            buyBtn.gameObject.SetActive(false);
            deactivateButton.SetActive(true);
            levelActiveTxt.text = level.ToString();
            levelActive.SetActive(true);
            overLayer.SetActive(true);
        }
    }
}