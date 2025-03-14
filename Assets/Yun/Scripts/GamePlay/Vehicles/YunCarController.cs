using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Managers;

namespace Yun.Scripts.GamePlay.Vehicles
{
    public class YunCarController : MonoBehaviour
    {
        [SerializeField] private Transform destination;
        [SerializeField] private WheelCollider frontRight;
        [SerializeField] private WheelCollider frontLeft;
        [SerializeField] private WheelCollider backRight;
        [SerializeField] private WheelCollider backLeft;

        [SerializeField] private Transform frontRightTransform;
        [SerializeField] private Transform frontLeftTransform;
        [SerializeField] private Transform backRightTransform;
        [SerializeField] private Transform backLeftTransform;
        
        public float breakingForce = 20f;
        public float maxBreakingForce = 300f;

        public float maxTurnAngle = 15f;

        public float motorTorque = 20f;
        public float maxSpeed = 2f;
        public Rigidbody carRigidbody;

        private float _currentTurnAngle = 0f;
        private bool _isTurningRight;
        private bool _isTurningLeft;
        private bool _isTowardToTheDestination;
        private bool _isReduceSpeed;
        private bool _isNoPhysics;
        
        [Button]
        private void UpdateWheelCollider(WheelCollider col, GameObject newGameObject)
        {
            // Cập nhật vị trí của WheelCollider
            col.center = transform.InverseTransformPoint(newGameObject.transform.localPosition);
            
            // Cập nhật kích thước
            var diameter = newGameObject.transform.lossyScale.y * 2;
            col.radius = diameter / 2;
            
            // Cập nhật hướng của WheelCollider
            // col.transform.rotation = newGameObject.transform.localRotation;
        }

        public void ReduceSpeed()
        {
            _isReduceSpeed = true;
            // MoveToDestination();
        }

        public void MoveToDestination()
        {
            _isNoPhysics = true;
            StopImmediately();
            transform.DOLocalMove(destination.transform.localPosition, 3);
            transform.DOLocalRotate(new Vector3(0,0,0), 0.3f);

            DOVirtual.DelayedCall(6, (() =>
            {
                carRigidbody.isKinematic = false;
            }));
        }

        private void StopImmediately()
        {
            carRigidbody.isKinematic = true;
            carRigidbody.velocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;
            /*frontLeft.enabled = false;
            frontRight.enabled = false;
            backLeft.enabled = false;
            backRight.enabled = false;*/
            UpdateWheel(frontLeft, frontLeftTransform,0,maxBreakingForce);
            UpdateWheel(frontRight, frontRightTransform,0,maxBreakingForce);
            UpdateWheel(backLeft, backLeftTransform,0,maxBreakingForce);
            UpdateWheel(backRight, backRightTransform,0,maxBreakingForce);
        }

        private bool _isDrifting;
        
        private float _startTime;

        private const float RotationDuration = 1f;

        private IEnumerator RotateOverTime()
        {
            var startRotation = carRigidbody.rotation;
            var endRotation = startRotation * Quaternion.Euler(0, 180, 0);
            var elapsedTime = 0f;

            while (elapsedTime < RotationDuration)
            {
                Debug.Log("before: " + elapsedTime);
                elapsedTime += Time.fixedDeltaTime;
                Debug.Log("after: " + elapsedTime);
                var t = elapsedTime / RotationDuration;
                carRigidbody.MoveRotation(Quaternion.Slerp(startRotation, endRotation, t));
                if (elapsedTime >= RotationDuration)
                {
                    _isDrifting = false;
                    _isReduceSpeed = false;
                    _isNoPhysics = false;
                    _isTowardToTheDestination = false;
                    // Debug.Log("AAAAAAAAAAAAAAAAAAAAAAA");
                }
                yield return new WaitForFixedUpdate();
            }

            carRigidbody.MoveRotation(endRotation);
        }

