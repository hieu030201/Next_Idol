using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class HospitalRoom : BaseRoom
    {
        [SerializeField] private List<Transform> sleepPositions;

        public override void AddClient(WarBaseClient client, bool immediately = false)
        {
            base.AddClient(client, immediately);

            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] == client)
                {
                    client.SetSleepPosition(sleepPositions[i]);
                }
            }

            // Nếu không phải là add vào hàng chờ
            if (WaitingClientsList.Count == 0)
            {
                if (immediately)
                {
                    var doneList = client.DoneList;
                    if (doneList[emotionStateToGetIn] < 2)
                        client.Treating(true);
                    else
                        client.ShowWaitToRest();
                }
                else
                {
                    client.EmotionState = WarBaseClient.ClientEmotionState.InTreating;
                }
            }
            else
            {
                client.HideConnectPoint();
            }
        }

        private void AddClientFromWaiting(WarBaseClient client)
        {
            base.AddClient(client);

            for (var i = 0; i < ClientsList.Count; i++)
            {
                if (ClientsList[i] == client)
                {
                    client.SetSleepPosition(sleepPositions[i]);
                }
            }

            DOVirtual.DelayedCall(0.3f,
                (() =>
                {
                    client.EmotionState = WarBaseClient.ClientEmotionState.InTreating;
                })).SetAutoKill(true);
        }

        protected override void AddWaitingClient(WarBaseClient client, bool immediately = false)
        {
            base.AddWaitingClient(client, immediately);
            client.EmotionState = immediately ? WarBaseClient.ClientEmotionState.Idle : WarBaseClient.ClientEmotionState.RuningToSlot;
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