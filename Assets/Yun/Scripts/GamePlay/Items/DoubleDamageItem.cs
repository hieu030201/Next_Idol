using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.Vehicles;

namespace Yun.Scripts.GamePlay.Items
{
    public class DoubleDamageItem : Item
    {
        public override void Active(YunVehicle vehicle)
        {
            vehicle.UpdatePowerMultiplier(3);
            var tween = DOVirtual.DelayedCall(duration, () =>
            {
                vehicle.UpdatePowerMultiplier(1);
                Destroy(gameObject);
            });
            AddTweenToTweenManager(tween);
        }
    }
}
