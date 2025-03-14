using System;
using DG.Tweening;
using UnityEngine;

namespace Yun.Scripts.GamePlay.Vehicles
{
    public class Tire : MonoBehaviour
    {
        private Vector3 _backLeftStartPosition;
        private Vector3 _backRightStartPosition;
        private Vector3 _frontLeftStartPosition;
        private Vector3 _frontRightStartPosition;

        private GameObject _backLeftTire;
        private GameObject _backRightTire;
        private GameObject _frontLeftTire;
        private GameObject _frontRightTire;
        
        private void Awake()
        {
            _backLeftTire = gameObject.transform.Find("Tires").Find("BackLeft").gameObject;
            _backRightTire = gameObject.transform.Find("Tires").Find("BackRight").gameObject;
            _frontLeftTire = gameObject.transform.Find("Tires").Find("FrontLeft").gameObject;
            _frontRightTire = gameObject.transform.Find("Tires").Find("FrontRight").gameObject;

            _backLeftStartPosition = _backLeftTire.transform.position;
            _backRightStartPosition = _backRightTire.transform.position;
            _frontLeftStartPosition = _frontLeftTire.transform.position;
            _frontRightStartPosition = _frontRightTire.transform.position;
        }

        private readonly Vector3 _putOnBackLeftStartPosition = new Vector3(-0.57f, 0.44f, -0.46f);
        private readonly Vector3 _putOnBackRightStartPosition = new Vector3(0.57f, 0.44f, -0.46f);
        private readonly Vector3 _putOnFrontLeftStartPosition = new Vector3(-0.57f, 0.44f, 0.58f);
        private readonly Vector3 _putOnFrontRightStartPosition = new Vector3(0.57f, 0.44f, 0.58f);
        private const float TimePutOn1 = 1;
        private const float TimePutOn2 = 1;
        private const float TimePutOn3 = 1;

        public void PutOn()
        {
            _backLeftTire.transform.localPosition = _putOnBackLeftStartPosition;
            _backRightTire.transform.localPosition = _putOnBackRightStartPosition;
            _frontLeftTire.transform.localPosition = _putOnFrontLeftStartPosition;
            _frontRightTire.transform.localPosition = _putOnFrontRightStartPosition;
            
            _backLeftTire.transform.DOLocalMoveY( _backLeftStartPosition.y,TimePutOn1);
            _backRightTire.transform.DOLocalMoveY( _backRightStartPosition.y,TimePutOn1);
            _frontLeftTire.transform.DOLocalMoveY( _frontLeftStartPosition.y,TimePutOn1);
            _frontRightTire.transform.DOLocalMoveY( _frontRightStartPosition.y,TimePutOn1);

            DOVirtual.DelayedCall(TimePutOn2, (() =>
            {
                _backLeftTire.transform.DOLocalMoveX( _backLeftStartPosition.x,TimePutOn3);
                _backRightTire.transform.DOLocalMoveX( _backRightStartPosition.x,TimePutOn3);
                _frontLeftTire.transform.DOLocalMoveX( _frontLeftStartPosition.x,TimePutOn3);
                _frontRightTire.transform.DOLocalMoveX( _frontRightStartPosition.x,TimePutOn3);
            }));
        }

        private const float DistanceTakeOff = 0.5f;
        private const float TimeTakeOff = 0.7f;

        public void TakeOff()
        {
            _backLeftTire.transform.DOLocalMoveX(_backLeftStartPosition.x - DistanceTakeOff,TimeTakeOff);
            _backRightTire.transform.DOLocalMoveX(_backRightStartPosition.x + DistanceTakeOff,TimeTakeOff);
            _frontLeftTire.transform.DOLocalMoveX(_frontLeftStartPosition.x - DistanceTakeOff,TimeTakeOff);
            _frontRightTire.transform.DOLocalMoveX(_frontRightStartPosition.x + DistanceTakeOff,TimeTakeOff);
        }
    }
}
