using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.Datas.IdleGame
{
    [Serializable]
    public class CameraData
    {
        public Vector3 position;
        public Quaternion rotation;
        public float FOV;
    }
}
