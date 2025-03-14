using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yun.Scripts.Audios;
using Yun.Scripts.Core;
using Yun.Scripts.GamePlay;
using Yun.Scripts.GamePlay.Enemies;
using Yun.Scripts.GamePlay.Transition;
using Yun.Scripts.GamePlay.Vehicles;

namespace Yun.Scripts.Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private GameObject vehicleModelInInventory;
        [SerializeField] private GameObject circleBigRoad;
        [SerializeField] public GameObject player;
        [SerializeField] public GameObject boss;
        [SerializeField] public TextMeshProUGUI debugText;

        [SerializeField] private TransitionControllerLevel1 transisionController;

        [Button]
        private void test()
        {
            // egg.Play("Spawn");
        }
        
        public enum GameStateType
        {
            Play,
            Pause,
            TransitScene,
        }

        private GameStateType _gameState;

        public GameStateType GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            Application.targetFrameRate = 60;
            GameState = GameStateType.Play;
            AudioManager.Instance.PlayBackgroundMusic(AudioManager.Instance.Home_BG);
        }

        public float VehiceSpeed;

        public void StartKillBoss()
        {
            transisionController.StartFightBoss();
        }

        public void GameOver()
        {
            player.GetComponent<YunVehicle>().GameOver();
            boss.GetComponent<BossLevel1>().StopMoving();
            DOVirtual.DelayedCall(3, (() =>
            {
                GameManager.Instance.RestartGame();
            }));
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void StartRotateBigRoad()
        {
            circleBigRoad.GetComponent<CircleBigRoad>().StartRotate();
        }

        public void OnEnemyAttack(Enemy enemy)
        {
            player.GetComponent<Player>().TakeDamage(enemy.AttackPower);
        }

        public void OnPlayerAchieveItem(GameObject item)
        {
            player.GetComponent<YunVehicle>().UseItem(item);
        }

        public void OnChangeVehicle(GameObject vehicleType)
        {
            player.GetComponent<YunVehicle>().ChangeVehicleBody(vehicleType);
            vehicleModelInInventory.GetComponent<YunVehicle>().ChangeVehicleBody(vehicleType);
        }
        
        public void OnChangeWeapon(GameObject weaponType)
        {
            player.GetComponent<YunVehicle>().ChangeGun(weaponType);
            vehicleModelInInventory.GetComponent<YunVehicle>().ChangeGun(weaponType);
        }
        
        public void OnChangeWheel(GameObject wheelType)
        {
            player.GetComponent<YunVehicle>().ChangeTires(wheelType);
            vehicleModelInInventory.GetComponent<YunVehicle>().ChangeTires(wheelType);
        }
        
        public void OnChangeProjectile(GameObject projectileType)
        {
            player.GetComponent<YunVehicle>().ChangeProjectile(projectileType);
            vehicleModelInInventory.GetComponent<YunVehicle>().ChangeProjectile(projectileType);
        }

        public void UpgradeGun(GameObject weaponType)
        {
            player.GetComponent<YunVehicle>().ChangeGun(weaponType);
        }
        
        public void UpgradeTires(GameObject weaponType)
        {
            player.GetComponent<YunVehicle>().ChangeTires(weaponType);
        }
        
        public void UpgradeDefense()
        {
            player.GetComponent<YunVehicle>().ChangeDefense();
        }

        public void StartGame()
        {
            AudioManager.Instance.PlayBackgroundMusic(AudioManager.Instance.Race_BG);
            transisionController.StartGame();
        }
    }
}