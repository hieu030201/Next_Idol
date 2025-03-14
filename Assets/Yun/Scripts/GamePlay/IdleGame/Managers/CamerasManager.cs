using System;
using System.Collections;
using Adverstising_Integration.Scripts;
using Advertising;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Managers
{
    public class CamerasManager : YunBehaviour
    {
        [SerializeField] private CinemachineBrain cineMachineBrain;
        [SerializeField] private CinemachineVirtualCamera cameraFollow;
        [SerializeField] private CinemachineVirtualCamera cameraFollowVipSoldier;
        [SerializeField] private CinemachineVirtualCamera cameraFix;
        [SerializeField] private CinemachineVirtualCamera cameraFix2;
        [SerializeField] private CinemachineVirtualCamera cameraFix3;
        [SerializeField] private CinemachineVirtualCamera cameraFix4;
        [SerializeField] private IdlePlayer player;

        private YunCameraController _cameraController;

        protected override void Start()
        {
            base.Start();
            _cameraController = new YunCameraController(cineMachineBrain);
            _cameraController.RegisterCamera(cameraFollow);
            _cameraController.RegisterCamera(cameraFollowVipSoldier);
            _cameraController.RegisterCamera(cameraFix);
            _cameraController.RegisterCamera(cameraFix2);
            _cameraController.RegisterCamera(cameraFix3);
            _cameraController.RegisterCamera(cameraFix4);
            _cameraController.SwitchCamera(cameraFollow);
        }

        private void Update()
        {
            if (_isFixCameraFollowPlayer && player)
            {
                var transform1 = cameraFix4.transform;
                var position = transform1.position;
                var xPos = position.x;
                var yPos = position.y;
                var zPos = position.z;
                if (player.BattleAngle is 0 or 180 or 360)
                    cameraFix4.transform.position = new Vector3(player.transform.position.x, yPos, zPos);
                else if (player.BattleAngle is 90 or 270)
                    cameraFix4.transform.position = new Vector3(xPos, yPos, player.transform.position.z);
            }
        }

        public void ZoomOutFollowCamera()
        {
            _currentCam = cameraFollow;
            ZoomToFOV(60);
        }

        public void ZoomInFollowCamera()
        {
            _currentCam = cameraFollow;
            ZoomToFOV(40);
        }

        public void ZoomFixCamera(float fov)
        {
            _currentCam = cameraFix;
            ZoomToFOV(fov);
        }

        private const float SmoothTime = 0.3f;
        private const float MinFov = 10f;
        private const float MaxFov = 60f;
        private float _currentVelocity;

        private void ZoomToFOV(float targetFOV)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothZoomCoroutine(targetFOV));
        }

        private CinemachineVirtualCamera _currentCam;

        private IEnumerator SmoothZoomCoroutine(float targetFOV)
        {
            targetFOV = Mathf.Clamp(targetFOV, MinFov, MaxFov);

            while (!Mathf.Approximately(_currentCam.m_Lens.FieldOfView, targetFOV))
            {
                _currentCam.m_Lens.FieldOfView = Mathf.SmoothDamp(
                    _currentCam.m_Lens.FieldOfView,
                    targetFOV,
                    ref _currentVelocity,
                    SmoothTime
                );
                yield return null;
            }
        }

        private bool _isFixCameraFollowPlayer;

        public void SwitchToPreviewBattleCamera(CameraData cameraData)
        {
            player.isLockedMoving = false;
            _isFixCameraFollowPlayer = true;
            SwitchToFixCamera4(cameraData);
        }

        public void SwitchToFixCamera(CameraData cameraData)
        {
            player.isLockedMoving = true;
            var transform1 = cameraFix.transform;
            transform1.rotation = cameraData.rotation;
            transform1.localPosition = cameraData.position;
            cameraFix.m_Lens.FieldOfView = cameraData.FOV;
            _cameraController.SwitchCamera(cameraFix, 1.5f);
        }

        public void SwitchToFixCamera4(CameraData cameraData)
        {
            var transform1 = cameraFix4.transform;
            transform1.rotation = cameraData.rotation;
            transform1.position = cameraData.position;
            cameraFix4.m_Lens.FieldOfView = cameraData.FOV;
            _cameraController.SwitchCamera(cameraFix4, 1.5f);
        }

        public void SwitchToFollowCamera()
        {
            player.BattleAngle = 0;
            player.isLockedMoving = false;
            _isFixCameraFollowPlayer = false;
            _cameraController.SwitchCamera(cameraFollow, 1.5f);
        }

        public void FocusCameraToRoom(Transform follow)
        {
            player.isLockedMoving = true;
            if (_cameraController.CurrentCam != cameraFix2)
            {
                cameraFix2.transform.position = follow.position;
                _cameraController.SwitchCamera(cameraFix2, 1);
            }
            else
            {
                cameraFix3.transform.position = follow.position;
                _cameraController.SwitchCamera(cameraFix3, 1);
            }
        }

        public void FocusCameraToGetUpgradeWorkerPoint(GetUpgradeWorkerPoint getUpgradeWorkerPoint)
        {
            player.isLockedMoving = true;
            var follow = getUpgradeWorkerPoint.CameraPoint.transform;
            if (_cameraController.CurrentCam != cameraFix2)
            {
                cameraFix2.transform.position = follow.position;
                _cameraController.SwitchCamera(cameraFix2, 1f);
            }
            else
            {
                cameraFix3.transform.position = follow.position;
                _cameraController.SwitchCamera(cameraFix3, 1f);
            }

            DOVirtual.DelayedCall(2.5f, (FocusCameraToPlayer)).SetAutoKill(true);
        }

        public void FocusCameraToHireEmployeePoint(HireEmployeePoint point, bool isNotReturnToPlayer = false)
        {
            player.isLockedMoving = true;
            var follow = point.CameraPoint.transform;
            if (_cameraController.CurrentCam != cameraFix2)
            {
                cameraFix2.transform.position = follow.position;
                _cameraController.SwitchCamera(cameraFix2, 1f);
            }
            else
            {
                cameraFix3.transform.position = follow.position;
                _cameraController.SwitchCamera(cameraFix3, 1f);
            }

            point.transform.localScale = Vector3.zero;
            DOVirtual.DelayedCall(1f,
                    (() => { point.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack); }))
                .SetAutoKill(true);
            point.gameObject.SetActive(true);
            if (!isNotReturnToPlayer)
                DOVirtual.DelayedCall(2.5f, (FocusCameraToPlayer)).SetAutoKill(true);
        }

        public void FocusCameraToPlayer()
        {
            if (FacilityManager.Instance.isWaitingToShowInterAds)
            {
                FacilityManager.Instance.isWaitingToShowInterAds = false;
                if(AdsManager.Instance)
                    AdsManager.Instance.ShowTakeABreakPopup();
                // DOVirtual.DelayedCall(1, (() => { AdsManager.Instance.ShowInterstitialAd(); })).SetAutoKill(true);
            }

            SwitchToFollowCamera();
        }

        public void FollowVipSoldier()
        {
            player.isLockedMoving = true;
            _cameraController.SwitchCamera(cameraFollowVipSoldier, 1.5f);
        }
    }
}