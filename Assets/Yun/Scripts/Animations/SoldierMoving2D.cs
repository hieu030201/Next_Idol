using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.UI.GamePlay.IdleGame;

namespace Yun.Scripts.Animations
{
    public class SoldierMoving2D : MonoBehaviour
    {
        [SerializeField] private GameObject man;
        [SerializeField] private GameObject gun;

        private Sequence _sequence1;
        private Sequence _sequence2;
        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = gun.transform.localPosition;
        }

        [Button]
        public void StartMoving()
        {
            _sequence1?.Kill();
            _sequence2?.Kill();
            
            man.transform.localRotation = Quaternion.Euler(0, 0, -5);
            _sequence1 = DOTween.Sequence();
            _sequence1
                .Append(man.transform.DOLocalRotate(new Vector3(0, 0, 5), 0.2f))
                .Append(man.transform.DOLocalRotate(new Vector3(0, 0, -5), 0.2f))
                .SetLoops(-1);

            gun.transform.localPosition = _startPosition;
            _sequence2 = DOTween.Sequence();
            _sequence2
                .Append(gun.transform.DOLocalMoveX(-9, 0.1f))
                .Append(gun.transform.DOLocalMoveX(3, 0.1f))
                .SetLoops(-1);
        }

        public void StopMoving()
        {
            _sequence1?.Kill();
            _sequence2?.Kill();
            
            man.transform.localRotation = Quaternion.Euler(0, 0, 0);
            gun.transform.localPosition = _startPosition;
        }
    }
}