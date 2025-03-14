using System.Collections.Generic;
using UnityEngine;
using Yun.Scripts.Datas.IdleGame;

namespace Yun.Scripts.GamePlay.IdleGame.Configs
{
    [CreateAssetMenu(fileName = "TestGameConfig", menuName = "GameConfig/TestGame")]
    public class TestGameConfig : ScriptableObject
    {
        public bool isForVideoRecording;
        public bool isShowLog;
        public bool isShowFPS;
        public bool isShowLogPrivate;
        public bool IsTestGame;
        public bool IsShowTestInSettingPopup;
        public bool isTestAds;
        public bool IsFastPlay;
        public bool IsSkipIntro;
        public bool isFastBattle;
        public bool isSkipGuide;
        public bool isSoundOn = true;
        public bool isMusicOn = true;
        public bool isTestAdsVip;
        public bool isTestWeakDevice;
        public List<AreaData> AreasList = new List<AreaData>();
        public float Speed = 8;
        public int Capacity = 1;
        public int Level = 1;
        public int LevelProgress = 1;
        public int Money = 1000;
    }
}