        private void FixedUpdate()
        {
            if(_isDrifting)
                return;
            
            if (_isNoPhysics)
            {
                return;
            }
            if (_isReduceSpeed)
            {
                /*var transform1 = transform;
                var targetDir = (this.destination.transform.position - transform1.position).normalized;
                var targetAngle = Vector3.SignedAngle(transform1.forward, targetDir, Vector3.up);

                // Giới hạn góc lái để xe không quay đầu quá nhanh
                var steerAngle = Mathf.Clamp(targetAngle, -maxTurnAngle, maxTurnAngle);

                frontLeft.steerAngle = steerAngle;
                frontRight.steerAngle = steerAngle;
                
                float mt2 = 0;
                float bf2 = 0;
                mt2 = 0;
                bf2 = breakingForce;
                if (transform.localPosition.z > destination.transform.localPosition.z - 5)
                {
                    Debug.Log("APPLY MAX BREAKING FORCE");
                    bf2 = maxBreakingForce;
                    _isNoPhysics = true;
                    GetComponent<Rigidbody>().isKinematic = true;
                    GameManager.Instance.StartRotateBigRoad();
                    transform.DOLocalMove(destination.transform.localPosition, 1);
                    var rotateX = transform.localRotation.x;
                    var rotateZ = transform.localRotation.z;
                    transform.DOLocalRotate(new Vector3(rotateX,0,rotateZ), 0.5f).OnComplete((() => GameManager.Instance.StartKillBoss()));
                }

                UpdateWheel(frontLeft, frontLeftTransform,mt2,bf2);
                UpdateWheel(frontRight, frontRightTransform,mt2,bf2);*/

                var abc = transform.localPosition.z - destination.transform.localPosition.z;
                // Debug.Log(abc);
                if (abc < -1)
                {
                    UpdateWheel(frontLeft, frontLeftTransform,0,breakingForce);
                    UpdateWheel(frontRight, frontRightTransform,0,breakingForce);
                    UpdateWheel(backRight, backRightTransform,0,breakingForce);
                    UpdateWheel(backLeft, backLeftTransform,0,breakingForce);
                    // Debug.Log("CCCCCCCCCCCCCCCCCCCCCCCCC " + distanceToTarget + ", " + motorTorque);
                }
                else
                {
                    var transform1 = transform;
                    var targetDir = (destination.localPosition - transform1.localPosition).normalized;
                    var targetAngle = Vector3.SignedAngle(transform1.forward, targetDir, Vector3.up);

                    // Giới hạn góc lái để xe không quay đầu quá nhanh
                    var steerAngle = Mathf.Clamp(targetAngle, -maxTurnAngle, maxTurnAngle);

                    frontLeft.steerAngle = steerAngle;
                    frontRight.steerAngle = steerAngle;
                    
                    var oppositeForce = -carRigidbody.velocity.normalized * 100000f;
                    carRigidbody.AddForce(oppositeForce, ForceMode.Force);
                    UpdateWheel(frontLeft, frontLeftTransform,0,maxBreakingForce);
                    UpdateWheel(frontRight, frontRightTransform,0,maxBreakingForce);
                    UpdateWheel(backLeft, backLeftTransform,0,maxBreakingForce);
                    UpdateWheel(backRight, backRightTransform,0,maxBreakingForce);
                    if (carRigidbody.velocity.magnitude < 1)
                    {
                        _isNoPhysics = true;
                        GetComponent<Rigidbody>().isKinematic = true;
                        GameManager.Instance.StartKillBoss();
                    }
                }
                
                return;
            }

            if (!_isTowardToTheDestination)
            {
                if (GetSteerInputFromTouch() < 0 && !_isTurningLeft && !_isTurningRight)
                {
                    _isTurningLeft = true;
                    Debug.Log("START TURNNING LEFT " + transform.eulerAngles.y);
                }

                if (GetSteerInputFromTouch() > 0 && !_isTurningLeft && !_isTurningRight)
                {
                    _isTurningRight = true;
                    Debug.Log("START TURNNING RIGHT " + transform.eulerAngles.y);
                }
            }
            
            // Debug.Log("eulerAngles " + transform.eulerAngles.y);

            if (_isTurningLeft)
            {
                // Debug.Log("IS TURNING LEFT " + transform.eulerAngles.y);
                _currentTurnAngle = maxTurnAngle * -1;
                frontLeft.steerAngle = _currentTurnAngle;
                frontRight.steerAngle = _currentTurnAngle;
                if (transform.eulerAngles.y < 350 && transform.eulerAngles.y > 200)
                {
                    Debug.Log("STOP TURNING LEFT");
                    _isTowardToTheDestination = true;
                    _isTurningLeft = false;
                    _isTurningRight = false;
                    DOVirtual.DelayedCall(0.5f, (() =>
                    {
                        frontLeft.steerAngle = 0;
                        frontRight.steerAngle = 0;
                        _isTowardToTheDestination = false;
                    }));
                }
            }

            if (_isTurningRight)
            {
                // Debug.Log("IS TURNING RIGHT");
                _currentTurnAngle = maxTurnAngle * 1;
                frontLeft.steerAngle = _currentTurnAngle;
                frontRight.steerAngle = _currentTurnAngle;
                if (transform.eulerAngles.y > 10 && transform.eulerAngles.y < 90)
                {
                    Debug.Log("STOP TURNING RIGHT");
                    _isTowardToTheDestination = true;
                    _isTurningLeft = false;
                    _isTurningRight = false;
                    DOVirtual.DelayedCall(0.5f, (() =>
                    {
                        frontLeft.steerAngle = 0;
                        frontRight.steerAngle = 0;
                        _isTowardToTheDestination = false;
                    }));
                }
            }

            if (_isTowardToTheDestination)
            {
                // Debug.Log("START TOWARD TO THE DESTINATION");
                // Tính toán hướng di chuyển mong muốn
                var destination = new Vector3(transform.localPosition.x, this.destination.localPosition.y,
                    this.destination.localPosition.z);
                Vector3 targetDir = (destination - transform.localPosition).normalized;
                var targetAngle = Vector3.SignedAngle(transform.forward, targetDir, Vector3.up);

                // Giới hạn góc lái để xe không quay đầu quá nhanh
                var steerAngle = Mathf.Clamp(targetAngle, -maxTurnAngle, maxTurnAngle);

                frontLeft.steerAngle = steerAngle;
                frontRight.steerAngle = steerAngle;
            }

            if (Input.GetAxis("Horizontal") != 0)
            {
                Debug.Log(frontLeft.steerAngle);
            }
            // Debug.Log(transform.rotation);

            float mt = 0;
            float bf = 0;
            
            mt = motorTorque;
            bf = 0;
            if (carRigidbody.velocity.magnitude > maxSpeed)
            {
                mt = 0;
            }

            // Debug.Log(mt + ", " + bf);
            UpdateWheel(frontLeft, frontLeftTransform,mt,bf);
            UpdateWheel(frontRight, frontRightTransform,mt,bf);
            
            // Debug.Log("SPEED " + carRigidbody.velocity.magnitude);
        }

        private static void UpdateWheel(WheelCollider col, Transform trans, float mt, float bf)
        {
            col.motorTorque = mt;
            col.brakeTorque = bf;
            col.GetWorldPose(out var position, out var rotation);
            trans.position = position;
            trans.rotation = rotation;
        }

        private static float GetSteerInputFromTouch()
        {
            if (Input.touchCount <= 0) return 0f;
            var touch = Input.GetTouch(0);

            if (touch.phase is not (TouchPhase.Moved or TouchPhase.Stationary)) return 0f;
            // Chuyển đổi vị trí chạm màn hình thành giá trị từ -1 đến 1
            var steerInput = (touch.position.x / Screen.width) * 2 - 1;
            return steerInput;
        }
    }
}