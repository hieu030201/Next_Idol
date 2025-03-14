using System.Collections.Generic;
using System.Linq;
using Advertising;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.Cores;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms;
using Yun.Scripts.GamePlay.IdleGame.Rooms.Others;
using Yun.Scripts.GamePlay.Vehicles.BattleService;
using Random = UnityEngine.Random;

namespace Yun.Scripts.GamePlay.IdleGame.Managers
{
    public class BattleManager : YunBehaviour
    {
        [SerializeField] protected IdlePlayer player;

        public List<BattleRoom> BattleRoomsList
        {
            get;
            set;
        }
        
        public bool isRunningBattle;

        protected const string AlliesLayerName = "Allies";
        protected const string AllyBulletsLayerName = "AllyBullets";

        protected int CountClientDead;
        protected int CountEnemyDead;
        protected const string EnemiesLayerName = "Enemies";
        protected const string EnemyBulletsLayerName = "EnemyBullets";
        private int _totalEnemyHpLeft;
        private int _totalProgress;
        protected BattleRoom BattleRoom;
        [ShowInInspector] protected List<WarBaseClient> CurrentAlliesList;

        [ShowInInspector] protected List<WarBaseClient> CurrentEnemiesList;
        [ShowInInspector] protected BossControl CurrentBoss;
        
        protected int TotalAllies;
        protected int TotalEnemies;
        private readonly CounterTime _counterClientThrowGrenade = new ();
        private readonly CounterTime _counterEnemyThrowGrenade = new ();
        
        [ShowInInspector] private List<GameObject> _metalTagList = new ();

        public int countMetalTurnNumber;
        private BattleRoom _currentBattleRoom;
        

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            // Ignore collisions within the same army
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(AlliesLayerName),
                LayerMask.NameToLayer(AllyBulletsLayerName), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(AlliesLayerName),
                LayerMask.NameToLayer(AlliesLayerName), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(EnemiesLayerName),
                LayerMask.NameToLayer(EnemyBulletsLayerName), true);

