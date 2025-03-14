using DG.Tweening;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.GamePlay.IdleGame.Rooms;

namespace Yun.Scripts.GamePlay.IdleGame.Employees
{
    public class CheckInEmployee : Employee
    {
        [SerializeField] private CheckInRoom checkInRoom;
        public override void StartWorking()
        {
            base.StartWorking();
            checkInRoom.ExitPoint.GetComponent<Collider>().enabled = false;
            InvokeRepeating(nameof(ActiveExitPointOfCheckInRoom),1.5f,1.5f);
            stateMachine.ActionState = EmployeeActionStateMachine.EmployeeActionState.IdleKhoanhTay;
        }

        protected override void FixedUpdate()
        {
            
        }

        private void ActiveExitPointOfCheckInRoom()
        {
            checkInRoom.CheckClientOut();
        }
    }
}
