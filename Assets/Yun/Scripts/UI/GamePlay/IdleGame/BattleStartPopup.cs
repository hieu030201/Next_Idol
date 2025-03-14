using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Logics;
using Yun.Scripts.GamePlay.IdleGame.Managers;
namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class BattleStartPopup : BasePopup
    {
        public void ClickFight()
        {
            FacilityManager.Instance.BattleManager.OnStartBattle();
            Close();
        }
    }
}
