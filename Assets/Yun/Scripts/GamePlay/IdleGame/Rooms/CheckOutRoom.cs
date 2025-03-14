using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class CheckOutRoom : BaseRoom
    {
        [SerializeField] private Animator employeeAnimator;
        [SerializeField] private StarPoint starPoint;

        [Button]
        public void TestSpawnStar()
        {
            SpawnStar(10);    
        }
        
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            // employeeAnimator.Play("Sit");
        }
        
        public void SpawnStar(int star)
        {
            starPoint.SpawnStar(star,EntryPoint.transform.position);
        }

        public int WithDrawStar()
        {
            return starPoint.WithdrawStar();
        }
    }
}
