using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class ChangeMapPopup : BasePopup
    {
        [SerializeField] private GameObject map1Btn;
        [SerializeField] private GameObject map2Btn;
        [SerializeField] private Button confirmBtn;
        
        protected override void Awake()
        {
            base.Awake();

            var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
            if (mapIndex == 1)
            {
                map1Btn.transform.Find("Current_State").gameObject.SetActive(true);
                map1Btn.transform.Find("Normal_State").gameObject.SetActive(false);
                map2Btn.transform.Find("Normal_State").gameObject.SetActive(true);
            }
            else if (mapIndex == 2)
            {
                map2Btn.transform.Find("Current_State").gameObject.SetActive(true);
                map2Btn.transform.Find("Normal_State").gameObject.SetActive(false);
                map1Btn.transform.Find("Normal_State").gameObject.SetActive(true);
            }
            ChangeMap(mapIndex);
        }

        public void Confirm()
        {
            ParticlePool.Reset();
            SimplePool.ResetPool();
            PlayerPrefs.SetInt("mapIndex", _currentMapIndex);
            Close();
            SceneManager.LoadSceneAsync(0);
        }

        private int _currentMapIndex;

        public void ChangeMap(int id)
        {
            _currentMapIndex = id;
            switch (id)
            {
                case 1:
                    map1Btn.transform.Find("Selected_State").gameObject.SetActive(true);
                    map1Btn.transform.Find("Locked_State").gameObject.SetActive(false);
                    map1Btn.transform.Find("Normal_State").gameObject.SetActive(false);

                    map2Btn.transform.Find("Selected_State").gameObject.SetActive(false);
                    map2Btn.transform.Find("Locked_State").gameObject.SetActive(false);
                    map2Btn.transform.Find("Normal_State").gameObject.SetActive(true);
                    break;
                case 2:
                    map2Btn.transform.Find("Selected_State").gameObject.SetActive(true);
                    map2Btn.transform.Find("Locked_State").gameObject.SetActive(false);
                    map2Btn.transform.Find("Normal_State").gameObject.SetActive(false);

                    map1Btn.transform.Find("Selected_State").gameObject.SetActive(false);
                    map1Btn.transform.Find("Locked_State").gameObject.SetActive(false);
                    map1Btn.transform.Find("Normal_State").gameObject.SetActive(true);
                    break;
            }

            var mapIndex = PlayerPrefs.GetInt("mapIndex", 1);
            confirmBtn.interactable = _currentMapIndex != mapIndex;
        }
    }
}