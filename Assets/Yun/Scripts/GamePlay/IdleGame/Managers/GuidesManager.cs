using System.Linq;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Players;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Managers
{
    public class GuidesManager : YunBehaviour
    {
        [SerializeField] private GuidePoint startBattlePoint;
        [SerializeField] private GuidePoint boxingPoint;
        [SerializeField] private GuidePoint trainingPoint;
        [SerializeField] private GuidePoint eatingPoint;
        [SerializeField] private IdlePlayer player;
        [SerializeField] private CheckInRoom checkInRoom;
        [SerializeField] public WaitingRoom waitingRoom;

        public void ShowBuildCheckInRoomGuide()
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;
            if (FacilityManager.Instance.GameSaveLoad.GameData.isFinishedBattleGuide)
                return;
            player.ShowGuideArrowToPoint(checkInRoom.BuildPoint.GetComponent<GuidePoint>());
        }

        public void ShowBuildFirstBedRoomGuide(BedRoom bedRoom)
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;
            if (FacilityManager.Instance.GameSaveLoad.GameData.isFinishedBattleGuide)
                return;
            player.ShowGuideArrowToPoint(bedRoom.BuildPoint.GetComponent<GuidePoint>());
        }

        public void CheckShowGuideLeadClientToBedRoom()
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;
            if (FacilityManager.Instance.GameSaveLoad.GameData.isFinishedBattleGuide)
                return;
            if (player.GuidePoint != waitingRoom.ExitPoint.GetComponent<GuidePoint>())
                return;
            var bedRoom =
                FacilityManager.Instance.GetCurrentFunctionRoom(WarBaseClient.ClientEmotionState.WaitToRest, false);
            player.ShowGuideArrowToPoint(bedRoom.EntryPoint.GetComponent<GuidePoint>());
        }

        private void ShowGuideConnectClient(Client client)
        {
            if (player.GuideClient)
                return;

            player.ShowGuideArrowToClient(client);
        }

        public void CheckShowGuideToWaitingRoom()
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;
            if (FacilityManager.Instance.GameSaveLoad.GameData.isFinishedBattleGuide)
                return;
            if (player.GuidePoint != checkInRoom.ExitPoint.GetComponent<GuidePoint>())
                return;
            player.StopGuideArrowToPoint(player.GuidePoint);
            DOVirtual.DelayedCall(2,
                    (() => { player.ShowGuideArrowToPoint(waitingRoom.ExitPoint.GetComponent<GuidePoint>()); }))
                .SetAutoKill(true);
        }

        // Hiện guide dẫn lính ra battle 1 sau khi connect lính
        public void CheckShowGuideLeadClientToBattleWhenConnectClient(Client client)
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;

            if (FacilityManager.Instance.GameSaveLoad.GameData.isFinishedBattleGuide)
                return;

            var battleRoom = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();

            if (!battleRoom || !battleRoom.isFirstBattle)
                return;

            var bedRoom = client.LastRoom;

            switch (battleRoom.CountClients())
            {
                case 0:
                    if (!FacilityManager.Instance.GameSaveLoad.GameData.isShowedCountSoldierTutorial)
                    {
                        FacilityManager.Instance.GameSaveLoad.GameData.isShowedCountSoldierTutorial = true;
                        FacilityManager.Instance.PlayerInfoUI.ShowCountSoldierTutorial();
                    }

                    player.GetComponent<IdlePlayer>()
                        .ShowGuideArrowToPoint(battleRoom.EntryPoint.GetComponent<GuidePoint>());
                    foreach (var c in bedRoom.ClientsList.Where(c => c && c != client))
                    {
                        c.HideGuideArrowOnPoint();
                    }

                    break;
                case 1:
                case 2:
                    player.GetComponent<IdlePlayer>()
                        .ShowGuideArrowToPoint(battleRoom.EntryPoint.GetComponent<GuidePoint>());
                    foreach (var c in bedRoom.ClientsList.Where(c => c && c != client))
                    {
                        c.HideGuideArrowOnPoint();
                    }

                    break;
            }
        }

        // Hiện hướng dẫn khi show nút start battle lần đầu
        public void CheckShowStartBattleGuide(BattleRoom room)
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;
            room.ShowStartBattlePoint();
            player.ShowGuideArrowToPoint(room.StartBattlePoint.GetComponent<GuidePoint>());
        }

        // Kiểm tra xem có show hướng dẫn về phòng ngủ dẫn lính ra battle không
        public void CheckShowGuideConnectClientAndLeadToBattle(BattleRoom battleRoom)
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;
            if (FacilityManager.Instance.GameSaveLoad.GameData.isFinishedBattleGuide)
                return;
            if (!battleRoom)
                return;
            if (!battleRoom.isFirstBattle)
                return;
            if (battleRoom.IsUnlocked)
                return;

            if (battleRoom.CountClients() < 3)
            {
                // Kiểm tra xem phòng ngủ có lính ko thì guide về phòng ngủ dẫn lính
                var bedRoom =
                    FacilityManager.Instance.GetCurrentFunctionRoom(WarBaseClient.ClientEmotionState.WaitToRest, false);
                if (!bedRoom)
                    return;
                foreach (var client in bedRoom.ClientsList)
                {
                    if (!client) continue;
                    if (client.EmotionState ==
                        WarBaseClient.ClientEmotionState.WaitToBattle)
                    {
                        ShowGuideConnectClient(client);
                        return;
                    }

                    if (client.EmotionState !=
                        WarBaseClient.ClientEmotionState.WaitToUpgrade) continue;
                    if (player.ClientsList.Count == 0)
                        player.ShowGuideArrowToClient(client);
                    return;
                }

                // Nếu phòng ngủ không có lính thì guide về phòng chờ dẫn lính
                if (waitingRoom.CountClients() > 0)
                {
                    player.ShowGuideArrowToPoint(waitingRoom.ExitPoint.GetComponent<GuidePoint>());
                    return;
                }

                // Nếu phòng ngủ và phòng chờ ko có lính thì guide về phòng check in dẫn lính
                player.ShowGuideArrowToPoint(checkInRoom.ExitPoint.GetComponent<GuidePoint>());
            }
            else if (battleRoom.CountClients() == 3)
            {
                if (player.GuidePoint != battleRoom.GetComponent<BattleRoom>().BuildTankPoint1
                        .GetComponent<GuidePoint>() && !battleRoom.StartBattlePoint.activeSelf)
                {
                    player.ShowGuideArrowToPoint(battleRoom.GetComponent<BattleRoom>().BuildTankPoint1
                        .GetComponent<GuidePoint>());
                    player.FollowTarget(battleRoom.GetComponent<BattleRoom>().StandPoint3.transform);
                }
            }
            else if (battleRoom.CountClients() > 3)
            {
                battleRoom.GetComponent<BattleRoom>().ShowStartBattlePoint();
                player.ShowGuideArrowToPoint(battleRoom.GetComponent<BattleRoom>().StartBattlePoint
                    .GetComponent<GuidePoint>());
                battleRoom.GetComponent<BattleRoom>().StartBattlePoint.GetComponent<GuidePoint>()
                    .ShowGuideArrow();
            }
        }

        public void CheckShowGuideForFirstClientAction(WarBaseClient client)
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;

            // Kiểm tra xem có phải lần đầu lính có task về 1 phòng chức năng không
            var countClientsDict = FacilityManager.Instance.GameSaveLoad.GameData.CountClientActionDictionary;
            var clientState = client.EmotionState;
            countClientsDict.TryAdd(clientState, 0);
            if (countClientsDict.ContainsKey(clientState) && countClientsDict[clientState] == 0)
            {
                player.ShowGuideArrowToClient(client);
                countClientsDict[clientState] = 1;
            }
        }

        public void CheckShowUpgradeWorkerGuide(GameObject getUpgradeWorkerPoint)
        {
            if (!FacilityManager.Instance.GameSaveLoad.GameData.isFirstTimeShowUpgradeWorker ||
                FacilityManager.Instance.MapIndex != 1) return;
            var isDelay = FacilityManager.Instance.BattleManager.isRunningBattle;
            if (isDelay)
            {
                var tween = DOVirtual.DelayedCall(10, (() => { CheckShowUpgradeWorkerGuide(getUpgradeWorkerPoint); }))
                    .SetAutoKill(true);
                AddTweenToTweenManager(tween);
                return;
            }

            FacilityManager.Instance.CamerasManager.FocusCameraToGetUpgradeWorkerPoint(
                getUpgradeWorkerPoint.GetComponent<GetUpgradeWorkerPoint>());
            FacilityManager.Instance.GameSaveLoad.GameData.isFirstTimeShowUpgradeWorker = false;
            player.ShowGuideArrowToPoint(getUpgradeWorkerPoint.GetComponent<GuidePoint>());
        }

        public void CheckShowVipSoldierGuide(GameObject getVipSoldierPoint)
        {
            if (FacilityManager.Instance.GameSaveLoad.GameData.isFirstTimeShowVipSoldier &&
                FacilityManager.Instance.MapIndex == 1)
            {
                FacilityManager.Instance.CamerasManager.SwitchToFollowCamera();
                FacilityManager.Instance.GameSaveLoad.GameData.isFirstTimeShowVipSoldier = false;
                player.ShowGuideArrowToPoint(getVipSoldierPoint.GetComponent<GuidePoint>());
            }
        }

        // Kiểm tra xem có phải lần đầu lính có task về 1 phòng chức năng thì show mũi tên guide
        public void CheckShowFirstClientToFunctionRoomGuide(WarBaseClient client)
        {
            var countClientsDict = FacilityManager.Instance.GameSaveLoad.GameData
                .CountClientActionDictionary;
            if (countClientsDict.ContainsKey(client.EmotionState) &&
                countClientsDict[client.EmotionState] == 1)
            {
                if (FacilityManager.Instance.MapIndex == 1)
                {
                    var room = FacilityManager.Instance.GetCurrentFunctionRoom(client.EmotionState, false);
                    player.ShowGuideArrowToPoint(room.EntryPoint.GetComponent<GuidePoint>());

                    var newList = FacilityManager.Instance.GetAllClientByState(client.EmotionState);
                    foreach (var c in newList)
                    {
                        c.HideGuideArrowOnPoint();
                    }
                }

                countClientsDict[client.EmotionState] = 2;
            }
        }

        public void CheckShowBattleFundGuide()
        {
            var room = FacilityManager.Instance.BattleManager.GetCurrentBattleRoom();
            if (room.name == "Battle_Room_2")
            {
                DOVirtual.DelayedCall(6f, () =>
                {
                    if (!FacilityManager.Instance.GameSaveLoad.GameData.isShowedBattleFundTutorial &&
                        FacilityManager.Instance.MapIndex == 1)
                    {
                        FacilityManager.Instance.PlayerInfoUI.ShowBattleFundTutorial();
                        FacilityManager.Instance.GameSaveLoad.GameData.isShowedBattleFundTutorial = true;
                    }

                    FacilityManager.Instance.PlayerInfoUI.SetActiveBattlePassObj(true);
                });
            }
        }

        public void OnPointShowGuide(GuidePoint point)
        {
            if (FacilityManager.Instance.MapIndex != 1)
                return;
            point.ShowGuideArrow();
            player.ShowGuideArrowToPoint(point);
        }
    }
}