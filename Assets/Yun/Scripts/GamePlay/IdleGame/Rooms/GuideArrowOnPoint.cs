using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class GuideArrowOnPoint : YunBehaviour
    {
        [SerializeField] private float moveDistance = 0.5f;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private bool isUp = true;
        [SerializeField] private bool isLeft;
        [SerializeField] private bool isOnGrid;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            var localPosition = transform.localPosition;
            /*var sequence = DOTween.Sequence();
            if (isUp)
            {
                if (isOnGrid)
                {
                    Vector3 targetPosition = new Vector3(localPosition.x, localPosition.y + moveDistance, 0f);  
                    sequence.Append(transform.DOLocalMove(targetPosition, moveDuration));  
                }
                else
                {
                    sequence.Append(transform.DOLocalMove(localPosition + transform.up * moveDistance, moveDuration));
                }
            }
            else if (isLeft)
                sequence.Append(transform.DOLocalMove(localPosition + transform.right * moveDistance, moveDuration));
            sequence.Append(transform.DOLocalMove(localPosition, moveDuration));
            sequence.SetLoops(-1);
            AddTweenToTweenManager(sequence);*/
        }
    }
}