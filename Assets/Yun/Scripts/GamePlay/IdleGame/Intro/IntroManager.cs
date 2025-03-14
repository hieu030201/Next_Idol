using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Intro
{
    public class IntroManager : MonoBehaviour
    {
        [SerializeField] private GameObject helicopter;
        [SerializeField] private GameObject rotor;
        [SerializeField] private GameObject rotorBack;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject bus;
        [SerializeField] private GameObject helicopterPoint;
        [SerializeField] private GameObject startPoint;
        [SerializeField] private GameObject midePoint;
        [SerializeField] private GameObject busPoint;
        [SerializeField] private GameObject spawnClientPoint;
        [SerializeField] private ParticleSystem rebornEffect;
        [SerializeField] private GameObject clientPrefab;
        [SerializeField] private List<GameObject> positions;

        private float _rotationSpeed = 2500f;

        // Start is called before the first frame update
        void Start()
        {
            originalRot = helicopter.transform.localRotation;
            rebornEffect.gameObject.SetActive(false);
        }

        private void Awake()
        {
            player.gameObject.SetActive(false);
        }

        public void ShowRebornEffect()
        {
            // Debug.Log("ShowRebornEffect");
            rebornEffect.gameObject.SetActive(true);
            rebornEffect.Play();
            DOVirtual.DelayedCall(4, (() =>
            {
                rebornEffect.gameObject.SetActive(false);
            }));
        }
        
        private float time;
        public float shakeAmount = 0.6f; // Độ mạnh của rung lắc
        public float shakeSpeed = 5f; // Tốc độ rung lắc
        private Quaternion originalRot;

        // Update is called once per frame
        void Update()
        {
            if (_isShowIntro)
            {
                rotor.transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
                rotorBack.transform.Rotate(Vector3.right * _rotationSpeed * Time.deltaTime);
                if (_isHelicopterDown)
                    player.transform.position = helicopter.transform.position;
            }

            if (_isHelicopterDown)
            {
                // Rung lắc góc quay (giới hạn trong một khoảng nhỏ)
                /*helicopter.transform.localRotation = originalRot * Quaternion.Euler(
                    Random.Range(-shakeAmount, shakeAmount) * 10f,
                    Random.Range(-shakeAmount, shakeAmount) * 10f,
                    0f);

                // Điều chỉnh tốc độ rung lắc
                helicopter.transform.localRotation = Quaternion.Lerp(helicopter.transform.localRotation, originalRot, Time.deltaTime * shakeSpeed);*/
            }
        }

        private bool _isShowIntro;
        private bool _isHelicopterDown;

        public void StartIntro()
        {
            // player.gameObject.SetActive(true);
            _isShowIntro = true;
            _isHelicopterDown = true;

            helicopter.transform.DOShakeRotation(flightDuration, new Vector3(0.5f, 0.5f, 0.5f), 3, 90, false)
                .SetEase(Ease.InOutSine);
                
            DOVirtual.DelayedCall(0.25f,
                (() => { helicopter.transform.DOMove(startPoint.transform.position, 5).OnComplete(HelicopterLeave); }));
        }

        public void SkipIntro()
        {
            player.gameObject.SetActive(true);
            // player.transform.position = startPoint.transform.position;
            bus.transform.position = busPoint.transform.position;
            FacilityManager.Instance.OnIntroFinish();
        }

        public float flightDuration = 5f;
        public float rotationDuration = 4f;
        private List<GameObject> _clientsList;

        public void RemoveClientsList()
        {
            if (_clientsList != null)
            {
                foreach (var client in _clientsList)
                {
                    Destroy(client);
                }
            }
        }

        private void HelicopterLeave()
        {
            player.gameObject.SetActive(true);
            _isHelicopterDown = false;
            // helicopter.transform.DOMove(helicopterPoint.transform.position, 5);
            bus.transform.DOMove(busPoint.transform.position, 2).OnComplete((() =>
            {
                _clientsList = new List<GameObject>();
                for (var i = 0; i < 3; i++)
                {
                    var clientObject = Instantiate(clientPrefab, transform);
                    _clientsList.Add(clientObject);
                    var client = clientObject.GetComponent<WarBaseClient>();
                    client.Type = WarBaseClient.ClientType.Man;
                    client.DeactivateNavmeshAgent();
                    clientObject.transform.position = spawnClientPoint.transform.position;
                    var position = positions[i];
                    DOVirtual.DelayedCall(i * 0.5f, (() =>
                    {
                        client.FollowSalute(position,5);
                    }));
                    DOVirtual.DelayedCall(2.5f, (() =>
                    {
                        client.Salute();
                    }));
                }
            }));


            DOVirtual.DelayedCall(6f, (() =>
            {
                FacilityManager.Instance.OnIntroFinish();
                _isShowIntro = false;
            }));
            
            
            // Tạo đường cong Bezier
            Vector3[] path = new Vector3[3] { startPoint.transform.position, midePoint.transform.position, helicopterPoint.transform.position };

            // Tạo sequence cho chuyển động
            Sequence sequence = DOTween.Sequence();

            // Thêm chuyển động theo đường cong
            sequence.Append(helicopter.transform.DOPath(path, flightDuration, PathType.CatmullRom)
                .SetEase(Ease.InOutSine));

            // Thêm xoay hướng về điểm C
            sequence.Join(helicopter.transform.DOLookAt(helicopterPoint.transform.position, rotationDuration, AxisConstraint.Y)
                .SetEase(Ease.InOutSine));

            // Tùy chọn: Thêm hiệu ứng lắc nhẹ
            // sequence.Join(helicopter.transform.DOShakeRotation(flightDuration, new Vector3(5, 5, 5), 10, 90, false)
            //     .SetEase(Ease.InOutSine));

            // Bắt đầu sequence
            sequence.Play();

            helicopter.transform.DORotate(new Vector3(-60, -140, 0), 4);
        }
    }
}