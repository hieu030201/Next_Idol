using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Random = UnityEngine.Random;

namespace Yun.Scripts.Animations
{
    public class FlyingMoney3D : MonoBehaviour
    {
        public float downDuration = 0.5f;
        public float upDuration = 0.75f;
        public float upHeight = 3f;

        private Transform _destination;

        private Vector3 _startPosition;

        private float _speed = 15;

        /*private void Update()
        {
            if (!_isFlyDown) return;
            var transform1 = transform;
            var position = transform1.position;
            var direction = (_destination.position - position).normalized;

            // Di chuyển object A về phía object B
            position += direction * _speed * Time.deltaTime;
            transform1.position = position;
            _speed += 0.5f;

            if (!(Vector3.Distance(_destination.position, transform.position) < 0.2f)) return;
            _isFlyDown = false;
            transform.DOKill();
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Cash_In_MGB);
            Destroy(gameObject);
        }*/

        private float _maxSpeed = 30f;
        private void FixedUpdate()
        {
            if (!_isFlyDown) return;
            // Tính khoảng cách đến player
            float distanceToPlayer = Vector3.Distance(transform.position, _destination.position);

            // Kiểm tra xem có nên di chuyển không
            if (distanceToPlayer > 0.2f)
            {
                // Tính hướng đến player
                Vector3 direction = (_destination.position - transform.position).normalized;

                // Tăng tốc độ theo thời gian
                _speed += 2 * Time.deltaTime;
                _speed = Mathf.Min(_speed, _maxSpeed);

                // Di chuyển về phía player
                transform.position += direction * _speed * Time.deltaTime;
            }
            else
            {
                _isFlyDown = false;
                transform.DOKill();
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Cash_In_MGB);
                Destroy(gameObject);
            }
        }

        private bool _isFlyDown;

        private GameObject _player;

        public void DelayToFlyToPlayer(float delay, GameObject player)
        {
            _player = player;
            if (delay == 0)
                FlyToPlayer();
            else
                Invoke(nameof(FlyToPlayer), delay);
        }

        public void FlyToPlayer()
        {
            _startPosition = transform.position;

            var destination1 = _player.GetComponent<IdlePlayer>().CashWalletPosition1;
            var destination2 = _player.GetComponent<IdlePlayer>().CashWalletPosition2;
            _destination = destination1.transform;
            if (Vector3.Distance(transform.position, destination1.transform.position) >
                Vector3.Distance(transform.position, destination2.transform.position))
                _destination = destination2.transform;

            var transform1 = transform;
            transform1.localScale = Vector3.one;
            transform1.position = _startPosition;
            var endPosition = transform1.position + Vector3.up * upHeight;
            var randomValueX = Random.Range(-1f, 1f);
            var randomValueZ = Random.Range(-1f, 1f);
            endPosition += new Vector3(randomValueX, 0, randomValueZ);
            transform.DOMove(endPosition, upDuration).SetEase(Ease.OutBack);

            var randomAngleY = Random.Range(30, 60);
            var randomAngleZ = Random.Range(30, 60);

            transform.DORotate(new Vector3(300f, randomAngleY, randomAngleZ), upDuration, RotateMode.LocalAxisAdd)
                .OnComplete((() =>
                {
                    _isFlyDown = true;
                    transform.DORotate(new Vector3(300f, 0, 0), downDuration, RotateMode.LocalAxisAdd);
                }));
        }
    }
}