using DG.Tweening;
using UnityEngine;
using Yun.Scripts.Audios;
using Yun.Scripts.Cores;
using Yun.Scripts.Datas.IdleGame;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Players
{
    public class ConnectClientControl : YunBehaviour
    {
        [SerializeField] private IdlePlayer player;
        private GameObject _inRoom;

        private Tween _tweenToConnectClient;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CheckGetInRoomArea>())
            {
                _inRoom = other.transform.parent.gameObject;
            }

            var parent = other.transform.parent;
            var client = parent.GetComponent<WarBaseClient>();
            if (!client) return;
            if (client.IsDeserting) return;
            if (!other.GetComponent<ConnectClientPoint>()) return;
            switch (client.EmotionState)
            {
                case WarBaseClient.ClientEmotionState.WaitToRest:
                case WarBaseClient.ClientEmotionState.WaitToEat:
                case WarBaseClient.ClientEmotionState.WaitToExercise:
                case WarBaseClient.ClientEmotionState.WaitToBattle:
                case WarBaseClient.ClientEmotionState.WaitToBoxing:
                case WarBaseClient.ClientEmotionState.WaitToTreating:
                    if (player.IsFullFollower)
                    {
                        var isSwapable = false;
                        foreach (var c in player.ClientsList)
                        {
                            if (c.EmotionState ==
                                _inRoom.GetComponent<BaseRoom>().emotionStateToGetIn)
                            {
                                isSwapable = true;
                            }
                        }

                        if (!isSwapable)
                            return;
                    }

                    client.ShowConnectAnim();
                    AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Loading_Soldier_MGB);
                    _tweenToConnectClient?.Kill();
                    _tweenToConnectClient = DOVirtual.DelayedCall(0.75f, (() =>
                    {
                        if ( /*!player.IsFullFollower && */!client.isConnectedToPlayer)
                        {
                            WarBaseClient swapClient = null;
                            foreach (var c in player.ClientsList)
                            {
                                if (c.EmotionState ==
                                    _inRoom.GetComponent<BaseRoom>().emotionStateToGetIn)
                                {
                                    swapClient = c;
                                    player.RemoveClient(c);
                                    break;
                                }
                            }

                            player.AddClient(client);
                            client.HideConnectPoint();
                            _inRoom.GetComponent<BaseRoom>().RemoveClient(client);
                            if (swapClient != null)
                                _inRoom.GetComponent<BaseRoom>().AddClient(swapClient);
                            
                            FacilityManager.Instance.GuidesManager.CheckShowFirstClientToFunctionRoomGuide(client);

                            if(client.EmotionState == WarBaseClient.ClientEmotionState.WaitToBattle)
                                FacilityManager.Instance.GuidesManager.CheckShowGuideLeadClientToBattleWhenConnectClient(client);
                        }
                        else
                        {
                            client.HideConnectAnim();
                        }
                    }));
                    break;
                case WarBaseClient.ClientEmotionState.WaitToUpgrade:
                    // FacilityManager.Instance.RemoveUpgradeClient(client);
                    QuestManager.CompleteOneTaskOfQuest(DailyQuestDataConfig.QuestId
                        .TrainRookie);

                    client.ShowUpgradeEffect();

                    client.EmotionState = WarBaseClient.ClientEmotionState.Idle;
                    client.LevelUp();
                    player.StopGuideArrowToClient(client);
                    FacilityManager.Instance.AddTokenWhenUpgrade(1);
                    DOVirtual.DelayedCall(1, (() =>
                    {
                        if (client.LastRoom.GetComponent<BedRoom>())
                        {
                            client.GetAvailableTask();
                        }
                        else
                        {
                            client.ShowWaitToRest();
                        }
                    })).SetAutoKill(true);
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<CheckGetInRoomArea>())
            {
                _inRoom = null;
            }

            var parent = other.transform.parent;

            var client = parent.GetComponent<WarBaseClient>();
            if (!client) return;
            if (!other.GetComponent<ConnectClientPoint>()) return;
            _tweenToConnectClient?.Kill();
            if (!client.isConnectedToPlayer)
            {
                client.HideConnectAnim();
            }
        }
    }
}