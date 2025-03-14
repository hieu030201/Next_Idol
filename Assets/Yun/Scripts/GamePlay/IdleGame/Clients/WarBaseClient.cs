using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Random = System.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Clients
{
    public class WarBaseClient : Client
    {
        [SerializeField] private ClientActionStateMachine stateMachine;
        [SerializeField] private GameObject manModel;
        [SerializeField] private GameObject soldierLv1Model;
        [SerializeField] private GameObject soldierLv2Model;
        [SerializeField] private GameObject soldierLv3Model;
        [SerializeField] private GameObject captainAModel;
        [SerializeField] private GameObject ironManModel;
        [SerializeField] private GameObject wonderWomanModel;
        [SerializeField] private GameObject cyborgModel;
        [SerializeField] private GameObject comandoModel;
        [SerializeField] private GameObject staffCanteenModel;
        [SerializeField] private GameObject staffBoxingModel;
        [SerializeField] private GameObject staffTrainingModel;
        [SerializeField] private GameObject enemyModel;
        [SerializeField] private GameObject enemyModel2;
        [SerializeField] private GameObject enemyModel3;
        [SerializeField] private GameObject dumbbell;
        [SerializeField] private ParticleSystem upgradeEffect;
        [SerializeField] private ParticleSystem waitToUpgradeEffect;
        [SerializeField] private ParticleSystem sleepEffect;
        [SerializeField] private GameObject clientGeometry;

        private GameObject _pickAxe;
        private GameObject _shovel;

        [Button]
        public void TestAnim()
        {
            // stateMachine.ActionState = ClientActionStateMachine.State.ShootPistol;
            stateMachine.ActionState = ClientActionStateMachine.State.WeightLifting;
        }

        public enum ClientEmotionState
        {
            CheckIn,
            WaitToRest,
            Rest,
            WaitToExercise,
            Exercise,
            WaitToEat,
            Eat,
            WaitToBoxing,
            Boxing,
            WaitToBattle,
            Completed,
            WaitToCheckIn,
            WaitToUpgrade,
            Idle,
            Salute,
            RuningToSlot,
            InTreating,
            WaitToTreating,
        }

        private ClientEmotionState _emotionState;

        public ClientEmotionState EmotionState
        {
            get => _emotionState;
            set
            {
                _emotionState = value;
                // if (EmotionStateUI)
                //     Destroy(EmotionStateUI);

                switch (value)
                {
                    case ClientEmotionState.WaitToBattle:
                    case ClientEmotionState.WaitToExercise:
                    case ClientEmotionState.WaitToEat:
                    case ClientEmotionState.WaitToBoxing:
                    case ClientEmotionState.WaitToUpgrade:
                    case ClientEmotionState.WaitToTreating:
                        ShowConnectPoint();
                        break;
                    default:
                        HideConnectPoint();
                        break;
                }

                switch (value)
                {
                    case ClientEmotionState.WaitToBattle:
                    case ClientEmotionState.WaitToRest:
                    case ClientEmotionState.WaitToExercise:
                    case ClientEmotionState.WaitToEat:
                    case ClientEmotionState.WaitToBoxing:
                    case ClientEmotionState.WaitToTreating:
                    case ClientEmotionState.WaitToUpgrade:
                    case ClientEmotionState.Idle:
                        stateMachine.ActionState = ClientActionStateMachine.State.Idle;
                        break;
                }

                if (value == ClientEmotionState.WaitToUpgrade)
                    ShowWaitToUpgradeEffect();

                _status.EmotionState = EmotionState;
            }
        }

        public enum ClientType
        {
            Man,
            SoldierLv1,
            SoldierLv2,
            SoldierLv3,
            Captain_A,
            Iron_Man,
            Wonder_Woman,
            Cyborg_Commando,
            Commando,
            StaffBoxing,
            StaffCanteen,
            StaffTraining,
            Enemy,
            Enemy2,
            Enemy3,
        }

        public Dictionary<ClientEmotionState, int> DoneList { get; set; }

        private ClientType _type;

        public ClientType Type
        {
            get => _type;
            set
            {
                _type = value;
                // Debug.Log("warbase client set type: " + gameObject.name + ", " + _type);
                HideAllModel();

                switch (value)
                {
                    case ClientType.Man:
                        manModel.SetActive(true);
                        break;
                    case ClientType.SoldierLv1:
                        soldierLv1Model.SetActive(true);
                        break;
                    case ClientType.SoldierLv2:
                        soldierLv2Model.SetActive(true);
                        break;
                    case ClientType.SoldierLv3:
                        soldierLv3Model.SetActive(true);
                        break;
                    case ClientType.Captain_A:
                        captainAModel.SetActive(true);
                        break;
                    case ClientType.Iron_Man:
                        ironManModel.SetActive(true);
                        break;
                    case ClientType.Wonder_Woman:
                        wonderWomanModel.SetActive(true);
                        break;
                    case ClientType.Cyborg_Commando:
                        cyborgModel.SetActive(true);
                        break;
                    case ClientType.Commando:
                        comandoModel.SetActive(true);
                        break;
                    case ClientType.Enemy:
                        enemyModel.SetActive(true);
                        break;
                    case ClientType.Enemy2:
                        enemyModel2.SetActive(true);
                        break;
                    case ClientType.Enemy3:
                        enemyModel3.SetActive(true);
                        break;
                    case ClientType.StaffTraining:
                        staffTrainingModel.SetActive(true);
                        break;
                    case ClientType.StaffBoxing:
                        staffBoxingModel.SetActive(true);
                        break;
                    case ClientType.StaffCanteen:
                        staffCanteenModel.SetActive(true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private ClientStatus _status;

        protected override void Awake()
        {
            base.Awake();

            _shovel = clientGeometry.transform.Find("Tools").transform.Find("shovel").gameObject;
            _pickAxe = clientGeometry.transform.Find("Tools").transform.Find("pickaxe").gameObject;

            clientGeometry.transform.Find("Tools").gameObject.SetActive(true);
            _shovel.SetActive(false);
            _pickAxe.SetActive(false);

            _status = transform.Find("Status").GetComponent<ClientStatus>();
            _status.InitTransform(transform);

            DoneList = new Dictionary<ClientEmotionState, int>
            {
            };
            
            HideAllModel();
            if (waitToUpgradeEffect)
                waitToUpgradeEffect.gameObject.SetActive(false);
            if (upgradeEffect)
                upgradeEffect.gameObject.SetActive(false);
            if (sleepEffect)
                sleepEffect.gameObject.SetActive(false);
        }

        public override void Reset()
        {
            base.Reset();
            HideAllModel();
            _shovel.SetActive(false);
            _pickAxe.SetActive(false);
            
            if (waitToUpgradeEffect)
                waitToUpgradeEffect.gameObject.SetActive(false);
            if (upgradeEffect)
                upgradeEffect.gameObject.SetActive(false);
            if (sleepEffect)
                sleepEffect.gameObject.SetActive(false);
            
            DoneList = new Dictionary<ClientEmotionState, int>
            {
            };
            
            IsDeserting = false;
        }

        private void HideAllModel()
        {
            manModel.SetActive(false);
            soldierLv1Model.SetActive(false);
            soldierLv2Model.SetActive(false);
            soldierLv3Model.SetActive(false);
            enemyModel.SetActive(false);
            staffBoxingModel.SetActive(false);
            staffCanteenModel.SetActive(false);
            staffTrainingModel.SetActive(false);
            captainAModel.SetActive(false);
            ironManModel.SetActive(false);
            wonderWomanModel.SetActive(false);
            cyborgModel.SetActive(false);
            comandoModel.SetActive(false);
        }

        private GameObject _saluteTarget;

        public void FollowSalute(GameObject player, float speed = 0)
        {
            TargetSlot = null;
            NavMeshAgent.enabled = true;
            IsMarked = false;
            // isConnectedToPlayer = true;
            NavMeshAgent.stoppingDistance = 0;
            if (speed != 0)
                NavMeshAgent.speed = speed;
            _saluteTarget = player;
            NavMeshAgent.SetDestination(_saluteTarget.transform.position);
            _emotionState = ClientEmotionState.Salute;
            IsMovingToSlot = true;
        }

        public void Salute()
        {
            transform.rotation = _saluteTarget.transform.rotation;
            stateMachine.ActionState = ClientActionStateMachine.State.Salute;
        }

        protected override void FixedUpdate()
        {
            try
            {
                base.FixedUpdate();

                /*if (_saluteTarget)
                {
                    _navMeshAgent.SetDestination(_saluteTarget.transform.position);
                }*/

                // if (FacilityManager.Instance.testGameConfig.isTestCrash)
                //     return;

                if (NavMeshAgent.velocity.magnitude == 0)
                {
                    // if (stateMachine.ActionState != ClientActionStateMachine.State.Run) return;

                    if (isConnectedToPlayer)
                    {
                        if (stateMachine.ActionState != ClientActionStateMachine.State.Idle)
                            stateMachine.ActionState = ClientActionStateMachine.State.Idle;
                        return;
                    }

                    if (IsMovingToSlot)
                    {
                        if (NavMeshAgent.pathPending || NavMeshAgent.remainingDistance > 1) return;
                        IsMovingToSlot = false;
                        OnMoveToSlotCompleted?.Invoke();
                        if (LastRoom)
                        {
                            LastRoom.OnClientMoveToSlotCompleted(this);
                        }

                        if (EmotionState == ClientEmotionState.Boxing)
                        {
                            Boxing();
                        }
                        else if (EmotionState == ClientEmotionState.Exercise)
                        {
                            Exercise();
                        }
                        else if (EmotionState == ClientEmotionState.Eat)
                        {
                            WaitForFood();
                        }
                        else if (EmotionState == ClientEmotionState.Rest)
                        {
                            Rest();
                        }
                        else if (EmotionState == ClientEmotionState.InTreating)
                        {
                            Treating();
                        }
                        else
                        {
                            if (LastRoom && LastRoom.GetComponent<BattleRoom>())
                            {
                                if (TargetSlot)
                                    transform.rotation = TargetSlot.transform.rotation;
                                if (GetComponent<ClientShootingControl>())
                                    GetComponent<ClientShootingControl>().ShowIdleRiffle();
                                GetComponent<WarBaseClient>().enabled = false;
                                NavMeshAgent.enabled = false;
                            }
                            else
                            {
                                if (stateMachine.ActionState != ClientActionStateMachine.State.Idle)
                                    stateMachine.ActionState = ClientActionStateMachine.State.Idle;
                                if (TargetSlot)
                                    transform.rotation = TargetSlot.transform.rotation;
                                if (_saluteTarget)
                                    transform.rotation = _saluteTarget.transform.rotation;
                            }
                        }
                    }

                    _status.DeactivateReposition();
                    _status.Reposition();
                }
                else
                {
                    if (EmotionState is ClientEmotionState.WaitToTreating or ClientEmotionState.InTreating)
                    {
                        if (stateMachine.ActionState != ClientActionStateMachine.State.WalkInjured)
                        {
                            stateMachine.ActionState = ClientActionStateMachine.State.WalkInjured;
                        }
                    }
                    else if (EmotionState == ClientEmotionState.InTreating)
                    {
                        if (stateMachine.ActionState != ClientActionStateMachine.State.WalkInjured)
                        {
                            stateMachine.ActionState = ClientActionStateMachine.State.WalkInjured;
                        }
                    }
                    else
                    {
                        if (stateMachine.ActionState != ClientActionStateMachine.State.Run)
                        {
                            stateMachine.ActionState = ClientActionStateMachine.State.Run;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override void FollowPlayer(GameObject player, float speed = 0, int index = 1)
        {
            base.FollowPlayer(player, speed, index);
            _status.ActiveReposition();
            stateMachine.StopIdle();
            // if (EmotionStateUI)
            //     EmotionStateUI.GetComponent<ClientStateUI>().ShowConnectedStatus();
            _status.ShowConnectedStatus();
            if (!guideArrowOnPoint || !guideArrowOnPoint.gameObject.activeSelf) return;
            HideGuideArrowOnPoint();
            if (!GuidePoint) return;
            if (!player.GetComponent<IdlePlayer>()) return;
            player.GetComponent<IdlePlayer>().ShowGuideArrowToPoint(GuidePoint);
            PlayerFollowingGuidePoint = player.GetComponent<IdlePlayer>();
        }

        public override void StopFollow()
        {
            base.StopFollow();
            HideConnectAnim();

            if (!GuidePoint) return;
            if (PlayerFollowingGuidePoint)
                PlayerFollowingGuidePoint.StopGuideArrowToPoint(GuidePoint);
            GuidePoint = null;
            PlayerFollowingGuidePoint = null;
        }

        public void HideConnectAnim()
        {
            _status.HideConnectAnimation();
        }

        public void ShowConnectAnim(float duration = 0.75f)
        {
            _status.ShowConnectAnimation(duration);
        }

        public override void LevelUp()
        {
            base.LevelUp();
            switch (Level)
            {
                case 1:
                    Type = ClientType.SoldierLv1;
                    break;
                case 2:
                    Type = ClientType.SoldierLv2;
                    break;
                case 3:
                    Type = ClientType.SoldierLv3;
                    break;
            }

            if (LastRoom)
            {
                DoneList[LastRoom.emotionStateToGetIn]++;
            }

            UpdateLastRoomToSaveData();
        }

        public void ShowUpgradeEffect()
        {
            // Debug.Log("ShowUpgradeEffect");
            if (waitToUpgradeEffect)
                waitToUpgradeEffect.gameObject.SetActive(false);
            if (upgradeEffect)
            {
                upgradeEffect.gameObject.SetActive(true);
                upgradeEffect.Play();
            }

            DOVirtual.DelayedCall(0.5f, (() =>
            {
                stateMachine.ShowUpgradeAnimation();
                upgradeEffect.gameObject.SetActive(false);
            }));
        }

        public void ShowWaitToUpgradeEffect()
        {
            if (waitToUpgradeEffect)
            {
                waitToUpgradeEffect.gameObject.SetActive(true);
                waitToUpgradeEffect.Play();
            }
        }

        public override void MoveToSlot(Transform slot, bool immediately = false, Action moveToSlotCompleted = null)
        {
            base.MoveToSlot(slot, immediately, moveToSlotCompleted);
            if (!immediately)
            {
                _status.ActiveReposition();
                stateMachine.StopIdle();
                stateMachine.ActionState = ClientActionStateMachine.State.Run;
            }
            else
            {
                _status.Reposition();
            }
        }

        private Transform _sleepPosition;

        public void SetSleepPosition(Transform sleepPosition)
        {
            _sleepPosition = sleepPosition;
        }

        private Transform _weightLiftPosition;
        private GameObject _roomDumbbell;

        public void SetWeightLiftPosition(Transform weightLiftPosition, GameObject roomDumbbell)
        {
            _weightLiftPosition = weightLiftPosition;
            _roomDumbbell = roomDumbbell;
        }

        public override void SetData(ClientData clientData)
        {
            base.SetData(clientData);
            GetComponent<WarBaseClient>().Type = clientData.Type;
            GetComponent<WarBaseClient>().DoneList = clientData.DoneList;
        }

        public void Rest(bool immediately = false)
        {
            if (immediately)
            {
                var delay = (float)(new Random().NextDouble() + 1);
                if (Level == 0)
                    Invoke(nameof(ShowWaitToUpgrade), delay);
                else
                    Invoke(nameof(GetAvailableTask), delay);
                return;
            }

            StopFollow();

            NavMeshAgent.enabled = false;

            var clientTransform = transform;
            var sleepPositionTransform = _sleepPosition.transform;
            clientTransform.rotation = sleepPositionTransform.rotation;

            // Nếu phòng của lính đang là phòng đào ngũ
            if (LastRoom && LastRoom.GetComponent<BedRoom>() && LastRoom.GetComponent<BedRoom>().IsDesertRoom)
            {
                StartDeserting();
                return;
            }

            EmotionState = ClientEmotionState.Rest;
            DOVirtual.DelayedCall(0.2f, (() =>
            {
                var main = sleepEffect.main;
                main.loop = true;
                sleepEffect.gameObject.SetActive(true);
                stateMachine.ActionState = ClientActionStateMachine.State.Sleep;
            }));

            var position = clientTransform.position;
            var position1 = sleepPositionTransform.position;
            var midPoint = (position + position1) / 2;
            midPoint.y += 0.5f;

            var path = new Vector3[3];
            path[0] = position;
            path[1] = midPoint;
            path[2] = position1;

            var path2 = new Vector3[3];
            path2[0] = position1;
            path2[1] = midPoint;
            path2[2] = position;

            transform.DOPath(path, 0.4f, PathType.CatmullRom)
                .SetEase(Ease.OutQuad);
            // transform.position = path[2];
            var sleepDelay = new Random().Next(6) + 3;
            if (FacilityManager.Instance.GameSaveLoad.GameData.countFirstClient < 3)
            {
                sleepDelay = 2;
            }

            DOVirtual.DelayedCall(sleepDelay, (() =>
            {
                sleepEffect.gameObject.SetActive(false);
                stateMachine.ActionState = ClientActionStateMachine.State.WakeUp;
                DOVirtual.DelayedCall(1.5f, (() =>
                {
                    transform.DOPath(path2, 0.4f, PathType.CatmullRom)
                        .SetEase(Ease.OutQuad).OnComplete(AfterGetUp);
                }));
            }));
        }

        private void AfterGetUp()
        {
            // Nếu phòng của lính đang là phòng đào ngũ
            if (LastRoom && LastRoom.GetComponent<BedRoom>() && LastRoom.GetComponent<BedRoom>().IsDesertRoom)
            {
                StartDeserting();
                return;
            }

            var randomActionNumber = new Random().Next(3);

            if (FacilityManager.Instance.GameSaveLoad.GameData.countFirstClient < 3)
            {
                randomActionNumber = 2;
                // Debug.Log("AfterGetUp 1");
            }

            switch (randomActionNumber)
            {
                case 0:
                    var delay = new Random().NextDouble() + 0.5f;
                    DOVirtual.DelayedCall((float)delay, (() =>
                    {
                        stateMachine.ActionState = ClientActionStateMachine.State.PushUp;
                        var pushUpNumber = new Random().Next(6) + 2;
                        if (Level == 0)
                            Invoke(nameof(ShowWaitToUpgrade), pushUpNumber * 0.7f);
                        else
                            Invoke(nameof(GetAvailableTask), pushUpNumber * 0.7f);
                    }));
                    break;
                case 1:
                    stateMachine.ActionState = ClientActionStateMachine.State.Look;
                    var delay1 = new Random().NextDouble() * 2 + 4f;
                    if (Level == 0)
                        Invoke(nameof(ShowWaitToUpgrade), (float)delay1);
                    else
                        Invoke(nameof(GetAvailableTask), (float)delay1);
                    break;
                case 2:
                    var delay2 = new Random().NextDouble() * 4 + 1.5f;
                    if (FacilityManager.Instance.GameSaveLoad.GameData.countFirstClient < 3)
                    {
                        delay2 = 1;
                        FacilityManager.Instance.GameSaveLoad.GameData.countFirstClient++;
                        // Debug.Log("AfterGetUp 2");
                    }

                    if (Level == 0)
                    {
                        DOVirtual.DelayedCall((float)delay2, (() =>
                        {
                            ShowWaitToUpgrade();
                            FacilityManager.Instance.GuidesManager.CheckShowGuideConnectClientAndLeadToBattle(FacilityManager.Instance.battleRoom1);
                        }));
                    }
                    else
                    {
                        
                        Invoke(nameof(GetAvailableTask), (float)delay2);
                    }
                    break;
            }
        }

        public void ShowWaitToUpgrade()
        {
            if (TargetSlot)
                transform.position = TargetSlot.position;
            EmotionState = ClientEmotionState.WaitToUpgrade;
            IsBusy = false;
            // Nếu phòng của lính đang là phòng đào ngũ
            if (LastRoom && LastRoom.GetComponent<BedRoom>() && LastRoom.GetComponent<BedRoom>().IsDesertRoom)
            {
                StartDeserting();
                return;
            }
            // FacilityManager.Instance.Navigation(gameObject);
            // FacilityManager.Instance.AddUpgradeClient(this);
        }

        public void ShowWaitToRest()
        {
            EmotionState = ClientEmotionState.WaitToRest;
            ShowConnectPoint();
        }

        public void GetAvailableTask()
        {
            if (TargetSlot)
                transform.position = TargetSlot.position;
            IsBusy = false;
            EmotionState = FacilityManager.Instance.GetAvailableTask(this);
            if (EmotionState == ClientEmotionState.Idle)
                DOVirtual.DelayedCall(5, GetAvailableTask);
            // Nếu phòng của lính đang là phòng đào ngũ
            if (LastRoom && LastRoom.GetComponent<BedRoom>() && LastRoom.GetComponent<BedRoom>().IsDesertRoom)
            {
                StartDeserting();
                return;
            }

            FacilityManager.Instance.GuidesManager.CheckShowGuideForFirstClientAction(this);
        }

        public void Boxing(bool immediately = false)
        {
            if (immediately)
            {
                ShowWaitToUpgrade();
                return;
            }

            stateMachine.ActionState = ClientActionStateMachine.State.Boxing;
            if (_weightLiftPosition)
            {
                stateMachine.ActionState = ClientActionStateMachine.State.Idle;
                NavMeshAgent.enabled = false;
                var transform1 = transform;
                var transform3 = _weightLiftPosition.transform;
                transform1.rotation = transform3.rotation;
                transform1.position = transform3.position;
                stateMachine.StartLiftingWeight(dumbbell, _roomDumbbell, FinishLiftingWeight);
            }
            else
            {
                IntoPosition();
                stateMachine.StartBoxing(FinishBoxing);
            }
        }

        private void FinishLiftingWeight()
        {
            transform.position = TargetSlot.position;
            ShowWaitToUpgrade();
        }

        private void FinishBoxing()
        {
            ShowWaitToUpgrade();
        }

        private void UpdateLastRoomToSaveData()
        {
            // Debug.Log("UpdateLastRoomToSaveData");
            FacilityManager.Instance.UpdateRoomsToSaveData(new List<BaseRoom> { LastRoom });
        }

        public void Exercise()
        {
            stateMachine.ActionState = ClientActionStateMachine.State.Exercise;
            DOVirtual.DelayedCall(1f, () => { GetComponent<ClientShootingControl>().IsParticalShoot = true; });
            IntoPosition();
            // DOVirtual.DelayedCall(5, (() => { CompleteOneTask(); }));
            DOVirtual.DelayedCall(5, () =>
            {
                GetComponent<ClientShootingControl>().IsParticalShoot = false;
                ShowWaitToUpgrade();
            });
        }

        private void WaitForFood()
        {
            stateMachine.ActionState = ClientActionStateMachine.State.Sit;
            IntoPosition();
            // Tween cho tình huống chờ mãi không được phục vụ đồ ăn
            _tweenToEat?.Kill();
            _tweenToEat = DOVirtual.DelayedCall(15, Eat);
            AddTweenToTweenManager(_tweenToEat);
        }

        private Transform _deserterDecor;

        public Transform DeserterDecor
        {
            set => _deserterDecor = value;
        }

        [ShowInInspector]
        public bool IsDeserting { get; set; }

        public void StartDeserting()
        {
            IsDeserting = true;
            _status.gameObject.SetActive(false);
            HideConnectAnim();
            if(EmotionState == ClientEmotionState.WaitToUpgrade)
                waitToUpgradeEffect.gameObject.SetActive(false);
            var random = new Random();
            var randomNumber = random.Next(0, 2);

            var randomDelay = (float)random.NextDouble() * 1;

            DOVirtual.DelayedCall(randomDelay, () =>
            {
                if (randomNumber == 1)
                {
                    Shovel();
                }
                else
                {
                    ChoppingAxe();
                }
            }).SetAutoKill(true);

            RotateToDeserterDecor();
        }

        [Button]
        public void RotateToDeserterDecor()
        {
            transform.LookAt(_deserterDecor);
        }

        public void FinishDesert()
        {
            StartCoroutine(JumpAndDropToPoint(transform.position, _deserterDecor.position, 0.5f, 0.5f, 1.8f, 0.2f));
        }

        // Hàm thực hiện nhảy từ điểm A đến điểm B và sau đó tụt xuống
        private IEnumerator JumpAndDropToPoint(Vector3 startPoint, Vector3 endPoint, float jumpHeight,
            float jumpDuration, float dropHeight, float dropDuration)
        {
            // Giai đoạn 1: Nhảy từ A tới B
            var elapsedJumpTime = 0f;
            while (elapsedJumpTime < jumpDuration)
            {
                elapsedJumpTime += Time.deltaTime;
                var percentageComplete = elapsedJumpTime / jumpDuration;

                // Tính toán vị trí ngang (x, z)
                var horizontalPosition = Vector3.Lerp(startPoint, endPoint, percentageComplete);

                // Tính toán độ cao theo đường parabol
                var height = 4f * jumpHeight * (1f - Mathf.Pow(2f * percentageComplete - 1f, 2f));

                // Kết hợp vị trí ngang và độ cao
                var newPosition = new Vector3(
                    horizontalPosition.x,
                    startPoint.y + height,
                    horizontalPosition.z
                );

                // Cập nhật vị trí của nhân vật
                transform.position = newPosition;

                yield return null;
            }

            // Giai đoạn 2: Tụt xuống điểm đích Y
            var elapsedDropTime = 0f;
            var dropEndPoint = new Vector3(endPoint.x, endPoint.y - dropHeight, endPoint.z);

            while (elapsedDropTime < dropDuration)
            {
                elapsedDropTime += Time.deltaTime;
                var percentageComplete = elapsedDropTime / dropDuration;

                // Sử dụng phép nội suy đường cong mượt
                var smoothPercentage = 1f - Mathf.Cos((percentageComplete * Mathf.PI) / 2f);

                // Tính toán vị trí mới
                var newPosition = Vector3.Lerp(endPoint, dropEndPoint, smoothPercentage);

                // Cập nhật vị trí của nhân vật
                transform.position = newPosition;

                yield return null;
            }

            // Destroy(gameObject);
            FacilityManager.Instance.ReturnClientToPool(gameObject);
        }

        public void StopDeserting()
        {
            if(EmotionState == ClientEmotionState.WaitToUpgrade)
                waitToUpgradeEffect.gameObject.SetActive(true);
            IsDeserting = false;
            _status.gameObject.SetActive(true);
            _shovel.SetActive(false);
            _pickAxe.SetActive(false);
            stateMachine.ActionState = ClientActionStateMachine.State.Idle;

            if (EmotionState == ClientEmotionState.Rest)
                Rest();
        }

        private void Shovel()
        {
            stateMachine.ActionState = ClientActionStateMachine.State.Shovel;
            _shovel.SetActive(true);
        }

        private void ChoppingAxe()
        {
            stateMachine.ActionState = ClientActionStateMachine.State.Chopping_Axe;
            _pickAxe.SetActive(true);
        }

        private Tween _tweenToEat;

        public void Eat()
        {
            _tweenToEat?.Kill();
            stateMachine.ActionState = ClientActionStateMachine.State.Eat;
            DOVirtual.DelayedCall(3, (ShowWaitToRest));
        }

        public void Treating(bool immediately = false)
        {
            if (immediately)
            {
                var delay = (float)(new Random().NextDouble() + 1);
                Invoke(nameof(ShowWaitToRest), delay);
                return;
            }

            StopFollow();

            NavMeshAgent.enabled = false;
            var clientTransform = transform;
            var sleepPositionTransform = _sleepPosition.transform;
            clientTransform.rotation = sleepPositionTransform.rotation;
            EmotionState = ClientEmotionState.Rest;
            DOVirtual.DelayedCall(0.2f, (() =>
            {
                var main = sleepEffect.main;
                main.loop = true;
                sleepEffect.gameObject.SetActive(true);
                stateMachine.ActionState = ClientActionStateMachine.State.Sleep;
            }));

            var position = clientTransform.position;
            var position1 = sleepPositionTransform.position;
            var midPoint = (position + position1) / 2;
            midPoint.y += 0.5f;

            var path = new Vector3[3];
            path[0] = position;
            path[1] = midPoint;
            path[2] = position1;

            var path2 = new Vector3[3];
            path2[0] = position1;
            path2[1] = midPoint;
            path2[2] = position;

            transform.DOPath(path, 0.4f, PathType.CatmullRom)
                .SetEase(Ease.OutQuad);
            var sleepDelay = new Random().Next(6) + 3;
            if (FacilityManager.Instance.GameSaveLoad.GameData.countFirstClient < 3)
            {
                sleepDelay = 2;
            }

            DOVirtual.DelayedCall(sleepDelay, (() =>
            {
                sleepEffect.gameObject.SetActive(false);
                stateMachine.ActionState = ClientActionStateMachine.State.WakeUp;
                DOVirtual.DelayedCall(1.5f, (() =>
                {
                    transform.DOPath(path2, 0.4f, PathType.CatmullRom)
                        .SetEase(Ease.OutQuad).OnComplete(AfterTreating);
                }));
            }));
        }

        private void AfterTreating()
        {
            var delay2 = new Random().NextDouble() * 4 + 1.5f;
            Invoke(nameof(ShowWaitToRest), (float)delay2);
        }

        private void IntoPosition()
        {
            StopFollow();
            NavMeshAgent.enabled = false;
            var transform1 = transform;
            var transform2 = TargetSlot.transform;
            transform1.rotation = transform2.rotation;
            transform1.position = transform2.position;
        }
    }
}