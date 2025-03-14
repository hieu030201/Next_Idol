using DG.Tweening;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class TrainingRoom : BaseRoom
    {
        public override void AddClient(WarBaseClient client, bool immediately = false)
        {
            base.AddClient(client, immediately);
            if (WaitingClientsList.Count == 0)
            {
                if (immediately)
                {
                    var doneList = client.DoneList;
                    if (doneList[emotionStateToGetIn] < 2)
                        client.Exercise();
                    else
                        client.ShowWaitToRest();
                }
                else
                {
                    client.EmotionState = WarBaseClient.ClientEmotionState.Exercise;
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
            DOVirtual.DelayedCall(0.3f,
                (() =>
                {
                    client.EmotionState =
                        WarBaseClient.ClientEmotionState.Exercise;
                })).SetAutoKill(true);
            // client.ShowConnectPoint();
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