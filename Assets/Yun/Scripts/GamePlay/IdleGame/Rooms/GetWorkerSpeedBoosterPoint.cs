using System;
using Advertising;
using UnityEngine;
using Yun.Scripts.Core;
using Yun.Scripts.Cores;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public class GetWorkerSpeedBoosterPoint : YunBehaviour
    {
        [SerializeField] private GameObject hideContent;
        [SerializeField] private Animator animator1;
        [SerializeField] private Animator animator2;
        [SerializeField] private Animator animator3;

        protected override void Awake()
        {
            base.Awake();
            
            animator1.Play("Run_Sprint");
            animator2.Play("Run_Sprint");
            animator3.Play("Run_Sprint");
        }

        public bool IsActive
        {
            get => hideContent.activeSelf;
            set
            {
                hideContent.SetActive(value);
                if (value)
                {
                    animator1.Play("Run_Sprint");
                    animator2.Play("Run_Sprint");
                    animator3.Play("Run_Sprint");
                }
            }
        }
    }
}