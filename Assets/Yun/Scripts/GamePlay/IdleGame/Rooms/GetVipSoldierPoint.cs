using UnityEngine;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class GetVipSoldierPoint : YunBehaviour
    {
        [SerializeField] private GameObject hideContent;
        
        public bool IsActive
        {
            get => hideContent.activeSelf;
            set { hideContent.SetActive(value); }
        }
    }
}