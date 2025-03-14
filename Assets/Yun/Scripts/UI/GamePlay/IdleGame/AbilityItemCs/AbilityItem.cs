using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.GamePlay.IdleGame.Configs;

namespace Yun.Scripts.UI.GamePlay.IdleGame.AbilityItemCs
{
    public class AbilityItem : MonoBehaviour
    {
        #region ITEM PROPERTY
        public int ID;
        public IncomeType incomeType;
        public int pointUpGrade;
        public int percentIncome;
        public bool isUnLock;
        #endregion

        [SerializeField] protected TextMeshProUGUI levelTxt;
        [SerializeField] protected TextMeshProUGUI incomeRewardTxt;

        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        protected List<Sprite> bgList;

        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        protected List<Sprite> iconActiveList;
        
        [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        protected List<Sprite> iconNotActiveList;

        [SerializeField] protected List<GameObject> colState;
        
        [SerializeField] protected Image bgItem;
        [SerializeField] protected Image iconItem;
        protected AbilitiesPopup abilitiesPopup;

        public void SetData(int id, AbilitiesPopup item)
        {
            ID = id;
            abilitiesPopup = item;
        }
        [Button]
        public virtual void SetItemProperty()
        {
            SetBackGround();
            SetIconItem();
            SetLevel();
            SetIncomeRewardTxt();

        }
      
        public virtual void SetBackGround()
        {
            if (!isUnLock)
            {
                bgItem.sprite = bgList[0];
                colState[0].SetActive(true);
            }
            else
            {
                bgItem.sprite = bgList[1];
                colState[1].SetActive(true);
            }
            bgItem.SetNativeSize();
        }

        public void SetIconItem()
        {
            int index = (int)incomeType;
            iconItem.sprite = (isUnLock ? iconActiveList : iconNotActiveList)[index];  
            iconItem.SetNativeSize();
        }

        public void SetLevel()
        {
            string colorHex = !isUnLock ? "#D52027" : "#FFEB3A";
            if (ColorUtility.TryParseHtmlString(colorHex, out Color selectColor))  
            {  
                levelTxt.color = selectColor;  
            } 
            levelTxt.text = "LV " + (ID + 1).ToString();
        }

        public void SetIncomeRewardTxt()
        {
            string colorHex = !isUnLock ? "#D52027" : "#FFEB3A";  
            if (ColorUtility.TryParseHtmlString(colorHex, out Color selectedColor))  
            {  
                incomeRewardTxt.color = selectedColor;  
            }    
            switch (incomeType)
            {
                case IncomeType.CashIncome:
                    incomeRewardTxt.text = "Cash Collection " + percentIncome + "%";
                    break;
                case IncomeType.TokenIncome:
                    incomeRewardTxt.text = "Token Collection " + percentIncome + "%";
                    break;
                case IncomeType.GemIncome:
                    incomeRewardTxt.text = "Gem Collection " + percentIncome + "%";
                    break;
                case IncomeType.SpeedIncome:
                    incomeRewardTxt.text = "Movement Speed " + percentIncome + "%";
                    break;
                case IncomeType.CashRadius:
                    incomeRewardTxt.text = "Cash Radius " + percentIncome + "%";
                    break;
            }
        }

        public void NextItem(AbilityItem item)
        {
            item.ID++;
        }

    }
}

