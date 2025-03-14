using Yun.Scripts.UI.GamePlay.IdleGame;
using Yun.Scripts.Utilities;

public class RepairDamagedRoomPopup : MonetizationPopup
{
    public void SetMoney(int money)
    {
        RootTransform.Find("Content").Find("Use_Money_Btn").transform.Find("Shadow_Text")
            .GetComponent<YunTextShadow>()
            .DisplayText = UtilitiesFunction.FormatNumber(money);
        MoneyCostForSkipAds = money;
    }
}