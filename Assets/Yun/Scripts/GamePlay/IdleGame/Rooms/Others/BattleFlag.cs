using System;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms.Others
{
    public class BattleFlag : MonoBehaviour
    {
        [SerializeField] private GameObject blueFlag;
        [SerializeField] private GameObject bluePole;

        [SerializeField] private GameObject redFlag;
        [SerializeField] private GameObject redPole;

        [SerializeField] private Animator animator;

        public void StartRun()
        {
            blueFlag.SetActive(false);
            bluePole.SetActive(false);
        }

        public void ShowWin()
        {
            animator.Play("Flag_Down");
            _isWaitingAnimComplete = true;
        }

        private void ShowBlueFlag()
        {
            blueFlag.SetActive(true);
            bluePole.SetActive(true);

            redFlag.SetActive(false);
            redPole.SetActive(false);
        }

        private bool _isWaitingAnimComplete;

        protected void Update()
        {
            if (!_isWaitingAnimComplete)
                return;
            var animStateInfo = animator.GetCurrentAnimatorStateInfo(0)
                .normalizedTime;

            if (animStateInfo >= 0.98f)
            {
                // Lấy thông tin về state hiện tại của layer 0
                var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                if (clipInfo.Length <= 0) return;
                switch (clipInfo[0].clip.name)
                {
                    case "Flag_Down":
                        _isWaitingAnimComplete = false;
                        ShowBlueFlag();
                        break;
                }
            }
        }
    }
}