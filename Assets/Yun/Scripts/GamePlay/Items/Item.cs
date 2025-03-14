using Yun.Scripts.Core;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.Vehicles;

namespace Yun.Scripts.GamePlay.Items
{
    public abstract class Item : YunBehaviour
    {
        public float duration;
        public abstract void Active(YunVehicle vehicle);
    }
}
