using System;
using System.Collections.Generic;
using DG.Tweening;
using Hovl_Studio.Toon_Projectiles_2.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.Items;
using Yun.Scripts.GamePlay.Weapons;
using Yun.Scripts.Managers;
using Yun.Scripts.UI;
using Yun.Scripts.Utilities;

namespace Yun.Scripts.GamePlay.Vehicles
{
    public class YunVehicle : Player
    {
        [SerializeField] private int healthNumber;

        [SerializeField] private GameObject vehicleBody,
            weapon;

        [SerializeField] private GameObject tire;

        [SerializeField] private GameObject vehicleFollow;
        [SerializeField] private GameObject defense;
        [SerializeField] private GameObject trailPos1;
        [SerializeField] private GameObject trailPos2;
        [SerializeField] private GameObject trail1;
        [SerializeField] private GameObject trail2;

        public enum VehicleState
        {
            Intro,
            Race,
            ReduceSpeed,
            PreFightBoss,
            FightBoss,
            GameOver
        }

        private VehicleState _state;

        public VehicleState State
        {
            get => _state;
            set
            {
                _state = value;
                switch (value)
                {
                    case VehicleState.Intro:
                        break;
                    case VehicleState.Race:
                        trail1.SetActive(true);
                        trail2.SetActive(true);
                        break;
                    case VehicleState.ReduceSpeed:
                        trail1.SetActive(false);
                        trail2.SetActive(false);
                        break;
                    case VehicleState.PreFightBoss:
                        break;
                    case VehicleState.FightBoss:
                        trail1.SetActive(true);
                        trail2.SetActive(true);
                        break;
                    case VehicleState.GameOver:
                        trail1.SetActive(false);
                        trail2.SetActive(false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        private GameObject healthBar;
        private RectTransform canvasRect;
        private int _totalHealth;

        private GameObject _projectilePosition1;
        private GameObject _projectilePosition2;

        public bool isShootWhenStart = true;
        public float distanceToVehicleFollow = 10;

        [Button]
        private void TestShoot()
        {
            // spawnOneProjectile(_projectilePosition2.transform.position, _projectilePosition2.transform.rotation);
            weapon.GetComponent<Weapons.Weapon>().Shoot();
        }

        [SerializeField] private GameObject startPoint;

        [Button]
        private void MoveToPoint()
        {
            transform.localPosition = startPoint.transform.localPosition;
        }

        private Animator weaponAnimator
        {
            get => weapon.transform.Find("Animator").GetComponent<Animator>();
        }

        protected override void Awake()
        {
            healthBarPos.SetActive(false);
            Health = healthNumber;
            _totalHealth = Health;
            weaponAnimator.enabled = false;
            weapon.SetActive(false);
            defense.SetActive(false);
            trailPos1.SetActive(false);
            trailPos2.SetActive(false);
            trail1.SetActive(false);
            trail2.SetActive(false);
        }

        private void ShowLog(string log)
        {
            var currentString = GameManager.Instance.debugText.text;
            currentString += ", " + log;
            GameManager.Instance.debugText.text = currentString;
        }

        protected override void Start()
        {
            base.Start();
            UpdateProjectilePosition();
            canvasRect = CanvasManager.Instance.canvas.GetComponent<RectTransform>();
            healthBar = Instantiate(healthBarPrefab, CanvasManager.Instance.overlayUILayer.transform);
            healthBar.gameObject.SetActive(false);
        }

        private void UpdateProjectilePosition()
        {
            /*_projectilePosition1 = weapon.transform.Find("projectilePos1").gameObject;
            _projectilePosition2 = weapon.transform.Find("projectilePos2").gameObject;
            _projectilePosition1.SetActive(false);
            _projectilePosition2.SetActive(false);*/
        }

        private float totalDistance = 0; // Tổng khoảng cách di chuyển
        private float _currentSpeed;
        private float _accleration = 0.08f;
        private float _raceSpeed = 10;
        private float duration1 = 25f; // Thời gian di chuyển (giây)
        private float duration2 = 45f; // Thời gian di chuyển (giây)

        private float elapsedTime = 0f; // Thời gian đã trôi qua
        private Vector3 startPosition;
        private const float SpeedZ = 0.06f;
        private const float leftTargetAngle = 110;
        private const float rightTargetAngle = 70;
        private const float leftTargetAngle2 = -20;
        private const float rightTargetAngle2 = 20;
        private const float rotationSpeed = 0.3f; // Tốc độ quay
        private const float rotationBackSpeed = 1; // Tốc độ quay
        private float _lastPositionZ;
        private float _lastPositionX;
        private bool _isRotateLeft;
        private bool _isRotateRight;

        private Vector3 _raceDestination;

        public void ShowIntro()
        {
            State = VehicleState.Intro;
            weapon.SetActive(true);
            weaponAnimator.enabled = true;
            weaponAnimator.Play("appear");
        }

        public void StartRacing(Vector3 destination)
        {
            _raceDestination = destination;
            _raceDestination.y = transform.localPosition.y;
            elapsedTime = 0;
            _currentSpeed = 0;
            startPosition = transform.localPosition;
            totalDistance = Vector3.Distance(startPosition, destination);
            _raceSpeed = GameManager.Instance.VehiceSpeed;
            if (isShootWhenStart)
                Shoot();
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Acceleration);
            State = VehicleState.Race;
        }

        public void ReduceSpeed(float delayTime)
        {
            AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Deceleration);
            StopShoot();
            State = VehicleState.ReduceSpeed;
            transform.LookAt(_raceDestination);
            transform.DOLocalMove(_raceDestination, delayTime);
        }

        public void MoveToFightBossStartPosition(GameObject waypointsContainer)
        {
            var waypoints = new List<GameObject> {transform.gameObject};
            for (var i = 0; i < waypointsContainer.transform.childCount; i++)
            {
                waypoints.Add(waypointsContainer.transform.GetChild(i).gameObject);
            }

            var xR = waypoints[^1].transform.localRotation.eulerAngles.x;
            var yR = waypoints[^1].transform.localRotation.eulerAngles.y;
            var zR = waypoints[^1].transform.localRotation.eulerAngles.z;
            Transform transform1;
            (transform1 = transform).DOLocalRotate(new Vector3(xR, yR, zR), 1);

            var a = transform1.localPosition;
            var b = waypoints[1].transform.localPosition;
            var c = waypoints[2].transform.localPosition;
            var points = GeneralLogic.GetWaypointsFromThreePoints(a, b, c);
            transform.DOPath(points, 1, PathType.CatmullRom).SetEase(Ease.Linear).OnComplete((() => { }));
            State = VehicleState.PreFightBoss;
        }

        public void StartFightBoss(Vector3 fightBossDestination)
        {
            elapsedTime = 0;
            startPosition = transform.localPosition;
            totalDistance = Vector3.Distance(startPosition, fightBossDestination);
            State = VehicleState.FightBoss;
            Shoot();
        }

        public void GameOver()
        {
            State = VehicleState.GameOver;
            StopShoot();
        }

        private void Update()
        {
            if (_isChangingGun)
            {
                if (weaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= -1f)
                {
                    _isChangingGun = false;
                    Destroy(weapon);
                    weapon = Instantiate(_newGunPrefab, transform);
                    UpdateProjectilePosition();
                    weaponAnimator.enabled = true;
                    weaponAnimator.Play("appear");
                }
            }
            
            if (State == VehicleState.GameOver)
                return;

            if (State == VehicleState.FightBoss)
            {
                if (_boss)
                    weapon.transform.LookAt(_boss.transform);
                
                trail1.transform.position = trailPos1.transform.position;
                trail1.transform.rotation = trailPos1.transform.rotation;
                trail2.transform.position = trailPos2.transform.position;
                trail2.transform.rotation = trailPos2.transform.rotation;

                elapsedTime += Time.deltaTime;

                if (elapsedTime < duration2)
                {
                    var newZ = transform.localPosition.z;

                    if (Input.touchCount > 0)
                    {
                        var touch = Input.GetTouch(0);
                        // Lấy vị trí chạm trên màn hình
                        var touchPosition = touch.position;

                        // Tạo ray từ vị trí chạm
                        var ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0));
                        RaycastHit hit;

                        // Kiểm tra va chạm với mặt đất hoặc các đối tượng khác
                        if (Physics.Raycast(ray, out hit))
                        {
                            _lastPositionZ = Mathf.Round(hit.point.z * 10);
                            _lastPositionZ /= 10;
                            // Debug.Log("hit point " + _lastPositionZ + ", " + transform.localPosition.z);
                            if (Mathf.Round(_lastPositionZ * 10) != Mathf.Round(transform.localPosition.z * 10))
                            {
                                // xe di chuyển sang trái của góc nhìn user
                                if (transform.localPosition.z > _lastPositionZ)
                                {
                                    newZ = transform.localPosition.z - SpeedZ;
                                    if (newZ < _lastPositionZ)
                                        newZ = _lastPositionZ;
                                    if (transform.localRotation.eulerAngles.y < leftTargetAngle)
                                        transform.Rotate(Vector3.up, rotationSpeed);
                                }

                                // xe di chuyển sang phải của góc nhìn user
                                if (transform.localPosition.z < _lastPositionZ)
                                {
                                    newZ = transform.localPosition.z + SpeedZ;
                                    if (newZ > _lastPositionZ)
                                        newZ = _lastPositionZ;
                                    if (transform.localRotation.eulerAngles.y > rightTargetAngle)
                                        transform.Rotate(Vector3.up, -rotationSpeed);
                                }
                            }
                            else
                            {
                                if (transform.localRotation.eulerAngles.y > 90)
                                    transform.Rotate(Vector3.up, -rotationBackSpeed);
                                else if (transform.localRotation.eulerAngles.y < 90)
                                    transform.Rotate(Vector3.up, rotationBackSpeed);
                            }

                            // if (hit.point.z <= 552 && hit.point.x >= 543)
                            //     transform.DOMove(new Vector3(xPos, yPos, hit.point.z), 1);
                        }
                    }

                    // Tính toán tiến trình (0 đến 1)
                    var t = elapsedTime / duration2;

                    // Tính toán vị trí mới
                    var newX = startPosition.x + totalDistance * t;
                    // Debug.Log("startPosition: " + startPosition);

                    // Cập nhật vị trí của object
                    transform.localPosition =
                        new Vector3(newX, transform.localPosition.y, newZ);
                }
                else
                {
                    // Kết thúc di chuyển
                    transform.localPosition = new Vector3(startPosition.x + totalDistance, transform.localPosition.y,
                        transform.localPosition.z);
                }
            }
            else if (State == VehicleState.Race)
            {
                elapsedTime += Time.deltaTime;

                trail1.transform.position = trailPos1.transform.position;
                trail1.transform.rotation = trailPos1.transform.rotation;
                trail2.transform.position = trailPos2.transform.position;
                trail2.transform.rotation = trailPos2.transform.rotation;

                // if (elapsedTime < duration1)
                // {
                var newX = transform.localPosition.x;

                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    // Lấy vị trí chạm trên màn hình
                    var touchPosition = touch.position;

                    // Tạo ray từ vị trí chạm
                    var ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0));
                    RaycastHit hit;

