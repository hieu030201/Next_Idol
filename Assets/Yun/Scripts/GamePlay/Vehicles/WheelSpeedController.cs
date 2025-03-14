using UnityEngine;

namespace Yun.Scripts.GamePlay.Vehicles
{
    public class WheelSpeedController : MonoBehaviour
    {
        public WheelCollider[] wheels;
        public float desiredSpeed = 10f; // Tốc độ mong muốn (m/s)
        public float kp = 5f;
        public float ki = 1f;
        public float kd = 0.5f;

        private float integral;
        private float previousError;

        void FixedUpdate()
        {
            foreach (WheelCollider wheel in wheels)
            {
                float currentSpeed = wheel.rpm * (2 * Mathf.PI / 60) * wheel.radius; // Tính vận tốc hiện tại (m/s)
                float error = desiredSpeed - currentSpeed;
                integral += error * Time.fixedDeltaTime;
                float derivative = (error - previousError) / Time.fixedDeltaTime;

                float output = kp * error + ki * integral + kd * derivative;
                wheel.motorTorque = output;

                previousError = error;
            }
        }
    }
}
