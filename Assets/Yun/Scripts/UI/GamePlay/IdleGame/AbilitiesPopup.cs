using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.UI;
using Yun.Scripts.UI.GamePlay.IdleGame.AbilityItemCs;

public class AbilitiesPopup : BasePopup
{
    [SerializeField] private GameObject objectNormal;
    [SerializeField] private GameObject objectSpecial;
    [SerializeField] private Transform spawnPosition;

    [SerializeField] private TextMeshProUGUI abilityLevelCurrent;
    [SerializeField] private TextMeshProUGUI tokenCurrent;
    [SerializeField] private TextMeshProUGUI TokenUpgradeRequide;
    [SerializeField] private List<GameObject> buttonState;
    
    private List<AbilityItem> abilityItems = new List<AbilityItem>();
    private AbilityItem abilityCurrent;
    protected override void Start()
    {
        base.Start();
        OnInit();
    }

    private int collectionCount;

    private void OnInit()
    {
        collectionCount = FacilityManager.Instance.AbilitiesDataCollection.Count;
        for (int i = 0; i < collectionCount; i++)
        {
            FacilityManager.Instance.AbilitiesDataCollection[i].unlock =   
                FacilityManager.Instance.AbilitiesDataCollection[i].id <= FacilityManager.Instance.GameSaveLoad.StableGameData.abilityLevel;  
        }
       
        for (int i = collectionCount ; i > 1; i--)  
        {
            var ability = FacilityManager.Instance.AbilitiesDataCollection[i-1];  
            var prefabToInstantiate = (ability.abilityItemType == AbilityItemType.AbilityItemSpecial) ? objectSpecial : objectNormal;
            var obj = Instantiate(prefabToInstantiate, spawnPosition);
            var item = obj.GetComponent<AbilityItem>();
            if (item != null) 
            {  
                item.SetData(i, this);  
                item.ID = ability.id;  
                item.incomeType = ability.incomeType;  
                item.isUnLock = ability.unlock;  
                item.percentIncome = ability.percentIncome;  
                item.pointUpGrade = ability.pointUpGrade;
                item.SetItemProperty();
                if (!item.isUnLock)
                {
                    abilityCurrent = item;
                }
                
                abilityItems.Add(item);
            }
        }
        
        SetStateButton();
        abilityItems.Reverse();
    }

    private void SetStateButton()  
    {  
        var gameData = FacilityManager.Instance.GameSaveLoad.GameData;  
        var abilityLevel = FacilityManager.Instance.GameSaveLoad.StableGameData.abilityLevel;
      
        abilityLevelCurrent.text = "Level " + (abilityLevel+1).ToString();
        tokenCurrent.text = gameData.token.ToString();  
        Debug.Log("abilityLevel:"+abilityLevel + "collectionCount: "+collectionCount );
        if (abilityLevel < collectionCount)
        {
            var itemIndex = FacilityManager.Instance.AbilitiesDataCollection[abilityLevel].pointUpGrade;
            TokenUpgradeRequide.text = itemIndex.ToString(); 
            bool canUpgrade = gameData.token >= itemIndex && abilityLevel < collectionCount;  
            buttonState[0].SetActive(!canUpgrade);  
            buttonState[1].SetActive(canUpgrade);  
        }
        else
        {
            buttonState[0].SetActive(false);  
            buttonState[1].SetActive(true);  
        }
     
    }  

    public void OnClickUpgradeLevel()  
    {
        var gameData = FacilityManager.Instance.GameSaveLoad.GameData;
        var stableGameData = FacilityManager.Instance.GameSaveLoad.StableGameData;
        var abilityLevel = stableGameData.abilityLevel;
        if (abilityLevel < collectionCount)
        {
            var pointUpLevel = FacilityManager.Instance.AbilitiesDataCollection[abilityLevel].pointUpGrade;

            if (gameData.token >= pointUpLevel)
            {
                FacilityManager.Instance.GameSaveLoad.GameData.token -= pointUpLevel;  
                FacilityManager.Instance.PlayerInfoUI.UpdateToken(FacilityManager.Instance.IdleGameData.Token, false);
                stableGameData.abilityLevel++;
                UpgradeItems(stableGameData.abilityLevel);
                SetStateButton(); 
            }
        }
    }  
    
    public void UpgradeItems(int abilityLevel)
    {
        FacilityManager.Instance.AbilitiesDataCollection[abilityLevel].unlock = true;
        DOVirtual.DelayedCall(0.5f, () =>
        {
            abilityCurrent.isUnLock = true;
            abilityCurrent.SetData(abilityLevel, this);
            abilityCurrent.SetItemProperty();
            FacilityManager.Instance.UpdateAbilityData(abilityCurrent);
            abilityCurrent = abilityItems[abilityLevel++];
        });
    }  

    public void OnClickBuyToken()
    {
        
    }
    
    public override void Close()
    {
        base.Close();
        FacilityManager.Instance.PlayerInfoUI.HideAbilitiesPopupBg();
    }
}
