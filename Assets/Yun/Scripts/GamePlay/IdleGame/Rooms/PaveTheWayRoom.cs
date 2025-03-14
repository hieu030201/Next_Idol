using DG.Tweening;
using Unity.AI.Navigation;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Clients;
using Yun.Scripts.GamePlay.IdleGame.Managers;

namespace Yun.Scripts.GamePlay.IdleGame.Rooms
{
    public sealed class PaveTheWayRoom : BaseRoom
    {
        [SerializeField] private GameObject retainingWall;

        public override void Hide(bool isUpdateNavmesh = true)
        {
            BuildPoint.IsActive = false;
            BuildPoint.SetLevelToActive(levelsPlayerToActive[0]);
        }

        public override void HideBuildPoint()
        {
            if (BuildPoint)
                BuildPoint.gameObject.SetActive(false);
        }

        public override void StartBuild(bool isBuildImmediately = false)
        {
            base.StartBuild(isBuildImmediately);
            retainingWall.SetActive(false);
        }
    }
}