using UnityEngine;
using Yun.Scripts.Cores;
using Yun.Scripts.GamePlay.IdleGame.Employees;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class GetUpgradeWorkerPoint : YunBehaviour
    {
        [SerializeField] private GameObject hideContent;
        [SerializeField] private Employee employee;
        [SerializeField] private GameObject cameraPoint;

        public GameObject CameraPoint => cameraPoint;

        public bool IsActive
        {
            get => hideContent.activeSelf;
            set { hideContent.SetActive(value); }
        }

        public void ShowWorker()
        {
            employee.gameObject.transform.position = transform.position;
            employee.gameObject.transform.rotation = Quaternion.Euler(0, -90, 0);
            employee.gameObject.SetActive(true);
        }

        public void HideWorker()
        {
            employee.StopWorking();
        }

        public void ActiveWorker()
        {
            employee.GetComponent<UpgradeEmployee>().StartWorking();
        }

        protected override void Awake()
        {
            base.Awake();
            employee.gameObject.SetActive(false);
        }
    }
}