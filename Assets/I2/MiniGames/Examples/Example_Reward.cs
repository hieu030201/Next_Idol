using UnityEngine;
using System.Collections;
using DG.Tweening;
using I2.MiniGames;
using ScriptableObjectArchitecture;

public class Example_Reward : MonoBehaviour 
{
	public MiniGame _Game;
	public UnityEventString _OnResult = new UnityEventString();
	public StringGameEvent ShowRewardSpinEvent;
	public void CollectReward( int index )
	{
		var r1 = _Game.mRewards[index];
		var namePrize = string.Format("{0}", r1.name);
		ShowRewardSpinEvent.Raise($"{namePrize}_{index}");
		//DOVirtual.DelayedCall(0.5f, () => { SoundRewardSpin(); });
	}
}
