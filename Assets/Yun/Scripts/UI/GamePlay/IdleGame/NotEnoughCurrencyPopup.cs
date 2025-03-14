using UnityEngine;
using Yun.Scripts.UI.GamePlay.IdleGame;

public class NotEnoughCurrencyPopup : MonetizationPopup
{
    [SerializeField] private YunTextShadow moneyTxt;

    public void SetMoney(int money)
    {
        moneyTxt.DisplayText = money.ToString();
    }
}
