using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Yun.Scripts.Core
{
    public class YunCameraController
    {
        private CinemachineBrain _cineMachineBrain;
        private List<CinemachineVirtualCamera> _cameraList = new();
        private float _defaultTransitionTime;

        public YunCameraController(CinemachineBrain cineMachineBrain)
        {
            _cineMachineBrain = cineMachineBrain;
            _defaultTransitionTime = _cineMachineBrain.m_DefaultBlend.m_Time;
        }

        public CinemachineVirtualCameraBase CurrentCam;
        public void SwitchCamera(CinemachineVirtualCameraBase cam, float transitionTime = 0)
        {
            CurrentCam = cam;
            if (transitionTime == 0)
                transitionTime = _defaultTransitionTime;
            _cineMachineBrain.m_DefaultBlend.m_Time = transitionTime;
            // Debug.Log("SWITCH CAMERA: " + cam.name);
            cam.Priority = 10;
            foreach (var c in _cameraList.Where(c => c != cam))
            {
                c.Priority = 0;
            }
        }

        public void RegisterCamera(CinemachineVirtualCamera cam)
        {
            _cameraList.Add(cam);
        }

        public void UnregisterCamera(CinemachineVirtualCamera cam)
        {
            _cameraList.Remove(cam);
        }
    }
}
