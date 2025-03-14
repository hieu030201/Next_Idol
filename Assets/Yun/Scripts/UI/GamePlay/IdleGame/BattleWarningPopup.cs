using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class BattleWarningPopup : BasePopup
    {
        public void OnStartBattle()
        {
            FacilityManager.Instance.BattleManager.OnStartBattle();
            Close();
        }
    }
}