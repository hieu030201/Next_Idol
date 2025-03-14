using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Advertising;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class BaseRoom : GameUnit
    {
        [SerializeField] public bool isFunctionalRoom;
        [SerializeField] public bool isLevelUpRoom;
        [SerializeField] public WarBaseClient.ClientEmotionState emotionStateToGetIn;

        [SerializeField] public int width = 1;
        [SerializeField] public int height = 1;
        [SerializeField] private bool isNoOldRoom;
        [SerializeField] protected List<GameObject> oldRoomsList;
        [SerializeField] protected GameObject clientPrefab;
        [SerializeField] public List<int> outcomes;
        [SerializeField] private GameObject noelDecorPrefab;

        public GameObject CameraPoint { get; private set; }

        public EntryPoint EntryPoint { get; private set; }
        public ExitPoint ExitPoint { get; private set; }
        public BuildPoint BuildPoint { get; private set; }
        private BuildPointLevel2 BuildPointLevel2 { get; set; }
        public CashPoint CashPoint { get; private set; }

        private RepairPoint RepairPoint { get; set; }

        public List<int> levelsPlayerToActive;
        public List<int> prices;
        public List<int> stars;
        public GameObject digPosition;

        [ShowInInspector] protected List<GameObject> BuildElementsList;

        private GameObject _slots;
        private GameObject _waitingSlots;

        private ParticleSystem _buildEffect;

        private HireEmployeePoint _hireEmployeePoint;

        protected RoomStatus Status;

        public HireEmployeePoint HireEmployeePoint { get; set; }

        public List<WarBaseClient> alliesList = new();
        public List<WarBaseClient> reinforceAlliesList = new();

        [Button]
        public void RepositionByUnit(float unit = 3.5f)
        {
            var position = transform.position;
            var newXPos = Mathf.Round(position.x / unit) * unit;
            var newZPos = Mathf.Round(position.z / unit) * unit;
            position = new Vector3(newXPos, position.y, newZPos);
            transform.position = position;
        }

        public int TotalHp { get; protected set; }
        public int AreaNumber { get; set; }

        private int _id;

        public int Id
        {
            set
            {
                _id = value;
                if (transform.Find("Id_Txt"))
                {
                    transform.Find("Id_Txt").GetComponent<TextMeshPro>().text = value.ToString();
                    transform.Find("Id_Txt").gameObject.SetActive(false);
                }
            }
            get => _id;
        }

        public int CountSlots => slotsList.Count + waitingSlotsList.Count;
        public int CountMainSlots => slotsList.Count;

        public bool IsBuilt { get; protected set; }

        public bool IsUnlocked { get; protected set; }

        public int Level { get; private set; } = 1;

        public string nextRoom;
        public string nextRoom2;
        public string nextWorker;

        public void SetConfig(RoomDataConfig config)
        {
            levelsPlayerToActive = config.levelsPlayerToActive;
            prices = config.prices;
            if (config.stars.Count > 0)
            {
                stars = config.stars;
            }
            else
            {
                stars = new List<int>() { 0 };
                if (GetComponent<BedRoom>())
                    stars = new List<int>() { 1, 2 };
                // Debug.Log("No stars selected: " + gameObject.name);
            }

            nextRoom = config.nextRoom;
            nextRoom2 = config.nextRoom2;
            nextWorker = config.nextWorker;
        }

        private int _repairPrice;

        public virtual void OnClientMoveToSlotCompleted(Client client)
        {
        }

        public List<WarBaseClient> ClientsList { get; private set; }

        public virtual void AddClient(WarBaseClient client, bool immediately = false)
        {
            if (!immediately)
            {
                client.DoneList.TryAdd(emotionStateToGetIn, 0);
                client.DoneList[emotionStateToGetIn]++;
            }

            var lastRoom = client.LastRoom;
            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] != null) continue;
                ClientsList[i] = client;
                client.MoveToSlot(slotsList[i], immediately);
                client.LastRoom = this;
                if (immediately) return;
                // Debug.Log("UpdateRoomsToSaveData 2");
                UpdateRoomsToSaveData(new List<BaseRoom> { this, lastRoom });
                // if (IsFull && entryPoint)
                //     entryPoint.IsActive = false;
                return;
            }

            if (IsFull)
            {
                // Debug.Log(gameObject.name + " IsFull, " + waitingSlotsList.Count);
                return;
            }

            client.LastRoom = this;
            if (ClientsList.Count < slotsList.Count)
            {
                ClientsList.Add(client);
                client.MoveToSlot(
                    ClientsList.Count <= slotsList.Count ? slotsList[ClientsList.Count - 1] : slotsList[^1],
                    immediately);
            }
            else if (waitingSlotsList.Count > 0)
            {
                client.HideConnectAnim();
                AddWaitingClient(client, immediately);
            }

            if (!immediately)
            {
                // Debug.Log("UpdateRoomsToSaveData 3");
                UpdateRoomsToSaveData(new List<BaseRoom> { this, lastRoom });
            }
        }

        public virtual void RemoveClient(WarBaseClient client, bool isKeepPosition = false, float delay = 0)
        {
            // if (entryPoint)
            //     entryPoint.IsActive = true;
            if (!IsDamagedRoom)
            {
                if (outcomes is { Count: > 0 })
                {
                    // Debug.Log("RemoveClient: " + outcomes.Count + ", " + Level);
                    if (outcomes[Level - 1] != 0)
                    {
                        SpawnMoney(outcomes[Level - 1], client.transform.position);
                    }
                }
            }

            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] != client) continue;
                if (isKeepPosition)
                {
                    ClientsList[i] = null;
                }
                else
                {
                    ClientsList.RemoveAt(i);
                    for (var j = 0; j < ClientsList.Count; j++)
                    {
                        ClientsList[j].MoveToSlot(slotsList[j]);
                    }
                }

                FacilityManager.Instance.CheckActivePointWhenRoomRemoveClient(this);

                return;
            }

            ShowErrorLog("CLIENT NOT EXIST !!!");
        }

        public virtual void RemoveAllClients()
        {
            WaitingClientsList = new List<WarBaseClient>();
            ClientsList = new List<WarBaseClient>();
            reinforceAlliesList = new List<WarBaseClient>();
            TotalHp = 0;

            UpdateRoomsToSaveData(new List<BaseRoom> { this });
        }

        public List<WarBaseClient> WaitingClientsList { get; private set; }

        protected virtual void AddWaitingClient(WarBaseClient client, bool immediately = false)
        {
            // Debug.Log("AddWaitingClient: " + gameObject.name + ", " + immediately);
            for (var i = 0; i < WaitingClientsList.Count; i++)
            {
                if (WaitingClientsList[i] != null) continue;
                WaitingClientsList[i] = client;
                client.MoveToSlot(waitingSlotsList[i], immediately);
                return;
            }

            WaitingClientsList.Add(client);
            var index = WaitingClientsList.Count - 1;
            client.MoveToSlot(waitingSlotsList[index], immediately);
        }

        protected void RemoveWaitingClient(Client client, bool isKeepPosition = false)
        {
            for (var i = 0; i < WaitingClientsList.Count; i++)
            {
                if (WaitingClientsList[i] != client) continue;

                if (isKeepPosition)
                {
                    WaitingClientsList[i] = null;
                }
                else
                {
                    WaitingClientsList.RemoveAt(i);
                    for (var j = 0; j < WaitingClientsList.Count; j++)
                    {
                        WaitingClientsList[j].MoveToSlot(waitingSlotsList[j]);
                    }
                }

                return;
            }
        }

        public virtual WarBaseClient GetFirstClient()
        {
            return ClientsList.Count > 0 ? ClientsList[0] : null;
        }

        public List<Transform> waitingSlotsList;
        public List<Transform> slotsList;

        private GameObject _decorContainer;
        protected Quaternion StartStatusRotation;
        protected GameObject NoelDecor;

        public virtual void Init()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<BuildPointLevel2>())
                    BuildPointLevel2 = transform.GetChild(i).GetComponent<BuildPointLevel2>();
                else if (transform.GetChild(i).GetComponent<BuildPoint>())
                    BuildPoint = transform.GetChild(i).GetComponent<BuildPoint>();

                if (transform.GetChild(i).GetComponent<EntryPoint>())
                    EntryPoint = transform.GetChild(i).GetComponent<EntryPoint>();

                if (transform.GetChild(i).GetComponent<ExitPoint>())
                    ExitPoint = transform.GetChild(i).GetComponent<ExitPoint>();

                if (transform.GetChild(i).GetComponent<CashPoint>())
                    CashPoint = transform.GetChild(i).GetComponent<CashPoint>();

                if (transform.GetChild(i).GetComponent<RepairPoint>())
                {
                    RepairPoint = transform.GetChild(i).GetComponent<RepairPoint>();
                    HideRepairPoint();
                }
            }

            BuildElementsList = new List<GameObject>();
            /*if (EntryPoint)
            {
                if (GetComponent<BedRoom>())
                    EntryPoint.gameObject.SetActive(false);
                else
                    BuildElementsList.Add(EntryPoint.gameObject);
            }*/

            /*if (ExitPoint)
                BuildElementsList.Add(ExitPoint.gameObject);*/

            if (transform.Find("Camera_Point"))
                CameraPoint = transform.Find("Camera_Point").gameObject;

            if (transform.Find("Colliders"))
                transform.Find("Colliders").gameObject.SetActive(true);

            stars = new List<int>() { 1, 2 };

            ClientsList = new List<WarBaseClient>();
            WaitingClientsList = new List<WarBaseClient>();

            slotsList = new List<Transform>();
            if (transform.Find("Slots"))
            {
                for (var i = 0; i < transform.Find("Slots").childCount; i++)
                {
                    slotsList.Add(transform.Find("Slots").GetChild(i));
                }
            }

            waitingSlotsList = new List<Transform>();
            if (transform.Find("Waiting_Slots"))
            {
                for (var i = 0; i < transform.Find("Waiting_Slots").childCount; i++)
                {
                    waitingSlotsList.Add(transform.Find("Waiting_Slots").GetChild(i));
                }
            }

            if (transform.Find("Builder_square"))
            {
                _buildEffect = transform.Find("Builder_square").GetComponent<ParticleSystem>();
                _buildEffect.gameObject.SetActive(false);
            }

            if (BuildElementsList != null)
            {
                var buildElements = transform.Find("Build_Elements");
                if (buildElements)
                {
                    foreach (Transform child in buildElements)
                    {
                        BuildElementsList.Add(child.gameObject);
                    }
                }
            }

            if (transform.Find("Dig_Position"))
                digPosition = transform.Find("Dig_Position").gameObject;

            HideBuildPoint();

            if (transform.Find("Decor"))
                _decorContainer = transform.Find("Decor").gameObject;

            // Debug.Log(name);
            if (_decorContainer)
            {
                _decorContainer.gameObject.SetActive(false);
                if (FireBaseManager.Instance.showNoel)
                {
                    // Debug.Log("showNoel: " + name + ", " + noelDecorPrefab);
                    if (noelDecorPrefab)
                    {
                        NoelDecor = Instantiate(noelDecorPrefab, _decorContainer.transform);
                    }
                }
            }

            if (CameraPoint && BuildPoint)
                CameraPoint.transform.position = new Vector3(BuildPoint.transform.position.x,
                    CameraPoint.transform.position.y, BuildPoint.transform.position.z - 10);

            /*if (cashPoint)
            {
                cashPoint.transform.eulerAngles = Vector3.zero;
            }*/

            if (BuildPoint)
            {
                BuildPoint.transform.eulerAngles = Vector3.zero;
            }

            if (BuildPointLevel2)
            {
                BuildPointLevel2.transform.eulerAngles = Vector3.zero;
            }

            if (EntryPoint)
            {
                EntryPoint.transform.eulerAngles = Vector3.zero;
            }

            if (RepairPoint)
            {
                RepairPoint.transform.eulerAngles = Vector3.zero;
            }

            if (transform.Find("Status_Container"))
                transform.Find("Status_Container").transform.eulerAngles = Vector3.zero;

            if (BuildPoint)
            {
                BuildPoint.Price = prices[0];
            }

            if (BuildPointLevel2)
            {
                BuildPointLevel2.Price = prices[1];
            }
        }

        public bool IsConnecting { get; private set; }

        public void ShowConnectAnim(float duration)
        {
            Status.ShowConnectAnimation(duration);
            IsConnecting = true;
        }

        public void HideConnectAnim()
        {
            Status.HideConnectAnimation();
            IsConnecting = false;
        }

        public int CountClients()
        {
            return ClientsList.Count(t => t != null) + WaitingClientsList.Count(t => t != null);
        }

        public bool IsFull => CountClients() >= slotsList.Count + waitingSlotsList.Count;

        protected List<Vector3> SavePositionsList;

        public virtual void Hide(bool isUpdateNavmesh = true)
        {
            var isOldRoomNotExist = false;
            if (oldRoomsList == null)
            {
                isOldRoomNotExist = true;
            }
            else
            {
                if (oldRoomsList.Count == 0)
                    isOldRoomNotExist = true;
                foreach (var oldRoom in oldRoomsList)
                {
                    if (!oldRoom)
                        isOldRoomNotExist = true;
                }
            }

            if (isOldRoomNotExist && !isNoOldRoom)
            {
                Debug.LogWarning(name + " WITHOUT OLD ROOM");
            }

            if (EntryPoint)
                EntryPoint.gameObject.SetActive(false);

            if (ExitPoint)
                ExitPoint.gameObject.SetActive(false);

            SavePositionsList = new List<Vector3>();
            foreach (var buildElement in BuildElementsList)
            {
                if (!buildElement) continue;
                var position = buildElement.transform.position;
                SavePositionsList.Add(position);
                position += new Vector3(0, -3f, 0);
                buildElement.transform.position = position;
                buildElement.SetActive(false);
            }

            if (isUpdateNavmesh)
                FacilityManager.Instance.UpdateNavMeshByRoomUpdated(gameObject, true);
        }

        public int GetDepositLeft()
        {
            if (IsBuilt)
            {
                return prices[Level] - _currentDeposit;
            }

            return prices[Level - 1] - _currentDeposit;
        }

        public int GetDepositLeftToRepair()
        {
            return _repairPrice - _currentDeposit;
        }

        public bool IsMoneyEnoughToBuild
        {
            get
            {
                if (IsBuilt)
                    return _currentDeposit >= prices[Level];
                return _currentDeposit >= prices[Level - 1];
            }
        }

        public bool IsMoneyEnoughToRepair => _currentDeposit >= _repairPrice;

        private int _currentDeposit;

        public void DepositToBuild(int money)
        {
            _currentDeposit += money;
            if (IsBuilt)
            {
                BuildPointLevel2.ShowSpendMoneyAnimation();
                BuildPointLevel2.DepositNumber += money;
            }
            else
            {
                BuildPoint.ShowSpendMoneyAnimation();
                BuildPoint.DepositNumber += money;
            }
        }

        public void DepositToRepair(int money)
        {
            _currentDeposit += money;
            RepairPoint.ShowSpendMoneyAnimation();
            RepairPoint.DepositNumber += money;
        }

        public virtual void LevelUp(int level)
        {
            Level = level;
        }

        public int Type { get; private set; }

        public virtual void SetType(int type, bool isImmediately = false)
        {
            Type = type;
        }

        // Biến isBuildImmediately là dùng cho trường hợp load room từ save data khi vào game lần đầu
        public virtual void StartBuild(bool isBuildImmediately = false)
        {
            if (EntryPoint)
            {
                EntryPoint.IsActive = false;
                EntryPoint.gameObject.SetActive(true);
            }

            if (ExitPoint)
                ExitPoint.gameObject.SetActive(true);

            if (_decorContainer)
                _decorContainer.gameObject.SetActive(true);
            var nextRoomsList = new List<string>();
            if (nextRoom != "")
                nextRoomsList.Add(nextRoom);
            if (nextRoom2 != "")
                nextRoomsList.Add(nextRoom2);
            if (!isBuildImmediately)
            {
                if (isFunctionalRoom)
                    FacilityManager.Instance.GameSaveLoad.GameData.CountClientActionDictionary.TryAdd(
                        emotionStateToGetIn,
                        0);
                FacilityManager.Instance.SaveRoomBuild(this);
            }

            if (HireEmployeePoint)
                HireEmployeePoint.gameObject.SetActive(true);

            if (_buildEffect && !isBuildImmediately)
            {
                _buildEffect.gameObject.SetActive(true);
                _buildEffect.Play();
                DOVirtual.DelayedCall(1.5f, (() => { _buildEffect.gameObject.SetActive(false); }));
            }

            HideBuildPoint();
            var isUpgradeRoom = IsBuilt;
            // Nếu room này đã được build trước đó rồi thì tức là đây là Upgrade Room
            if (IsBuilt && !isBuildImmediately)
                Level++;
            IsBuilt = true;
            if (!isBuildImmediately)
            {
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Building_MGB);
                UpdateRoomsToSaveData(new List<BaseRoom> { this });
                if (gameObject.name != "Boxing_Room_1")
                {
                    if (GetComponent<BoxingRoom>())
                        FacilityManager.Instance.isWaitingToShowInterAds = true;
                    else if (GetComponent<TrainingRoom>())
                        FacilityManager.Instance.isWaitingToShowInterAds = true;
                    else if (GetComponent<DiningRoom>())
                        FacilityManager.Instance.isWaitingToShowInterAds = true;
                }

                if (gameObject.name == "Bed_Room_2")
                {
                    FacilityManager.Instance.PlayerInfoUI.SetActiveShopObj(true);
                    FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveShop = true;
                    FacilityManager.Instance.GameSaveLoad.OrderToSaveData();
                }

                if (gameObject.name == "Bed_Room_3")
                {
                    FacilityManager.Instance.PlayerInfoUI.SetActiveSkinIconObj(true);
                    //FacilityManager.Instance.PlayerInfoUI.ShowSkinRequestAnimation();
                    FacilityManager.Instance.GameSaveLoad.StableGameData.isActiveSkinShop = true;
                    FacilityManager.Instance.GameSaveLoad.OrderToSaveData();
                }
            }

            if (oldRoomsList != null)
            {
                foreach (var oldRoom in oldRoomsList.Where(oldRoom => oldRoom))
                {
                    oldRoom.SetActive(false);
                }
            }

            if (isBuildImmediately)
            {
                foreach (var be in BuildElementsList.Where(be => be))
                {
                    be.SetActive(true);
                }

                if (!isUpgradeRoom)
                {
                    FacilityManager.Instance.OnShowNextRoom(nextRoomsList, nextWorker, isBuildImmediately);
                }

                return;
            }

            var starCount = stars[Level - 1];
            if (Type is 2 or 5)
                starCount *= 2;
            for (var i = 0; i < starCount; i++)
            {
                DOVirtual.DelayedCall(0.5f * i, (() => FacilityManager.Instance.IncreaseLevelProgress(1)));
            }

            var levelRequirements = FacilityManager.Instance.LevelConfig.LevelRequirements;
            var totalProgress = levelRequirements[FacilityManager.Instance.IdleGameData.Level - 1].totalProgress;
            var isLevelUp = (FacilityManager.Instance.IdleGameData.LevelProgress + starCount >= totalProgress);

            if (!isLevelUp)
            {
                if (!isUpgradeRoom)
                {
                    FacilityManager.Instance.OnShowNextRoom(nextRoomsList, nextWorker, isBuildImmediately);
                }
            }

            for (var i = 0; i < BuildElementsList.Count; i++)
            {
                StartCoroutine(BuildOneElement(i * 0.2f, SavePositionsList[i], BuildElementsList[i]));
            }

            DOVirtual.DelayedCall(3, (() =>
            {
                if (GetComponent<NavMeshModifier>())
                    FacilityManager.Instance.UpdateNavMeshByRoomUpdated(gameObject, false);
            }));
        }

        private static IEnumerator BuildOneElement(float delayTime, Vector3 position, GameObject element)
        {
            yield return new WaitForSeconds(delayTime);
            if (!element) yield break;
            element.SetActive(true);
            element.transform.DOMove(position, 0.4f).SetEase(Ease.OutBack);
        }

        protected RoomData RoomData;

        public virtual void UpdateRoomBySaveData(RoomData roomData)
        {
            RoomData = roomData;
            Level = roomData.Level;
            SetType(roomData.Type, true);
            StartBuild(true);

            foreach (var clientData in roomData.ClientsList)
            {
                // var clientObject = Instantiate(clientPrefab, FacilityManager.Instance.ClientsContainer.transform);
                var clientObject = YunObjectPool.Instance.GetObject(clientPrefab);
                var client = clientObject.GetComponent<WarBaseClient>();
                client.SetData(clientData);

                client.EmotionState = WarBaseClient.ClientEmotionState.Completed;
                AddClient(client, true);

                if (!GetComponent<CheckInRoom>() && !GetComponent<BattleRoom>())
                    FacilityManager.Instance.AddClientToFacility(client);
            }

            foreach (var clientData in roomData.ReinforcesList)
            {
                // var clientObject = Instantiate(clientPrefab, FacilityManager.Instance.ClientsContainer.transform);
                var clientObject = YunObjectPool.Instance.GetObject(clientPrefab);
                var client = clientObject.GetComponent<WarBaseClient>();
                client.SetData(clientData);
                AddClient(client, true);
            }

            foreach (var clientData in roomData.WaitingClientsList)
            {
                // var clientObject = Instantiate(clientPrefab, FacilityManager.Instance.ClientsContainer.transform);
                var clientObject = YunObjectPool.Instance.GetObject(clientPrefab);
                var client = clientObject.GetComponent<WarBaseClient>();
                client.SetData(clientData);

                client.LastRoom = this;
                AddWaitingClient(client, true);

                if (!GetComponent<CheckInRoom>())
                    FacilityManager.Instance.AddClientToFacility(client);
            }

            if (roomData.Cash != 0)
                CashPoint.SpawnMoneyImmediate(roomData.Cash);

            if (roomData.IsDamaged)
            {
                ShowRepairPoint();
                HideBuildPoint();
                ActiveDamagedRoom(true);
            }
        }

        protected bool _isShowedBuildPointLevel2;

        public void ShowBuildPoint()
        {
            if (IsDamagedRoom)
                return;
            _currentDeposit = 0;
            if (IsBuilt && BuildPointLevel2)
            {
                if (Level < 2)
                {
                    BuildPointLevel2.Show();
                    _isShowedBuildPointLevel2 = true;
                }
            }
            else if (!IsBuilt && BuildPoint)
            {
                if (BuildPointLevel2)
                    BuildPointLevel2.Hide();
                BuildPoint.Show();
            }
        }

        public bool IsBuildable
        {
            get
            {
                if (!IsBuilt)
                    return false;
                if (BuildPointLevel2 && BuildPointLevel2.gameObject.activeSelf)
                    return true;
                if (BuildPoint && BuildPoint.gameObject.activeSelf)
                    return true;
                return false;
            }
        }

        public virtual void HideBuildPoint()
        {
            if (BuildPoint)
                BuildPoint.Hide();
            if (BuildPointLevel2)
                BuildPointLevel2.Hide();
        }

        private void ShowRepairPoint()
        {
            _currentDeposit = 0;
            RepairPoint.Show();
        }

        private void HideRepairPoint()
        {
            RepairPoint.Hide();
        }

        public void SpawnMoney(int money, Vector3 startPosition)
        {
            CashPoint.SpawnMoney(money, startPosition);
        }

        public int WithDrawMoney(GameObject player)
        {
            return CashPoint.WithdrawMoney(player);
        }

        public int GetTotalCash()
        {
            return !CashPoint ? 0 : CashPoint.TotalCash;
        }

        public void ActiveEntryPoint()
        {
            if (IsFull)
                return;
            if (EntryPoint)
            {
                EntryPoint.IsActive = true;
            }
        }

        public void DeactivateEntryPoint()
        {
            if (EntryPoint)
            {
                // entryPoint.Deactivate();
                EntryPoint.IsActive = false;
            }
        }

        private Tween _tweenToShowNavigation;

        protected void UpdateRoomsToSaveData(List<BaseRoom> baseRooms)
        {
            FacilityManager.Instance.UpdateRoomsToSaveData(baseRooms);
        }

        public bool IsDamagedRoom { get; private set; }

        public virtual void ActiveDamagedRoom(bool isImmediately = false)
        {
            IsDamagedRoom = true;
            DeactivateEntryPoint();
            for (var i = ClientsList.Count - 1; i >= 0; i--)
            {
                var client = ClientsList[i];
                if (!client) continue;
                RemoveClient(client);
                FacilityManager.Instance.RemoveClientFromFacility(client);
                // Destroy(client.gameObject);
                if (i == 0)
                    client.FinishDesert();
                else
                    DOVirtual.DelayedCall(i * 0.3f, client.FinishDesert)
                        .SetAutoKill(true);
            }

            _repairPrice = FacilityManager.Instance.LevelConfig
                .LevelRequirements[FacilityManager.Instance.IdleGameData.Level - 1].AdsReward * 2;
            RepairPoint.Price = _repairPrice;
            RepairPoint.DepositNumber = 0;

            ShowRepairPoint();
            HideBuildPoint();

            UpdateRoomsToSaveData(new List<BaseRoom> { this });
        }

        public virtual void RepairDamagedRoom()
        {
            if (_buildEffect)
            {
                _buildEffect.gameObject.SetActive(true);
                _buildEffect.Play();
                DOVirtual.DelayedCall(1.5f, (() => { _buildEffect.gameObject.SetActive(false); }));
            }

            IsDamagedRoom = false;
            HideRepairPoint();
            if (_isShowedBuildPointLevel2)
                ShowBuildPoint();

            UpdateRoomsToSaveData(new List<BaseRoom> { this });
        }
    }
}