                    // Kiểm tra va chạm với mặt đất hoặc các đối tượng khác
                    if (Physics.Raycast(ray, out hit))
                    {
                        _lastPositionX = Mathf.Round(hit.point.x * 10);
                        _lastPositionX /= 10;
                        // Debug.Log("hit point " + _lastPositionZ + ", " + transform.localPosition.z);
                        if (Mathf.Round(_lastPositionX * 10) != Mathf.Round(transform.localPosition.x * 10))
                        {
                            // xe di chuyển sang trái của góc nhìn user
                            if (transform.localPosition.x > _lastPositionX)
                            {
                                newX = transform.localPosition.x - SpeedZ;
                                if (newX < _lastPositionX)
                                    newX = _lastPositionX;
                                if (transform.localRotation.eulerAngles.y > 340)
                                {
                                    transform.Rotate(Vector3.up, -rotationSpeed);
                                    // Debug.Log("MOVE LEFT 1: " + transform.localRotation.eulerAngles.y);
                                }
                                else if (transform.localRotation.eulerAngles.y > leftTargetAngle2 &&
                                         transform.localRotation.eulerAngles.y < 60)
                                {
                                    transform.Rotate(Vector3.up, -rotationSpeed);
                                    // Debug.Log("MOVE LEFT 2: " + transform.localRotation.eulerAngles.y);
                                }
                            }

                            // xe di chuyển sang phải của góc nhìn user
                            if (transform.localPosition.x < _lastPositionX)
                            {
                                newX = transform.localPosition.x + SpeedZ;
                                if (newX > _lastPositionX)
                                    newX = _lastPositionX;
                                if (transform.localRotation.eulerAngles.y < rightTargetAngle2)
                                    transform.Rotate(Vector3.up, rotationSpeed);

                                if (transform.localRotation.eulerAngles.y > 340)
                                {
                                    transform.Rotate(Vector3.up, rotationSpeed);
                                    // Debug.Log("MOVE RIGHT 1: " + transform.localRotation.eulerAngles.y);
                                }
                                else if (transform.localRotation.eulerAngles.y < rightTargetAngle2)
                                {
                                    transform.Rotate(Vector3.up, rotationSpeed);
                                    // Debug.Log("MOVE RIGHT 2: " + transform.localRotation.eulerAngles.y);
                                }
                            }
                        }
                        else
                        {
                            if (transform.localRotation.eulerAngles.y > 0 &&
                                transform.localRotation.eulerAngles.y < 30)
                                transform.Rotate(Vector3.up, -rotationBackSpeed);
                            else if (transform.localRotation.eulerAngles.y < 0 ||
                                     transform.localRotation.eulerAngles.y > 330)
                                transform.Rotate(Vector3.up, rotationBackSpeed);
                        }

                        // if (hit.point.z <= 552 && hit.point.x >= 543)
                        //     transform.DOMove(new Vector3(xPos, yPos, hit.point.z), 1);
                    }
                }
                else
                {
                    if (transform.localRotation.eulerAngles.y > 0 && transform.localRotation.eulerAngles.y < 30)
                        transform.Rotate(Vector3.up, -rotationBackSpeed);
                    else if (transform.localRotation.eulerAngles.y < 0 ||
                             transform.localRotation.eulerAngles.y > 330)
                        transform.Rotate(Vector3.up, rotationBackSpeed);
                }

