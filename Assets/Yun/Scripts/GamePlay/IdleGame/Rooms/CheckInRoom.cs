using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Animations;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class CheckInRoom : BaseRoom
    {
        [SerializeField] private GameObject noBed;
        [SerializeField] private GameObject healthBar;

        [Button]
        private void TestSpawnMoney()
        {
            SpawnMoney(120, ExitPoint.transform.position);
        }

        private ParticleSystem _greenLight;
        
        protected override void Awake()
        {
            base.Awake();
        }

        public override void Init()
        {
            base.Init();
            
            noBed.SetActive(false);
            // Debug.Log("checkinRoom: " + buildPoint);
            _greenLight = transform.Find("PowerOrbSmallGreen").gameObject.GetComponent<ParticleSystem>();
            _greenLight.gameObject.SetActive(false);
        }

        private Tween _tweenHideGreenLight;
        public void ShowGreenLight()
        {
            _greenLight.Play();
            _greenLight.gameObject.SetActive(true);
            _tweenHideGreenLight?.Kill();
            _tweenHideGreenLight = DOVirtual.DelayedCall(1, (() =>
            {
                _greenLight.gameObject.SetActive(false);
            }));
        }

        public void ActiveHealthBar()
        {
            healthBar.transform.DOLocalMoveY(1.58f, 0.7f).OnComplete((() =>
            {
                healthBar.transform.DOLocalMoveY(2.28f, 0.3f).SetAutoKill(true);
            })).SetAutoKill(true);
        }

        public override void AddClient(WarBaseClient client, bool immediately = false)
        {
            base.AddClient(client, immediately);
            client.EmotionState = WarBaseClient.ClientEmotionState.CheckIn;
        }

        public void CheckNoBed()
        {
            if (FacilityManager.Instance.CheckFacilityFullClient())
            {
                noBed.SetActive(true);
                noBed.GetComponent<FlutterAnimation>().StartFlutter(1.1f, 1);
            }
            else
            {
                noBed.GetComponent<FlutterAnimation>().StopFlutter();
                noBed.SetActive(false);
            }

            // DOVirtual.DelayedCall(1, CheckNoBed);
        }

        private WarBaseClient _firstClient;

        public override WarBaseClient GetFirstClient()
        {
            return _firstClient;
        }

        private Tween _tweenToCheckClientOut;
        private bool _isCheckingClientOut;

        public void StartCheckClientOut()
        {
            _isCheckingClientOut = true;
            // _tweenToCheckClientOut?.Kill();
            // _tweenToCheckClientOut = DOVirtual.DelayedCall(0.5f, StartCheckClientOut);
            CheckClientOut();
            // InvokeRepeating(nameof(CheckClientOut), 0.1f, 0.3f);
        }

        public void CheckClientOut()
        {
            // Debug.Log("CheckClientOut");
            FacilityManager.Instance.MoveClientFromCheckInToWaitingRoom();
        }

        public void StopCheckClientOut()
        {
            _isCheckingClientOut = false;
            // _tweenToCheckClientOut?.Kill();
            CancelInvoke(nameof(CheckClientOut));
        }

        public override void OnClientMoveToSlotCompleted(Client client)
        {
            base.OnClientMoveToSlotCompleted(client);
            if (ClientsList[0] == client)
            {
                // Debug.Log("CHECK IN ROOM OnClientMoveToSlotCompleted");
                _firstClient = ClientsList[0];
                if(_isCheckingClientOut)
                    CheckClientOut();
            }
        }

        public void RemoveFirstClient()
        {
            // Debug.Log("RemoveFirstClient");
            _firstClient = null;
        }
    }
}