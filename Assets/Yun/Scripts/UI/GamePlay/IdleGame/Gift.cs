using System;
using TMPro;
using UnityEngine;

namespace Yun.Scripts.UI.GamePlay.IdleGame
{
    public class Gift : MonoBehaviour
    {
        [SerializeField] public GameObject overLayout;
        [SerializeField] public GameObject highlightIcon;
        [SerializeField] public GameObject receivedIcon;
        [SerializeField] public GameObject moneyIcon;
        [SerializeField] public GameObject starIcon;
        [SerializeField] public GameObject gemIcon;
        [SerializeField] public GameObject tokenIcon;
        [SerializeField] public GameObject secretLayout;
        [SerializeField] public TextMeshProUGUI dayTxt;
        [SerializeField] public TextMeshProUGUI rewardTxt;

        public int id;
        private void Awake()
        {
            overLayout.SetActive(false);
            highlightIcon.SetActive(false);
            receivedIcon.SetActive(false);
            secretLayout.SetActive(false);
            
            // moneyIcon.SetActive(false);
            // starIcon.SetActive(false);
            // gemIcon.SetActive(false);
            // tokenIcon.SetActive(false);
        }
    }
}