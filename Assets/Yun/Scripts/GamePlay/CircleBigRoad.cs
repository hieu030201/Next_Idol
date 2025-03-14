using UnityEngine;

namespace Yun.Scripts.GamePlay
{
    public class CircleBigRoad : MonoBehaviour
    {
        public Vector3 rotationSpeed = new Vector3(0, 100, 0);

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (_isStartRotate)
                transform.Rotate(rotationSpeed * Time.deltaTime);
        }

        private bool _isStartRotate;

        public void StartRotate()
        {
            _isStartRotate = true;
        }
    }
}