                // Tính toán tiến trình (0 đến 1)
                var t = elapsedTime / duration1;

                // Tính toán vị trí mới
                // var newZ = startPosition.z + totalDistance * t;
                if (_currentSpeed < _raceSpeed)
                    _currentSpeed += _accleration;
                var newZ = transform.localPosition.z + _currentSpeed * Time.deltaTime;

                // Cập nhật vị trí của object
                var transform1 = transform;
                transform1.localPosition =
                    new Vector3(newX, transform1.localPosition.y, newZ);
                // }
                // else
                // {
                //     // Kết thúc di chuyển
                //     var transform1 = transform;
                //     var localPosition = transform1.localPosition;
                //     localPosition = new Vector3(startPosition.x + totalDistance, localPosition.y,
                //         localPosition.z);
                //     transform1.localPosition = localPosition;
                // }
            }
        }

        private void LateUpdate()
        {
            if (GameManager.Instance.GameState == GameManager.GameStateType.Pause)
                return;
            if (healthBar)
            {
                Vector2 viewportPosition = MainCamera.WorldToViewportPoint(healthBarPos.transform.position);

                var sizeDelta = canvasRect.sizeDelta;
                var screenPoint = new Vector2(
                    ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                    ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));
                healthBar.transform.localPosition = screenPoint;
            }

            var transform1 = transform;
            var localPosition = transform1.localPosition;
            vehicleFollow.transform.localPosition = new Vector3(localPosition.x,
                vehicleFollow.transform.localPosition.y, localPosition.z + distanceToVehicleFollow);
        }

        public void ChangeVehicleBody(GameObject newVehicleBody)
        {
            Destroy(vehicleBody);
            vehicleBody = Instantiate(newVehicleBody, transform);
        }

        private bool _isChangingGun;
        private GameObject _newGunPrefab;
        public void ChangeGun(GameObject newGun)
        {
            _newGunPrefab = newGun;
            // _isChangingGun = true;
            // weaponAnimator.SetFloat("Speed", -1f);
            
            Destroy(weapon);
            weapon = Instantiate(_newGunPrefab, transform);
            UpdateProjectilePosition();
            weaponAnimator.enabled = true;
            weaponAnimator.Play("appear");
        }

        private GameObject _oldTire;
        public void ChangeTires(GameObject newWheels)
        {
            // Destroy(wheels);
            // wheels = Instantiate(newWheels, transform);
            _oldTire = tire;
            _oldTire.GetComponent<Tire>().TakeOff();
            DOVirtual.DelayedCall(2, (() =>
            {
                Destroy(_oldTire);
            }));

            tire = Instantiate(newWheels, transform);
            tire.GetComponent<Tire>().PutOn();
        }

        public void ChangeDefense()
        {
            Health += 400;
            defense.SetActive(true);
        }

        public void ChangeProjectile(GameObject newProjectile)
        {
            projectilePrefab = newProjectile;
        }

        private Tween _leftShootTween;
        private Tween _rightShootTween;

        public override void Shoot()
        {
            base.Shoot();
            weaponAnimator.Play("shoot");
            // SpawnLeftProjectile();
            // SpawnRightProjectile();
            weapon.GetComponent<Weapons.Weapon>().Shoot();
        }

        public void StopShoot()
        {
            weaponAnimator.enabled = false;
            // _rightShootTween.Kill();
            // _leftShootTween.Kill();
            weapon.GetComponent<Weapons.Weapon>().StopShoot();
        }

        private void SpawnLeftProjectile()
        {
            if (!_projectilePosition1)
            {
                _leftShootTween?.Kill();
                _leftShootTween = DOVirtual.DelayedCall(projectilePrefab.GetComponent<ProjectileMover>().spawnTime,
                    SpawnLeftProjectile);
                AddTweenToTweenManager(_leftShootTween, "_leftShootTween");
                return;
            }

            spawnOneProjectile(_projectilePosition1.transform.position, _projectilePosition1.transform.rotation);
            _leftShootTween?.Kill();
            _leftShootTween = DOVirtual.DelayedCall(projectilePrefab.GetComponent<ProjectileMover>().spawnTime,
                SpawnLeftProjectile);
            AddTweenToTweenManager(_leftShootTween, "_leftShootTween");
        }

        private void SpawnRightProjectile()
        {
            if (!_projectilePosition2)
            {
                _rightShootTween?.Kill();
                _rightShootTween = DOVirtual.DelayedCall(projectilePrefab.GetComponent<ProjectileMover>().spawnTime,
                    SpawnRightProjectile);
                AddTweenToTweenManager(_rightShootTween, "_rightShootTween");
                return;
            }

            spawnOneProjectile(_projectilePosition2.transform.position, _projectilePosition2.transform.rotation);
            _rightShootTween?.Kill();
            _rightShootTween = DOVirtual.DelayedCall(projectilePrefab.GetComponent<ProjectileMover>().spawnTime,
                SpawnRightProjectile);
            AddTweenToTweenManager(_rightShootTween, "_rightShootTween");
        }

        private void spawnOneProjectile(Vector3 position, Quaternion rotation)
        {
            if (GameManager.Instance.GameState == GameManager.GameStateType.Pause)
                return;
            var projectile = Instantiate(projectilePrefab);
            projectile.GetComponent<ProjectileMover>().powerNumber =
                projectilePrefab.GetComponent<ProjectileMover>().powerNumber * _multiplier;
            projectile.transform.position = position;
            projectile.transform.rotation = rotation;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            // CanvasManager.Instance.ShowTakeDamageText(damage, transform.position);
            if (Health <= 0)
            {
                // _healthBar.GetComponent<HealthBar>().Health = 0;
                StopShoot();
                Destroy(healthBar);
                Destroy(gameObject);
                DOVirtual.DelayedCall(3, (() =>
                {
                    GameManager.Instance.RestartGame();
                }));
            }
            else
            {
                // _healthBar.SetActive(true);
                healthBar.GetComponent<HealthBar>().Health = (float) Health / _totalHealth;
            }
        }

        private GameObject _boss;

        public void LookatBoss(GameObject boss)
        {
            _boss = boss;
            healthBar.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        private int _multiplier = 1;

        public void UpdatePowerMultiplier(int multiplier)
        {
            _multiplier = multiplier;
        }

        private GameObject _item;

        public void UseItem(GameObject item)
        {
            _item = Instantiate(item, transform);
            _item.GetComponent<Item>().Active(this);
        }
    }
}