            // Allow collisions between armies and enemy bullets
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(AlliesLayerName),
                LayerMask.NameToLayer(EnemyBulletsLayerName), false);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer(EnemiesLayerName),
                LayerMask.NameToLayer(AllyBulletsLayerName), false);
        }
        
        public void ViewBattle()
        {
            if (player.IsInBattleRoom)
                return;
            var room = GetCurrentBattleRoom();
            FacilityManager.Instance.SwitchToFixCamera(room.cameraData);
            DOVirtual.DelayedCall(3, (() => { FacilityManager.Instance.CamerasManager.SwitchToFollowCamera(); })).SetAutoKill(true);
        }
        
        private static void LogEvent(string eventName, string paramName = "", int paramValue = 0)
        {
            if (FireBaseManager.IsInstanceExisted())
            {
                FireBaseManager.Instance.LogEvent(eventName, paramName, paramValue);
            }
        }

        private void OnUpdateLoseBattle()
        {
            FacilityManager.Instance.monetizationPointsManager.gameObject.SetActive(true);
            switch (FacilityManager.Instance.MapIndex)
            {
                case 1:
                    LogEvent(FireBaseManager.LOSE_BATTLE_MAP_1, "id", GetCurrentBattleRoom().Id);
                    break;
                case 2:
                    LogEvent(FireBaseManager.LOSE_BATTLE_MAP_2, "id", GetCurrentBattleRoom().Id);
                    break;
                case 3:
                    LogEvent(FireBaseManager.LOSE_BATTLE_MAP_3, "id", GetCurrentBattleRoom().Id);
                    break;
            }

            FacilityManager.Instance.CamerasManager.SwitchToPreviewBattleCamera(_currentBattleRoom.cameraData);
            FacilityManager.PlayBackgroundMusic();

            // Nếu thua lần đầu ở battle 1 , và chưa thuê checkin worker thì gợi ý player ra chỗ check in
            if (_currentBattleRoom.isFirstBattle && FacilityManager.Instance.MapIndex == 1)
            {
                FacilityManager.Instance.ShowDialogBox(
                    "The enemy has been significantly weakened. Let's recruit more soldiers to achieve victory, Commander.",
                    8);
                FacilityManager.Instance.GuidesManager.CheckShowGuideConnectClientAndLeadToBattle(_currentBattleRoom);
                FacilityManager.Instance.GameSaveLoad.GameData.isFinishedBattleGuide = true;
            }

            DOVirtual.DelayedCall(1.0f, () =>
            {
                FacilityManager.Instance.PlayerInfoUI.ShowMetalTagTurnNumber(countMetalTurnNumber);
                FacilityManager.Instance.PlayerInfoUI.LevelBattlePassProgress();
            });

            player.GetComponent<IdlePlayer>().enabled = true;
            player.GetComponent<PlayerShootingControl>().enabled = false;
            player.GetComponent<IdlePlayer>().StartGame();

            DOVirtual.DelayedCall(1, (() =>
            {
                foreach (var employee in FacilityManager.Instance.EmployeesList)
                {
                    employee.ResumeWhenEndBattle();
                }
            })).SetAutoKill(true);

            FacilityManager.Instance.PlayerInfoUI.ShowInfoAfterBattle();
            if (player.IsSpeedBooster)
                player.ShowMiniTank();
        }
        
        public BattleRoom GetCurrentBattleRoom()
        {
            return BattleRoomsList.FirstOrDefault(room => room.IsBuilt && !room.IsUnlocked);
        }

        public void BuildFirstBattle()
        {
            BattleRoomsList[0].StartBuild();
        }
        
        public void OnStartBattle()
        {
            var room = GetCurrentBattleRoom();
            _currentBattleRoom = room;

            foreach (var employee in FacilityManager.Instance.EmployeesList)
            {
                employee.PauseWhenStartBattle();
            }
            
            player.StopGuideArrowToPoint(_currentBattleRoom.EntryPoint.GetComponent<GuidePoint>());
            room.HideBarrier();
            player.GetComponent<IdlePlayer>().HideTrail();
            AudioManager.Instance.PlayBackgroundMusic(AudioManager.Instance.Music_Battle_MGB);
            StartBattle(room);
            
            FacilityManager.Instance.PlayerInfoUI.HideInfoWhenStartBattle();
            FacilityManager.Instance.UpdateBattleHp(0, false);
            FacilityManager.Instance.SwitchToFixCamera(room.GetComponent<BattleRoom>().cameraData);

            FacilityManager.Instance.monetizationPointsManager.gameObject.SetActive(false);
        }
        
        public void OnUpdateWinBattle()
        {
            player.BattleAngle = 0;
            FacilityManager.PlayBackgroundMusic();
            FacilityManager.Instance.PlayerInfoUI.ShowInfoAfterBattle();

            if (_currentBattleRoom.isFirstBattle)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.metalTagNumber = 4;
            }
            
            DOVirtual.DelayedCall(1.5f, () =>
            {
                FacilityManager.Instance.PlayerInfoUI.ShowMetalTagTurnNumber(countMetalTurnNumber);
                FacilityManager.Instance.PlayerInfoUI.LevelBattlePassProgress();
                FacilityManager.Instance.monetizationPointsManager.gameObject.SetActive(true);
            });
            WinBattle();
            if (player.IsSpeedBooster)
                player.ShowMiniTank();

            _currentBattleRoom.RemoveAllClients();
            _currentBattleRoom.Unlock();

            QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId.ExpandAreas);

            switch (FacilityManager.Instance.MapIndex)
            {
                case 1:
                    LogEvent(FireBaseManager.WIN_BATTLE_MAP_1, "id", _currentBattleRoom.Id);
                    break;
                case 2:
                    LogEvent(FireBaseManager.WIN_BATTLE_MAP_2, "id", _currentBattleRoom.Id);
                    break;
                case 3:
                    LogEvent(FireBaseManager.WIN_BATTLE_MAP_3, "id", _currentBattleRoom.Id);
                    break;
            }

            for (var i = 0; i < BattleRoomsList.Count; i++)
            {
                if(BattleRoomsList[i] == _currentBattleRoom && i + 1 < BattleRoomsList.Count)
                    BattleRoomsList[i + 1].StartBuild();
            }
        }

        protected void Update()
        {
            _counterClientThrowGrenade.Execute();
            _counterEnemyThrowGrenade.Execute();
        }

        public void StartBattle(BattleRoom battleRoom)
        {
            if (isRunningBattle)
                return;
            
            // Advertisements.Instance.HideBanner();
            // AdsManager.Instance.ShowMrec();
            
            FacilityManager.Instance.PlayerInfoUI.ShowMovingSoldierIcon();
            isRunningBattle = true;
            // Debug.Log("StartBattle ==============>");
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Rifle_Battle_MGB, true);
            CountClientDead = 0;
            CountEnemyDead = 0;
            BattleRoom = battleRoom;
            CurrentAlliesList = BattleRoom.alliesList;
            CurrentEnemiesList = BattleRoom.EnemiesList;
            // if (BattleRoom.bossRoom != null)
            // {
            //     CurrentBoss = BattleRoom.bossRoom;
            //     battleRoom.bossRoom.SetTargetList(battleRoom.alliesList);
            //     battleRoom.bossRoom.StartFighting(_enemyBulletsLayerName);
            // }
            TimmingThrowClient();
            TimmingThrowEnemy();
            foreach (var enemy in battleRoom.EnemiesList)
            {
                enemy.gameObject.layer = LayerMask.NameToLayer(EnemiesLayerName);
                var shootingControl = enemy.GetComponent<ClientShootingControl>();
                shootingControl.SetTargetList(battleRoom.alliesList);
    
                if (!battleRoom.isResume)
                {
                    var hp = shootingControl.enemyData.hp * (battleRoom.isFirstBattle ? 2 : 1);
                    shootingControl.UpdateHp(hp);
                }
                shootingControl.StartFighting(EnemyBulletsLayerName);
            }

            foreach (var ally in battleRoom.alliesList)
            {
                ally.gameObject.layer = LayerMask.NameToLayer(AlliesLayerName);
                var shootingControl = ally.GetComponent<ClientShootingControl>();
                var clientHp = ally.Level * 3 * (battleRoom.isFirstBattle ? 2 : 1);
    
                shootingControl.UpdateHp(clientHp);
                shootingControl.SetTargetList(battleRoom.EnemiesList);
                shootingControl.StartFighting(AllyBulletsLayerName);
            }
            player.gameObject.layer = LayerMask.NameToLayer(AlliesLayerName);
            var shootingControlPlayer = player.GetComponent<PlayerShootingControl>();
            var idlePlayer = player.GetComponent<IdlePlayer>();

            shootingControlPlayer.enabled = true;
            if (idlePlayer.IsSpeedBooster)
                idlePlayer.HideMiniTank();
            idlePlayer.enabled = false;

            shootingControlPlayer.UpdateHp(8);
            shootingControlPlayer.SetTargetList(battleRoom.EnemiesList);
            shootingControlPlayer.StartFighting(AllyBulletsLayerName);
            StartShootingEnemyAbility(battleRoom);
            StartShootingClientAbility(battleRoom);
            TotalEnemies = battleRoom.EnemiesList.Count + BattleRoom.ReinforceEnemiesList.Count;
            TotalAllies = battleRoom.alliesList.Count + BattleRoom.reinforceAlliesList.Count;
            battleRoom.HideBarrier();
          
            var hp1 = ClientShootingControl.HpByLevel[0];
            var hp2 = ClientShootingControl.HpByLevel[1];
            var hp3 = ClientShootingControl.HpByLevel[2];
            _totalProgress = BattleRoom.Data.enemyLv1 * hp1 + BattleRoom.Data.enemyLv2 * hp2 +
                             BattleRoom.Data.enemyLv3 * hp3;
            
            if (battleRoom.isFirstBattle)
                _totalProgress *= 2;

            _totalEnemyHpLeft = 0;
            foreach (var enemy in BattleRoom.ReinforceEnemiesList)
            {
                var control = enemy.GetComponent<ClientShootingControl>();
                _totalEnemyHpLeft += control.enemyData.hp;
            }
            foreach (var enemy in BattleRoom.EnemiesList)
            {
                var control = enemy.GetComponent<ClientShootingControl>();
                _totalEnemyHpLeft += control.Hp;
            }
            
            battleRoom.isResume = true;

            DOVirtual.DelayedCall(0.1f, StartShooting).SetAutoKill(true);
        }

        public void FinishBattle(bool isWin)
        {
            countMetalTurnNumber = 0;
            FacilityManager.Instance.PlayerInfoUI.HideMovingSoldierIcon();
            _counterClientThrowGrenade.Exit();
            _counterEnemyThrowGrenade.Exit();
            AudioManager.Instance.StopSoundEffect(AudioManager.Instance.Rifle_Battle_MGB);
            AudioManager.Instance.StopBackgroundMusic();
            isRunningBattle = false;

            StopShootingClientAbility(BattleRoom);
            StopShootingEnemyAbility(BattleRoom);
            player.GetComponent<PlayerShootingControl>().StopFighting();
            
            if (_metalTagList.Count != 0)

            {
                countMetalTurnNumber = _metalTagList.Count;

                foreach (var metalTag in _metalTagList)
                {
                    metalTag.GetComponent<MetalTag>().MovingToTarget(metalTag, player.transform);
                    FacilityManager.Instance.GameSaveLoad.GameData.metalTagNumber++;
                }
            }
            if (isWin)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.isFinishedBattleGuide = true;
                AudioManager.Instance.StopBackgroundMusic();
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Win_sound_9);
                FacilityManager.Instance.PlayerInfoUI.HideBattleResult();
                FacilityManager.Instance.GameSaveLoad.GameData.tanksList = new List<BattleVehicleData>();
                FacilityManager.Instance.GameSaveLoad.GameData.armoredList = new List<BattleVehicleData>();
                FacilityManager.Instance.GameSaveLoad.GameData.missilesList = new List<BattleVehicleData>();
                BattleRoom.ShowWinFlag();
                FacilityManager.Instance.GameSaveLoad.GameData.enemiesList = new List<EnemyData>();
                foreach (var ally in BattleRoom.alliesList)
                    if (!ally.GetComponent<ClientShootingControl>().IsDead)
                        ally.GetComponent<ClientShootingControl>().ShowWin();

                DestroyEnemyAbility(BattleRoom);
                
                DOVirtual.DelayedCall(5, OnUpdateWinBattle)
                    .SetAutoKill(true);
            }
            else
            {
                BattleRoom.ShowBarrier();
                foreach (var enemy in BattleRoom.EnemiesList)
                    if (!enemy.GetComponent<ClientShootingControl>().IsDead)
                        enemy.GetComponent<ClientShootingControl>().ShowWin();
                
                BattleRoom.RemoveAllClients();
                FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);

                // DestroyClientAbility(BattleRoom);
                DOVirtual.DelayedCall(1, OnUpdateLoseBattle)
                    .SetAutoKill(true);
            }
            _metalTagList.Clear();
            FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
        }

        protected void StartShooting()
        {
            foreach (var enemy in CurrentEnemiesList) enemy.GetComponent<ClientShootingControl>().StartShooting();

            foreach (var client in CurrentAlliesList) client.GetComponent<ClientShootingControl>().StartShooting();
            
            player.GetComponent<PlayerShootingControl>().StartShooting();
            if (CurrentBoss!= null)
            {
                CurrentBoss.StartShooting();
            }
        }

        public void WinBattle()
        {
            foreach (var client in CurrentAlliesList)
            {
                if (client.GetComponent<ClientShootingControl>().healthBar != null)
                    SimplePool.Despawn(client.GetComponent<ClientShootingControl>().healthBar);

                // Destroy(client.gameObject);
                FacilityManager.Instance.ReturnClientToPool(client.gameObject);
            }

            foreach (var clientReinforce in BattleRoom.reinforceAlliesList)
            {
                if (clientReinforce.GetComponent<ClientShootingControl>().healthBar != null)
                    SimplePool.Despawn(clientReinforce.GetComponent<ClientShootingControl>().healthBar);

                Destroy(clientReinforce.gameObject);
            }

            foreach (var enemy in BattleRoom.ReinforceEnemiesList)
            {
                if (enemy.GetComponent<ClientShootingControl>().healthBar != null)
                    SimplePool.Despawn(enemy.GetComponent<ClientShootingControl>().healthBar);

                Destroy(enemy.gameObject);
            }

            player.GetComponent<PlayerShootingControl>().ResetData();
        }

        private float _previousProgress;

        public void UpdateTakeDamage(GameObject entity, int damage)
        {
            if (!isRunningBattle)
                return;
            if (LayerMask.LayerToName(entity.layer) != EnemiesLayerName) return;
            _totalEnemyHpLeft -= damage;
            // Debug.Log("UpdateTakeDamage: " + _totalEnemyHpLeft);

            // Debug.Log("UpdateBattleProgress: " + BattleRoom.reinforceEnemiesList.Count + ", " +
            //           BattleRoom.enemiesList.Count);
            var progress = (float) (_totalProgress - _totalEnemyHpLeft) / _totalProgress;
            // Debug.Log("_totalEnemyHpLeft: " + _totalEnemyHpLeft);
            // Debug.Log("UpdateTakeDamage: " + progress + " ======================>");
            for (var i = 0; i < BattleRoom.Data.milestonesList.Count; i++)
            {
                var milestone = BattleRoom.Data.milestonesList[i];
                var milestoneProgress = (float) milestone.progress / 100;
                // Debug.Log("UpdateTakeDamage: " +);
                if (_previousProgress != 0 && progress >= milestoneProgress && _previousProgress < milestoneProgress)
                {
                    FacilityManager.Instance.AddMoney(milestone.reward, Vector3.zero);
                    FacilityManager.Instance.PlayerInfoUI.ArchiveMilestones(i, milestone.reward);
                }
            }

            _previousProgress = progress;
            FacilityManager.Instance.PlayerInfoUI.UpdateBattleProgress(progress);
            BattleRoom.UpdateStatusWhenBattle(progress);
        }

        public virtual void UpdateDead(GameObject entity)
        {
            if (LayerMask.LayerToName(entity.layer) == AlliesLayerName)
            {
                DOVirtual.DelayedCall(1, () =>
                {
                    entity.GetComponent<ClientShootingControl>().Reset();
                    FacilityManager.Instance.ReturnClientToPool(entity);
                }).SetAutoKill(true);
                MoveAllyFromCamp(entity);

                CountClientDead++;

                if (CountClientDead == TotalAllies)
                    FinishBattle(false);
            }
            else if (LayerMask.LayerToName(entity.layer) == EnemiesLayerName)
            {
                if (isRunningBattle)
                {
                    var metal = SimplePool.Spawn<MetalTag>(PoolType.MetalTag, entity.transform.position, quaternion.identity);

                    metal.OnInit();
                    if (metal != null)  
                    {  
                        _metalTagList.Add(metal.gameObject);
                    }  
                }
                for (var i = 0; i < FacilityManager.Instance.GameSaveLoad.GameData.enemiesList.Count; i++)
                {
                    var entityData = entity.GetComponent<ClientShootingControl>().enemyData;
                    var compareData = FacilityManager.Instance.GameSaveLoad.GameData.enemiesList[i];
                    if (entityData != compareData) continue;
                    FacilityManager.Instance.GameSaveLoad.GameData.enemiesList.RemoveAt(i);
                    break;
                }
                
                DOVirtual.DelayedCall(1, () =>
                {
                    entity.GetComponent<ClientShootingControl>().Reset();
                    FacilityManager.Instance.ReturnClientToPool(entity);
                }).SetAutoKill(true);
                
                MoveEnemyFromCamp(entity);

                CountEnemyDead++;

                if (CountEnemyDead < TotalEnemies) return;
                FacilityManager.Instance.PlayerInfoUI.UpdateBattleProgress(1);
                FinishBattle(true);
            }
        }

        private void MoveEnemyFromCamp(GameObject entity)
        {
            for (var i = 0; i < BattleRoom.EnemiesList.Count; i++)
                if (BattleRoom.EnemiesList[i] == entity.GetComponent<Client>())
                {
                    BattleRoom.EnemiesList.RemoveAt(i);
                    break;
                }

            if (BattleRoom.ReinforceEnemiesList.Count <= 0) return;
            var newEnemy = BattleRoom.ReinforceEnemiesList[0];
            BattleRoom.ReinforceEnemiesList.RemoveAt(0);
            BattleRoom.EnemiesList.Add(newEnemy);

            var newEnemyShoot = newEnemy.GetComponent<ClientShootingControl>();
            newEnemy.gameObject.layer = LayerMask.NameToLayer(EnemiesLayerName);
            newEnemyShoot.SetTargetList(BattleRoom.alliesList);
            newEnemyShoot.HideBody();
            newEnemy.transform.position = entity.transform.position;
            newEnemy.transform.rotation = entity.transform.rotation;
            newEnemyShoot.gameObject.SetActive(true);
            DOVirtual.DelayedCall(1, () =>
            {
                if (!isRunningBattle) return;
                newEnemyShoot.ShowReborn(EnemyBulletsLayerName);
                newEnemyShoot.UpdateHp(newEnemyShoot.enemyData.hp);
            }).SetAutoKill(true);
        }

        private void MoveAllyFromCamp(GameObject entity)
        {
            for (var i = 0; i < BattleRoom.alliesList.Count; i++)
                if (BattleRoom.alliesList[i] == entity.GetComponent<Client>())
                {
                    BattleRoom.alliesList.RemoveAt(i);
                    break;
                }

            if (BattleRoom.reinforceAlliesList.Count > 0)
            {
                var newAlly = BattleRoom.reinforceAlliesList[0];
                BattleRoom.reinforceAlliesList.RemoveAt(0);
                BattleRoom.alliesList.Add(newAlly);

                newAlly.gameObject.layer = LayerMask.NameToLayer(AlliesLayerName);
                var newAllyShot = newAlly.GetComponent<ClientShootingControl>();
                newAlly.GetComponent<ClientShootingControl>().SetTargetList(BattleRoom.EnemiesList);
                var level = newAlly.GetComponent<Client>().Level;

                newAllyShot.HideBody();
                newAllyShot.transform.position = entity.transform.position;
                newAllyShot.transform.rotation = entity.transform.rotation;
                newAllyShot.gameObject.SetActive(true);
                DOVirtual.DelayedCall(1, () =>
                {
                    if (isRunningBattle)
                    {
                        newAllyShot.ShowReborn(AllyBulletsLayerName);
                        var clientHp = 3;
                        switch (level)
                        {
                            case 1:
                                clientHp = 3;
                                break;
                            case 2:
                                clientHp = 6;
                                break;
                            case 3:
                                clientHp = 9;
                                break;
                        }

                        newAllyShot.UpdateHp(clientHp);
                    }
                }).SetAutoKill(true);
            }
        }

        public void TimmingThrowClient()
        {
            if (isRunningBattle)
            {
                float randomTime = Random.Range(3f, 5f);
                _counterClientThrowGrenade.Start(() =>
                {
                    if (isRunningBattle)
                    {
                        ThrowGrenade(CurrentAlliesList);
                        TimmingThrowClient();
                    }
                }, randomTime);
            }
        }

        public void TimmingThrowEnemy()
        {
            if (isRunningBattle)
            {
                float randomTime = Random.Range(3f, 5f);
                _counterEnemyThrowGrenade.Start(() =>
                {
                    if (isRunningBattle)
                    {
                        ThrowGrenade(CurrentEnemiesList);
                        TimmingThrowEnemy();
                    }
                }, randomTime);
            }
        }


        public void ThrowGrenade(List<WarBaseClient> listObject)
        {
            if (isRunningBattle)
            {
                List<WarBaseClient> aliveAllies = new List<WarBaseClient>();

                for (int i = 0; i < listObject.Count; i++)
                {
                    if (!listObject[i].GetComponent<ClientShootingControl>().IsDead)
                    {
                        aliveAllies.Add(listObject[i]);
                    }
                }

                if (aliveAllies.Count > 0)
                {
                    int randomIndex = Random.Range(0, aliveAllies.Count);
                    Client randomAlly = aliveAllies[randomIndex];
                    randomAlly.GetComponent<ClientShootingControl>().SpawnGrenade();
                }
            }
        }

        #region START SHOOTING ALILITY

        public void StartShootingEnemyAbility(BattleRoom battleRoom)
        {
            var numberTank = battleRoom.EnemyTanks.Count;
            var numberAmored = battleRoom.EnemyArmoreds.Count;
            var numberMissile = battleRoom.EnemyMissiles.Count;

            if (numberTank > 0)
                for (var i = 0; i < numberTank; i++)
                {
                    battleRoom.EnemyTanks[i].gameObject.layer = LayerMask.NameToLayer(EnemyBulletsLayerName);
                    battleRoom.EnemyTanks[i].SetTargetsList(battleRoom.alliesList);
                    battleRoom.EnemyTanks[i].SetIsShootingTank(true);
                    battleRoom.EnemyTanks[i].StartShootingTank(battleRoom, 2);
                }

            if (numberMissile > 0)
                for (var i = 0; i < numberMissile; i++)
                {
                    battleRoom.EnemyMissiles[i].gameObject.layer = LayerMask.NameToLayer(EnemyBulletsLayerName);
                    battleRoom.EnemyMissiles[i].SetTargetsList(battleRoom.alliesList);
                    BattleRoom.EnemyMissiles[i].SetIsShooting(true);
                    // numberFaction = 2 => Client Zone
                    battleRoom.EnemyMissiles[i].StartShootingMissile(battleRoom, 2);
                }

            if (numberAmored > 0)
                for (var i = 0; i < numberAmored; i++)
                {
                    battleRoom.EnemyArmoreds[i].gameObject.layer = LayerMask.NameToLayer(EnemyBulletsLayerName);
                    battleRoom.EnemyArmoreds[i].SetTargetList(battleRoom.alliesList);
                    battleRoom.EnemyArmoreds[i].StartFighting(EnemyBulletsLayerName);
                    battleRoom.EnemyArmoreds[i].StartShooting();
                }
        }

        public void StartShootingClientAbility(BattleRoom battleRoom)
        {
            var numberTank = battleRoom.ClientTanks.Count;
            var numberAmored = battleRoom.ClientArmoreds.Count;
            var numberMissile = battleRoom.ClientMissiles.Count;

            if (numberTank > 0)
                for (var i = 0; i < numberTank; i++)
                    if (battleRoom.ClientTanks[i].gameObject.activeSelf)
                    {
                        battleRoom.ClientTanks[i].gameObject.layer = LayerMask.NameToLayer(AlliesLayerName);
                        battleRoom.ClientTanks[i].SetIsShootingTank(true);
                        battleRoom.ClientTanks[i].SetTargetsList(battleRoom.EnemiesList);
                        // numberFaction = 1 => Enemy Zone
                        battleRoom.ClientTanks[i].StartShootingTank(battleRoom, 1);
                    }

            if (numberMissile > 0)
            {
                for (var i = 0; i < numberMissile; i++)
                    if (battleRoom.ClientMissiles[i].gameObject.activeSelf)
                    {
                        battleRoom.ClientMissiles[i].gameObject.layer = LayerMask.NameToLayer(AlliesLayerName);
                        BattleRoom.ClientMissiles[i].SetIsShooting(true);
                        battleRoom.ClientMissiles[i].SetTargetsList(battleRoom.EnemiesList);
                        battleRoom.ClientMissiles[i].StartShootingMissile(battleRoom, 1);
                    }
            }

            if (numberAmored > 0)
                for (var i = 0; i < numberAmored; i++)
                    if (battleRoom.ClientArmoreds[i].gameObject.activeSelf)
                    {
                        battleRoom.ClientArmoreds[i].gameObject.layer = LayerMask.NameToLayer(AlliesLayerName);
                        battleRoom.ClientArmoreds[i].SetTargetList(battleRoom.EnemiesList);
                        battleRoom.ClientArmoreds[i].StartFighting(AllyBulletsLayerName);
                        battleRoom.ClientArmoreds[i].StartShooting();
                    }
        }

        #endregion

        #region STOP FIGHTING ALIBITY

        public void StopShootingEnemyAbility(BattleRoom battleRoom)
        {
            StopShootingUnits(battleRoom.EnemyTanks, battleRoom.EnemyMissiles, battleRoom.EnemyArmoreds);
        }

        private static void StopShootingClientAbility(BattleRoom battleRoom)
        {
            StopShootingUnits(battleRoom.ClientTanks, battleRoom.ClientMissiles, battleRoom.ClientArmoreds);
        }

        private static void StopShootingUnits(List<Tank> tanks, List<Missile> missiles, List<Armored> armoreds)
        {
            if (tanks is { Count: > 0 })
                foreach (var tank in tanks)
                    tank.StopShoot();

            if (missiles is { Count: > 0 } )
                foreach (var missile in missiles)
                    missile.SetIsShooting(false);

            if (armoreds is not { Count: > 0 }) return;
            foreach (var armored in armoreds)
                armored.StopShoot();
        }

        #endregion

        #region REMOVE CLIENT ABILITY

        private static void DestroyEnemyAbility(BattleRoom battleRoom)
        {
            if (battleRoom.EnemyTanks is { Count: > 0 })
                // Lặp qua tất cả các phần tử trong _tankEnemy
                foreach (var tank in battleRoom.EnemyTanks.Where(tank => tank != null))
                {
                    tank.IsLose();
                    DOVirtual.DelayedCall(2.0f, () => HideAbility(tank.gameObject));
                }

            if (battleRoom.EnemyArmoreds != null && battleRoom.EnemyArmoreds.Count > 0)
                foreach (var armored in battleRoom.EnemyArmoreds)
                {
                    armored.IsLose();
                    DOVirtual.DelayedCall(2.0f, () => HideAbility(armored.gameObject));
                }

            if (battleRoom.EnemyMissiles is not { Count: > 0 }) return;
            foreach (var missile in battleRoom.EnemyMissiles)
            {
                missile.IsLose();
                DOVirtual.DelayedCall(2.0f, () => HideAbility(missile.gameObject));
            }
        }


        private static void HideAbility(GameObject gameObject)
        {
            // Destroy(gameObject);
            gameObject.SetActive(false);
        }

        #endregion
    }
}