using Cinemachine;
using UnityEngine;

namespace Yun.Scripts.Cameras
{
    [ExecuteInEditMode]
    public class CircularPathGenerator : MonoBehaviour
    {
        public float radius = 5f;
        public int numberOfPoints = 8;
    
        private CinemachineSmoothPath path;

        private void OnValidate()
        {
            path = GetComponent<CinemachineSmoothPath>();
            if (path == null) return;

            GenerateCircularPath();
        }

        private void GenerateCircularPath()
        {
            path.m_Waypoints = new CinemachineSmoothPath.Waypoint[numberOfPoints];

            for (var i = 0; i < numberOfPoints; i++)
            {
                var angle = i * (360f / numberOfPoints);
                var x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                var z = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                var waypoint = new CinemachineSmoothPath.Waypoint
                {
                    position = new Vector3(x, 0, z)
                };
                path.m_Waypoints[i] = waypoint;
            }

            path.InvalidateDistanceCache();
        }
    }
}