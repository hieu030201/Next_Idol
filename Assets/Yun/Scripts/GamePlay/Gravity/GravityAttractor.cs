using UnityEngine;

namespace Yun.Scripts.GamePlay.Gravity
{
    public class GravityAttractor : MonoBehaviour
    {
        public float gravity = -900.8f;
        public float rotationSpeed = 10f;
        public void Attract(Transform body)
        {
            Vector3 gravityDirection = (body.position - transform.position).normalized;
            Vector3 bodyUp = body.up;

            body.GetComponent<Rigidbody>().AddForce(gravityDirection * gravity);

            Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityDirection) * body.rotation;
            body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
        }

        private void Update()
        {
            // transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
    }
}
