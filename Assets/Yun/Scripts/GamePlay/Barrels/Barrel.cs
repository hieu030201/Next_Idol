using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;
using Yun.Scripts.Managers;
using Yun.Scripts.UI;

namespace Yun.Scripts.GamePlay.Barrels
{
    public class Barrel : YunBehaviour, IReceiveProjectile
    {
        [SerializeField] private int healthNumber = 10;
        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private float rotateSpeed = 250;
        [SerializeField] private GameObject text;
        [SerializeField] private GameObject healthBarPrefab;
        [SerializeField] private GameObject healthBarPosition;
        [SerializeField] private GameObject explosion;
        [SerializeField] private GameObject item;

        private GameObject _healthBar;
        private int _health;
        private RectTransform _canvasRect;
        private bool _isMoving;

        protected override void Awake()
        {
            text.SetActive(false);
            healthBarPosition.SetActive(false);
            _health = healthNumber;
            _healthBar = Instantiate(healthBarPrefab, CanvasManager.Instance.canvas.GetComponent<RectTransform>());
            _healthBar.SetActive(false);
            _canvasRect = CanvasManager.Instance.canvas.GetComponent<RectTransform>();
            _isMoving = true;
            explosion.SetActive(false);
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            // CanvasManager.Instance.ShowTakeDamageText(damage, transform.position);
            if (_health <= 0)
            {
                _isMoving = false;
                ShowEffect(YunEffectType.BubbleEffect, text.transform);
                _isMoving = false;
                transform.Find("Barrel").gameObject.SetActive(false);
                Destroy(_healthBar);
                GetComponent<Collider>().enabled = false;
                explosion.SetActive(true);
                explosion.GetComponent<ParticleSystem>().Play();
                GameManager.Instance.OnPlayerAchieveItem(item);
                var tween = DOVirtual.DelayedCall(explosion.GetComponent<ParticleSystem>().main.duration, () =>
                {
                    Destroy(gameObject);
                });
                AddTweenToTweenManager(tween);
            }
            else
            {
                _healthBar.SetActive(true);
                _healthBar.GetComponent<HealthBar>().Health = (float)_health / healthNumber;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if(!_isMoving)
                return;
            
            // Di chuyển đối tượng về phía trước theo hệ tọa độ cục bộ
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            // Xoay đối tượng quanh trục y (xoay theo hệ tọa độ cục bộ)
            transform.Find("Barrel").Rotate(Vector3.down, rotateSpeed * Time.deltaTime);

            if (transform.localPosition.z > 55)
            {
                Destroy((_healthBar));
                Destroy(gameObject);
            }

            if (!_healthBar) return;
            Vector2 viewportPosition = MainCamera.WorldToViewportPoint(healthBarPosition.transform.position);

            var sizeDelta = _canvasRect.sizeDelta;
            var screenPoint = new Vector2(
                ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));
            _healthBar.transform.localPosition = screenPoint;
        }
    }
}
