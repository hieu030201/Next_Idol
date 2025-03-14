using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Employees;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class UpgradeRoom : BaseRoom
    {
        [SerializeField] private Employee employee;
        [SerializeField] private Employee employee2;
        [SerializeField] private ParticleSystem upgradeEffect;
        [SerializeField] private ParticleSystem buyWorkerEffect;
        [SerializeField] public GameObject workerPoint;

        public override void Init()
        {
            base.Init();
            
            upgradeEffect.gameObject.SetActive(false);
            buyWorkerEffect.gameObject.SetActive(false);
            employee.gameObject.SetActive(false);
            employee2.gameObject.SetActive(false);

            BuildElementsList.Add(employee.gameObject);
            BuildElementsList.Add(employee2.gameObject);
            if (transform.Find("Upgrade_Point"))
            {
                transform.Find("Upgrade_Point").gameObject.SetActive(false);
                BuildElementsList.Add(transform.Find("Upgrade_Point").gameObject);
            }

            if (!transform.Find("Buy_Worker_Point")) return;
            transform.Find("Buy_Worker_Point").gameObject.SetActive(false);
            BuildElementsList.Add(transform.Find("Buy_Worker_Point").gameObject);
        }

        public override void StartBuild(bool isBuildImmediately = false)
        {
            base.StartBuild(isBuildImmediately);
            DOVirtual.DelayedCall(4, (() =>
            {
                employee.Computer();
                if (isBuildImmediately)
                {
                    FacilityManager.Instance.CheckCanUpgrade();
                    FacilityManager.Instance.CheckCanBuyWorker();
                }
            }));
            DOVirtual.DelayedCall(3, (() => { employee2.Computer(); }));
        }

        public void ShowUpgradeEffect()
        {
            upgradeEffect.gameObject.SetActive(true);
        }

        public void HideUpgradeEffect()
        {
            upgradeEffect.gameObject.SetActive(false);
        }

        public void ShowBuyWorkerEffect()
        {
            buyWorkerEffect.gameObject.SetActive(true);
        }

        public void HideBuyWorkerEffect()
        {
            buyWorkerEffect.gameObject.SetActive(false);
        }
    }
}