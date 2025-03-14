namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class EntryPoint : CheckPoint
    {
        public override void StepIn()
        {
            if(!IsActive)
                return;
            base.StepIn();
        }

        public override void StepOut()
        {
            base.StepOut();
            IsActive = false;
        }
    }
}
