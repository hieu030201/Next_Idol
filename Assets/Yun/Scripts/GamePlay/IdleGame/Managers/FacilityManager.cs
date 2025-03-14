using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Adverstising_Integration.Scripts;
using Advertising;
using DG.Tweening;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.Ads;
using Yun.Scripts.Audios;
using Yun.Scripts.Core;
using Yun.Scripts.Datas;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Employees;
using Yun.Scripts.GamePlay.IdleGame.Intro;
using Yun.Scripts.GamePlay.IdleGame.Logics;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Yun.Scripts.GamePlay.IdleGame.Vip_Soldiers;
using Yun.Scripts.Managers;
using Yun.Scripts.Notifications;
using Yun.Scripts.UI;
using Yun.Scripts.UI.GamePlay.IdleGame;
using Yun.Scripts.UI.GamePlay.IdleGame.AbilityItemCs;

namespace Yun.Scripts.GamePlay.IdleGame.Managers
{
    public class FacilityManager : MonoSingleton<FacilityManager>
    {
        [SerializeField] public IdlePlayer player;
        [SerializeField] private VipSoldier vipSoldier;
        [SerializeField] private NavMeshSurface navMeshSurface;

        [SerializeField] private List<NavMeshData> navMeshDataList;
        [SerializeField] private List<GameObject> outsideList;

        [FoldoutGroup("Point")] [SerializeField]
        private GameObject spawnClientPoint,
            startPoint,
            getVipSoldierPoint,
            getUpgradeWorkerPoint;

        [FoldoutGroup("Prefab")] [SerializeField]
        private GameObject clientPrefab,
            clientsContainer,
            employeesContainer,
            bedroomStaffPrefab,
            boxingStaffPrefab,
            trainingStaffPrefab,
            diningStaffPrefab;

        [FoldoutGroup("initRoom")] [SerializeField]
        private CheckInRoom checkInRoom;

        [FoldoutGroup("initRoom")] [SerializeField]
        public WaitingRoom waitingRoom;

        [FoldoutGroup("initRoom")] [SerializeField]
        public BedRoom bedRoom;

        [FoldoutGroup("initRoom")] [SerializeField]
        public UpgradeRoom upgradeRoom;

        [FoldoutGroup("initRoom")] [SerializeField]
        public DiningRoom diningRoom;

        [FoldoutGroup("initRoom")] [SerializeField]
        public BoxingRoom boxingRoom;

        [FoldoutGroup("initRoom")] [SerializeField]
        public BattleRoom battleRoom1;

        [FoldoutGroup("Config")] [SerializeField]
        private PlayerConfig playerConfig;

        [FoldoutGroup("Config")] [SerializeField]
        public WorkerConfig workerConfig;

        [FoldoutGroup("Config")] [SerializeField]
        public IAPConfig iapConfig;

        [FoldoutGroup("Config")] [SerializeField]
        public TestGameConfig testGameConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private LevelConfig levelConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private RoomConfig roomConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private BattleVehiclesConfig battleVehiclesConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private BattleConfig battleConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private HireEmployeeConfig hireEmployeeConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private DailyGiftConfig dailyGiftConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private DailyQuestConfig dailyQuestConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private SkinDataCollection skinDataCollection;

        [FoldoutGroup("Config")] [SerializeField]
        private WeaponDataCollection weaponDataCollection;

        [FoldoutGroup("Config")] [SerializeField]
        private BattlePassDataCollection battlePassDataCollection;

        [FoldoutGroup("Config")] [SerializeField]
        private BattlePassLevelConfig battlePassLevelConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private BattleFundConfig battleFundConfig;

        [FoldoutGroup("Config")] [SerializeField]
        private SpinWheelDataCollection spinWheelDataCollection;

        [FoldoutGroup("Config")] [SerializeField]
        private AbilitiesDataCollection abilitiesDataCollection;

        [FoldoutGroup("Manager")] [SerializeField]
        public MonetizationPointsManager monetizationPointsManager;

        [FoldoutGroup("Manager")] [SerializeField]
        private CamerasManager camerasManager;

        [FoldoutGroup("Manager")] [SerializeField]
        private IntroManager introManager;

        [FoldoutGroup("Manager")] [SerializeField]
        private GuidesManager guidesManager;

        [FoldoutGroup("Manager")] [SerializeField]
        private QuestManager questManager;

        [FoldoutGroup("Manager")] [SerializeField]
        private BattleManager battleManager;

        [FoldoutGroup("UI_Model_Renderer")] [SerializeField]
        private Model3DRenderer bedRoomRenderer,
            vipSoldierRenderer,
            upgradeWorkerRenderer,
            speedBoosterRenderer,
            workerSpeedBoosterRenderer,
            moneyRenderer,
            playerRenderer;

        [FoldoutGroup("UI_Model_Renderer")] [SerializeField]
        private List<GameObject> bedRoomModelsList;

        [FoldoutGroup("UI_Model_Renderer")] [SerializeField]
        private GameObject skinPlayerCharacter;

        [FoldoutGroup("UI_Model_Renderer")] [SerializeField]
        private GameObject skinPlayerCharacterSprunky;

        [SerializeField] private IdlePlayerActionStateMachine skinPlayerStateMachine;

        [SerializeField] private GameObject gateNoel;
        [SerializeField] private GameObject fps;

        public GuidesManager GuidesManager => guidesManager;
        public QuestManager QuestManager => questManager;
        public DailyGiftConfig DailyGiftConfig => dailyGiftConfig;
        public DailyQuestConfig DailyQuestConfig => dailyQuestConfig;
        public PlayerConfig PlayerConfig => playerConfig;
        public WorkerConfig WorkerConfig => workerConfig;
        public BattleManager BattleManager => battleManager;

        public BattleVehiclesConfig BattleVehiclesConfig => battleVehiclesConfig;
        public SkinDataCollection SkinDataCollection => skinDataCollection;
        public WeaponDataCollection WeaponDataCollection => weaponDataCollection;
        public BattlePassDataCollection BattlePassDataCollection => battlePassDataCollection;
        public BattlePassLevelConfig BattlePassLevelConfig => battlePassLevelConfig;
        public BattleFundConfig BattleFundConfig => battleFundConfig;
        public SpinWheelDataCollection SpinWheelDataCollection => spinWheelDataCollection;
        public AbilitiesDataCollection AbilitiesDataCollection => abilitiesDataCollection;
        public GameObject ClientsContainer => clientsContainer;
        public CamerasManager CamerasManager => camerasManager;

        public LevelConfig LevelConfig => levelConfig;

        private List<GameObject> _roomsAreaList;

        private List<HireEmployeePoint> _hireEmployeePointsList;

        public List<Employee> EmployeesList { get; private set; }

        public List<WarBaseClient> ClientsList { get; private set; }

        public bool isWaitingToShowInterAds;

        [HideInInspector] public bool isShowBattleFundFromSkin;

        public bool isWeakDevice;

        public void AddEmployee(HireEmployeePoint point)
        {
            // Nếu mua nhân viên check in lúc đang có mũi tên hướng dẫn vào nút Check In thì ẩn mũi tên đi
            if (point.name == "Hire_Check_In_Employee_Point")
            {
                player.StopGuideArrowToPoint(checkInRoom.ExitPoint.GetComponent<GuidePoint>());
            }

            EmployeesList.Add(point.employee);
            GameSaveLoad.UpdateHireEmployee(point);

            if (EmployeesList.Count >= 3)
            {
                ShowWorkerSpeedBoosterPoint();
            }
        }

        public void AddClientToFacility(WarBaseClient client)
        {
            ClientsList.Add(client);
            CheckNoBed();
        }

        public void CheckNoBed()
        {
            // Debug.Log("CheckNoBed");
            checkInRoom.CheckNoBed();
        }

