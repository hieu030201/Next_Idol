using Cinemachine;
using UnityEngine;

namespace Yun.Scripts.Cameras
{
    public class DollyCameraController : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;
        public float speed = 0.5f;

        private CinemachineTrackedDolly dolly;

        void Start()
        {
            dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }

        private bool _isStartMoving;
        public void StartMoving()
        {
            _isStartMoving = true;
        }

        void Update()
        {
            if(!_isStartMoving)
                return;
            // Di chuyển camera dọc theo path
            dolly.m_PathPosition += speed * Time.deltaTime;

            // Kiểm tra xem đã đến cuối path chưa
            if (dolly.m_PathPosition >= dolly.m_Path.PathLength)
            {
                if (dolly.m_Path.Looped)
                {
                    // Nếu loop, quay lại điểm bắt đầu
                    dolly.m_PathPosition %= dolly.m_Path.PathLength;
                }
                else
                {
                    // Nếu không loop, dừng tại điểm cuối
                    dolly.m_PathPosition = dolly.m_Path.PathLength;
                }
            }
        }
    }
}