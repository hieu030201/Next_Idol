using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class WaitingRoom : BaseRoom
    {
        public override void AddClient(WarBaseClient client, bool immediately = false)
        {
            base.AddClient(client, immediately);
            client.EmotionState = WarBaseClient.ClientEmotionState.WaitToRest;
        }
    }
}
