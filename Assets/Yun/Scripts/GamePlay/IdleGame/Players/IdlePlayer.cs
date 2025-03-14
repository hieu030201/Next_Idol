using System.Linq;
using Advertising;
using DG.Tweening;
using Joystick_Pack.Scripts.Joysticks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Yun.Scripts.Audios;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Yun.Scripts.GamePlay.IdleGame.Vip_Soldiers;
using Yun.Scripts.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Players
{
    public class IdlePlayer : Person
    {
        [SerializeField] public GameObject CashWalletPosition1;
        [SerializeField] public GameObject CashWalletPosition2;
        [SerializeField] private ParticleSystem trailEffect;
        [SerializeField] private GameObject BaseObject;
        [SerializeField] private GameObject SprunkyObject;
        [SerializeField] private GameObject modelSkin;
        [SerializeField] private GameObject modelSkinSprunky;
        [SerializeField] private GameObject baseModel;

        [SerializeField] private GameObject defaultStatePrefab,
            maxStatePrefab,
            playerFollow,
            playerRotateBody,
            guideArrow,
            speedUpEffect,
            capacityUpEffect;

        [SerializeField] public IdlePlayerActionStateMachine stateMachine;

        private Rigidbody _rigidbody;
        private CharacterController _characterController;
        private bool _isStartGame;

        private Tween _tweenToActiveGetWorkerBoosterPoint;
        private Tween _tweenToActiveGetBoosterPoint;
        private Tween _tweenToActiveGetMoneyPoint;
        private Tween _tweenToActiveBuildPoint;
        private Tween _tweenToActiveHireEmployeePoint;
        private TextMeshPro _capacityTxt;
        private GameObject _miniTank;
        private GameObject _santaSleight;
        private FloatingJoystick _floatingJoystick;

        [Button]
        public void TestShoot(GameObject nearestTarget)
        {
            var direction = (nearestTarget.transform.position - playerRotateBody.transform.position).normalized;
            // Tạo rotation mới hướng về mục tiêu
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z - 90));
            // Áp dụng rotation mới
            playerRotateBody.transform.rotation =
                Quaternion.Slerp(playerRotateBody.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        [Button]
        public void TestAnim()
        {
            stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.ShootPistol;
        }

        [Button]
        public void TestRiffleEffect()
        {
            FacilityManager.Instance.UpdateBattleHp(2);
        }

        private int _idSkin;

        public void ChangeSkinPlayer(int id)
        {
            var oldName = FacilityManager.Instance.SkinDataCollection.GetNameItem(_idSkin);
            var newName = FacilityManager.Instance.SkinDataCollection.GetNameItem(id);

            if (modelSkin.transform.Find(oldName))
                modelSkin.transform.Find(oldName).gameObject.SetActive(false);
            else if (modelSkinSprunky.transform.Find(oldName))
                modelSkinSprunky.transform.Find(oldName).gameObject.SetActive(false);

            if (modelSkin.transform.Find(newName))
                modelSkin.transform.Find(newName).gameObject.SetActive(true);
            else if (modelSkinSprunky.transform.Find(newName))
                modelSkinSprunky.transform.Find(newName).gameObject.SetActive(true);

            _idSkin = id;
        }

        public float CollectMoneyRadius { get; private set; }
        public bool IsInBattleRoom { get; set; }

        private Tween _tweenToScaleTank;

        private bool _isSpeedBooster;

        public bool IsSpeedBooster
        {
            get => _isSpeedBooster;
            set
            {
                _isSpeedBooster = value;
                _tweenToScaleTank?.Kill();
                if (value)
                {
                    ShowMiniTank();
                    _characterController.radius = 0.65f;
                    baseModel.transform.localPosition =
                        FacilityManager.Instance.GameSaveLoad.StableGameData.idSkinSelected < 5
                            ? new Vector3(0, 0, -0.43f)
                            : new Vector3(0, 0, -0.536f);
                    CollectMoneyRadius = FacilityManager.Instance.PlayerConfig.BoosterCollectMoneyRadius;
                }
                else
                {
                    HideMiniTank();
                    _characterController.radius = 0.4f;
                    baseModel.transform.localPosition = new Vector3(0, 0, 0);
                    CollectMoneyRadius = FacilityManager.Instance.PlayerConfig.StartCollectMoneyRadius;
                }
            }
        }

        public void ShowMiniTank()
        {
            if (FireBaseManager.Instance.showNoel)
            {
                _santaSleight.SetActive(true);
                _santaSleight.transform.localScale = Vector3.zero;
                _tweenToScaleTank = _santaSleight.transform.DOScale(1f, 1f).SetEase(Ease.OutBounce);
            }
            else
            {
                _miniTank.SetActive(true);
                _miniTank.transform.localScale = Vector3.zero;
                _tweenToScaleTank = _miniTank.transform.DOScale(1, 1f).SetEase(Ease.OutBounce);
            }
        }

        public void HideMiniTank()
        {
            if (FireBaseManager.Instance.showNoel)
            {
                _tweenToScaleTank = _santaSleight.transform.DOScale(0, 0.5f).SetEase(Ease.InBounce).OnComplete((() =>
                {
                    _santaSleight.SetActive(false);
                }));
            }
            else
            {
                _tweenToScaleTank = _miniTank.transform.DOScale(0, 0.5f).SetEase(Ease.InBounce).OnComplete((() =>
                {
                    _miniTank.SetActive(false);
                }));
            }
        }

        protected override void Awake()
        {
            base.Awake();

            GetComponent<NavMeshAgent>().enabled = false;
            guideArrow.SetActive(false);
            trailEffect.gameObject.SetActive(false);

            if (transform.Find("capacityTxt"))
            {
                _capacityTxt = transform.Find("capacityTxt").GetComponent<TextMeshPro>();
                _capacityTxt.text = "";
            }

            _miniTank = transform.Find("Player_Rotate_Body").Find("Mini_Tank").gameObject;
            _santaSleight = transform.Find("Player_Rotate_Body").Find("Sleight_Base").gameObject;
            _miniTank.SetActive(false);
            _santaSleight.SetActive(false);
        }

        protected override void Start()
        {
            var data = FacilityManager.Instance.GameSaveLoad.StableGameData;
            _floatingJoystick = CanvasManager.Instance.floatingJoystick;
            _floatingJoystick.OnPointerUpAction = OnPointerUpAction;
            _floatingJoystick.OnPointerDownAction = OnPointerDownAction;

            if (modelSkin.transform.Find("Player"))
                modelSkin.transform.Find("Player").gameObject.SetActive(false);

            _idSkin = data.isActiveRetal ? data.idSkinRented : data.idSkinSelected;

            if (_idSkin < 5)
            {
                var currentSkin = FacilityManager.Instance.SkinDataCollection.GetNameItem(_idSkin);
                if (modelSkin.transform.Find(currentSkin))
                    modelSkin.transform.Find(currentSkin).gameObject.SetActive(true);
            }
            else
            {
                var currentSkin = FacilityManager.Instance.SkinDataCollection.GetNameItem(_idSkin);
                if (modelSkinSprunky.transform.Find(currentSkin))
                    modelSkinSprunky.transform.Find(currentSkin).gameObject.SetActive(true);
            }
        }

        private void OnPointerDownAction()
        {
            if (FacilityManager.Instance.monetizationPointsManager.gameObject.activeSelf)
                FacilityManager.Instance.monetizationPointsManager.StartCheckToShowPoints(gameObject);
        }

        private void OnPointerUpAction()
        {
            FacilityManager.Instance.monetizationPointsManager.StopCheckToShowPoints();
            if (_currentTriggerCollider)
            {
                // Debug.Log(" chay vao ham On _currentTriggerCollider OnPointerUpAction");
                if (_currentTriggerCollider.GetComponent<BuildPoint>())
                {
                    // Debug.Log("Lay component BuildPoin");
                    var parent = _currentTriggerCollider.transform.parent;
                    var room = parent.GetComponent<BaseRoom>();
                    if (room)
                    {
                        // Debug.Log("Neu ton tai room");
                        _countMoney = 0;
                        OnWithDrawMoneyToBuildRoom(room);
                    }
                }
                else if (_currentTriggerCollider.GetComponent<RepairPoint>())
                {
                    var parent = _currentTriggerCollider.transform.parent;
                    var room = parent.GetComponent<BaseRoom>();
                    if (room)
                    {
                        // Debug.Log("Neu ton tai room");
                        // _countMoney = 0;
                        // OnWithDrawMoneyToRepairRoom(room);
                        FacilityManager.Instance.ShowRepairDamagedRoomPopup(room.GetDepositLeftToRepair(), room);
                    }
                }
                else if (_currentTriggerCollider.GetComponent<HireEmployeePoint>())
                {
                    _countMoney = 0;
                    OnWithDrawMoneyToHireEmployee(_currentTriggerCollider.GetComponent<HireEmployeePoint>());
                }
                else if (_currentTriggerCollider.GetComponent<BuildVehiclePoint>())
                {
                    _countToken = 0;
                    OnWithDrawTokenToBuildVehicle(_currentTriggerCollider.GetComponent<BuildVehiclePoint>());
                }
                else if (_currentTriggerCollider.GetComponent<UpgradeVehiclePoint>())
                {
                    _countMoney = 0;
                    OnWithDrawMoneyToUpgradeVehicle(_currentTriggerCollider.GetComponent<UpgradeVehiclePoint>());
                }
                else if (_currentTriggerCollider.GetComponent<GetBoosterPoint>())
                {
                    FacilityManager.Instance.ShowGetBoosterPopup();
                    FireBaseManager.Instance.LogEvent(FireBaseManager.OPEN_BOOSTER_SPEED);
                }
                else if (_currentTriggerCollider.GetComponent<GetWorkerSpeedBoosterPoint>())
                {
                    FacilityManager.Instance.ShowGetWorkerSpeedBoosterPopup();
                    FireBaseManager.Instance.LogEvent(FireBaseManager.OPEN_BOOSTER_SPEED_WORKER);
                }
                else if (_currentTriggerCollider.GetComponent<GetMoneyPoint>())
                {
                    FacilityManager.Instance.ShowGetMoneyPopup(_currentTriggerCollider.GetComponent<GetMoneyPoint>());
                    FireBaseManager.Instance.LogEvent(FireBaseManager.OPEN_BOOSTER_CASH);
                }
                else if (_currentTriggerCollider.GetComponent<StartBattlePoint>())
                {
                    FacilityManager.Instance.PlayerInfoUI.ShowStartBattleButton();
                    StopGuideArrowToPoint(_currentTriggerCollider.GetComponent<GuidePoint>());
                }
                else if (_currentTriggerCollider.GetComponent<GetVipSoldierPoint>())
                {
                    FacilityManager.Instance.OnShowVipSoldierPopup();
                    FireBaseManager.Instance.LogEvent(FireBaseManager.OPEN_DOG);
                }
                else if (_currentTriggerCollider.GetComponent<GetUpgradeWorkerPoint>())
                {
                    StopGuideArrowToPoint(_currentTriggerCollider.GetComponent<GuidePoint>());
                    FacilityManager.Instance.OnShowUpgradeWorkerPopup();
                    FireBaseManager.Instance.LogEvent(FireBaseManager.OPEN_LADY);
                }
                else if (_currentTriggerCollider.GetComponent<UpgradePoint>())
                {
                    FacilityManager.Instance.ShowUpgradePopup();
                }
                else if (_currentTriggerCollider.GetComponent<BuyWorkerPoint>())
                {
                    FacilityManager.Instance.ShowBuyWorkerPopup();
                }
                else
                {
                    var parent = _currentTriggerCollider.transform.parent;
                    if (parent.GetComponent<VipSoldier>())
                    {
                        _currentTriggerCollider = null;
                    }
                }
            }

            _currentTriggerCollider = null;
        }

        public void StartGame()
        {
            _isStartGame = true;
            // connectClient.OnConnectClient += OnConnectClient;
            _characterController = GetComponent<CharacterController>();
            _characterController.enabled = true;
            _characterController.stepOffset = 0.1f;
            //stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.Idle;
            UpdateSpeed();
            CollectMoneyRadius = FacilityManager.Instance.PlayerConfig.StartCollectMoneyRadius;
        }

        public bool IsFullFollower => ClientsList.Count >= FacilityManager.Instance.IdleGameData.Capacity;

        private float Speed { set; get; }

        public void UpdateSpeed()
        {
            Speed = FacilityManager.Instance.IdleGameData.RealSpeed;
            // Debug.Log("UpdateSpeed: " + Speed);
        }

        public void UpdateClientSpeed()
        {
            foreach (var client in ClientsList.Where(client => client))
            {
                client.UpdateSpeed(Speed);
            }
        }

        private void LateUpdate()
        {
            if (playerFollow)
            {
                playerFollow.transform.position = transform.position;
            }
        }

        public Client GuideClient { get; private set; }

        public void ShowGuideArrowToClient(Client client)
        {
            client.IsGuidingClient = true;
            GuideClient = client;
            if (IsFullFollower || GuidePoint)
                return;

            if (!GuidePoint && GuideClient.IsGuidingClient)
                guideArrow.transform.LookAt(GuideClient.transform);

            if (!guideArrow.activeSelf)
            {
                guideArrow.transform.localScale = Vector3.zero;
                _scaleGuideArrowTween?.Kill();
                _scaleGuideArrowTween = guideArrow.transform.DOScale(1, 0.5f);
            }

            guideArrow.SetActive(true);
            client.ShowGuideArrowToSelf(null);
        }

        private Tween _scaleGuideArrowTween;

        public void StopGuideArrowToClient(Client client)
        {
            if (client)
                client.HideGuideArrowOnPoint();
            if (GuideClient != client)
                return;
            GuideClient = null;
            if (!GuidePoint)
                guideArrow.SetActive(false);
        }

        public void ShowGuideArrowToPoint(GuidePoint point)
        {
            if (!point)
                return;
            if (GuidePoint)
                GuidePoint.HideGuideArrow();
            GuidePoint = point;
            guideArrow.transform.LookAt(GuidePoint.transform);
            if (!guideArrow.activeSelf)
            {
                guideArrow.transform.localScale = Vector3.zero;
                _scaleGuideArrowTween?.Kill();
                _scaleGuideArrowTween = guideArrow.transform.DOScale(1, 0.5f);
            }
            guideArrow.SetActive(true);
            point.ShowGuideArrow();
        }

        public void StopGuideArrowToPoint(GuidePoint point)
        {
            if (!point)
            {
                Debug.LogError("GUIDE POINT NULL ON StopGuideArrowToPoint");
                return;
            }

            if (GuidePoint != point) return;
            if (!GuidePoint)
                return;
            GuidePoint.HideGuideArrow();
            GuidePoint = null;
            if (!GuideClient)
                guideArrow.SetActive(false);
        }

        private bool _isMovingInToBattle;

        public bool isLockedMoving;

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!_isStartGame)
                return;

            // if (FacilityManager.Instance.testGameConfig.isTestCrash)
            //     return;
            if (GuidePoint)
            {
                guideArrow.transform.LookAt(GuidePoint.transform);
                guideArrow.SetActive(!(Vector3.Distance(transform.position, GuidePoint.transform.position) < 5));
            }
            else if (GuideClient)
            {
                if (GuideClient.IsGuidingClient)
                {
                    guideArrow.transform.LookAt(GuideClient.transform);
                    guideArrow.SetActive(!(Vector3.Distance(transform.position, GuideClient.transform.position) < 5));
                }
                else
                {
                    StopGuideArrowToClient(GuideClient);
                }
            }

            if (_isFollowingTarget && _target)
            {
                // Tính toán khoảng cách và hướng
                var followingDirection = _target.transform.position - transform.position;
                var distance = followingDirection.magnitude;

                if (distance > 0.5f)
                {
                    // Chuẩn hóa hướng và di chuyển
                    followingDirection = followingDirection.normalized;
                    var newMove = followingDirection * Speed * Time.deltaTime;
                    _characterController.Move(newMove);

                    // Xoay nhân vật về hướng di chuyển
                    if (newMove != Vector3.zero)
                    {
                        playerRotateBody.transform.forward = newMove;
                    }
                }
                else
                {
                    // Dừng lại khi đến gần mục tiêu
                    _isFollowingTarget = false;
                    _target = null;
                }

                return;
            }

            if (isLockedMoving)
            {
                if (stateMachine.ActionState != IdlePlayerActionStateMachine.PlayerActionState.Idle)
                    stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.Idle;
                return;
            }

            var direction = Vector3.forward * _floatingJoystick.Vertical + Vector3.right * _floatingJoystick.Horizontal;

            Vector3 move = new Vector3(_floatingJoystick.Horizontal, 0, _floatingJoystick.Vertical).normalized;
            if (move != Vector3.zero)
            {
                // Debug.Log("move " + move + ", " + _floatingJoystick.Vertical + ", " + _floatingJoystick.Horizontal);
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + BattleAngle;
                move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                if (!_isMovingInToBattle)
                {
                    // playerRotateBody.transform.rotation = Quaternion.Euler(0f, worldAngle, 0f);
                    playerRotateBody.transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                }
            }

            // Debug.Log("update: " + direction.magnitude);
            if (direction.magnitude > 0.8f)
            {
                trailEffect.gameObject.SetActive(true);
            }
            else
            {
                trailEffect.gameObject.SetActive(false);
            }

            if (direction.magnitude > 0)
            {
                direction = direction.normalized;
            }

            if (!_isMovingInToBattle)
            {
                _characterController.Move(move * (Speed * Time.deltaTime));
                // Đặt lại tọa độ y
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }

            if (IsSpeedBooster)
            {
                if (FireBaseManager.Instance.showNoel)
                {
                    if (stateMachine.ActionState != IdlePlayerActionStateMachine.PlayerActionState.DriveSleigh)
                        stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.DriveSleigh;
                }
                else
                {
                    if (stateMachine.ActionState != IdlePlayerActionStateMachine.PlayerActionState.DriveTank)
                        stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.DriveTank;
                }
            }
            else
            {
                if (_floatingJoystick.Direction.magnitude == 0)
                {
                    if (stateMachine.ActionState != IdlePlayerActionStateMachine.PlayerActionState.Idle)
                        stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.Idle;
                }
                else
                {
                    if (stateMachine.ActionState != IdlePlayerActionStateMachine.PlayerActionState.Run)
                        stateMachine.ActionState = IdlePlayerActionStateMachine.PlayerActionState.Run;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
        }

        public void HideTrail()
        {
            trailEffect.gameObject.SetActive(false);
        }

        private Collider _currentTriggerCollider;

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log("Run In OnTriggerEnter");
            if (other.GetComponent<HireEmployeePoint>())
            {
                _currentTriggerCollider = other;
            }

            if (other.GetComponent<BuildVehiclePoint>())
            {
                _currentTriggerCollider = other;
            }

            if (other.GetComponent<UpgradeVehiclePoint>())
            {
                _currentTriggerCollider = other;
            }
            else if (other.GetComponent<GetBoosterPoint>())
            {
                if (other.GetComponent<GetBoosterPoint>().IsActive)
                    _currentTriggerCollider = other;
            }
            else if (other.GetComponent<GetWorkerSpeedBoosterPoint>())
            {
                _currentTriggerCollider = other;
            }
            else if (other.GetComponent<GetMoneyPoint>())
            {
                if (other.GetComponent<GetMoneyPoint>().IsActive)
                    _currentTriggerCollider = other;
            }
            else if (other.GetComponent<StartBattlePoint>())
            {
                _currentTriggerCollider = other;
            }
            else if (other.GetComponent<GetVipSoldierPoint>())
            {
                if (other.GetComponent<GetVipSoldierPoint>().IsActive)
                    _currentTriggerCollider = other;
            }
            else if (other.GetComponent<GetUpgradeWorkerPoint>())
            {
                if (other.GetComponent<GetUpgradeWorkerPoint>().IsActive)
                    _currentTriggerCollider = other;
            }

            var parent = other.transform.parent;

            if (parent.GetComponent<VipSoldier>())
            {
                _currentTriggerCollider = other;
            }

            var room = parent.GetComponent<BaseRoom>();
            if (other.GetComponent<EntryPoint>())
            {
                other.GetComponent<EntryPoint>().StepIn();
                FacilityManager.Instance.OnEntryPointActive(parent.GetComponent<BaseRoom>());
            }
            else if (other.GetComponent<ExitPoint>())
            {
                other.GetComponent<ExitPoint>().StepIn();
                FacilityManager.Instance.OnExitPointActive(parent.GetComponent<BaseRoom>());
            }
            else if (other.GetComponent<BuildPoint>())
            {
                FacilityManager.Instance.PlayerInfoUI.IsLocked = true;
                _currentTriggerCollider = other;
            }
            else if (other.GetComponent<RepairPoint>())
            {
                _currentTriggerCollider = other;
            }
            else if (other.GetComponent<CashPoint>())
            {
                FacilityManager.Instance.WithDrawMoneyFromRoom(room);
            }
            else if (other.GetComponent<StarPoint>())
            {
            }
            else if (other.GetComponent<UpgradePoint>())
            {
                _currentTriggerCollider = other;
            }
            else if (other.GetComponent<BuyWorkerPoint>())
            {
                _currentTriggerCollider = other;
            }
            else if (other.GetComponent<CheckGetInRoomArea>())
            {
                if (room.GetComponent<BattleRoom>())
                {
                    IsInBattleRoom = true;
                    FacilityManager.Instance.OnStepInBattle();
                }
                else if (room.GetComponent<BedRoom>() && room.GetComponent<BedRoom>().IsDesertRoom)
                {
                    if (room.IsConnecting)
                        return;
                    var fixedTime = 3;
                    room.ShowConnectAnim(fixedTime);
                    _tweenToDeactivateDesertRoom?.Kill();
                    _tweenToDeactivateDesertRoom = DOVirtual.DelayedCall(fixedTime,
                        (() =>
                        {
                            if (room.GetComponent<BedRoom>().IsDesertRoom)
                                room.GetComponent<BedRoom>().DeactivateDeserters();
                        }));
                }
            }
        }

        public float BattleAngle;

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<GetBoosterPoint>())
            {
                _currentTriggerCollider = null;
                _tweenToActiveGetBoosterPoint?.Kill();
            }
            else if (other.GetComponent<GetWorkerSpeedBoosterPoint>())
            {
                _currentTriggerCollider = null;
                _tweenToActiveGetWorkerBoosterPoint?.Kill();
            }
            else if (other.GetComponent<GetMoneyPoint>())
            {
                _currentTriggerCollider = null;
                _tweenToActiveGetMoneyPoint?.Kill();
            }
            else if (other.GetComponent<StartBattlePoint>())
            {
                _currentTriggerCollider = null;
                FacilityManager.Instance.PlayerInfoUI.HideStartBattleButton();
            }
            else if (other.GetComponent<GetVipSoldierPoint>())
            {
                _currentTriggerCollider = null;
            }
            else if (other.GetComponent<GetUpgradeWorkerPoint>())
            {
                _currentTriggerCollider = null;
            }
            else if (other.GetComponent<UpgradePoint>())
            {
                _currentTriggerCollider = null;
            }
            else if (other.GetComponent<BuyWorkerPoint>())
            {
                _currentTriggerCollider = null;
            }
            else if (other.GetComponent<HireEmployeePoint>())
            {
                _currentTriggerCollider = null;
                _tweenToActiveHireEmployeePoint?.Kill();
                _delayToHireEmployeeTween?.Kill();
            }

            if (other.GetComponent<BuildVehiclePoint>())
            {
                _currentTriggerCollider = null;
                _delayToBuildVehicleTween?.Kill();
            }

            if (other.GetComponent<UpgradeVehiclePoint>())
            {
                _currentTriggerCollider = null;
                _delayToBuildVehicleTween?.Kill();
            }

            var parent = other.transform.parent;

            if (parent.GetComponent<VipSoldier>())
            {
                _currentTriggerCollider = null;
            }

            var room = parent.GetComponent<BaseRoom>();
            if (!room) return;
            if (other.GetComponent<EntryPoint>())
            {
                other.GetComponent<EntryPoint>().StepOut();
                FacilityManager.Instance.OnEntryPointDeactivate(room);
            }
            else if (other.GetComponent<ExitPoint>())
            {
                other.GetComponent<ExitPoint>().StepOut();
                FacilityManager.Instance.OnExitPointDeactivate(parent.GetComponent<BaseRoom>());
            }
            else if (other.GetComponent<BuildPoint>())
            {
                FacilityManager.Instance.PlayerInfoUI.IsLocked = false;
                _currentTriggerCollider = null;
                _tweenToActiveBuildPoint?.Kill();
                _delayToBuildRoomTween?.Kill();
            }
            else if (other.GetComponent<RepairPoint>())
            {
                FacilityManager.Instance.OnMonetPointExit();
                _currentTriggerCollider = null;
                _tweenToActiveBuildPoint?.Kill();
                _delayToBuildRoomTween?.Kill();
            }
            else if (other.GetComponent<CheckGetInRoomArea>())
            {
                if (room.GetComponent<BattleRoom>())
                {
                    IsInBattleRoom = false;
                    FacilityManager.Instance.OnStepOutBattle();
                }
                else if (room.GetComponent<BedRoom>() && room.GetComponent<BedRoom>().IsDesertRoom)
                {
                    room.HideConnectAnim();
                    _tweenToDeactivateDesertRoom?.Kill();
                }
            }
        }

        private Tween _tweenToDeactivateDesertRoom;

        private Tween _delayToHireEmployeeTween;

        private void OnWithDrawMoneyToHireEmployee(HireEmployeePoint point)
        {
            _countMoney++;
            // Debug.Log("Run OnWithDrawMoneyToHire");
            if (_countMoney > point.GetDepositLeft())
                _countMoney = point.GetDepositLeft();
            if (point.GetDepositLeft() <= 10)
                _countMoney = 1;
            var money = FacilityManager.Instance.WithdrawMoney(_countMoney);
            if (money <= 0)
            {
                DOVirtual.DelayedCall(1, (() => { FacilityManager.Instance.ShowNearestMoneyPoint(); }));
                return;
            }

            point.DepositNumber += money;
            // Debug.Log("OnWithDrawMoneyToHireEmployee: " + point.DepositNumber + ", " + money);

            if (point.DepositNumber >= point.Price)
            {
                QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId.HireWorker);
                point.ActiveEmployee();
                FacilityManager.Instance.AddEmployee(point);
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Upgrade_Worker_MGB);
                Destroy(point.gameObject);
                return;
            }

            point.ShowSpendMoneyAnimation();

            _delayToHireEmployeeTween?.Kill();
            _delayToHireEmployeeTween = DOVirtual.DelayedCall(0.05f, (() => { OnWithDrawMoneyToHireEmployee(point); }));
        }

        private Tween _delayToBuildRoomTween;

        private int _countMoney;

        private void OnWithDrawMoneyToBuildRoom(BaseRoom room)
        {
            _countMoney++;
            // Debug.Log("Run OnWithDrawMoneyToBuildRoom");
            if (_countMoney > room.GetDepositLeft())
                _countMoney = room.GetDepositLeft();
            if (room.GetDepositLeft() <= 10)
                _countMoney = 1;

            var money = FacilityManager.Instance.WithdrawMoney(_countMoney);
            if (money <= 0)
            {
                _currentTriggerCollider = null;
                FacilityManager.Instance.PlayerInfoUI.IsLocked = false;
                DOVirtual.DelayedCall(1, (() => { FacilityManager.Instance.ShowNearestMoneyPoint(); }));
                return;
            }

            room.DepositToBuild(money);
            if (room.IsMoneyEnoughToBuild)
            {
                _currentTriggerCollider = null;

                if (room.GetComponent<TrainingRoom>())
                {
                    QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId
                        .BuildTrainingRoom);
                }
                else if (room.GetComponent<BedRoom>() && room.GetComponent<BedRoom>().IsBuilt)
                {
                    QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId
                        .UpgradeSleepRoom);
                    FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_BED_ROOM_2);
                }

                FacilityManager.Instance.PlayerInfoUI.IsLocked = false;
                if (room.GetComponent<BedRoom>())
                {
                    FacilityManager.Instance.GameSaveLoad.UpdateTemporaryBedRoom(room);
                    FacilityManager.Instance.ShowBuildBedRoomPopup(room.GetComponent<BedRoom>());
                }
                else
                {
                    room.StartBuild();
                    FacilityManager.Instance.OnRoomBuilt(room);
                }

                return;
            }

            _delayToBuildRoomTween?.Kill();
            _delayToBuildRoomTween = DOVirtual.DelayedCall(0.05f, (() => { OnWithDrawMoneyToBuildRoom(room); }));
        }

        private int _countToken;
        private Tween _delayToBuildVehicleTween;

        private void OnWithDrawTokenToBuildVehicle(BuildVehiclePoint point)
        {
            _countToken++;
            if (_countToken > point.GetDepositLeft())
                _countToken = point.GetDepositLeft();
            if (point.GetDepositLeft() <= 10)
                _countToken = 1;

            var token = FacilityManager.Instance.WithdrawToken(_countToken);
            if (token <= 0)
            {
                _currentTriggerCollider = null;
                return;
            }

            point.DepositNumber += token;
            point.ShowSpendMoneyAnimation();
            if (point.DepositNumber == point.Price)
            {
                _currentTriggerCollider = null;
                var room = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
                if (!room) return;
                room.BuildVehicle(point);
                if (!room.isFirstBattle) return;
                FacilityManager.Instance.GuidesManager.CheckShowStartBattleGuide(room);
                return;
            }

            _delayToBuildVehicleTween?.Kill();
            const float duration2 = 0.1f;
            _delayToBuildVehicleTween =
                DOVirtual.DelayedCall(duration2, (() => { OnWithDrawTokenToBuildVehicle(point); }));
        }

        private void OnWithDrawMoneyToUpgradeVehicle(UpgradeVehiclePoint point)
        {
            _countMoney++;
            if (_countMoney > point.GetDepositLeft())
                _countMoney = point.GetDepositLeft();
            if (point.GetDepositLeft() <= 10)
                _countMoney = 1;

            var money = FacilityManager.Instance.WithdrawMoney(_countMoney);
            if (money <= 0)
            {
                _currentTriggerCollider = null;
                return;
            }

            // Debug.Log("OnWithDrawMoneyToUpgradeVehicle: " + _countMoney + ", " + point.DepositNumber + ", " +
            //           point.Price);
            point.DepositNumber += money;
            point.ShowSpendMoneyAnimation();
            // Debug.Log("OnWithDrawMoneyToUpgradeVehicle: " + point.DepositNumber + ", " + point.Price);
            if (point.DepositNumber >= point.Price)
            {
                _currentTriggerCollider = null;
                var room = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
                if (room)
                {
                    room.UpgradeVehicle(point);
                }

                return;
            }

            _delayToBuildVehicleTween?.Kill();
            _delayToBuildVehicleTween =
                DOVirtual.DelayedCall(0.05f, (() => { OnWithDrawMoneyToUpgradeVehicle(point); }));
        }

        private bool _isFollowingTarget;
        private Transform _target;

        public void FollowTarget(Transform target)
        {
            _isFollowingTarget = true;
            _target = target;
        }

        public override void AddClient(WarBaseClient client)
        {
            base.AddClient(client);
            for (var i = 0; i < ClientsList.Count; i++)
            {
                ClientsList[i].FollowPlayer(gameObject, Speed, i + 1);
            }

            if (IsFullFollower)
            {
                if (_capacityTxt)
                    _capacityTxt.text = "Max";
            }
            else
            {
                if (_capacityTxt)
                    _capacityTxt.text = ClientsList.Count + "/" + FacilityManager.Instance.IdleGameData.Capacity;
            }

            // State = PlayerState.Max;
            FacilityManager.Instance.CheckEnableEntryPointWhenPlayerLeadClient();
        }

        public override void RemoveClient(WarBaseClient client)
        {
            base.RemoveClient(client);
            client.StopFollow();
            if (!IsFullFollower)
            {
                if (ClientsList.Count == 0)
                {
                    if (_capacityTxt)
                        _capacityTxt.text = "";
                }
                else
                {
                    if (_capacityTxt)
                        _capacityTxt.text = ClientsList.Count + "/" + FacilityManager.Instance.IdleGameData.Capacity;
                }
            }
            else
            {
                if (_capacityTxt)
                    _capacityTxt.text = "Max";
            }

            // State = PlayerState.Default;
            FacilityManager.Instance.OnPlayerRemoveClient();

            if (!GuideClient) return;
            if (GuideClient.IsGuidingClient)
                ShowGuideArrowToClient(GuideClient);
            else
                GuideClient = null;
        }

        public void ShowSpeedUpEffect()
        {
            var eff = Instantiate(speedUpEffect, CanvasManager.Instance.gamePlayUILayer.transform);
            CanvasManager.Instance.ShowEffect(CanvasManager.EffectType.BubbleEffect, eff.transform);
            DOVirtual.DelayedCall(3, (() => { Destroy(eff); }));
        }

        public void ShowCapacityUpEffect()
        {
            var eff = Instantiate(capacityUpEffect, CanvasManager.Instance.gamePlayUILayer.transform);
            CanvasManager.Instance.ShowEffect(CanvasManager.EffectType.BubbleEffect, eff.transform);
            DOVirtual.DelayedCall(3, (() => { Destroy(eff); }));
        }

        public WarBaseClient GetClientByState(WarBaseClient.ClientEmotionState emotionState)
        {
            return ClientsList.FirstOrDefault(t => t.GetComponent<WarBaseClient>().EmotionState == emotionState);
        }

        public void Active()
        {
            // enabled = true;
        }

        public void Deactive()
        {
            // enabled = false;
        }
    }
}