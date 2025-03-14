using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class BoxingRoom : BaseRoom
    {
        [SerializeField] private GameObject weightLiftPositions;
        [SerializeField] private GameObject dumbble1;
        [SerializeField] private GameObject dumbble2;
        [SerializeField] private Renderer nativeAdWall;

        public override void AddClient(WarBaseClient client, bool immediately = false)
        {
            base.AddClient(client, immediately);
            if (WaitingClientsList.Count == 0)
            {
                if (immediately)
                {
                    var doneList = client.DoneList;
                    if(doneList[emotionStateToGetIn] < 2)
                        client.Boxing(true);
                    else
                        client.ShowWaitToRest();
                }
                else
                {
                    client.EmotionState = WarBaseClient.ClientEmotionState.Boxing;
                }
            }
            else
            {
                client.HideConnectPoint();
            }

            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] == client)
                {
                    switch (i)
                    {
                        case 0:
                            client.SetWeightLiftPosition(weightLiftPositions.transform.GetChild(0), dumbble1);
                            break;
                        case 2:
                            client.SetWeightLiftPosition(weightLiftPositions.transform.GetChild(1), dumbble2);
                            break;
                    }
                }
            }
        }

        private void AddClientFromWaiting(WarBaseClient client)
        {
            base.AddClient(client, false);
            DOVirtual.DelayedCall(0.3f,
                (() =>
                {
                    client.EmotionState = WarBaseClient.ClientEmotionState.Boxing;
                })).SetAutoKill(true);

            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] == client)
                {
                    switch (i)
                    {
                        case 0:
                            client.SetWeightLiftPosition(weightLiftPositions.transform.GetChild(0), dumbble1);
                            break;
                        case 2:
                            client.SetWeightLiftPosition(weightLiftPositions.transform.GetChild(1), dumbble2);
                            break;
                    }
                }
            }
        }

        protected override void AddWaitingClient(WarBaseClient client, bool immediately = false)
        {
            base.AddWaitingClient(client, immediately);
            if (immediately)
            {
                client.EmotionState = WarBaseClient.ClientEmotionState.Idle;
            }
            else
            {
                client.EmotionState = WarBaseClient.ClientEmotionState.RuningToSlot;
            }
        }

        public override void RemoveClient(WarBaseClient client, bool isKeepPosition = false, float delay = 0)
        {
            base.RemoveClient(client, true, delay);
            client.HideConnectPoint();
            if (WaitingClientsList.Count <= 0) return;
            var waitingClient = WaitingClientsList[0];
            RemoveWaitingClient(waitingClient);
            AddClientFromWaiting(waitingClient);
        }
    }
}