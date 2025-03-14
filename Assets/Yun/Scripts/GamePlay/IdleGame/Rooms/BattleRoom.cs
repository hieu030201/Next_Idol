using System.Collections.Generic;
using System.Linq;
using Advertising;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Yun.Scripts.Animations;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Areas;
using Yun.Scripts.GamePlay.IdleGame.Battles;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms.Others;
using Yun.Scripts.GamePlay.Vehicles.BattleService;
using Random = UnityEngine.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class BattleRoom : BaseRoom
    {
        [SerializeField] public bool isFirstBattle;
        [SerializeField] public bool isLastBattle;
        [SerializeField] public CameraData cameraData;
        [SerializeField] private GameObject nextArea;

        //[SerializeField] public BossControl bossRoom;

        // public int totalEnemies;

        public List<Tank> ClientTanks { get; private set; }
        public List<Armored> ClientArmoreds { get; private set; }
        public List<Missile> ClientMissiles { get; private set; }

        public List<Tank> EnemyTanks { get; private set; }
        public List<Armored> EnemyArmoreds { get; private set; }
        public List<Missile> EnemyMissiles { get; private set; }

        public GameObject OutSide { set; private get; }

        public GameObject NextOutSide { set; private get; }

        private GameObject _startBattlePoint;

        public GameObject StartBattlePoint
        {
            get => _startBattlePoint;
            set => _startBattlePoint = value;
        }

        public NavMeshData NextNavMeshData { get; set; }

        private Transform _battle;
        private GameObject _gateToBattle;
        private List<TransformData> _enemySlots;
        private TextMeshPro _countSoldierTxt;
        private GameObject _barrier;
        private GameObject _checkGetInRoomArea;
        private BattleFlag _battleFlag;

        public GameObject StandPoint3 { get; private set; }

        public GameObject allyReinforcePoint;
        public GameObject enemyReinforcePoint;

        private BuildVehiclePoint _buildTankPoint2;

        private BuildVehiclePoint _buildMissilePoint1;
        private BuildVehiclePoint _buildMissilePoint2;

        private BuildVehiclePoint _buildArmoredPoint1;
        private BuildVehiclePoint _buildArmoredPoint2;

        private UpgradeVehiclePoint _upgradeTank1PointLv2;
        private UpgradeVehiclePoint _upgradeTank1PointLv3;
        private UpgradeVehiclePoint _upgradeTank2PointLv2;
        private UpgradeVehiclePoint _upgradeTank2PointLv3;

        private UpgradeVehiclePoint _upgradeArmored1PointLv2;
        private UpgradeVehiclePoint _upgradeArmored1PointLv3;
        private UpgradeVehiclePoint _upgradeArmored2PointLv2;
        private UpgradeVehiclePoint _upgradeArmored2PointLv3;

        private UpgradeVehiclePoint _upgradeMissile1PointLv2;
        private UpgradeVehiclePoint _upgradeMissile1PointLv3;
        private UpgradeVehiclePoint _upgradeMissile2PointLv2;
        private UpgradeVehiclePoint _upgradeMissile2PointLv3;

        private SmokesAndFiresController _smokesAndFiresController;

        public List<WarBaseClient> EnemiesList { get; private set; }
        public List<WarBaseClient> ReinforceEnemiesList { get; private set; }

        public bool isResume;

        public override void Init()
        {
            base.Init();
            
            _enemySlots = new List<TransformData>();
            for (var i = 0; i < transform.Find("Enemy_Slots").childCount; i++)
            {
                var transformData = new TransformData(transform.Find("Enemy_Slots").GetChild(i));
                _enemySlots.Add(transformData);
            }

            if (transform.Find("gate"))
            {
                _gateToBattle = transform.Find("gate").gameObject;
                _gateToBattle.SetActive(false);
            }

            _battle = transform.Find("Battle");
            var points = transform.Find("Battle").Find("Points");
            var vehicles = transform.Find("Battle").Find("Vehicles");

            ClientTanks = new List<Tank>();
            if (vehicles.transform.Find("Tank_1"))
                ClientTanks.Add(vehicles.transform.Find("Tank_1").GetComponent<Tank>());
            if (vehicles.transform.Find("Tank_2"))
                ClientTanks.Add(vehicles.transform.Find("Tank_2").GetComponent<Tank>());

            ClientArmoreds = new List<Armored>();
            if (vehicles.transform.Find("Armored_1"))
                ClientArmoreds.Add(vehicles.transform.Find("Armored_1").GetComponent<Armored>());
            if (vehicles.transform.Find("Armored_2"))
                ClientArmoreds.Add(vehicles.transform.Find("Armored_2").GetComponent<Armored>());

            ClientMissiles = new List<Missile>();
            if (vehicles.transform.Find("Missle_1"))
                ClientMissiles.Add(vehicles.transform.Find("Missle_1").GetComponent<Missile>());
            if (vehicles.transform.Find("Missle_2"))
                ClientMissiles.Add(vehicles.transform.Find("Missle_2").GetComponent<Missile>());


            EnemyTanks = new List<Tank>();
            if (vehicles.transform.Find("Tank_Red_1"))
                EnemyTanks.Add(vehicles.transform.Find("Tank_Red_1").GetComponent<Tank>());
            if (vehicles.transform.Find("Tank_Red_2"))
                EnemyTanks.Add(vehicles.transform.Find("Tank_Red_2").GetComponent<Tank>());

            EnemyArmoreds = new List<Armored>();
            if (vehicles.transform.Find("Armored_Red_1"))
                EnemyArmoreds.Add(vehicles.transform.Find("Armored_Red_1").GetComponent<Armored>());
            if (vehicles.transform.Find("Armored_Red_2"))
                EnemyArmoreds.Add(vehicles.transform.Find("Armored_Red_2").GetComponent<Armored>());

            EnemyMissiles = new List<Missile>();
            if (vehicles.transform.Find("Missle_Red_1"))
                EnemyMissiles.Add(vehicles.transform.Find("Missle_Red_1").GetComponent<Missile>());
            if (vehicles.transform.Find("Missle_Red_2"))
                EnemyMissiles.Add(vehicles.transform.Find("Missle_Red_2").GetComponent<Missile>());

            if (_battle.Find("Count_Soldier_Txt"))
            {
                _countSoldierTxt = _battle.Find("Count_Soldier_Txt")
                    .GetComponent<TextMeshPro>();
                _countSoldierTxt.text = "";
            }

            if (_battle.Find("Smokes_And_Fires_Battle"))
                _smokesAndFiresController = _battle.Find("Smokes_And_Fires_Battle")
                    .GetComponent<SmokesAndFiresController>();

            if (points.Find("Upgrade_Tank_1_Point_Lv2"))
            {
                _upgradeTank1PointLv2 = points.Find("Upgrade_Tank_1_Point_Lv2")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeTank1PointLv2.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Tank_1_Point_Lv3"))
            {
                _upgradeTank1PointLv3 = points.Find("Upgrade_Tank_1_Point_Lv3")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeTank1PointLv3.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Tank_2_Point_Lv2"))
            {
                _upgradeTank2PointLv2 = points.Find("Upgrade_Tank_2_Point_Lv2")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeTank2PointLv2.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Tank_2_Point_Lv3"))
            {
                _upgradeTank2PointLv3 = points.Find("Upgrade_Tank_2_Point_Lv3")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeTank2PointLv3.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Armored_1_Point_Lv2"))
            {
                _upgradeArmored1PointLv2 = points.Find("Upgrade_Armored_1_Point_Lv2")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeArmored1PointLv2.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Armored_1_Point_Lv3"))
            {
                _upgradeArmored1PointLv3 = points.Find("Upgrade_Armored_1_Point_Lv3")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeArmored1PointLv3.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Armored_2_Point_Lv2"))
            {
                _upgradeArmored2PointLv2 = points.Find("Upgrade_Armored_2_Point_Lv2")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeArmored2PointLv2.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Armored_2_Point_Lv3"))
            {
                _upgradeArmored2PointLv3 = points.Find("Upgrade_Armored_2_Point_Lv3")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeArmored2PointLv3.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Missile_1_Point_Lv2"))
            {
                _upgradeMissile1PointLv2 = points.Find("Upgrade_Missile_1_Point_Lv2")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeMissile1PointLv2.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Missile_1_Point_Lv3"))
            {
                _upgradeMissile1PointLv3 = points.Find("Upgrade_Missile_1_Point_Lv3")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeMissile1PointLv3.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Missile_2_Point_Lv2"))
            {
                _upgradeMissile2PointLv2 = points.Find("Upgrade_Missile_2_Point_Lv2")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeMissile2PointLv2.gameObject.SetActive(false);
            }

            if (points.Find("Upgrade_Missile_2_Point_Lv3"))
            {
                _upgradeMissile2PointLv3 = points.Find("Upgrade_Missile_2_Point_Lv3")
                    .GetComponent<UpgradeVehiclePoint>();
                _upgradeMissile2PointLv3.gameObject.SetActive(false);
            }

            if (points.Find("Start_Battle_Point"))
                _startBattlePoint = points.Find("Start_Battle_Point").gameObject;

            if (points.Find("Build_Tank_Point_1"))
                BuildTankPoint1 = points.Find("Build_Tank_Point_1")
                    .GetComponent<BuildVehiclePoint>();
            if (points.Find("Build_Tank_Point_2"))
                _buildTankPoint2 = points.Find("Build_Tank_Point_2")
                    .GetComponent<BuildVehiclePoint>();
            if (points.Find("Build_Missile_Point_1"))
                _buildMissilePoint1 = points.Find("Build_Missile_Point_1")
                    .GetComponent<BuildVehiclePoint>();
            if (points.Find("Build_Missile_Point_2"))
                _buildMissilePoint2 = points.Find("Build_Missile_Point_2")
                    .GetComponent<BuildVehiclePoint>();
            if (points.Find("Build_Armored_Point_1"))
                _buildArmoredPoint1 = points.Find("Build_Armored_Point_1")
                    .GetComponent<BuildVehiclePoint>();
            if (points.Find("Build_Armored_Point_2"))
                _buildArmoredPoint2 = points.Find("Build_Armored_Point_2")
                    .GetComponent<BuildVehiclePoint>();

            if (_battle.Find("Flag_Base"))
                _battleFlag = _battle.Find("Flag_Base").GetComponent<BattleFlag>();

            allyReinforcePoint = transform.Find("Stand_Point_2").gameObject;

            if (transform.Find("Stand_Point_3"))
                StandPoint3 = transform.Find("Stand_Point_3").gameObject;

            if (transform.Find("Stand_Point_Enemy"))
                enemyReinforcePoint = transform.Find("Stand_Point_Enemy").gameObject;
            _checkGetInRoomArea = transform.Find("Check_Get_In_Room_Area").gameObject;
            _barrier = _battle.Find("barie").gameObject;

            // Debug.Log("Awake " + gameObject.name);
            _enemySlots = new List<TransformData>();
            for (var i = 0; i < transform.Find("Enemy_Slots").childCount; i++)
            {
                var transformData = new TransformData(transform.Find("Enemy_Slots").GetChild(i));
                _enemySlots.Add(transformData);
            }


            if (_barrier)
                _barrier.SetActive(true);

            if (NextOutSide)
                NextOutSide.SetActive(false);

            foreach (var vehicle in ClientTanks)
            {
                vehicle.gameObject.SetActive(false);
            }

            foreach (var vehicle in ClientMissiles)
            {
                vehicle.gameObject.SetActive(false);
            }

            foreach (var vehicle in ClientArmoreds)
            {
                vehicle.gameObject.SetActive(false);
            }

            foreach (var oldRoom in oldRoomsList)
            {
                if (!oldRoom)
                    Debug.LogError("MISSING OLD ROOM IN " + name);
            }
            
            if (nextArea)
            {
                nextArea.GetComponent<Area>().defaultRoomsContainer.SetActive(false);
                nextArea.GetComponent<Area>().functionRoomsContainer.SetActive(false);
            }
        }

        public void UpdateStatusWhenBattle(float progress)
        {
            if (_smokesAndFiresController)
                _smokesAndFiresController.UpdateProgress(progress);
        }

        public void ShowWinFlag()
        {
            if (_battleFlag)
                _battleFlag.ShowWin();
            if (_smokesAndFiresController)
                _smokesAndFiresController.Stop();
        }

        protected override void Start()
        {
            //Reset(6);
        }

        private BattleData _data;

        public BattleData Data
        {
            get => _data;
            set => _data = value;
        }

        // private GameObject battleContainer
        public override void AddClient(WarBaseClient client, bool immediately = false)
        {
            if (!immediately)
            {
                TotalHp += client.Level;
            }

            client.EmotionState = WarBaseClient.ClientEmotionState.Completed;
            if (immediately)
            {
                client.enabled = false;
            }

            if (!IsFull)
            {
                var lastRoom = client.LastRoom;
                ClientsList.Add(client);

                var destination = ClientsList.Count <= slotsList.Count
                    ? slotsList[ClientsList.Count - 1]
                    : slotsList[^1];
                if (immediately)
                {
                    client.enabled = false;
                    client.DeactivateNavmeshAgent();
                    client.GetComponent<ClientShootingControl>().ShowIdleRiffle();
                    client.transform.position = destination.position;
                    client.transform.rotation = destination.rotation;
                    client.LastRoom = this;
                }
                else
                {
                    MoveWithoutNavmeshAgent(client, destination);
                    UpdateRoomsToSaveData(new List<BaseRoom> { this, lastRoom });
                }

                alliesList.Add(client);
            }
            else
            {
                var lastRoom = client.LastRoom;
                client.LastRoom = this;
                reinforceAlliesList.Add(client);
                if (immediately)
                {
                    client.enabled = false;
                    client.DeactivateNavmeshAgent();
                    client.GetComponent<ClientShootingControl>().ShowIdleRiffle();
                    client.transform.position = allyReinforcePoint.transform.position;
                    client.gameObject.SetActive(false);
                }
                else
                {
                    // client.MoveToSlot(allyReinforcePoint.transform);
                    MoveWithoutNavmeshAgent(client, allyReinforcePoint.transform, true);
                    UpdateRoomsToSaveData(new List<BaseRoom> { this, lastRoom });
                }

                //DOVirtual.DelayedCall(4, () => { Destroy(client.gameObject); }).SetAutoKill(true);
            }

            if (_countSoldierTxt)
                _countSoldierTxt.text = (ClientsList.Count + reinforceAlliesList.Count).ToString();
            FacilityManager.Instance.UpdateBattleHp(ClientsList.Count + reinforceAlliesList.Count, false);
        }

        private void MoveWithoutNavmeshAgent(Client client, Transform destination, bool isHideWhenMoveComplete = false)
        {
            var direction = (destination.position - client.transform.position).normalized;
            // Tạo rotation mới hướng về mục tiêu
            var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            client.transform.rotation = lookRotation;
            client.enabled = false;
            client.DeactivateNavmeshAgent();
            var distance = Vector3.Distance(destination.position, client.transform.position);
            var duration = distance / 5;
            client.GetComponent<ClientShootingControl>().stateMachine.ActionState =
                ClientActionStateMachine.State.RunFast;
            client.transform.DOMove(destination.position, duration).SetEase(Ease.Linear).OnComplete((() =>
            {
                if (isHideWhenMoveComplete)
                {
                    client.gameObject.SetActive(false);
                }
                else
                {
                    client.GetComponent<ClientShootingControl>().ShowIdleRiffle();
                    client.transform.rotation = destination.rotation;
                }
            }));
        }

        public void FakeAddClient(int level)
        {
            TotalHp += level;
            FacilityManager.Instance.UpdateBattleHp(TotalHp);
            // var clientObject = Instantiate(clientPrefab, FacilityManager.Instance.ClientsContainer.transform);
            var clientObject = YunObjectPool.Instance.GetObject(clientPrefab);
            var client = clientObject.GetComponent<WarBaseClient>();
            switch (level)
            {
                case 1:
                    client.Type = WarBaseClient.ClientType.SoldierLv1;
                    break;
                case 2:
                    client.Type = WarBaseClient.ClientType.SoldierLv2;
                    break;
                case 3:
                    client.Type = WarBaseClient.ClientType.SoldierLv3;
                    break;
            }

            client.Level = level;
            client.GetComponent<ClientShootingControl>().ShowIdleRiffle();
            AddClient(client, true);
        }

        public void HideBarrier()
        {
            if (_barrier)
                _barrier.SetActive(false);
        }

        public void ShowBarrier()
        {
            if (_barrier)
                _barrier.SetActive(true);
        }

        public override void RemoveAllClients()
        {
            base.RemoveAllClients();
            _countSoldierTxt.text = "";
        }

        public override void UpdateRoomBySaveData(RoomData roomData)
        {
            IsUnlocked = roomData.IsUnLocked;
            TotalHp = roomData.Hp;
            base.UpdateRoomBySaveData(roomData);
        }

        private void ShowEnemyVehicles()
        {
            foreach (var tank in EnemyTanks)
            {
                tank.Show();
            }
            
            foreach (var armored in EnemyArmoreds)
            {
                armored.Show();
            }
            
            foreach (var missile in EnemyMissiles)
            {
                missile.Show();
            }
        }

        public override void StartBuild(bool isBuildImmediately = false)
        {
            IsBuilt = true;
            // Debug.Log(gameObject.name);
            if (!isBuildImmediately)
            {
                // Debug.Log("UpdateRoomsToSaveData 1");
                UpdateRoomsToSaveData(new List<BaseRoom> { this });
            }

            if (oldRoomsList != null)
            {
                foreach (var oldRoom in oldRoomsList.Where(oldRoom => oldRoom))
                {
                    oldRoom.SetActive(false);
                }
            }

            if (IsUnlocked)
            {
                Unlock(false, isBuildImmediately);
                RoomData.ClientsList = new List<ClientData>();
                RoomData.WaitingClientsList = new List<ClientData>();
            }
            else
            {
                if (isFirstBattle && !isBuildImmediately && FacilityManager.Instance.MapIndex == 1)
                {
                    HideStartBattlePoint();
                }

                if (_battleFlag)
                    _battleFlag.StartRun();
                if (OutSide)
                    OutSide.SetActive(true);
                ShowBarrier();

                ShowEnemyVehicles();

                if (nextArea)
                {
                    nextArea.GetComponent<Area>().defaultRoomsContainer.SetActive(false);
                    nextArea.GetComponent<Area>().functionRoomsContainer.SetActive(false);
                }

                if (BuildTankPoint1)
                {
                    BuildTankPoint1.Init();
                    BuildTankPoint1.Price = _data.tank1Price;

                    if (FacilityManager.Instance.GameSaveLoad.GameData.tanksList.Count == 1)
                    {
                        ClientTanks[0].Show();
                        ClientTanks[0].Level = FacilityManager.Instance.GameSaveLoad.GameData.tanksList[0].level;
                        BuildTankPoint1.gameObject.SetActive(false);
                        if (ClientTanks[0].Level == 0)
                            ShowUpgradePoint(_upgradeTank1PointLv2);
                        else if (ClientTanks[0].Level == 1)
                            ShowUpgradePoint(_upgradeTank1PointLv3);
                    }
                }

                if (_buildTankPoint2)
                {
                    _buildTankPoint2.Init();
                    _buildTankPoint2.Price = _data.tank2Price;

                    if (FacilityManager.Instance.GameSaveLoad.GameData.tanksList.Count == 2)
                    {
                        ClientTanks[1].Show();
                        ClientTanks[1].Level = FacilityManager.Instance.GameSaveLoad.GameData.tanksList[1].level;
                        _buildTankPoint2.gameObject.SetActive(false);
                        if (ClientTanks[1].Level == 0)
                            ShowUpgradePoint(_upgradeTank2PointLv2);
                        else if (ClientTanks[1].Level == 1)
                            ShowUpgradePoint(_upgradeTank2PointLv3);
                    }
                }

                if (_buildMissilePoint1)
                {
                    _buildMissilePoint1.Init();
                    _buildMissilePoint1.Price = _data.missilePrice;

                    if (FacilityManager.Instance.GameSaveLoad.GameData.missilesList.Count == 1)
                    {
                        ClientMissiles[0].Show();
                        ClientMissiles[0].Level = FacilityManager.Instance.GameSaveLoad.GameData.missilesList[0].level;
                        _buildMissilePoint1.gameObject.SetActive(false);

                        if (ClientMissiles[0].Level == 0)
                            ShowUpgradePoint(_upgradeMissile1PointLv2);
                        else if (ClientMissiles[0].Level == 1)
                            ShowUpgradePoint(_upgradeMissile1PointLv3);
                    }
                }

                if (_buildMissilePoint2)
                {
                    _buildMissilePoint2.Init();
                    _buildMissilePoint2.Price = _data.missile2Price;

                    if (FacilityManager.Instance.GameSaveLoad.GameData.missilesList.Count == 2)
                    {
                        ClientMissiles[1].Show();
                        ClientMissiles[1].Level = FacilityManager.Instance.GameSaveLoad.GameData.missilesList[1].level;
                        _buildMissilePoint2.gameObject.SetActive(false);

                        if (ClientMissiles[1].Level == 0)
                            ShowUpgradePoint(_upgradeMissile2PointLv2);
                        else if (ClientMissiles[1].Level == 1)
                            ShowUpgradePoint(_upgradeMissile2PointLv3);
                    }
                }

                if (_buildArmoredPoint1)
                {
                    _buildArmoredPoint1.Init();
                    _buildArmoredPoint1.Price = _data.armoredPrice;

                    if (FacilityManager.Instance.GameSaveLoad.GameData.armoredList.Count == 1)
                    {
                        ClientArmoreds[0].Show();
                        ClientArmoreds[0].Level = FacilityManager.Instance.GameSaveLoad.GameData.armoredList[0].level;
                        _buildArmoredPoint1.gameObject.SetActive(false);

                        if (ClientArmoreds[0].Level == 0)
                            ShowUpgradePoint(_upgradeArmored1PointLv2);
                        else if (ClientArmoreds[0].Level == 1)
                            ShowUpgradePoint(_upgradeArmored1PointLv3);
                    }
                }

                if (_buildArmoredPoint2)
                {
                    _buildArmoredPoint2.Init();
                    _buildArmoredPoint2.Price = _data.armored2Price;

                    if (FacilityManager.Instance.GameSaveLoad.GameData.armoredList.Count == 2)
                    {
                        ClientArmoreds[1].Show();
                        ClientArmoreds[1].Level = FacilityManager.Instance.GameSaveLoad.GameData.armoredList[1].level;
                        _buildArmoredPoint2.gameObject.SetActive(false);

                        if (ClientArmoreds[1].Level == 0)
                            ShowUpgradePoint(_upgradeArmored2PointLv2);
                        else if (ClientArmoreds[1].Level == 1)
                            ShowUpgradePoint(_upgradeArmored2PointLv3);
                    }
                }

                if (_upgradeTank1PointLv2)
                    _upgradeTank1PointLv2.Price = _data.tank1Prices[0];
                if (_upgradeTank1PointLv3)
                    _upgradeTank1PointLv3.Price = _data.tank1Prices[1];

                if (_upgradeTank2PointLv2)
                    _upgradeTank2PointLv2.Price = _data.tank2Prices[0];
                if (_upgradeTank2PointLv3)
                    _upgradeTank2PointLv3.Price = _data.tank2Prices[1];

                if (_upgradeArmored1PointLv2)
                    _upgradeArmored1PointLv2.Price = _data.armored1Prices[0];
                if (_upgradeArmored1PointLv3)
                    _upgradeArmored1PointLv3.Price = _data.armored1Prices[1];

                if (_upgradeArmored2PointLv2)
                    _upgradeArmored2PointLv2.Price = _data.armored2Prices[0];
                if (_upgradeArmored2PointLv3)
                    _upgradeArmored2PointLv3.Price = _data.armored2Prices[1];

                if (_upgradeMissile1PointLv2)
                    _upgradeMissile1PointLv2.Price = _data.armored1Prices[0];
                if (_upgradeMissile1PointLv3)
                    _upgradeMissile1PointLv3.Price = _data.armored1Prices[1];

                if (_upgradeMissile2PointLv2)
                    _upgradeMissile2PointLv2.Price = _data.missile2Prices[0];
                if (_upgradeMissile2PointLv3)
                    _upgradeMissile2PointLv3.Price = _data.missile2Prices[1];

                _battle.gameObject.SetActive(true);
                EntryPoint.gameObject.SetActive(true);

                var saveEnemiesList = FacilityManager.Instance.GameSaveLoad.GameData.enemiesList;

                if (saveEnemiesList.Count == 0)
                {
                    for (var i = 0; i < _data.enemyLv1; i++)
                    {
                        var enemyData = new EnemyData
                        {
                            hp = ClientShootingControl.HpByLevel[0],
                            type = 1
                        };
                        saveEnemiesList.Add(enemyData);
                    }

                    for (var i = 0; i < _data.enemyLv2; i++)
                    {
                        var enemyData = new EnemyData
                        {
                            hp = ClientShootingControl.HpByLevel[1],
                            type = 2
                        };
                        saveEnemiesList.Add(enemyData);
                    }

                    for (var i = 0; i < _data.enemyLv3; i++)
                    {
                        var enemyData = new EnemyData
                        {
                            hp = ClientShootingControl.HpByLevel[2],
                            type = 3
                        };
                        saveEnemiesList.Add(enemyData);
                    }
                }

                UpdateEnemiesListBySaveData(saveEnemiesList);

                var currentTotalHp = 0;
                foreach (var enemyData in FacilityManager.Instance.GameSaveLoad.GameData.enemiesList)
                {
                    currentTotalHp += enemyData.hp;
                }

                var hp1 = ClientShootingControl.HpByLevel[0];
                var hp2 = ClientShootingControl.HpByLevel[1];
                var hp3 = ClientShootingControl.HpByLevel[2];
                var totalProgress = Data.enemyLv1 * hp1 + Data.enemyLv2 * hp2 +
                                    Data.enemyLv3 * hp3;
                // Debug.Log("UpdateBattleProgress: " + currentTotalHp + ", " + totalProgress);
                FacilityManager.Instance.PlayerInfoUI.UpdateBattleProgress((float)(totalProgress - currentTotalHp) /
                                                                           totalProgress);
                FacilityManager.Instance.PlayerInfoUI.InitBattleMilestone(Data);
            }
        }

        private void UpdateEnemiesListBySaveData(List<EnemyData> dataList)
        {
            EnemiesList = new List<WarBaseClient>();
            ReinforceEnemiesList = new List<WarBaseClient>();
            // Debug.Log("UpdateEnemiesListBySaveData UpdateEnemiesListBySaveData UpdateEnemiesListBySaveData");
            for (var i = 0; i < dataList.Count; i++)
            {
                // var enemy = Instantiate(clientPrefab, transform).GetComponent<WarBaseClient>();
                var enemy = YunObjectPool.Instance.GetObject(clientPrefab).GetComponent<WarBaseClient>();
                // enemy.transform.parent = transform;
                enemy.GetComponent<NavMeshAgent>().enabled = false;
                switch (dataList[i].type)
                {
                    case 1:
                        enemy.Type = WarBaseClient.ClientType.Enemy;
                        break;
                    case 2:
                        enemy.Type = WarBaseClient.ClientType.Enemy2;
                        break;
                    case 3:
                        enemy.Type = WarBaseClient.ClientType.Enemy3;
                        break;
                }

                enemy.GetComponent<Collider>().enabled = false;

                enemy.GetComponent<ClientShootingControl>().enemyData = dataList[i];
                if (i < _enemySlots.Count)
                {
                    enemy.transform.position = _enemySlots[i].position;
                    enemy.transform.rotation = _enemySlots[i].rotation;
                    EnemiesList.Add(enemy);
                    var randomNumber = Random.Range(0, 10) * 0.1f;
                    DOVirtual.DelayedCall(randomNumber,
                        (() =>
                        {
                            enemy.GetComponent<ClientShootingControl>()
                                .ShowIdleRiffle();
                        })).SetAutoKill(true);
                }
                else
                {
                    enemy.transform.position = enemyReinforcePoint.transform.position;
                    enemy.transform.rotation = enemyReinforcePoint.transform.rotation;
                    ReinforceEnemiesList.Add(enemy);
                }
            }

            foreach (var enemy in ReinforceEnemiesList)
            {
                enemy.gameObject.SetActive(false);
            }
        }

        public override void Hide(bool isUpdateNavmesh = true)
        {
            _battle.gameObject.SetActive(false);
            EntryPoint.gameObject.SetActive(false);

            if (isUpdateNavmesh)
                FacilityManager.Instance.UpdateNavMeshByRoomUpdated(gameObject, true);
        }

        private List<Vector3> _saveBattlePositionsList;

        public void Unlock(bool isSaveData = true, bool isBuildImmediately = false)
        {
            if (NextNavMeshData)
                FacilityManager.Instance.UpdateNavmeshData(NextNavMeshData);

            switch (name)
            {
                case "Battle_Room_3":
                    FacilityManager.Instance.PlayerInfoUI.ShowChangeMapBtn();
                    break;
                case "Battle_Room_2":
                    FacilityManager.Instance.WaitToActiveDeserter();
                    break;
                case "Battle_Room_1":
                    FacilityManager.Instance.PlayerInfoUI.ShowSoldierCountBtn();
                    break;
            }
            
            if (isLastBattle)
            {
                if (isBuildImmediately)
                {
                    if (_gateToBattle)
                        _gateToBattle.SetActive(false);
                    gameObject.SetActive(false);
                    return;
                }

                _startBattlePoint.gameObject.SetActive(false);
            }

            if (!nextArea)
            {
                FacilityManager.Instance.ShowDialogBox(
                    "It's incredible, you have brought peace to our country, join me in conquering new lands, Commander.",
                    6);
                FacilityManager.Instance.ContinueGameAfterBattle();
                FacilityManager.Instance.CamerasManager.SwitchToFollowCamera();
                IsUnlocked = true;
                if (isSaveData)
                    FacilityManager.Instance.UpdateRoomsToSaveData(new List<BaseRoom> { this });
                return;
            }

            // Debug.Log("Unlock: " + gameObject.name);
            if (OutSide)
                OutSide.SetActive(false);
            if (NextOutSide)
                NextOutSide.SetActive(true);
            IsUnlocked = true;
            _checkGetInRoomArea.SetActive(false);

            var nextRoomsList = new List<string>();
            if (nextRoom != "")
                nextRoomsList.Add(nextRoom);
            if (nextRoom2 != "")
                nextRoomsList.Add(nextRoom2);

            if (isBuildImmediately)
            {
                _battle.gameObject.SetActive(false);
                EntryPoint.gameObject.SetActive(false);

                FacilityManager.Instance.OnShowNextRoom(nextRoomsList, nextWorker, true);
            }
            else
            {
                _battle.transform.DOMoveY(-10, 1f).OnComplete((() => { _battle.gameObject.SetActive(false); }))
                    .SetAutoKill(true);

                EntryPoint.transform.DOMoveY(-10, 1f).OnComplete((() => { _battle.gameObject.SetActive(false); }))
                    .SetAutoKill(true);

                var savePosY = nextArea.transform.position.y;
                nextArea.transform.position += new Vector3(0, -7f, 0);

                DOVirtual.DelayedCall(1,
                        (() => { nextArea.transform.DOMoveY(savePosY, 0.5f); })).OnComplete((() => { }))
                    .SetAutoKill(true);

                DOVirtual.DelayedCall(2, (() =>
                {
                    var starCount = Data.reward;
                    for (var i = 0; i < starCount; i++)
                    {
                        DOVirtual.DelayedCall(0.5f * i, (() => FacilityManager.Instance.IncreaseLevelProgress(1)));
                    }

                    var levelRequirements = FacilityManager.Instance.LevelConfig.LevelRequirements;
                    var totalProgress = levelRequirements[FacilityManager.Instance.IdleGameData.Level - 1]
                        .totalProgress;
                    var isLevelUp = (FacilityManager.Instance.IdleGameData.LevelProgress + starCount >= totalProgress);
                    if (!isLevelUp)
                    {
                        if (name == "Battle_Room_3")
                        {
                            var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
                            if (!FacilityManager.Instance.GameSaveLoad.GameData.isShowedChangeMapTutorial &&
                                mapIndex == 1)
                            {
                                DOVirtual.DelayedCall(11f,
                                    (() => { FacilityManager.Instance.PlayerInfoUI.ShowChangeMapTutorial(); }));
                            }

                            FacilityManager.Instance.GameSaveLoad.GameData.isShowedChangeMapTutorial = true;
                        }

                        FacilityManager.Instance.OnShowNextRoom(nextRoomsList, nextWorker);
                    }
                    else
                    {
                        FacilityManager.Instance.SaveRoomBuild(this);
                    }
                })).SetAutoKill(true);
            }

            nextArea.GetComponent<Area>().defaultRoomsContainer.SetActive(true);
            nextArea.GetComponent<Area>().functionRoomsContainer.SetActive(true);

            if (!isBuildImmediately)
                FacilityManager.Instance.ContinueGameAfterBattle();

            if (isSaveData)
                FacilityManager.Instance.UpdateRoomsToSaveData(new List<BaseRoom> { this });
        }

        private void HideStartBattlePoint()
        {
            _startBattlePoint.SetActive(false);
        }

        public void ShowStartBattlePoint()
        {
            _startBattlePoint.SetActive(true);
        }

        private static void ShowUpgradePoint(UpgradeVehiclePoint point)
        {
            point.transform.localScale = Vector3.zero;
            point.gameObject.SetActive(true);
            point.transform.DOScale(1.5f, 1);
            FacilityManager.Instance.CheckMoneyEnoughToUpgradeVehicle(point.Price);
        }

        public void BuildVehicle(BuildVehiclePoint point)
        {
            point.gameObject.SetActive(false);
            var data = new BattleVehicleData();
            if (point == BuildTankPoint1)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.tanksList.Add(data);
                ClientTanks[0].Show();
                ClientTanks[0].Level = 0;
                ShowUpgradePoint(_upgradeTank1PointLv2);

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_TANK, "battleId", Id);
            }
            else if (point == _buildTankPoint2)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.tanksList.Add(data);
                ClientTanks[1].Show();
                ClientTanks[1].Level = 0;
                ShowUpgradePoint(_upgradeTank2PointLv2);

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_TANK, "battleId", Id);
            }
            else if (point == _buildMissilePoint1)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.missilesList.Add(data);
                ClientMissiles[0].Show();
                ClientMissiles[0].Level = 0;
                ShowUpgradePoint(_upgradeMissile1PointLv2);

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_MISSILE, "battleId", Id);
            }
            else if (point == _buildMissilePoint2)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.missilesList.Add(data);
                ClientMissiles[1].Show();
                ClientMissiles[1].Level = 0;
                ShowUpgradePoint(_upgradeMissile2PointLv2);

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_MISSILE, "battleId", Id);
            }
            else if (point == _buildArmoredPoint1)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.armoredList.Add(data);
                ClientArmoreds[0].Show();
                ClientArmoreds[0].Level = 0;
                ShowUpgradePoint(_upgradeArmored1PointLv2);

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_ARMORED, "battleId", Id);
            }
            else if (point == _buildArmoredPoint2)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.armoredList.Add(data);
                ClientArmoreds[1].Show();
                ClientArmoreds[1].Level = 0;
                ShowUpgradePoint(_upgradeArmored2PointLv2);

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUY_ARMORED, "battleId", Id);
            }

            FacilityManager.Instance.GameSaveLoad.OrderToSaveData();
        }

        public void UpgradeVehicle(UpgradeVehiclePoint point)
        {
            var room = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
            var battleId = room.AreaNumber + 1;
            point.gameObject.SetActive(false);
            if (point == _upgradeTank1PointLv2 || point == _upgradeTank1PointLv3)
            {
                if (point == _upgradeTank1PointLv2)
                    ShowUpgradePoint(_upgradeTank1PointLv3);
                ClientTanks[0].UpdateLevel();
                if (FacilityManager.Instance.GameSaveLoad.GameData.tanksList.Count > 0)
                    FacilityManager.Instance.GameSaveLoad.GameData.tanksList[0].level++;

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_TANK, "battleId", battleId);
            }

            if (point == _upgradeTank2PointLv2 || point == _upgradeTank2PointLv3)
            {
                if (point == _upgradeTank2PointLv2)
                    ShowUpgradePoint(_upgradeTank2PointLv3);
                ClientTanks[1].UpdateLevel();
                if (FacilityManager.Instance.GameSaveLoad.GameData.tanksList.Count > 1)
                    FacilityManager.Instance.GameSaveLoad.GameData.tanksList[1].level++;

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_TANK, "battleId", battleId);
            }

            if (point == _upgradeArmored1PointLv2 || point == _upgradeArmored1PointLv3)
            {
                if (point == _upgradeArmored1PointLv2)
                    ShowUpgradePoint(_upgradeArmored1PointLv3);
                ClientArmoreds[0].UpdateLevel();
                if (FacilityManager.Instance.GameSaveLoad.GameData.armoredList.Count > 0)
                    FacilityManager.Instance.GameSaveLoad.GameData.armoredList[0].level++;

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_ARMORED, "battleId", battleId);
            }

            if (point == _upgradeArmored2PointLv2 || point == _upgradeArmored2PointLv3)
            {
                if (point == _upgradeArmored2PointLv2)
                    ShowUpgradePoint(_upgradeArmored2PointLv3);
                ClientArmoreds[1].UpdateLevel();
                if (FacilityManager.Instance.GameSaveLoad.GameData.armoredList.Count > 1)
                    FacilityManager.Instance.GameSaveLoad.GameData.armoredList[1].level++;

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_ARMORED, "battleId", battleId);
            }

            if (point == _upgradeMissile1PointLv2 || point == _upgradeMissile1PointLv3)
            {
                if (point == _upgradeMissile1PointLv2)
                    ShowUpgradePoint(_upgradeMissile1PointLv3);
                ClientMissiles[0].UpdateLevel();
                if (FacilityManager.Instance.GameSaveLoad.GameData.missilesList.Count > 0)
                    FacilityManager.Instance.GameSaveLoad.GameData.missilesList[0].level++;

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_MISSILE, "battleId", battleId);
            }

            if (point == _upgradeMissile2PointLv2 || point == _upgradeMissile2PointLv3)
            {
                if (point == _upgradeMissile2PointLv2)
                    ShowUpgradePoint(_upgradeMissile2PointLv3);
                ClientMissiles[1].UpdateLevel();
                if (FacilityManager.Instance.GameSaveLoad.GameData.missilesList.Count > 1)
                    FacilityManager.Instance.GameSaveLoad.GameData.missilesList[1].level++;

                FireBaseManager.Instance.LogEvent(FireBaseManager.BUILD_MISSILE, "battleId", battleId);
            }

            FacilityManager.Instance.GameSaveLoad.OrderToSaveData();
        }

        public BuildVehiclePoint BuildTankPoint1 { get; private set; }

        public void ShowAllBuildPoints()
        {
            if (_upgradeTank1PointLv2)
            {
                _upgradeTank1PointLv2.gameObject.SetActive(false);
                _upgradeTank1PointLv2.DepositNumber = 0;
            }

            if (_upgradeTank1PointLv3)
            {
                _upgradeTank1PointLv3.gameObject.SetActive(false);
                _upgradeTank1PointLv3.DepositNumber = 0;
            }

            if (_upgradeTank2PointLv2)
            {
                _upgradeTank2PointLv2.gameObject.SetActive(false);
                _upgradeTank2PointLv2.DepositNumber = 0;
            }

            if (_upgradeTank2PointLv3)
            {
                _upgradeTank2PointLv3.gameObject.SetActive(false);
                _upgradeTank2PointLv3.DepositNumber = 0;
            }

            if (_upgradeArmored1PointLv2)
            {
                _upgradeArmored1PointLv2.gameObject.SetActive(false);
                _upgradeArmored1PointLv2.DepositNumber = 0;
            }

            if (_upgradeArmored1PointLv3)
            {
                _upgradeArmored1PointLv3.gameObject.SetActive(false);
                _upgradeArmored1PointLv3.DepositNumber = 0;
            }

            if (_upgradeArmored2PointLv2)
            {
                _upgradeArmored2PointLv2.gameObject.SetActive(false);
                _upgradeArmored2PointLv2.DepositNumber = 0;
            }

            if (_upgradeArmored2PointLv3)
            {
                _upgradeArmored2PointLv3.gameObject.SetActive(false);
                _upgradeArmored2PointLv3.DepositNumber = 0;
            }

            if (_upgradeMissile1PointLv2)
            {
                _upgradeMissile1PointLv2.gameObject.SetActive(false);
                _upgradeMissile1PointLv2.DepositNumber = 0;
            }

            if (_upgradeMissile1PointLv3)
            {
                _upgradeMissile1PointLv3.gameObject.SetActive(false);
                _upgradeMissile1PointLv3.DepositNumber = 0;
            }

            if (_upgradeMissile2PointLv2)
            {
                _upgradeMissile2PointLv2.gameObject.SetActive(false);
                _upgradeMissile2PointLv2.DepositNumber = 0;
            }

            if (_upgradeMissile2PointLv3)
            {
                _upgradeMissile2PointLv3.gameObject.SetActive(false);
                _upgradeMissile2PointLv3.DepositNumber = 0;
            }

            if (BuildTankPoint1)
            {
                BuildTankPoint1.gameObject.SetActive(true);
                BuildTankPoint1.DepositNumber = 0;
                ClientTanks[0].GetComponent<ExploseAnimation>().Reborn();
            }

            if (_buildTankPoint2)
            {
                _buildTankPoint2.gameObject.SetActive(true);
                _buildTankPoint2.DepositNumber = 0;
                ClientTanks[1].GetComponent<ExploseAnimation>().Reborn();
            }

            if (_buildMissilePoint1)
            {
                _buildMissilePoint1.gameObject.SetActive(true);
                _buildMissilePoint1.DepositNumber = 0;
                ClientMissiles[0].GetComponent<ExploseAnimation>().Reborn();
            }

            if (_buildMissilePoint2)
            {
                _buildMissilePoint2.gameObject.SetActive(true);
                _buildMissilePoint2.DepositNumber = 0;
                ClientMissiles[1].GetComponent<ExploseAnimation>().Reborn();
            }

            if (_buildArmoredPoint1)
            {
                _buildArmoredPoint1.gameObject.SetActive(true);
                _buildArmoredPoint1.DepositNumber = 0;
                ClientArmoreds[0].GetComponent<ExploseAnimation>().Reborn();
            }

            if (_buildArmoredPoint2)
            {
                _buildArmoredPoint2.gameObject.SetActive(true);
                _buildArmoredPoint2.DepositNumber = 0;
                ClientArmoreds[1].GetComponent<ExploseAnimation>().Reborn();
            }
        }
    }
}