        public void RemoveClientFromFacility(Client client)
        {
            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] != client) continue;
                ClientsList.RemoveAt(i);
                CheckNoBed();
                return;
            }

            ShowErrorLog("CLIENT NOT EXIST !!!");
        }

        public void ReturnClientToPool(GameObject client)
        {
            YunObjectPool.Instance.ReturnObject(clientPrefab, client);
        }

        public void MoveClientFromCheckInToWaitingRoom()
        {
            // Debug.Log("MoveClientFromCheckInToWaitingRoom");
            if (CheckFacilityFullClient())
                return;

            if (waitingRoom.IsFull)
                return;

            var client = checkInRoom.GetFirstClient();
            if (client)
            {
                checkInRoom.RemoveFirstClient();
                checkInRoom.ActiveHealthBar();
                checkInRoom.ShowGreenLight();
                DOVirtual.DelayedCall(1f, (() =>
                {
                    checkInRoom.RemoveClient(client);
                    AddClientToFacility(client);
                    AddClientToRoom(waitingRoom, client);
                    SpawnNewClient();
                })).SetAutoKill(true);
            }
        }

        // Kiểm tra xem toàn cơ sở đã đầy khách chưa
        public bool CheckFacilityFullClient()
        {
            var totalSlotsInFacility =
                BedRoomsList.Where(room => room.IsBuilt && !room.IsDamagedRoom).Sum(room => room.CountSlots);

            return ClientsList.Count >= totalSlotsInFacility;
        }

        public IdleGameData IdleGameData { get; private set; }
        public SettingGameData SettingGameData { get; private set; }

        public int MapIndex { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            if (fps)
                fps.SetActive(false);
            
            YunObjectPool.Instance.CreatePool(clientPrefab, 10, clientsContainer.transform);

            MapIndex = PlayerPrefs.GetInt("mapIndex", 1);

            moneyRenderer.gameObject.SetActive(false);
            playerRenderer.gameObject.SetActive(false);
            workerSpeedBoosterRenderer.gameObject.SetActive(false);
            bedRoomRenderer.gameObject.SetActive(false);
            speedBoosterRenderer.gameObject.SetActive(false);
            vipSoldierRenderer.gameObject.SetActive(false);
            upgradeWorkerRenderer.gameObject.SetActive(false);
            vipSoldier.gameObject.SetActive(false);
            getVipSoldierPoint.gameObject.SetActive(false);
            getVipSoldierPoint.GetComponent<GetVipSoldierPoint>().IsActive = false;
            getUpgradeWorkerPoint.GetComponent<GetUpgradeWorkerPoint>().IsActive = false;

            foreach (var outside in outsideList)
            {
                outside.gameObject.SetActive(false);
            }

            outsideList[0].gameObject.SetActive(true);

            _roomsAreaList = new List<GameObject>();
            battleManager.BattleRoomsList = new List<BattleRoom>();
            for (var i = 0; i < transform.Find("Areas_Container").childCount; i++)
            {
                var area = transform.Find("Areas_Container").GetChild(i);
                _roomsAreaList.Add(area.Find("Function_Rooms_Container").gameObject);
                BattleRoom battleRoom = null;
                for (var j = area.Find("Function_Rooms_Container").childCount - 1; j >= 0; j--)
                {
                    if (!area.Find("Function_Rooms_Container").GetChild(j).GetComponent<BattleRoom>()) continue;
                    battleRoom = area.Find("Function_Rooms_Container").GetChild(j).GetComponent<BattleRoom>();
                    break;
                }

                if (battleRoom != null)
                {
                    battleRoom.gameObject.SetActive(false);
                    battleRoom.OutSide = outsideList[i];
                    if (i + 1 < outsideList.Count)
                        battleRoom.NextOutSide = outsideList[i + 1];
                    if (!outsideList[i])
                        Debug.LogError("MISSING OUTSIDE: " + (i + 1));

                    if (navMeshDataList.ElementAtOrDefault(i))
                        battleRoom.NextNavMeshData = navMeshDataList[i];
                    else if (!battleRoom.isLastBattle)
                        Debug.LogError("MISSING NAVMESH DATA: " + (i + 1));

                    battleRoom.Id = i + 1;
                    battleManager.BattleRoomsList.Add(battleRoom);
                }
                else
                {
                    Debug.LogError("MISSING BATTLE IN AREA: " + (i + 1));
                }
            }

            _hireEmployeePointsList = new List<HireEmployeePoint>();
            for (var i = 0; i < transform.Find("Hire_Employee_Points").childCount; i++)
            {
                var point = transform.Find("Hire_Employee_Points").GetChild(i).GetComponent<HireEmployeePoint>();
                if (!point) continue;
                _hireEmployeePointsList.Add(point);
                point.Init();
                point.gameObject.SetActive(false);
                foreach (var config in
                         hireEmployeeConfig.hireEmployeeConfigs.Where(config => point.name == config.name))
                {
                    point.Price = config.price;
                }
            }

            ClientsList = new List<WarBaseClient>();
            EmployeesList = new List<Employee>();
            SettingGameData = new SettingGameData();
            SettingGameData.Init();

            PlayBackgroundMusic();

            if (gateNoel)
                gateNoel.gameObject.SetActive(false);
            if (gateNoel && FireBaseManager.Instance.showNoel)
            {
                gateNoel.gameObject.SetActive(true);
            }
        }

        private RoomsManager _roomsManager;

        public IdleGamePlayerInfoUI PlayerInfoUI { get; private set; }

        public GameObject BattlePassPopupUI { get; private set; }

        private IntroUI _introUI;

        public List<BedRoom> BedRoomsList { get; private set; }

        private Tween _delayToShowIntroTween;

        private void Start()
        {
            Application.targetFrameRate = 60;

            DOVirtual.DelayedCall(1, (() =>
            {
                PlayerInfoUI = CanvasManager.Instance.ShowUI(UIName.Name.IDLE_GAME_PLAYER_INFO_UI)
                    .GetComponent<IdleGamePlayerInfoUI>();
                PlayerInfoUI.SettingButtonClick = ShowSettingPopup;
                PlayerInfoUI.gameObject.SetActive(false);

                BattlePassPopupUI = CanvasManager.Instance.GetPopup(UIName.Name.BATTLE_PASS_POPUP);

                _introUI = CanvasManager.Instance.ShowUI(UIName.Name.INTRO_UI)
                    .GetComponent<IntroUI>();

                Init();

                if (testGameConfig.IsSkipIntro)
                {
                    introManager.SkipIntro();
                }
                else
                {
                    if (GameSaveLoad.GameData
                            .isFirstTimePlaying && MapIndex == 1)
                    {
                        _introUI.StartShow();
                    }
                    else
                    {
                        introManager.SkipIntro();
                        _introUI.HideNativeAds();
                    }
                }
            }));
        }

        public void OnUserCloseConsentPopup()
        {
            _delayToShowIntroTween?.Kill();
            if (GameSaveLoad.GameData
                .isFirstTimePlaying)
                _introUI.StartShow();
        }

        public void StartIntro()
        {
            introManager.StartIntro();
        }

        public void OnIntroFinish()
        {
            StartGame();
        }

        private void ShowSettingPopup()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.SETTING_POPUP);
        }

        public void ShowShopPopup()
        {
            var popup = CanvasManager.Instance.GetPopup(UIName.Name.SHOP_POPUP);
            if (popup)
                return;
            CanvasManager.Instance.ShowPopup(UIName.Name.SHOP_POPUP);
            PlayerInfoUI.ShowShopBg();
        }

        public void ShowBuyNoAdsPopup()
        {
            var popup = CanvasManager.Instance.GetPopup(UIName.Name.BUY_NO_ADS_POPUP);
            if (popup)
                return;
            CanvasManager.Instance.ShowPopup(UIName.Name.BUY_NO_ADS_POPUP);
        }

        public void ShowSkinPopup()
        {
            //LogEvent(FireBaseManager.OPEN_SKIN);
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.SKIN_POPUP);
            PlayerInfoUI.ShowSkinPopupBg();
            popup.GetComponent<SkinPopup>().renderModel = playerRenderer.gameObject;
            popup.GetComponent<SkinPopup>().ModelCharacter = skinPlayerCharacter;
            popup.GetComponent<SkinPopup>().ModelCharacterSprunky = skinPlayerCharacterSprunky;
            popup.GetComponent<SkinPopup>().stateMachine = skinPlayerStateMachine;
            playerRenderer.gameObject.SetActive(true);
            playerRenderer.StartRender(popup.GetComponent<SkinPopup>().rawImage);
        }

        public void ShowSpinWheelPopup()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.SPIN_WHEEL_POPUP);
            PlayerInfoUI.ShowSpinWheelPopupBg();
        }

        public void ShowBattlePass()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.BATTLE_PASS_POPUP);
            //_battlePassUI = CanvasManager.Instance.ShowUI(UIName.Name.BATTLE_PASS_POPUP).GetComponent<BattlePassPopup>();
            PlayerInfoUI.ShowBattlePassPopupBg();
        }

        public void ShowBattleFund()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.BATTLE_PASS_BUY_PACK_POPUP);
            PlayerInfoUI.ShowBattleFundPopupBg();
        }

        public void ShowAbilitiesPopup()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.ABILITIES_POPUP);
            PlayerInfoUI.ShowAbilitiesPopupBg();
        }

        [Button]
        public void ShowTakeABreakPopup()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.TAKE_A_BREAK_POPUP);
        }

        public void OnShowInterAdsSuccess()
        {
            var reward = LevelConfig.LevelRequirements[IdleGameData.Level - 1].AdsReward / 2;
            AddMoney(reward, Vector3.zero, true);
        }

        public void ShowBattleWarningPopup()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.BATTLE_WARNING_POPUP);
        }

        private GameObject _dailyQuestPopup;

        public void ShowDailyQuestPopup()
        {
            _dailyQuestPopup = CanvasManager.Instance.ShowPopup(UIName.Name.DAILY_QUEST_POPUP);

            var data = IdleGameLogic.GetDailyQuestData();
            _dailyQuestPopup.GetComponent<DailyQuestPopup>().SetData(data);
            _dailyQuestPopup.GetComponent<DailyQuestPopup>().SetConfig(GameSaveLoad.GameData.dailyQuestData);
        }

        private void UpdateEmployeesBySaveData()
        {
            foreach (var data in GameSaveLoad.GameData.buyBedRoomStaffsList)
            {
                if (!data.isBought) continue;
                var bedroomStaff = Instantiate(bedroomStaffPrefab, employeesContainer.transform);
                bedroomStaff.transform.position = upgradeRoom.workerPoint.transform.position;
                bedroomStaff.transform.rotation = upgradeRoom.workerPoint.transform.rotation;
                bedroomStaff.GetComponent<BedRoomEmployee>().StartWorking();
                EmployeesList.Add(bedroomStaff.GetComponent<BedRoomEmployee>());
            }

            foreach (var data in GameSaveLoad.GameData.buyBoxingStaffsList)
            {
                if (data.isBought)
                {
                    AddFunctionalEmployeeInUpgradeRoom(boxingStaffPrefab);
                }
            }

            foreach (var data in GameSaveLoad.GameData.buyTrainingStaffsList)
            {
                if (data.isBought)
                {
                    AddFunctionalEmployeeInUpgradeRoom(trainingStaffPrefab);
                }
            }

            foreach (var data in GameSaveLoad.GameData.buyDiningStaffsList)
            {
                if (data.isBought)
                {
                    AddFunctionalEmployeeInUpgradeRoom(diningStaffPrefab);
                }
            }

            foreach (var point in _hireEmployeePointsList)
            {
                point.gameObject.SetActive(false);
                // Nếu những điểm thuê nhân viên này đã sử dụng
                if (GameSaveLoad.GameData.HireEmployeePointsDictionary[point.name])
                {
                    point.ActiveEmployee();
                    EmployeesList.Add(point.employee);
                    Destroy(point.gameObject);
                    if (EmployeesList.Count >= 3)
                        ShowWorkerSpeedBoosterPoint();
                }
                else
                {
                    /*if (point.LevelActive > IdleGameData.Level)
                        point.gameObject.SetActive(false);
                    else
                        point.gameObject.SetActive(true);*/
                    foreach (var room in _roomsManager.RoomsList)
                    {
                        if (room.nextWorker == point.name)
                        {
                            if (room.GetComponent<BattleRoom>())
                            {
                                if (room.IsUnlocked)
                                {
                                    point.gameObject.SetActive(true);
                                    return;
                                }
                            }
                            else
                            {
                                if (room.IsBuilt)
                                {
                                    point.gameObject.SetActive(true);
                                    return;
                                }
                            }
                        }
                    }
                    // Debug.Log("UpdateEmployeesBySaveData 2: " + point.LevelActive + ", " + IdleGameData.Level + ", " + point.name + ", " + point.gameObject.activeSelf);
                }
            }
        }

        public IdleGameSaveLoad GameSaveLoad { get; private set; }

        private void Init()
        {
            _roomsManager = new RoomsManager();
            for (var i = 0; i < _roomsAreaList.Count; i++)
            {
                _roomsManager.InitRoomsInArea(_roomsAreaList[i], i, roomConfig);
            }

            InitAllBattleRooms();

            GameSaveLoad = new IdleGameSaveLoad();
            var newSpeed = PlayerConfig.Speed * FireBaseManager.Instance.playerSpeed / 100;
            GameSaveLoad.Init(_roomsManager.RoomsList, _hireEmployeePointsList, IdleGameData.GameVersion,
                PlayerConfig.StartingMoney, PlayerConfig.StartingCapacity, newSpeed);

            IdleGameData = new IdleGameData();
            IdleGameData.SetGameSaveLoad(GameSaveLoad);

            BedRoomsList = new List<BedRoom>();
            foreach (var room in _roomsManager.RoomsList.Where(room => room.GetComponent<BedRoom>()))
            {
                BedRoomsList.Add(room.GetComponent<BedRoom>());
            }

            _roomsManager.UpdateRoomBySaveData(GameSaveLoad.GameData);
            UpdateEmployeesBySaveData();

            // Xử lý cho trường hợp user chơi lần đầu và chưa build CheckIn Room
            if (!checkInRoom.IsBuilt)
            {
                BedRoomsList[0].HideBuildPoint();
                PlayerInfoUI.HideSpeedBoosterBtn();
            }
            else
            {
                ShowSpeedBoosterPoint();
            }
        }

        private bool _isFirstTimePlaying;

        private void StartGame()
        {
            // Advertisements.Instance.RequestNativeAd();

            if (fps)
                fps.SetActive(testGameConfig.isShowFPS);

            _isFirstTimePlaying = GameSaveLoad.GameData.isFirstTimePlaying;
            if (GameSaveLoad.StableGameData.isBoughtNoAdsVip)
            {
                IdleGameLogic.CheckAnotherDay();
            }

            IdleGameLogic.CheckAnotherDaySpin();
            if (GameSaveLoad.GameData.isFirstTimePlaying)
            {
                DOVirtual.DelayedCall(FireBaseManager.Instance.firstTimeShowBannerDelay,
                    Advertisements.Instance.ShowBanner);

                if (MapIndex == 1)
                {
                    GameSaveLoad.StableGameData.firstPlayDate = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    PlayerInfoUI.HideSoldierCountBtn();
                }
            }
            else
            {
                DOVirtual.DelayedCall(1, Advertisements.Instance.ShowBanner);

                if (IdleGameLogic.CheckHaveDailyGift())
                {
                    if (battleRoom1.IsUnlocked)
                        CanvasManager.Instance.ShowPopup(UIName.Name.DAILY_GIFT_POPUP);
                }

                if (bedRoom.IsBuilt)
                    GuidesManager.CheckShowGuideConnectClientAndLeadToBattle(battleRoom1);
            }

            var currentDate = DateTime.Now.Date;
            // Lấy ngày đầu tiên người dùng chơi game
            var firstPlayDateStr = GameSaveLoad.StableGameData.firstPlayDate;
            if (firstPlayDateStr != null)
            {
                var firstPlayDate = DateTime.Parse(firstPlayDateStr);

                // Tính số ngày từ ngày đầu tiên
                var daysSinceFirstPlay = (int)(currentDate - firstPlayDate).TotalDays;
                var eventName = "D" + daysSinceFirstPlay;
                if (daysSinceFirstPlay > 0 && !GameSaveLoad.StableGameData.RetentionDictionary.ContainsKey(eventName) &&
                    daysSinceFirstPlay <= 30)
                {
                    GameSaveLoad.StableGameData.RetentionDictionary[eventName] = true;
                    LogEvent(eventName);
                    Debug.Log("LOG EVENT FIREBASE RETENTION : " + eventName);
                }
            }

            IdleGameLogic.CheckAnotherDayDailyShop();

            GameSaveLoad.GameData.isFirstTimePlaying = false;
            // _gameSaveLoad.SaveData();

            _introUI.HideNativeAds();
            PlayerInfoUI.gameObject.SetActive(true);
            PlayerInfoUI.UpdateMoney(IdleGameData.Money);
            PlayerInfoUI.UpdateGem(IdleGameData.Gem);
            PlayerInfoUI.UpdateToken(IdleGameData.Token);
            PlayerInfoUI.UpdateLevel(IdleGameData.Level);
            var totalProgress = LevelConfig.LevelRequirements[IdleGameData.Level - 1].totalProgress;
            PlayerInfoUI.UpdateLevelProgress(IdleGameData.LevelProgress, totalProgress);

            if (!checkInRoom.IsBuilt)
            {
                checkInRoom.ShowBuildPoint();
                GuidesManager.ShowBuildCheckInRoomGuide();
            }

            if (checkInRoom.IsBuilt && !bedRoom.IsBuilt)
            {
                bedRoom.ShowBuildPoint();
                GuidesManager.ShowBuildFirstBedRoomGuide(bedRoom);
            }

            if (boxingRoom.IsBuilt)
            {
                foreach (var point in _hireEmployeePointsList.Where(point => point))
                {
                    if (point.name == "Hire_Check_In_Employee_Point")
                    {
                        point.gameObject.SetActive(true);
                    }

                    if (point.name == "Hire_Bedroom_Employee_Point_Area_1")
                    {
                        point.gameObject.SetActive(true);
                    }
                }
            }

            player.GetComponent<IdlePlayer>().enabled = true;
            player.StartGame();

            if (!IdleGameLogic.CheckHaveCompletedQuest())
                PlayerInfoUI.HideDailyQuestAnimation();

            SpawnNewClient();
            // InvokeRepeating(nameof(SpawnNewClient), SpawnNewClientDelay, SpawnNewClientDelay);

            if (boxingRoom.IsBuilt)
                WaitToShowVipSoldier();

            foreach (var room in _roomsManager.RoomsList)
            {
                if (room.IsBuilt && room.name == "Bed_Room_4")
                {
                    GameSaveLoad.GameData.isFirstTimeShowUpgradeWorker = false;
                    ShowUpgradeWorkerPoint();
                }
            }

            if (MapIndex != 1)
                PlayerInfoUI.ShowChangeMapBtn();

            if (testGameConfig.isForVideoRecording)
            {
                PlayerInfoUI.HideInfoWhenStartBattle();
                PlayerInfoUI.ShowTestBtn();
            }

            if (BasicNotifications.Instant)
                BasicNotifications.Instant.onTouchSendNativeNotification();

#if UNITY_EDITOR
            if (testGameConfig.IsTestGame)
            {
                SettingGameData.IsSoundOn = testGameConfig.isSoundOn;
                SettingGameData.IsMusicOn = testGameConfig.isMusicOn;
            }
#endif
        }

        private void ShowSpeedBoosterPoint()
        {
            // PlayerInfoUI.ShowSpeedBoosterBtn();
            monetizationPointsManager.IsShowSpeedPoint = true;
        }

        public void WithDrawMoneyFromRoom(BaseRoom room)
        {
            var money = room.WithDrawMoney(player.gameObject);
            if (money <= 0) return;
            AddMoney(money, room.CashPoint.transform.position);
            UpdateRoomsToSaveData(new List<BaseRoom> { room });
        }

        private bool _isPassSound;

        public int WithdrawMoney(int money)
        {
            if (IdleGameData.Money < money)
                money = IdleGameData.Money;
            _isPassSound = !_isPassSound;
            if (!_isPassSound)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Cost_Money);
            IdleGameData.Money -= money;
            PlayerInfoUI.UpdateMoney(IdleGameData.Money);
            return money;
        }

        public int WithdrawToken(int token)
        {
            if (IdleGameData.Token < token)
                token = IdleGameData.Token;
            _isPassSound = !_isPassSound;
            if (!_isPassSound)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Cost_Money);
            IdleGameData.Token -= token;
            PlayerInfoUI.UpdateToken(IdleGameData.Token);
            return token;
        }

        public int WithdrawGem(int gem)
        {
            if (IdleGameData.Gem < gem)
                gem = IdleGameData.Gem;
            _isPassSound = !_isPassSound;
            if (!_isPassSound)
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Cost_Money);
            IdleGameData.Gem -= gem;
            PlayerInfoUI.UpdateGem(IdleGameData.Gem);
            return gem;
        }

        private UpgradePopup _upgradePopup;

        public void ShowUpgradePopup()
        {
            _upgradePopup = CanvasManager.Instance.ShowPopup(UIName.Name.UPGRADE_POPUP).GetComponent<UpgradePopup>();
            _upgradePopup.UpdateView();
            _upgradePopup.OnUpgradePlayerSpeedAction += OnUpgradePlayerSpeed;
            _upgradePopup.OnUpgradePlayerCapacityAction += OnUpgradePlayerCapacity;
        }

        public void CheckCanUpgrade()
        {
            if (!upgradeRoom.IsBuilt)
                return;
            if (IdleGameLogic.CheckCanUpgrade(IdleGameData.Level,
                    IdleGameData.Speed, IdleGameData.Capacity, playerConfig))
            {
                upgradeRoom.ShowUpgradeEffect();
            }
            else
            {
                upgradeRoom.HideUpgradeEffect();
            }
        }

        private void OnUpgradePlayerSpeed(int level)
        {
            QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId
                .UpgradeCommanderSpeed);
            var speedData = playerConfig.UpgradeSpeedsList[level - 1];
            IdleGameData.Speed = speedData.SpeedNumber;
            WithdrawMoney(speedData.Price);
            CheckCanUpgrade();
            _upgradePopup.UpdateView();
        }

        private void OnUpgradePlayerCapacity(int level)
        {
            QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId
                .UpgradeCommanderLoader);
            var capacityData = playerConfig.UpgradCapacitysList[level - 1];
            IdleGameData.Capacity = capacityData.CapacityNumber;
            WithdrawMoney(capacityData.Price);
            CheckCanUpgrade();
            _upgradePopup.UpdateView();
        }

        public void AddMoney(int money, Vector3 effectPosition, bool isShow = false, Transform parentTransform = null)
        {
            IdleGameData.Money += money;

            PlayerInfoUI.UpdateMoney(IdleGameData.Money, true, effectPosition, isShow, parentTransform);

            CheckCanBuyWorker();
            CheckCanUpgrade();
        }

        public void AddToken(int token, Vector3 effectPosition, bool isShow = false, bool isShowEffect = true,
            Transform parentTransform = null)
        {
            // Debug.Log(parentTransform);
            IdleGameData.Token += token;
            PlayerInfoUI.UpdateToken(IdleGameData.Token, isShowEffect, effectPosition, isShow, parentTransform);
        }

        public void AddGem(int gem, Vector3 effectPosition, bool isShow = false, Transform parentTransform = null)
        {
            IdleGameData.Gem += gem;
            PlayerInfoUI.UpdateGem(IdleGameData.Gem, true, effectPosition, isShow, parentTransform);
        }

        public void Navigation(GameObject target)
        {
            // PlayerInfoUI.UpdateNavigation(target);
            // PlayerInfoUI.Navigate(waitingRoom.gameObject);
        }

        public void ShowNotEnoughGemPopup()
        {
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.ALERT_POPUP);
            popup.GetComponent<AlertPopup>().SetContent("You do not have enough gem.");
        }

        /*public void ShowNotEnoughMoneyPopup()
        {
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.ALERT_POPUP);
            popup.GetComponent<AlertPopup>().SetContent("You do not have enough money.");
            ShowNearestMoneyPoint();
        }*/

        public void AddMoneyWhenUseGem(int money, int gem, Vector3 spawnPosition)
        {
            if (IdleGameData.Gem < gem)
            {
                ShowNotEnoughGemPopup();
                return;
            }

            IdleGameData.Gem -= gem;
            PlayerInfoUI.UpdateGem(IdleGameData.Gem);
            var shopPopup = CanvasManager.Instance.GetPopup(UIName.Name.SHOP_POPUP);
            AddMoney(money, spawnPosition, true, shopPopup.transform);
        }

        public void AddGemWhenInAppPurchase(int gem, Transform spawnPosition)
        {
            var shopPopup = CanvasManager.Instance.GetPopup(UIName.Name.SHOP_POPUP);
            if (shopPopup)
                AddGem(gem, spawnPosition.position, true, shopPopup.transform);
            else
                AddGem(gem, Vector3.zero, true);
        }

        public void AddTokenWhenUpgrade(int token, bool isShow = true, bool isShowEffect = true)
        {
            AddToken(token, Vector3.zero, isShow, isShowEffect);
        }

        public void RemoveAdsWhenInAppPurchase()
        {
            GameSaveLoad.StableGameData.isPurchasedRemoveAds = true;
            GameSaveLoad.OrderToSaveData(true);
            AdsManager.Instance.HideBanner();
            Debug.Log("super yun hide banner");
        }

        public void RemoveAdsVipWhenInAppPurchase()
        {
            GameSaveLoad.StableGameData.isBoughtNoAdsVip = true;
            IdleGameLogic.CheckAnotherDay();
            PlayerInfoUI.ShowTicketRemoveAdsVipInHeader();
            RemoveAdsWhenInAppPurchase();
        }

        public void BoughtHeroPack(Vector3 position, Transform parent)
        {
            GameSaveLoad.StableGameData.isBoughtBattlePassHero = true;
            AddGem(BattleFundConfig.heroGem, position, true, parent);
            AddMoney(BattleFundConfig.heroCash, position, true, parent);
            AddToken(BattleFundConfig.heroToken, position, true, true, parent);
            PlayerInfoUI.ShowBattlePassRequestAnimation();
            GameSaveLoad.GameData.listIDBattlePassHero.Add(1);
            if (!GameSaveLoad.StableGameData.isBoughtBattlePassLegend)
                battlePassDataCollection[1].battlePassLengend.rewardType = RewardBattlePassType.Diamon;

            GameSaveLoad.OrderToSaveData(true);
        }

        public void BoughtLegendPack(Vector3 position, Transform parent)
        {
            GameSaveLoad.StableGameData.isBoughtBattlePassLegend = true;
            AddGem(BattleFundConfig.legendGem, position, true, parent);
            AddMoney(BattleFundConfig.legendCash, position, true, parent);
            AddToken(BattleFundConfig.legendToken, position, true, true, parent);
            PlayerInfoUI.ShowSkinRequestAnimation();
            PlayerInfoUI.ShowBattlePassRequestAnimation();
            GameSaveLoad.GameData.listIDBattlePassLegend.Add(0);
            GameSaveLoad.GameData.listIDBattlePassLegend.Add(1);
            GameSaveLoad.StableGameData.listIDSkinUnlock.Add(4);
            if (!GameSaveLoad.StableGameData.isBoughtBattlePassHero)
                battlePassDataCollection[1].battlePassHero.rewardType = RewardBattlePassType.Diamon;

            if (!GameSaveLoad.StableGameData.isActiveSkinShop)
            {
                PlayerInfoUI.skinIconObj.SetActive(true);
                GameSaveLoad.StableGameData.isActiveSkinShop = true;
            }

            GameSaveLoad.OrderToSaveData(true);
        }

        public void BoughtHeroLegendPack(Vector3 position, Transform parent)
        {
            GameSaveLoad.StableGameData.isBoughtBattlePassHero = true;
            GameSaveLoad.StableGameData.isBoughtBattlePassLegend = true;
            AddGem(BattleFundConfig.heroLegendGem, position, true, parent);
            AddMoney(BattleFundConfig.heroLegendCash, position, true, parent);
            AddToken(BattleFundConfig.heroLegendToken, position, true, true, parent);
            PlayerInfoUI.ShowSkinRequestAnimation();
            PlayerInfoUI.ShowBattlePassRequestAnimation();
            GameSaveLoad.GameData.listIDBattlePassHero.Add(1);
            GameSaveLoad.GameData.listIDBattlePassLegend.Add(0);
            GameSaveLoad.GameData.listIDBattlePassLegend.Add(1);
            GameSaveLoad.StableGameData.listIDSkinUnlock.Add(4);
            battlePassDataCollection[1].battlePassLengend.rewardType = RewardBattlePassType.Diamon;
            if (!GameSaveLoad.StableGameData.isActiveSkinShop)
            {
                PlayerInfoUI.skinIconObj.SetActive(true);
                GameSaveLoad.StableGameData.isActiveSkinShop = true;
            }

            GameSaveLoad.OrderToSaveData(true);
        }

        public void SetDataBeginBattlePass()
        {
            battlePassDataCollection[1].battlePassLengend.rewardType = RewardBattlePassType.RemoveAd;
            battlePassDataCollection[1].battlePassHero.rewardType = RewardBattlePassType.RemoveAd;
        }

        public void UpdateBattleHp(int battleHp, bool isShowEffect = true)
        {
            if (player.IsInBattleRoom)
            {
                PlayerInfoUI.UpdateBattleHp(battleHp, false, player.transform.position);
                return;
            }

            PlayerInfoUI.UpdateBattleHp(battleHp, isShowEffect, player.transform.position);
        }

        public void IncreaseLevelProgress(int progress, Transform parentTransform = null)
        {
            IdleGameData.LevelProgress += progress;
            PlayerInfoUI.ShowAddStarEffect(1, new Vector2(0, 0), parentTransform);
            var totalProgress = LevelConfig.LevelRequirements[IdleGameData.Level - 1].totalProgress;
            // Debug.Log("IncreaseLevelProgress: " + IdleGameData.LevelProgress + ", " + totalProgress);
            PlayerInfoUI.UpdateLevelProgress(IdleGameData.LevelProgress, totalProgress);
            if (IdleGameData.LevelProgress != totalProgress)
            {
                return;
            }

            var levelDescription = levelConfig.LevelRequirements[IdleGameData.Level - 1];
            IdleGameData.Level++;
            PlayerInfoUI.UpdateLevel(IdleGameData.Level);
            IdleGameData.LevelProgress = 0;
            totalProgress = LevelConfig.LevelRequirements[IdleGameData.Level - 1].totalProgress;
            // Debug.Log("LevelUp: " + IdleGameData.LevelProgress + ", " + totalProgress);
            PlayerInfoUI.UpdateLevelProgress(IdleGameData.LevelProgress, totalProgress);

            DOVirtual.DelayedCall(2, (() =>
            {
                var levelUpPopup = CanvasManager.Instance.ShowPopup(UIName.Name.LEVEL_UP_POPUP);
                levelUpPopup.GetComponent<LevelUpPopup>()
                    .SetData(levelDescription);
                QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId
                    .PlayerLevelUp);
            })).SetAutoKill(true);
        }

        public void FakeLevelUp()
        {
            var totalProgress = LevelConfig.LevelRequirements[IdleGameData.Level - 1].totalProgress -
                                IdleGameData.LevelProgress;
            for (var i = 0; i < totalProgress; i++)
            {
                // Debug.Log("FakeLevelUp");
                IncreaseLevelProgress(1);
            }
        }

        public void FakeAddClient(int level)
        {
            battleManager.GetCurrentBattleRoom().FakeAddClient(level);
        }

        public void AddStar(Vector3 effectPosition)
        {
            IdleGameData.LevelProgress += 1;
            var screenPoint = CanvasManager.Instance.ConvertWorldPositionToScreenPosition(effectPosition);
            PlayerInfoUI.ShowAddStarEffect(1, screenPoint);
            var totalProgress = LevelConfig.LevelRequirements[IdleGameData.Level - 1].totalProgress;
            PlayerInfoUI.UpdateLevelProgress(IdleGameData.LevelProgress, totalProgress);
        }

        public void LevelUp(bool isX2 = false)
        {
            LogEvent(FireBaseManager.LEVEL_UP, "level", IdleGameData.Level);
            isWaitingToShowInterAds = true;
            var reward = levelConfig.LevelRequirements[IdleGameData.Level - 2].reward;
            if (isX2)
                reward *= 2;
            if (reward > 0)
                AddMoney(reward, Vector3.zero, true);

            if (!_lastRoomBuild) return;
            if (_lastRoomBuild.Level != 1) return;
            var nextRoomsList = new List<string>();
            if (_lastRoomBuild.nextRoom != "")
                nextRoomsList.Add(_lastRoomBuild.nextRoom);
            if (_lastRoomBuild.nextRoom2 != "")
                nextRoomsList.Add(_lastRoomBuild.nextRoom2);
            DOVirtual.DelayedCall(1, (() => { OnShowNextRoom(nextRoomsList, _lastRoomBuild.nextWorker); }))
                .SetAutoKill(true);

            CheckCanBuyWorker();
            CheckCanUpgrade();

            GuidesManager.CheckShowBattleFundGuide();
        }

        public void OnEmployeeActiveExitPoint(BaseRoom room)
        {
            var client = room.GetFirstClient();
            room.RemoveClient(client);
        }

        public bool OnEmployeeActiveEntryPoint(BaseRoom room, WarBaseClient client)
        {
            if (!room)
                return false;
            if (!room.GetComponent<BattleRoom>() && room.IsFull) return false;
            AddClientToRoom(room, client);
            if (room.GetComponent<BattleRoom>())
            {
                // UpdateBattleHp(room.GetComponent<BattleRoom>().TotalHp, false);
            }

            return true;
        }

        private void AddClientToRoom(BaseRoom room, WarBaseClient client)
        {
            room.AddClient(client);

            if (room.GetComponent<BattleRoom>())
            {
                RemoveClientFromFacility(client);
            }
        }

        public void UpdateRoomsToSaveData(List<BaseRoom> baseRooms)
        {
            GameSaveLoad.UpdateRooms(baseRooms);
        }

        private BattleRoom _currentBattleRoom;

        public static void PlayBackgroundMusic()
        {
            AudioManager.Instance.PlayBackgroundMusic(FireBaseManager.Instance.showNoel
                ? AudioManager.Instance.noel_bg
                : AudioManager.Instance.War_Base_Background_Music);
        }

        public void UpdateAbilityData(AbilityItem item)
        {
            switch (item.incomeType)
            {
                case IncomeType.CashIncome:
                    GameSaveLoad.StableGameData.abilityIncomeCheckIn += item.percentIncome;
                    break;
                case IncomeType.TokenIncome:
                    GameSaveLoad.StableGameData.abilityIncomeCheckIn += item.percentIncome;
                    break;
                case IncomeType.GemIncome:

                    break;
                case IncomeType.SpeedIncome:
                    GameSaveLoad.StableGameData.abilitySpeed += item.percentIncome;
                    break;
                case IncomeType.CashRadius:
                    GameSaveLoad.StableGameData.abilityCashRadius++;
                    break;
                case IncomeType.WorkerSpeedIncome:
                    GameSaveLoad.StableGameData.abilityWorkerSpeed += item.percentIncome;
                    break;
                case IncomeType.VipSoldierIncome:
                    GameSaveLoad.StableGameData.abilityIncomeVipSoldier += item.percentIncome;
                    break;
                case IncomeType.Capacity:
                    GameSaveLoad.StableGameData.abilityCapacity++;
                    break;
            }

            GameSaveLoad.OrderToSaveData();
        }

        public void ContinueGameAfterBattle()
        {
            player.GetComponent<IdlePlayer>().enabled = true;
            player.GetComponent<PlayerShootingControl>().enabled = false;
            player.GetComponent<IdlePlayer>().StartGame();
            DOVirtual.DelayedCall(1, (() =>
            {
                player.IsInBattleRoom = false;
                player.BattleAngle = 0;
                foreach (var employee in EmployeesList)
                {
                    employee.ResumeWhenEndBattle();
                }
            }));
        }

        public void OnEntryPointActive(BaseRoom room)
        {
            if (room.IsDamagedRoom)
                return;
            player.StopGuideArrowToPoint(room.EntryPoint.GetComponent<GuidePoint>());
            if (room.GetComponent<BattleRoom>())
            {
                for (var i = player.ClientsList.Count - 1; i >= 0; i--)
                {
                    var client = player.GetClientByState(room.emotionStateToGetIn);
                    if (!client) continue;
                    player.RemoveClient(client);
                    AddClientToRoom(room, client);
                }

                if (!GameSaveLoad.GameData.isFinishedBattleGuide)
                {
                    player.StopGuideArrowToPoint(room.EntryPoint.GetComponent<GuidePoint>());
                    guidesManager.CheckShowGuideConnectClientAndLeadToBattle(room.GetComponent<BattleRoom>());
                }
            }

            if (room.GetComponent<BedRoom>() && player.GuidePoint == room.EntryPoint.GetComponent<GuidePoint>())
            {
                player.StopGuideArrowToPoint(player.GuidePoint);
            }

            for (var i = player.ClientsList.Count - 1; i >= 0; i--)
            {
                if (room.IsFull) continue;
                var client = player.GetClientByState(room.emotionStateToGetIn);
                if (!client) return;
                player.RemoveClient(client);
                AddClientToRoom(room, client);
            }
        }

        public void OnEntryPointDeactivate(BaseRoom room)
        {
            if (room.GetComponent<BattleRoom>())
            {
                if (player.IsInBattleRoom)
                {
                    player.BattleAngle = room.GetComponent<BattleRoom>().cameraData.rotation.eulerAngles.y;
                    camerasManager.SwitchToPreviewBattleCamera(room.GetComponent<BattleRoom>().cameraData);
                    PlayerInfoUI.ShowBattleResult();
                }
                else
                {
                    camerasManager.SwitchToFollowCamera();
                    PlayerInfoUI.HideBattleResult();
                }
            }
        }

        public void OnExitPointActive(BaseRoom room)
        {
            if (room == checkInRoom)
            {
                checkInRoom.StartCheckClientOut();
                guidesManager.CheckShowGuideToWaitingRoom();
            }
            else
            {
                if (player.IsFullFollower)
                {
                    player.StopGuideArrowToPoint(room.ExitPoint.GetComponent<GuidePoint>());
                    return;
                }

                var hasClient = false;
                for (var i = room.ClientsList.Count - 1; i >= 0; i--)
                {
                    var client = room.GetFirstClient();
                    if (!client) continue;
                    if (player.IsFullFollower) continue;
                    room.RemoveClient(client);
                    player.AddClient(client);
                    hasClient = true;
                }

                if (hasClient && room == waitingRoom)
                    guidesManager.CheckShowGuideLeadClientToBedRoom();
            }

            player.StopGuideArrowToPoint(room.ExitPoint.GetComponent<GuidePoint>());
        }

        public void OnExitPointDeactivate(BaseRoom room)
        {
            if (room == checkInRoom)
            {
                checkInRoom.StopCheckClientOut();
            }
            else if (room == waitingRoom)
            {
            }
        }

        private GetMoneyPoint _currentGetMoneyPoint;

        public void ShowNearestMoneyPoint()
        {
            if (!battleRoom1.IsUnlocked)
                return;
            monetizationPointsManager.ShowNearestMoneyPoint();
            monetizationPointsManager.IsShowMoneyPoint = true;
        }

        private BaseRoom _currentRepairRoom;
        private MonetizationPopup _notEnoughCurrencyPopup;

        public void ShowNotEnoughCurrencyToRepairPopup(int money, BaseRoom room = null)
        {
            if (CanvasManager.Instance.GetPopup(UIName.Name.NOT_ENOUGH_CURRENCY_POPUP))
                return;

            if (room != null)
                _currentRepairRoom = room;
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.NOT_ENOUGH_CURRENCY_POPUP)
                .GetComponent<NotEnoughCurrencyPopup>();
            popup.SetMoney(money);

            popup.SetData(RepairRoom, AdsManager.RewardType.GET_MONEY_2, false, false, false);

            _notEnoughCurrencyPopup = popup;
        }

        private void RepairRoom()
        {
            _currentRepairRoom.RepairDamagedRoom();
            CheckEnableEntryPointWhenPlayerLeadClient();
        }

        public void OnMonetPointExit()
        {
            if (_notEnoughCurrencyPopup)
            {
                _notEnoughCurrencyPopup.Close();
                _notEnoughCurrencyPopup = null;
            }
        }

        public void ShowRepairDamagedRoomPopup(int money, BaseRoom room)
        {
            if (CanvasManager.Instance.GetPopup(UIName.Name.REPAIR_DAMAGED_ROOM_POPUP))
                return;

            _currentRepairRoom = room;
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.REPAIR_DAMAGED_ROOM_POPUP)
                .GetComponent<RepairDamagedRoomPopup>();
            popup.SetMoney(money);

            popup.SetData(RepairRoom, AdsManager.RewardType.GET_MONEY_2, false, false, false);

            _notEnoughCurrencyPopup = popup;
        }

        public void ShowGetMoneyPopup(GetMoneyPoint point = null)
        {
            if (CanvasManager.Instance.GetPopup(UIName.Name.GET_MONEY_POPUP))
                return;
            _currentGetMoneyPoint = point;

            var getMoneyPopup = CanvasManager.Instance.ShowPopup(UIName.Name.GET_MONEY_POPUP)
                .GetComponent<GetMoneyPopup>();

            var isShowMrec = FireBaseManager.Instance && FireBaseManager.Instance.showMrecGetMoneyPopup;
            var isShowNativeAds = FireBaseManager.Instance && FireBaseManager.Instance.showNativeGetMoneyPopup;
            getMoneyPopup.SetData(GetMoney, AdsManager.RewardType.GET_MONEY, isShowMrec, isShowNativeAds,
                false);

            var reward = LevelConfig.LevelRequirements[IdleGameData.Level - 1].AdsReward;
            getMoneyPopup.SetReward(reward);

            getMoneyPopup.SetRenderModel(moneyRenderer.gameObject);

            var type = point ? point.Type : 0;
            getMoneyPopup.SetType(type);
        }

        public bool IsWaitingToShowGetMoneyPoint { get; set; }

        public void GetMoney()
        {
            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                _currentGetMoneyPoint = null;
                return;
            }

            FireBaseManager.Instance.LogEvent(FireBaseManager.BOOSTER_CASH_ADS);
            var reward = LevelConfig.LevelRequirements[IdleGameData.Level - 1].AdsReward;
            if (_currentGetMoneyPoint)
            {
                monetizationPointsManager.IsShowMoneyPoint = false;
                IsWaitingToShowGetMoneyPoint = true;
                _currentGetMoneyPoint.AddMoney(reward);
                _currentGetMoneyPoint.WithDrawMoney(player.gameObject);
                AddMoney(reward, Vector3.zero, true);
                _currentGetMoneyPoint = null;
                DOVirtual.DelayedCall(2, (() => { monetizationPointsManager.HideAllMoneyPoints(); }));
            }
            else
            {
                AddMoney(reward, Vector3.zero, true);
            }

            QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId.UseMoneyBag);
        }

        public void ShowChangeMapPopup()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.CHANGE_MAP_POPUP);
        }

        public void ShowGetBoosterPopup()
        {
            if (CanvasManager.Instance.GetPopup(UIName.Name.GET_BOOSTER_POPUP))
                return;
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.GET_BOOSTER_POPUP)
                .GetComponent<MonetizationPopup>();

            var isShowMrec = FireBaseManager.Instance && FireBaseManager.Instance.showMrecSpeedBoosterPopup;
            var isShowNativeAds = FireBaseManager.Instance && FireBaseManager.Instance.showNativeSpeedBoosterPopup;
            var isFirstTimeUsing = GameSaveLoad.StableGameData.isFirstTimeUseSpeedBooster;
            popup.SetData(GetSpeedBooster, AdsManager.RewardType.SPEED_BOOSTER, isShowMrec, isShowNativeAds,
                isFirstTimeUsing);

            popup.SetRenderModel(speedBoosterRenderer.gameObject);

            speedBoosterRenderer.transform.Find("Tank_Idle").gameObject.SetActive(true);
            speedBoosterRenderer.transform.Find("Sleight_Base").gameObject.SetActive(false);
            if (FireBaseManager.Instance.showNoel)
            {
                speedBoosterRenderer.transform.Find("Tank_Idle").gameObject.SetActive(false);
                speedBoosterRenderer.transform.Find("Sleight_Base").gameObject.SetActive(true);
            }
        }

        public void ShowGetWorkerSpeedBoosterPopup()
        {
            if (CanvasManager.Instance.GetPopup(UIName.Name.GET_WORKER_SPEED_BOOSTER_POPUP))
                return;
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.GET_WORKER_SPEED_BOOSTER_POPUP)
                .GetComponent<MonetizationPopup>();
            popup.SetRenderModel(workerSpeedBoosterRenderer.gameObject);
            var isFirstTimeUsing = GameSaveLoad.StableGameData.isFirstTimeUseWorkerSpeedBooster;
            popup.SetData(GetWorkerSpeedBooster, AdsManager.RewardType.WORKER_SPEED_BOOSTER, false, false,
                isFirstTimeUsing);
            workerSpeedBoosterRenderer.transform.Find("base_1").GetComponent<Animator>().Play("Run_Sprint");
            workerSpeedBoosterRenderer.transform.Find("base_2").GetComponent<Animator>().Play("Run_Sprint");
            workerSpeedBoosterRenderer.transform.Find("base_3").GetComponent<Animator>().Play("Run_Sprint");
        }

        private void GetWorkerSpeedBooster()
        {
            GameSaveLoad.StableGameData.isFirstTimeUseWorkerSpeedBooster = false;

            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }

            FireBaseManager.Instance.LogEvent(FireBaseManager.BOOSTER_SPEED_WORKER_ADS);

            monetizationPointsManager.IsShowWorkerSpeedPoint = false;
            monetizationPointsManager.HideAllWorkerSpeedPoints();

            var tween = DOVirtual.DelayedCall(playerConfig.WorkerSpeedBoosterTime, (ShowWorkerSpeedBoosterPoint))
                .SetAutoKill(true);
            AddTweenToTweenManager(tween);

            IdleGameData.WorkerSpeedMultiplier = 3f;
            foreach (var employee in EmployeesList)
            {
                employee.UpdateClientSpeed();
            }

            PlayerInfoUI.StartCountDownForWorkerSpeedBooster(playerConfig.WorkerSpeedBoosterTime);
        }

        public void StopWorkerSpeedBooster()
        {
            IdleGameData.WorkerSpeedMultiplier = 1;
            foreach (var employee in EmployeesList)
            {
                employee.UpdateClientSpeed();
            }

            PlayerInfoUI.HideWorkerSpeedBoosterBtn();
        }

        private void GetSpeedBooster()
        {
            GameSaveLoad.StableGameData.isFirstTimeUseSpeedBooster = false;

            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }

            FireBaseManager.Instance.LogEvent(FireBaseManager.BOOSTER_SPEED_ADS);

            monetizationPointsManager.IsShowSpeedPoint = false;
            monetizationPointsManager.HideAllSpeedPoints();

            var tween = DOVirtual.DelayedCall(playerConfig.ShowSpeedBoosterDelay, (ShowSpeedBoosterPoint))
                .SetAutoKill(true);
            AddTweenToTweenManager(tween);

            if (IdleGameData.Speed * 2 < 6)
                IdleGameData.SpeedMultiplier = 6 / IdleGameData.Speed;
            else
                IdleGameData.SpeedMultiplier = 2;
            player.UpdateClientSpeed();
            player.IsSpeedBooster = true;
            PlayerInfoUI.StartCountDownForSpeedBooster(playerConfig.SpeedBoosterTime);
        }

        public void StopBooster()
        {
            IdleGameData.SpeedMultiplier = 1;
            player.UpdateClientSpeed();
            player.IsSpeedBooster = false;
            PlayerInfoUI.HideSpeedBoosterBtn();
        }

        [Button]
        public void SetPlayerSpeed(float speed)
        {
            IdleGameData.Speed = speed;
        }

        public void OnPlayerUpdateSpeed()
        {
            player.UpdateSpeed();
            // Debug.Log("OnPlayerUpdateSpeed");
        }

        public void OnAllWorkersUpdateSpeed()
        {
            // player.UpdateSpeed();
            foreach (var employee in EmployeesList)
            {
                employee.UpdateSpeed();
            }
        }

        private int _countClient = 1;

        private void SpawnNewClient()
        {
            // Debug.Log("SpawnNewClient");
            if (!checkInRoom.IsBuilt)
                return;
            if (checkInRoom.IsFull)
                return;
            // var clientObject = Instantiate(clientPrefab, clientsContainer.transform);
            var clientObject = YunObjectPool.Instance.GetObject(clientPrefab, spawnClientPoint.transform.position);
            // Debug.Log(clientObject.activeSelf);
            var client = clientObject.GetComponent<WarBaseClient>();
            client.DeactivateNavmeshAgent();
            client.Type = WarBaseClient.ClientType.Man;
            client.ActiveNavmeshAgent();
            client.Id = _countClient;
            _countClient++;
            AddClientToRoom(checkInRoom, client);
            DOVirtual.DelayedCall(0.5f, SpawnNewClient).SetAutoKill(true);
        }

        private void ShowErrorLog(string message)
        {
            message = ": " + message;
            Debug.LogError($"[{this.GetType().Name}] {message}");
        }

        public void UpdateNavMeshByRoomUpdated(GameObject room, bool isHidden)
        {
            /*var modifier = room.GetComponent<NavMeshModifier>();
            if (modifier == null)
            {
                modifier = room.AddComponent<NavMeshModifier>();
            }

            modifier.overrideArea = isHidden;
            modifier.area = isHidden ? NavMesh.GetAreaFromName("Not Walkable") : NavMesh.GetAreaFromName("Walkable");

            UpdateNavmeshInArea(room.GetComponent<BaseRoom>().AreaNumber);*/
        }

        [Button]
        public void UpdateNavmeshData(NavMeshData navMeshData)
        {
            navMeshSurface.RemoveData();
            navMeshSurface.navMeshData = navMeshData;
            navMeshSurface.AddData();
        }

        public void CheckEnableEntryPointWhenPlayerLeadClient()
        {
            if (player.ClientsList.Count == 0)
                return;
            foreach (var room in _roomsManager.RoomsList)
            {
                if (!room.IsBuilt || room.IsDamagedRoom)
                    continue;
                room.DeactivateEntryPoint();
                foreach (var client in player.ClientsList)
                {
                    if (client.EmotionState == room.emotionStateToGetIn)
                    {
                        // Debug.Log("Active entry point 1: " + room.name);
                        room.ActiveEntryPoint();
                    }
                }
            }
        }

        public void CheckActivePointWhenRoomRemoveClient(BaseRoom room)
        {
            room.DeactivateEntryPoint();
            foreach (var client in player.ClientsList)
            {
                if (client.EmotionState == room.emotionStateToGetIn)
                {
                    // Debug.Log("Active entry point 1: " + room.name);
                    room.ActiveEntryPoint();
                }
            }
        }

        public void OnPlayerRemoveClient()
        {
            foreach (var room in _roomsManager.RoomsList.Where(room => room.IsBuilt))
            {
                room.DeactivateEntryPoint();
                foreach (var unused in player.ClientsList.Where(client =>
                             client.EmotionState == room.emotionStateToGetIn))
                {
                    room.ActiveEntryPoint();
                }
            }
        }

        public List<WarBaseClient> GetAllClientByState(WarBaseClient.ClientEmotionState state)
        {
            var newList = new List<WarBaseClient>();
            foreach (var client in ClientsList)
            {
                if (client &&
                    client.EmotionState == state)
                    newList.Add(client);
            }

            return newList;
        }

        public BaseRoom GetCurrentFunctionRoom(WarBaseClient.ClientEmotionState stateToGetIn, bool isExceptFullRoom)
        {
            var roomsList = new List<BaseRoom>();
            foreach (var room in _roomsManager.RoomsList)
            {
                if (room.IsFull && isExceptFullRoom)
                    continue;
                if (room.IsBuilt && room.emotionStateToGetIn == stateToGetIn)
                    roomsList.Add(room.GetComponent<BaseRoom>());
            }

            if (roomsList.Count <= 0) return null;
            var minClientRoom = roomsList[0];
            for (var i = 1; i < roomsList.Count; i++)
            {
                if (minClientRoom.CountClients() > roomsList[i].CountClients())
                    minClientRoom = roomsList[i];
            }

            return minClientRoom;
        }

        public BedRoom FindAvailableBedRoom()
        {
            return BedRoomsList.FirstOrDefault(room =>
                !room.IsFull && room.IsBuilt && !room.IsDesertRoom && !room.IsDamagedRoom);
        }

        private void InitAllBattleRooms()
        {
            for (var i = 0; i < battleConfig.battleList.Count; i++)
            {
                battleManager.BattleRoomsList[i].Data = battleConfig.battleList[i];
            }
        }

        public void ShowDialogBox(string content, float delay = 5)
        {
            _introUI.ShowDialog(content, delay);
        }

        public void HideGuideTxt()
        {
            PlayerInfoUI.HideGuideTxt();
        }

        public void SwitchToFixCamera(CameraData cameraData)
        {
            camerasManager.SwitchToFixCamera(cameraData);
        }

        public void ZoomFixCamera(float fov)
        {
            camerasManager.ZoomFixCamera(fov);
        }

        public WarBaseClient.ClientEmotionState GetAvailableTask(WarBaseClient checkClient)
        {
            return IdleGameLogic.GetAvailableTask(_roomsManager.RoomsList, ClientsList, checkClient);
        }

        public void OnStepInBattle()
        {
        }

        public void OnStepOutBattle()
        {
            // camerasManager.ZoomInFollowCamera();
        }

        public void OnClaimDailyGift(int day)
        {
            if (GameSaveLoad.StableGameData.receivedGiftDays.Count > 10)
                GameSaveLoad.StableGameData.receivedGiftDays.RemoveAt(0);
            GameSaveLoad.StableGameData.receivedGiftDays.Add(day);

            switch (dailyGiftConfig.giftList[day].rewardType)
            {
                case DailyQuestDataConfig.RewardType.Money:
                    AddMoney(dailyGiftConfig.giftList[day].reward, Vector3.zero, true);
                    break;
                case DailyQuestDataConfig.RewardType.Star:
                    for (var i = 0; i < dailyGiftConfig.giftList[day].reward; i++)
                    {
                        DOVirtual.DelayedCall(0.5f * i,
                            (() => IncreaseLevelProgress(1)));
                    }

                    break;
                case DailyQuestDataConfig.RewardType.Gem:
                    AddGem(dailyGiftConfig.giftList[day].reward, player.transform.position, true);
                    break;
                case DailyQuestDataConfig.RewardType.Token:
                    AddToken(dailyGiftConfig.giftList[day].reward, player.transform.position, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GameSaveLoad.StableGameData.lastReceivedGiftDay = day;

            switch (day)
            {
                case 0:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_1);
                    break;
                case 1:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_2);
                    break;
                case 2:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_3);
                    break;
                case 3:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_4);
                    break;
                case 4:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_5);
                    break;
                case 5:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_6);
                    break;
                case 6:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_7);
                    break;
                default:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_8);
                    break;
            }

            GameSaveLoad.OrderToSaveData();
        }

        public void OnClaimX2DailyGift()
        {
            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                OnClaimDailyGift(GameSaveLoad.StableGameData.lastReceivedGiftDay);
                return;
            }

            var day = GameSaveLoad.StableGameData.lastReceivedGiftDay;
            if (GameSaveLoad.StableGameData.receivedGiftDays.Count > 10)
                GameSaveLoad.StableGameData.receivedGiftDays.RemoveAt(0);
            GameSaveLoad.StableGameData.receivedGiftDays.Add(day);

            switch (dailyGiftConfig.giftList[day].rewardType)
            {
                case DailyQuestDataConfig.RewardType.Money:
                    AddMoney(dailyGiftConfig.giftList[day].reward * 2, Vector3.zero, true);
                    break;
                case DailyQuestDataConfig.RewardType.Star:
                    for (var i = 0; i < dailyGiftConfig.giftList[day].reward * 2; i++)
                    {
                        DOVirtual.DelayedCall(0.5f * i,
                            (() => IncreaseLevelProgress(1)));
                    }

                    break;
                case DailyQuestDataConfig.RewardType.Gem:
                    AddGem(dailyGiftConfig.giftList[day].reward * 2, Vector3.zero, true);
                    break;
                case DailyQuestDataConfig.RewardType.Token:
                    AddToken(dailyGiftConfig.giftList[day].reward * 2, Vector3.zero, true);
                    break;
            }

            switch (day)
            {
                case 0:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_1_X3);
                    break;
                case 1:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_2_X3);
                    break;
                case 2:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_3_X3);
                    break;
                case 3:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_4_X3);
                    break;
                case 4:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_5_X3);
                    break;
                case 5:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_6_X3);
                    break;
                case 6:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_7_X3);
                    break;
                default:
                    LogEvent(FireBaseManager.RECEIVE_GIFT_DAY_8_X3);
                    break;
            }

            GameSaveLoad.OrderToSaveData();
        }

        public void OnClaimX2LevelUp()
        {
            var isX2 = !AdsManager.Instance.isCancelRewardedAd;
            LevelUp(isX2);
            AdsManager.Instance.isCancelRewardedAd = false;
        }

        public void OnClaimDailyQuest(DailyQuestDataConfig.QuestId id)
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.CLAIM_DAILY_QUEST, "questId", (int)id);
            foreach (var quest in GameSaveLoad.GameData.dailyQuestData.questList)
            {
                if (quest.id == id)
                {
                    quest.isReceived = true;
                    switch (quest.rewardType)
                    {
                        case DailyQuestDataConfig.RewardType.Money:
                            AddMoney(quest.reward, Vector3.zero, true, _dailyQuestPopup.transform);
                            break;
                        case DailyQuestDataConfig.RewardType.Gem:
                            AddGem(quest.reward, Vector3.zero, true, _dailyQuestPopup.transform);
                            break;
                        case DailyQuestDataConfig.RewardType.Token:
                            AddToken(quest.reward, Vector3.zero, true, _dailyQuestPopup.transform);
                            break;
                        case DailyQuestDataConfig.RewardType.Star:
                            for (var i = 0; i < quest.reward; i++)
                            {
                                DOVirtual.DelayedCall(0.5f * i,
                                    (() => IncreaseLevelProgress(1, _dailyQuestPopup.transform)));
                            }

                            break;
                    }
                }
            }

            if (!IdleGameLogic.CheckHaveCompletedQuest())
                PlayerInfoUI.HideDailyQuestAnimation();
        }

        public void OnClaimCompleteAllDailyQuest()
        {
            switch (dailyQuestConfig.rewardType)
            {
                case DailyQuestDataConfig.RewardType.Money:
                    AddMoney(GameSaveLoad.GameData.dailyQuestData.reward, Vector3.zero, true,
                        _dailyQuestPopup.transform);
                    break;
                case DailyQuestDataConfig.RewardType.Gem:
                    AddGem(GameSaveLoad.GameData.dailyQuestData.reward, Vector3.zero, true,
                        _dailyQuestPopup.transform);
                    break;
                case DailyQuestDataConfig.RewardType.Token:
                    AddToken(GameSaveLoad.GameData.dailyQuestData.reward, Vector3.zero, true,
                        _dailyQuestPopup.transform);
                    break;
                case DailyQuestDataConfig.RewardType.Star:
                    for (var i = 0; i < GameSaveLoad.GameData.dailyQuestData.reward; i++)
                    {
                        DOVirtual.DelayedCall(0.5f * i, (() => IncreaseLevelProgress(1, _dailyQuestPopup.transform)));
                    }

                    break;
            }

            GameSaveLoad.GameData.dailyQuestData.isReceived = true;
            if (!IdleGameLogic.CheckHaveCompletedQuest())
                PlayerInfoUI.HideDailyQuestAnimation();
        }

        private readonly List<WarBaseClient> _upgradeList = new();

        public void AddUpgradeClient(WarBaseClient client)
        {
            _upgradeList.Add(client);
            PlayerInfoUI.UpdateNavigation(client.gameObject, _upgradeList.Count);
        }

        public void RemoveUpgradeClient(Client client)
        {
            for (var i = _upgradeList.Count - 1; i >= 0; i--)
            {
                if (_upgradeList[i] == client)
                {
                    _upgradeList.RemoveAt(i);
                    break;
                }
            }

            if (_upgradeList.Count == 0)
                PlayerInfoUI.HideNavigation();
        }

        private BedRoom _waitingBedRoom;

        public void OnSelectBedRoomType(int type, bool isUseGem = false)
        {
            _waitingBedRoom.SetType(type);
            if (type != 2 && type != 5)
            {
                _waitingBedRoom.StartBuild();
            }
            else
            {
                LogEvent(type == 2
                    ? FireBaseManager.BUY_BED_ROOM_LEVEL_1_CLICK
                    : FireBaseManager.BUY_BED_ROOM_LEVEL_2_CLICK);
                if (isUseGem)
                {
                    OnViewRewardToBuildBedRoom();
                }
                else
                {
                    AdsManager.Instance.ShowReward(OnViewRewardToBuildBedRoom, AdsManager.RewardType.VIP_BED_ROOM);
                }
            }
        }

        private void OnViewRewardToBuildBedRoom()
        {
            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                if (_waitingBedRoom.Type == 2)
                    _waitingBedRoom.SetType(0);
                else if (_waitingBedRoom.Type == 5)
                    _waitingBedRoom.SetType(3);
                _waitingBedRoom.StartBuild();
                return;
            }

            var roomId = 0;
            if (_waitingBedRoom.name.Contains("1", StringComparison.OrdinalIgnoreCase))
                roomId = 1;
            else if (_waitingBedRoom.name.Contains("2", StringComparison.OrdinalIgnoreCase))
                roomId = 2;
            else if (_waitingBedRoom.name.Contains("3", StringComparison.OrdinalIgnoreCase))
                roomId = 3;
            else if (_waitingBedRoom.name.Contains("4", StringComparison.OrdinalIgnoreCase))
                roomId = 4;
            else if (_waitingBedRoom.name.Contains("5", StringComparison.OrdinalIgnoreCase))
                roomId = 5;
            else if (_waitingBedRoom.name.Contains("6", StringComparison.OrdinalIgnoreCase))
                roomId = 6;
            else if (_waitingBedRoom.name.Contains("7", StringComparison.OrdinalIgnoreCase))
                roomId = 7;
            else if (_waitingBedRoom.name.Contains("8", StringComparison.OrdinalIgnoreCase))
                roomId = 8;
            else if (_waitingBedRoom.name.Contains("9", StringComparison.OrdinalIgnoreCase))
                roomId = 9;
            else if (_waitingBedRoom.name.Contains("10", StringComparison.OrdinalIgnoreCase))
                roomId = 10;
            else if (_waitingBedRoom.name.Contains("11", StringComparison.OrdinalIgnoreCase))
                roomId = 11;
            else if (_waitingBedRoom.name.Contains("12", StringComparison.OrdinalIgnoreCase))
                roomId = 12;
            else if (_waitingBedRoom.name.Contains("13", StringComparison.OrdinalIgnoreCase))
                roomId = 13;
            else if (_waitingBedRoom.name.Contains("14", StringComparison.OrdinalIgnoreCase))
                roomId = 14;
            else if (_waitingBedRoom.name.Contains("15", StringComparison.OrdinalIgnoreCase))
                roomId = 15;
            else if (_waitingBedRoom.name.Contains("16", StringComparison.OrdinalIgnoreCase))
                roomId = 16;
            else if (_waitingBedRoom.name.Contains("17", StringComparison.OrdinalIgnoreCase))
                roomId = 17;
            else if (_waitingBedRoom.name.Contains("18", StringComparison.OrdinalIgnoreCase))
                roomId = 18;
            else if (_waitingBedRoom.name.Contains("19", StringComparison.OrdinalIgnoreCase))
                roomId = 19;
            else if (_waitingBedRoom.name.Contains("20", StringComparison.OrdinalIgnoreCase))
                roomId = 20;

            if (_waitingBedRoom.Type == 2)
            {
                FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_VIP_BED_ROOM, "Id", roomId);
            }
            else if (_waitingBedRoom.Type == 5)
            {
                FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_VIP_BED_ROOM_2, "Id", roomId);
            }

            if (_waitingBedRoom.Type == 2)
                LogEvent(FireBaseManager.BUY_BED_ROOM_LEVEL_1_SUCCESS);
            else if (_waitingBedRoom.Type == 5)
                LogEvent(FireBaseManager.BUY_BED_ROOM_LEVEL_2_SUCCESS);
            _waitingBedRoom.StartBuild();
        }

        public void ShowBuildBedRoomPopup(BedRoom room)
        {
            _waitingBedRoom = room;
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.BUILD_BED_ROOM_POPUP);
            popup.GetComponent<BuildBedRoomPopup>().typeList =
                room.IsBuilt ? new List<int>() { 3, 4, 5 } : new List<int>() { 0, 1, 2 };

            popup.GetComponent<BuildBedRoomPopup>().bedRoomModelsList = bedRoomModelsList;
            popup.GetComponent<BuildBedRoomPopup>().Init(room);

            bedRoomRenderer.StartRender(popup.GetComponent<BuildBedRoomPopup>().rawImage);
        }

        public void ShowBuyWorkerPopup()
        {
            CanvasManager.Instance.ShowPopup(UIName.Name.BUY_WORKER_POPUP);
        }

        public BaseRoom CurrentDesertRoom { get; private set; }

        [Button]
        public void ActiveDesertRoom()
        {
            var isDelay = BattleManager.isRunningBattle;
            if (isDelay)
            {
                var tween = DOVirtual.DelayedCall(10, (ActiveDesertRoom)).SetAutoKill(true);
                AddTweenToTweenManager(tween);
                return;
            }

            if (CurrentDesertRoom)
                return;
            var bedRoomsList = new List<BaseRoom>();
            foreach (var room in _roomsManager.RoomsList.Where(room =>
                         room.GetComponent<BedRoom>() && room.CountClients() >= 2))
            {
                if (room.GetComponent<BedRoom>().IsDesertRoom)
                    return;
                if (room.IsBuilt)
                    bedRoomsList.Add(room.GetComponent<BaseRoom>());
            }

            if (bedRoomsList.Count == 0)
                return;
            var randomRoom = bedRoomsList[UnityEngine.Random.Range(0, bedRoomsList.Count)];
            if (!randomRoom) return;
            CurrentDesertRoom = randomRoom;
            randomRoom.GetComponent<BedRoom>().ActiveDeserters();
        }

        public void OnRoomDeactivatedDesert(BaseRoom room)
        {
            // WaitToActiveDeserter();
            if (room != CurrentDesertRoom)
                return;
            CurrentDesertRoom = null;
        }

        public void FocusCameraToDesertRoom()
        {
            PlayerInfoUI.IsFocusingOnDesertRoom = true;
            camerasManager.FocusCameraToRoom(CurrentDesertRoom.CameraPoint.transform);
            DOVirtual.DelayedCall(2.5f, camerasManager.FocusCameraToPlayer).SetAutoKill(true);
            DOVirtual.DelayedCall(3.5f, (() => { PlayerInfoUI.IsFocusingOnDesertRoom = false; })).SetAutoKill(true);
        }

        public void OnRoomBuilt(BaseRoom room)
        {
            if (room.GetComponent<BoxingRoom>() && room.GetComponent<BoxingRoom>() == boxingRoom)
            {
                WaitToShowVipSoldier();
                DOVirtual.DelayedCall(1.5f,
                    (() => { camerasManager.FocusCameraToRoom(upgradeRoom.CameraPoint.transform); })).SetAutoKill(true);
                DOVirtual.DelayedCall(2f, (() => { upgradeRoom.StartBuild(); })).SetAutoKill(true);
                foreach (var point in _hireEmployeePointsList)
                {
                    if (point)
                    {
                        switch (point.name)
                        {
                            case "Hire_Check_In_Employee_Point":
                                DOVirtual.DelayedCall(3.5f,
                                        () => { camerasManager.FocusCameraToHireEmployeePoint(point, true); })
                                    .SetAutoKill(true);
                                break;
                            case "Hire_Bedroom_Employee_Point_Area_1":
                                DOVirtual.DelayedCall(5.5f,
                                        () => { camerasManager.FocusCameraToHireEmployeePoint(point); })
                                    .SetAutoKill(true);
                                break;
                        }
                    }
                }
            }
            else if (room.GetComponent<CheckInRoom>())
            {
                introManager.RemoveClientsList();
                battleManager.BuildFirstBattle();
                waitingRoom.StartBuild();
                bedRoom.ShowBuildPoint();

                var tween = DOVirtual.DelayedCall(60, (ShowSpeedBoosterPoint)).SetAutoKill(true);
                AddTweenToTweenManager(tween);
                SpawnNewClient();

                GuidesManager.ShowBuildFirstBedRoomGuide(bedRoom);
            }
            else if (room.GetComponent<BedRoom>())
            {
                if (room.GetComponent<BedRoom>() == bedRoom)
                    GuidesManager.CheckShowGuideConnectClientAndLeadToBattle(battleRoom1);
                CheckEnableEntryPointWhenPlayerLeadClient();
            }
        }

        [Button]
        public void ShowWorkerSpeedBoosterPoint()
        {
            monetizationPointsManager.IsShowWorkerSpeedPoint = true;
            // PlayerInfoUI.ShowWorkerSpeedBoosterBtn();
        }

        [Button]
        public void ShowVipSoldier()
        {
            _delayToShowVipSoldier?.Kill();
            var isDelay = IdleGameData.Level < 2 || battleManager.isRunningBattle;
            if (isDelay)
            {
                var tween = DOVirtual.DelayedCall(10, ShowVipSoldier).SetAutoKill(true);
                AddTweenToTweenManager(tween);
                return;
            }

            getVipSoldierPoint.gameObject.SetActive(true);
            getVipSoldierPoint.GetComponent<GetVipSoldierPoint>().IsActive = true;
            vipSoldier.transform.position = startPoint.transform.position;
            vipSoldier.transform.rotation = quaternion.Euler(Vector3.zero);
            vipSoldier.gameObject.SetActive(true);
            if (GameSaveLoad.GameData.isFirstTimeShowVipSoldier && MapIndex == 1)
                camerasManager.FollowVipSoldier();
            DOVirtual.DelayedCall(2,
                (() =>
                {
                    vipSoldier.MoveToSlot(getVipSoldierPoint.transform, false,
                        OnVipSoldierMoveToStartPointCompleted);
                })).SetAutoKill(true);
        }

        [Button]
        public void ShowUpgradeWorkerPoint()
        {
            getUpgradeWorkerPoint.GetComponent<GetUpgradeWorkerPoint>().IsActive = true;
            getUpgradeWorkerPoint.GetComponent<GetUpgradeWorkerPoint>().ShowWorker();
            PlayerInfoUI.ShowUpgradeWorkerBtn();
            GuidesManager.CheckShowUpgradeWorkerGuide(getUpgradeWorkerPoint);
        }

        private Tween _delayToShowVipSoldier;

        public void WaitToShowVipSoldier()
        {
            _delayToShowVipSoldier?.Kill();
            _delayToShowVipSoldier = DOVirtual.DelayedCall(playerConfig.ShowVipSoldierDelay, ShowVipSoldier)
                .SetAutoKill(true);
            AddTweenToTweenManager(_delayToShowVipSoldier);
        }

        private Tween _delayToActiveDeserter;

        public void WaitToActiveDeserter()
        {
            /*_delayToActiveDeserter?.Kill();
            _delayToActiveDeserter = DOVirtual.DelayedCall(FireBaseManager.Instance.deserterDelay, ActiveDesertRoom)
                .SetAutoKill(true);
            AddTweenToTweenManager(_delayToActiveDeserter);*/

            InvokeRepeating(nameof(ActiveDesertRoom), FireBaseManager.Instance.deserterDelay,
                30 + FireBaseManager.Instance.deserterDelay);
        }

        private void OnVipSoldierMoveToStartPointCompleted()
        {
            guidesManager.CheckShowVipSoldierGuide(getVipSoldierPoint);

            PlayerInfoUI.ShowVipSoldierBtn();
            PlayerInfoUI.StartCountDownForVipSoldier(playerConfig.VipSoldierTime);
        }

        public void OnShowVipSoldierPopup()
        {
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.GET_VIP_SOLDIER_POPUP)
                .GetComponent<MonetizationPopup>();
            player.StopGuideArrowToPoint(getVipSoldierPoint.GetComponent<GuidePoint>());

            popup.SetRenderModel(vipSoldierRenderer.gameObject);
            var isFirstTimeUsing = GameSaveLoad.StableGameData.isFirstTimeUseVipSoldier;
            popup.SetData(OnVipSoldierStartWorking, AdsManager.RewardType.GET_VIP_SOLDIER, false, false,
                isFirstTimeUsing);

            vipSoldierRenderer.transform.Find("Dog_Base").GetComponent<Animator>().Play("Dog_Run");
            var dogBase = vipSoldierRenderer.transform.Find("Dog_Base");
            var geometry = dogBase.transform.Find("Geometry");
            if (geometry)
            {
                geometry.transform.Find("Vip_Soldier").gameObject.SetActive(false);
                geometry.transform.Find("Vip_Soldier_Noel").gameObject.SetActive(false);
                if (FireBaseManager.Instance.showNoel)
                    geometry.transform.Find("Vip_Soldier_Noel").gameObject.SetActive(true);
                else
                    geometry.transform.Find("Vip_Soldier").gameObject.SetActive(true);
            }
        }

        private void OnVipSoldierStartWorking()
        {
            GameSaveLoad.StableGameData.isFirstTimeUseVipSoldier = false;

            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }

            FireBaseManager.Instance.LogEvent(FireBaseManager.VIP_DOG_ADS);
            getVipSoldierPoint.GetComponent<GetVipSoldierPoint>().IsActive = false;
            vipSoldier.StartWorking(_roomsManager, LevelConfig.LevelRequirements[IdleGameData.Level - 1].AdsReward);
            PlayerInfoUI.StopCountDownVipSoldier();
        }

        public void OnVipSoldierStartWorkingGem()
        {
            FireBaseManager.Instance.LogEvent(FireBaseManager.VIP_DOG_GEM);
            getVipSoldierPoint.GetComponent<GetVipSoldierPoint>().IsActive = false;
            vipSoldier.StartWorking(_roomsManager, LevelConfig.LevelRequirements[IdleGameData.Level - 1].AdsReward);
            PlayerInfoUI.StopCountDownVipSoldier();
        }

        public void OnHideVipSoldierPoint()
        {
            if (!vipSoldier.IsWorking)
                vipSoldier.gameObject.SetActive(false);
            getVipSoldierPoint.gameObject.SetActive(false);
            WaitToShowVipSoldier();

            if (player.GuidePoint == getVipSoldierPoint.GetComponent<GuidePoint>())
                player.StopGuideArrowToPoint(getVipSoldierPoint.GetComponent<GuidePoint>());
        }

        public void OnShowUpgradeWorkerPopup()
        {
            var popup = CanvasManager.Instance.ShowPopup(UIName.Name.GET_UPGRADE_WORKER_POPUP)
                .GetComponent<MonetizationPopup>();

            var isFirstTimeUsing = GameSaveLoad.StableGameData.isFirstTimeUseUpgradeWorker;
            popup.SetData(OnUpgradeWorkerStartWorking, AdsManager.RewardType.GET_LADY_WORKER, false, false,
                isFirstTimeUsing);
            popup.SetRenderModel(upgradeWorkerRenderer.gameObject);
            popup.GetComponent<GetUpgradeWorkerPopup>().ShowUp();

            player.StopGuideArrowToPoint(getUpgradeWorkerPoint.GetComponent<GuidePoint>());
        }

        private void OnUpgradeWorkerStartWorking()
        {
            GameSaveLoad.StableGameData.isFirstTimeUseUpgradeWorker = false;

            if (AdsManager.Instance.isCancelRewardedAd)
            {
                AdsManager.Instance.isCancelRewardedAd = false;
                return;
            }

            FireBaseManager.Instance.LogEvent(FireBaseManager.VIP_LADY_ADS);
            PlayerInfoUI.StartCountDownForUpgradeWorker(playerConfig.UpgradeWorkerTime);
            getUpgradeWorkerPoint.GetComponent<GetUpgradeWorkerPoint>().IsActive = false;
            getUpgradeWorkerPoint.GetComponent<GetUpgradeWorkerPoint>().ActiveWorker();

            var tween = DOVirtual.DelayedCall(playerConfig.UpgradeWorkerTime,
                () => { getUpgradeWorkerPoint.GetComponent<GetUpgradeWorkerPoint>().HideWorker(); }).SetAutoKill(true);
            AddTweenToTweenManager(tween);
        }

        private BaseRoom _lastRoomBuild;

        public void SaveRoomBuild(BaseRoom room)
        {
            _lastRoomBuild = room;
        }

        public void CheckMoneyEnoughToUpgradeVehicle(int price)
        {
            if (price > IdleGameData.Money)
            {
                ShowGetMoneyPoints();
            }
        }

        public void ShowGetMoneyPoints()
        {
            if (IsWaitingToShowGetMoneyPoint)
                return;
            if (!battleRoom1.IsUnlocked)
                return;
            // PlayerInfoUI.ShowMoneyBtn();
            monetizationPointsManager.IsShowMoneyPoint = true;
        }

        public void OnShowNextRoom(List<string> nextRoomsList, string nextWorker, bool isBuildImmediately = false)
        {
            if (_lastRoomBuild && !isBuildImmediately)
            {
                if (_lastRoomBuild.name is "Boxing_Room_1" or "Upgrade_Room")
                {
                    if (!GameSaveLoad.GameData.isShowedSpinWheelTutorial && MapIndex == 1)
                    {
                        GameSaveLoad.GameData.isShowedSpinWheelTutorial = true;
                        DOVirtual.DelayedCall(9f, (() =>
                        {
                            PlayerInfoUI.ShowSpinWheelTutorial();
                            PlayerInfoUI.SetActiveSpinIconObj(true);
                        }));
                    }
                    else
                    {
                        // PlayerInfoUI.SetActiveSpinIconObj(true);
                    }

                    GameSaveLoad.StableGameData.isActiveSpin = true;
                    GameSaveLoad.OrderToSaveData();
                }
                else if (_lastRoomBuild.name == "Battle_Room_3")
                {
                    if (!GameSaveLoad.GameData.isShowedChangeMapTutorial && MapIndex == 1)
                    {
                        DOVirtual.DelayedCall(11f, (() => { PlayerInfoUI.ShowChangeMapTutorial(); }));
                    }

                    GameSaveLoad.GameData.isShowedChangeMapTutorial = true;
                }
            }

            if (nextRoomsList.Count == 0 && nextWorker == "")
                return;
            var isNextRoomBedRoom3 = false;
            for (var i = 0; i < nextRoomsList.Count; i++)
            {
                var nextRoom = nextRoomsList[i];
                foreach (var room in _roomsManager.RoomsList)
                {
                    if (room.name == nextRoom)
                    {
                        if (isBuildImmediately)
                        {
                            room.ShowBuildPoint();
                        }
                        else
                        {
                            if (nextRoom == "Bed_Room_3" && MapIndex == 1)
                                isNextRoomBedRoom3 = true;
                            DOVirtual.DelayedCall(1 + (i * 2),
                                    (() =>
                                    {
                                        CamerasManager.FocusCameraToRoom(room.CameraPoint.transform);
                                        room.ShowBuildPoint();
                                    }))
                                .SetAutoKill(true);
                        }
                    }
                }
            }

            if (nextWorker != "")
            {
                foreach (var point in _hireEmployeePointsList)
                {
                    if (point && point.name == nextWorker)
                    {
                        if (point.relatedRoom && !point.relatedRoom.IsBuilt)
                        {
                            nextWorker = "";
                            point.relatedRoom.HireEmployeePoint = point;
                        }
                    }
                }
            }

            if (nextWorker != "")
            {
                if (isBuildImmediately)
                {
                    foreach (var point in _hireEmployeePointsList)
                    {
                        if (point && point.name == nextWorker)
                        {
                            point.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    DOVirtual.DelayedCall(1.5f + 2 * nextRoomsList.Count, (() =>
                    {
                        foreach (var point in _hireEmployeePointsList)
                        {
                            if (point && point.name == nextWorker)
                            {
                                camerasManager.FocusCameraToHireEmployeePoint(point);
                                return;
                            }
                        }
                    })).SetAutoKill(true);

                    if (isNextRoomBedRoom3 && IdleGameLogic.CheckHaveDailyGift() && _isFirstTimePlaying)
                    {
                        _isFirstTimePlaying = false;
                        DOVirtual.DelayedCall(4 + 2 * nextRoomsList.Count,
                                (() => { CanvasManager.Instance.ShowPopup(UIName.Name.DAILY_GIFT_POPUP); }))
                            .SetAutoKill(true);
                    }

                    if (_lastRoomBuild && _lastRoomBuild.GetComponent<BattleRoom>())
                    {
                        isWaitingToShowInterAds = true;
                    }

                    if (_lastRoomBuild && _lastRoomBuild.name == "Bed_Room_4")
                    {
                        var tween = DOVirtual.DelayedCall(20, (ShowUpgradeWorkerPoint)).SetAutoKill(true);
                        AddTweenToTweenManager(tween);
                    }
                }
            }
            else
            {
                if (!isBuildImmediately)
                {
                    DOVirtual.DelayedCall(1 + 2 * nextRoomsList.Count,
                            (() => { CamerasManager.FocusCameraToPlayer(); }))
                        .SetAutoKill(true);

                    if (isNextRoomBedRoom3 && IdleGameLogic.CheckHaveDailyGift() && _isFirstTimePlaying)
                    {
                        _isFirstTimePlaying = false;
                        DOVirtual.DelayedCall(2 + 2 * nextRoomsList.Count,
                                (() => { CanvasManager.Instance.ShowPopup(UIName.Name.DAILY_GIFT_POPUP); }))
                            .SetAutoKill(true);
                    }

                    if (_lastRoomBuild && _lastRoomBuild.GetComponent<BattleRoom>())
                    {
                        isWaitingToShowInterAds = true;
                    }

                    if (_lastRoomBuild && _lastRoomBuild.name == "Bed_Room_4")
                    {
                        var tween = DOVirtual.DelayedCall(20, (ShowUpgradeWorkerPoint)).SetAutoKill(true);
                        AddTweenToTweenManager(tween);
                    }
                }
            }

            _lastRoomBuild = null;

            if (!IdleGameLogic.CheckMoneyEnoughToBuildMinRoom(_roomsManager.RoomsList, IdleGameData.Money))
            {
                ShowGetMoneyPoints();
            }

            if (!IdleGameLogic.CheckMoneyEnoughToHireEmployee(_hireEmployeePointsList, IdleGameData.Money))
            {
                ShowGetMoneyPoints();
            }
        }

        public void OnBuyBedroomStaff()
        {
            foreach (var data in GameSaveLoad.GameData.buyBedRoomStaffsList)
            {
                if (!data.isBought)
                {
                    data.isBought = true;
                    WithdrawMoney(data.price);
                    var staff = Instantiate(bedroomStaffPrefab, employeesContainer.transform);
                    staff.transform.position = upgradeRoom.workerPoint.transform.position;
                    staff.transform.rotation = upgradeRoom.workerPoint.transform.rotation;
                    staff.GetComponent<BedRoomEmployee>().ShowStartEffect();
                    staff.GetComponent<BedRoomEmployee>().StartWorking();
                    EmployeesList.Add(staff.GetComponent<BedRoomEmployee>());
                    break;
                }
            }
        }

        public void OnBuyBoxingStaff()
        {
            foreach (var data in GameSaveLoad.GameData.buyBoxingStaffsList)
            {
                if (!data.isBought)
                {
                    data.isBought = true;
                    WithdrawMoney(data.price);
                    AddFunctionalEmployeeInUpgradeRoom(boxingStaffPrefab);
                    break;
                }
            }
        }

        public void OnBuyTrainingStaff()
        {
            foreach (var data in GameSaveLoad.GameData.buyTrainingStaffsList)
            {
                if (!data.isBought)
                {
                    data.isBought = true;
                    WithdrawMoney(data.price);
                    AddFunctionalEmployeeInUpgradeRoom(trainingStaffPrefab);
                    break;
                }
            }
        }

        public void OnBuyDiningStaff()
        {
            foreach (var data in GameSaveLoad.GameData.buyDiningStaffsList)
            {
                if (!data.isBought)
                {
                    data.isBought = true;
                    WithdrawMoney(data.price);
                    AddFunctionalEmployeeInUpgradeRoom(diningStaffPrefab);
                    break;
                }
            }
        }

        public void CheckCanBuyWorker()
        {
            if (!upgradeRoom.IsBuilt)
                return;
            if (IdleGameLogic.CheckCanBuyWorker())
            {
                upgradeRoom.ShowBuyWorkerEffect();
            }
            else
            {
                upgradeRoom.HideBuyWorkerEffect();
            }
        }

        [Button]
        public void AddLog(string logStr)
        {
            if (testGameConfig.isShowLog)
                PlayerInfoUI.AddLog(logStr);
        }

        public void AddLogPrivate(string logStr)
        {
            if (testGameConfig.isShowLogPrivate)
                PlayerInfoUI.AddLog(logStr);
        }

        public WarBaseClient FindNearestWaitToUpgradeClient(GameObject main)
        {
            WarBaseClient nearestClient = null;
            foreach (var room in _roomsManager.RoomsList)
            {
                if (!room || !room.IsBuilt || room.GetComponent<BattleRoom>()) continue;
                foreach (var client in room.ClientsList)
                {
                    if (!client || client.EmotionState != WarBaseClient.ClientEmotionState.WaitToUpgrade) continue;
                    if (nearestClient == null)
                    {
                        nearestClient = client;
                    }
                    else
                    {
                        var distance = Vector3.Distance(main.transform.position,
                            nearestClient.transform.position);
                        var newDistance =
                            Vector3.Distance(main.transform.position, client.transform.position);
                        if (newDistance < distance)
                            nearestClient = client;
                    }
                }
            }

            return nearestClient;
        }

        private void AddFunctionalEmployeeInUpgradeRoom(GameObject employeePrefab)
        {
            var staff = Instantiate(employeePrefab, employeesContainer.transform);
            staff.transform.position = upgradeRoom.workerPoint.transform.position;
            staff.transform.rotation = upgradeRoom.workerPoint.transform.rotation;
            staff.GetComponent<FunctionRoomEmployee>().ShowStartEffect();
            staff.GetComponent<FunctionRoomEmployee>().StartWorking();
            EmployeesList.Add(staff.GetComponent<FunctionRoomEmployee>());
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(ActiveDesertRoom));
            // Debug.Log("OnDestroy");
            KillAllTween();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                // Game được focus trở lại
                Debug.Log("Game đã được focus");
                // Có thể resume game, phục hồi âm thanh, etc.
            }
            else
            {
                // Game mất focus
                // Có thể pause game, tạm dừng âm thanh, etc.
                /*DateTime currentTime = DateTime.Now;
                int hour = currentTime.Hour;     // Giờ (0-23)
                int minute = currentTime.Minute;
                var log = hour + ":" + minute + " - " + "OnApplicationFocus";
                AddLog(log);*/
                // GameSaveLoad?.SaveDataNew();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Debug.LogError("SUPER YUN APP PAUSE");
            if (pauseStatus)
            {
                /*DateTime currentTime = DateTime.Now;
                int hour = currentTime.Hour;     // Giờ (0-23)
                int minute = currentTime.Minute;
                var log = hour + ":" + minute + " - " + "OnApplicationPause";
                AddLog(log);*/
                GameSaveLoad?.SaveDataNew();
            }
            else
            {
                StartCoroutine(DelayShowAppOpenAd());
            }
        }

        private IEnumerator DelayShowAppOpenAd()
        {
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();

            ShowAppOpenAd();
        }

        private void ShowAppOpenAd()
        {
            if (FireBaseManager.Instance.showAoa)
                AdsManager.Instance.ShowAppOpenAd(OnAppOpenAdHide);
        }

        private void OnAppOpenAdHide()
        {
        }

        private void OnApplicationQuit()
        {
            // Debug.LogError("SUPER YUN APP QUIT");
            Debug.Log("OnApplicationQuit");
            // GameSaveLoad?.SaveDataNew();
        }

        private static void LogEvent(string eventName, string paramName = "", int paramValue = 0)
        {
            if (FireBaseManager.IsInstanceExisted())
            {
                FireBaseManager.Instance.LogEvent(eventName, paramName, paramValue);
            }
        }

        private int _countTween;

        private Dictionary<string, Tween> TweenManager { get; } = new();

        private void AddTweenToTweenManager(Tween tween, string tweenName = "")
        {
            if (tweenName == "")
            {
                tweenName = "tween" + _countTween;
                _countTween++;
            }

            TweenManager[tweenName] = tween;
        }

        private void KillAllTween()
        {
            foreach (var variable in TweenManager)
            {
                variable.Value?.Kill();
            }
        }
    }
}