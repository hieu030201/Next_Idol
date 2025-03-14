using System;
using System.Collections.Generic;
using DG.Tweening;
using Hovl_Studio.Toon_Projectiles_2.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using Yun.Scripts.GamePlay.States.BossState_Level_1;
using Yun.Scripts.Managers;

namespace Yun.Scripts.GamePlay.Enemies
{
    public class BossLevel1 : Enemy
    {
        [SerializeField] private GameObject projectilePrefab;

        private List<Transform> _projectilePositionList;

        [Button]
        private void Test()
        {
            foreach (Transform projectilePosition in _projectilePositionList)
            {
                spawnOneProjectile(projectilePosition.position, projectilePosition.rotation);
            }
        }
    
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            StateMachine.GetComponent<BossStateMachineLevel1>().OnAttack += OnAttack;

            _projectilePositionList = new List<Transform>();
            for (var i = 0; i < transform.Find("projectilePositions").childCount; i++)
            {
                _projectilePositionList.Add(transform.Find("projectilePositions").GetChild(i).transform);
                transform.Find("projectilePositions").GetChild(i).gameObject.SetActive(false);
            }
        }
        
        public void FirstAttack()
        {
            // StateMachine.GetComponent<BossStateMachineLevel1>().FirstAttack();
        }

        private bool _isStartFollowPlayer;
        public void StartFollowPlayer(Transform _target)
        {
            _targetObject = _target;
            fixedDistance = Vector3.Distance(transform.localPosition, _target.localPosition);
            _isStartFollowPlayer = true;
            StateMachine.GetComponent<BossStateMachineLevel1>().Chase();
        }

        public float walkTime = 3;
        public void WalkToStartPoint(Vector3 startPoint)
        {
            StateMachine.GetComponent<BossStateMachineLevel1>().Walk();
            var tween = transform.DOLocalMove(startPoint, walkTime - 0.5f).OnComplete((() =>
            {
                StateMachine.GetComponent<BossStateMachineLevel1>().Scream();
            }));
            transform.DOLocalMove(startPoint, walkTime);
            AddTweenToTweenManager(tween);
        }

        private Transform _targetObject;     // Đối tượng cần theo dõi
        public float fixedDistance = 0;   // Khoảng cách cố định
        public bool followOnRight = true;
        
        private void LateUpdate()
        {
            
        }

        public void StopMoving()
        {
            _isStartFollowPlayer = false;
        }

        private void OnAttack()
        {
            foreach (var projectilePosition in _projectilePositionList)
            {
                spawnOneProjectile(projectilePosition.position, projectilePosition.rotation);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_isStartFollowPlayer)
            {
                // Lấy vị trí hiện tại của đối tượng này
                var currentPosition = transform.localPosition;

                // Tính toán vị trí mới trên trục X
                var newX = _targetObject.localPosition.x - (followOnRight ? fixedDistance : -fixedDistance);

                // Cập nhật vị trí, giữ nguyên Y và Z
                transform.localPosition = new Vector3(newX, currentPosition.y, currentPosition.z);
            }
        }
    
        private void spawnOneProjectile(Vector3 position, Quaternion rotation)
        {

            var projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<ProjectileMover>().powerNumber =
                projectilePrefab.GetComponent<ProjectileMover>().powerNumber;
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;
        }
    }
}
