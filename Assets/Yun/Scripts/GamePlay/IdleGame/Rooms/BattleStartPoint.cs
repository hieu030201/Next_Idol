using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class BattleStartPoint : YunBehaviour
    {
        [SerializeField] private GameObject background;
        private Tween _scaleUpTween;
        public void StepIn()
        {
            Debug.Log("Run in BattleStartPoint");
            _scaleUpTween?.Kill();
            _scaleUpTween = background.transform.DOScale(1.1f, 0.2f);
        }

        public void StepOut()
        {
            background.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}