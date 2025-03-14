using System;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Animations;

namespace Yun.Scripts.GamePlay.IdleGame.Clients
{
    public class ClientStatus : MonoBehaviour
    {
        [SerializeField] private GameObject waitToBedroomStatus;
        [SerializeField] private GameObject waitToBattleStatus;
        [SerializeField] private GameObject waitToBoxingStatus;
        [SerializeField] private GameObject waitToTrainingStatus;
        [SerializeField] private GameObject waitToDiningStatus;
        [SerializeField] private GameObject waitToUpgradeStatus;
        [SerializeField] private GameObject waitToTreatingStatus;
        [SerializeField] private RadialFill radialFill;
        [SerializeField] private GameObject statusActive;

        private float _bgStartScale;

        private void Awake()
        {
            gameObject.SetActive(false);
            waitToBedroomStatus.SetActive(false);
            waitToBattleStatus.SetActive(false);
            waitToBoxingStatus.SetActive(false);
            waitToTreatingStatus.SetActive(false);
            waitToTrainingStatus.SetActive(false);
            waitToDiningStatus.SetActive(false);
            waitToUpgradeStatus.SetActive(false);
            statusActive.SetActive(false);

            _bgStartScale = transform.localScale.x;
            
            HideConnectAnimation();
        }

        private Vector3 _startDistance;
        private Quaternion _startRotation;
        private Transform _parentTransform;

        public void InitTransform(Transform parentTransform)
        {
            _parentTransform = parentTransform;
            var statusTransform = transform;
            _startRotation = statusTransform.rotation;
            _startDistance = _parentTransform.position - statusTransform.position;
        }

        public void Reposition()
        {
            transform.rotation = _startRotation;
            transform.position = _parentTransform.position - _startDistance;
        }

        private bool _isReposition;

        public void ActiveReposition()
        {
            _isReposition = true;
        }

        public void DeactivateReposition()
        {
            _isReposition = false;
        }

        private void Update()
        {
            if (!_isReposition)
                return;
            Reposition();
        }

        private void HideAllStatus()
        {
            gameObject.SetActive(false);
            waitToBedroomStatus.SetActive(false);
            waitToBattleStatus.SetActive(false);
            waitToBoxingStatus.SetActive(false);
            waitToTreatingStatus.SetActive(false);
            waitToTrainingStatus.SetActive(false);
            waitToDiningStatus.SetActive(false);
            waitToUpgradeStatus.SetActive(false);
        }

        private WarBaseClient.ClientEmotionState _emotionState;

        public WarBaseClient.ClientEmotionState EmotionState
        {
            get => _emotionState;
            set
            {
                HideAllStatus();
                _emotionState = value;
                switch (value)
                {
                    case WarBaseClient.ClientEmotionState.WaitToBattle:
                        ScaleUp();
                        gameObject.SetActive(true);
                        waitToBattleStatus.SetActive(true);
                        break;
                    case WarBaseClient.ClientEmotionState.WaitToBoxing:
                        ScaleUp();
                        gameObject.SetActive(true);
                        waitToBoxingStatus.SetActive(true);
                        break;
                    case WarBaseClient.ClientEmotionState.WaitToTreating:
                        ScaleUp();
                        gameObject.SetActive(true);
                        waitToTreatingStatus.SetActive(true);
                        break;
                    case WarBaseClient.ClientEmotionState.WaitToExercise:
                        ScaleUp();
                        gameObject.SetActive(true);
                        waitToTrainingStatus.SetActive(true);
                        break;
                    case WarBaseClient.ClientEmotionState.WaitToEat:
                        ScaleUp();
                        gameObject.SetActive(true);
                        waitToDiningStatus.SetActive(true);
                        break;
                    case WarBaseClient.ClientEmotionState.WaitToRest:
                        ScaleUp();
                        gameObject.SetActive(true);
                        waitToBedroomStatus.SetActive(true);
                        break;
                    case WarBaseClient.ClientEmotionState.WaitToUpgrade:
                        ScaleUp();
                        gameObject.SetActive(true);
                        waitToUpgradeStatus.SetActive(true);
                        break;
                }
            }
        }

        public void ShowConnectedStatus()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            statusActive.SetActive(true);
            radialFill.ShowFullFill();
            // Tạo sequence để quản lý chuỗi hoạt ảnh
            var scaleSequence = DOTween.Sequence();

            // Thêm hoạt ảnh scale ra
            scaleSequence.Append(
                transform.DOScale(Vector3.one * 1.3f * _bgStartScale, 0.3f));

            // Thêm hoạt ảnh scale vào
            scaleSequence.Append(transform.DOScale(Vector3.one * _bgStartScale, 0.3f));

            // Đặt số lần lặp lại
            scaleSequence.SetLoops(2, LoopType.Restart);

            // Bắt đầu chuỗi hoạt ảnh
            scaleSequence.Play();
        }

        public void ShowConnectAnimation(float duration)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            statusActive.SetActive(false);
            radialFill.StartFill(duration);
        }

        public void HideConnectAnimation()
        {
            radialFill.StopFill();
            GetComponent<SpriteRenderer>().enabled = false;
            statusActive.SetActive(false);
        }

        private void ScaleUp()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(_bgStartScale, 1).SetEase(Ease.OutBack);
        }
    }
}