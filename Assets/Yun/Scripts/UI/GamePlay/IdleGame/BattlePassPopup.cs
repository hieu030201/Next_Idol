using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.Managers;
using Yun.Scripts.UI;
using Yun.Scripts.UI.GamePlay.IdleGame.SkinItemCs;

public class BattlePassPopup : BasePopup
{
   [SerializeField] private TextMeshProUGUI levelText;
   [SerializeField] private Slider progressBarLevel;
   
   [SerializeField] private List<GameObject> itemPrefab;
   [SerializeField] private List<Transform> parentSpawnUnlock;
   [SerializeField] private List<Transform> parentSpawnAreaLock;
   private List<RectTransform > pointLevelUnlock = new List<RectTransform >(); // Danh sách lưu Transform  
   public float duration = 1.0f;
   private int countLevel = 0;
   [SerializeField] private SetHeightUI _setHeightUI;
   [SerializeField] private SetHeightContentUI _setHeightContentUI;

   [SerializeField] private List<GameObject> heroPackState = new List<GameObject>();
   [SerializeField] private List<GameObject> legendPackState = new List<GameObject>();

   [SerializeField] private List<BattlePassHeroItem> heroItemList = new List<BattlePassHeroItem>();
   [SerializeField] private List<BattlePassLegendItem> legendItemList = new List<BattlePassLegendItem>();
   [SerializeField] private CanvasGroup canvasGroup;
   [SerializeField] private GameObject overlayMask;

   public override void Show()
   {
      base.Show();
      UpdateLevel();
      Initialize(OnInitComplete); 
      canvasGroup.alpha = 0;
      RectTransform rectTransform = canvasGroup.GetComponent<RectTransform>();
      Sequence sequence = DOTween.Sequence();
      
      sequence.AppendInterval(0.5f);
      
      sequence.Append(rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y, 1f)
         .SetEase(Ease.OutQuint));
      sequence.Join(canvasGroup.DOFade(1, 1f));
      sequence.Play();

   }

   public void OnInit()
   {
      Initialize(OnInitComplete); 
   }

   public void Initialize(System.Action callback)
   {
      int countValues = FacilityManager.Instance.BattlePassDataCollection.Value.Count;
      var GameSaveLoad = FacilityManager.Instance.GameSaveLoad;
     
      if (GameSaveLoad.StableGameData.isBoughtBattlePassHero)
      {
         GameSaveLoad.GameData.listIDBattlePassHero.Add(1);
         // FacilityManager.Instance.GameSaveLoad.GameData.listIDBattlePassHero.Add(1);
         // if (!GameSaveLoad.StableGameData.isBoughtBattlePassLegend)
         // FacilityManager.Instance.BattlePassDataCollection[1].battlePassLengend.rewardType = RewardBattlePassType.Diamon;
      }
      
      if (GameSaveLoad.StableGameData.isBoughtBattlePassLegend)
      {
         GameSaveLoad.GameData.listIDBattlePassLegend.Add(0);
         GameSaveLoad.GameData.listIDBattlePassLegend.Add(1);
      }

      for (int i = 0; i <= level+1; i++)  
      {
         var valueIndex = FacilityManager.Instance.BattlePassDataCollection[i];
         if (i == 0)
         {
            CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnUnlock[0], valueIndex.battlePassFree.rewardType,i,FacilityManager.Instance.BattlePassDataCollection[0].battlePassFree.amount);  
            CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnUnlock[2], valueIndex.battlePassHero.rewardType,i,FacilityManager.Instance.BattlePassDataCollection[0].battlePassHero.amount);  
            CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnUnlock[1], valueIndex.battlePassHero.rewardType,i,FacilityManager.Instance.BattlePassDataCollection[0].battlePassHero.amount);
            
            if (valueIndex.battlePassLengend.rewardType == RewardBattlePassType.Skin)
               CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnUnlock[3], valueIndex.battlePassLengend.rewardType,i,FacilityManager.Instance.BattlePassDataCollection[0].battlePassLengend.amount);
            else
               CreateAndSetupItem<BattlePassItem>(itemPrefab[2], parentSpawnUnlock[3], valueIndex.battlePassLengend.rewardType,i,FacilityManager.Instance.BattlePassDataCollection[0].battlePassLengend.amount);
         }
         else
         {
            if (valueIndex.battlePassFree.rewardType == RewardBattlePassType.Skin)
               CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnUnlock[0], valueIndex.battlePassFree.rewardType,i,valueIndex.battlePassFree.amount);
            else
               CreateAndSetupItem<BattlePassItem>(itemPrefab[0], parentSpawnUnlock[0], valueIndex.battlePassFree.rewardType,i,valueIndex.battlePassFree.amount);  
         
            if (valueIndex.battlePassHero.rewardType == RewardBattlePassType.Skin)
               CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnUnlock[2], valueIndex.battlePassHero.rewardType,i,valueIndex.battlePassHero.amount);
            else
               CreateAndSetupItem<BattlePassItem>(itemPrefab[1], parentSpawnUnlock[2], valueIndex.battlePassHero.rewardType,i,valueIndex.battlePassHero.amount);  
         
            if (valueIndex.battlePassLengend.rewardType == RewardBattlePassType.Skin)
               CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnUnlock[3], valueIndex.battlePassLengend.rewardType,i,valueIndex.battlePassLengend.amount);
            else
               CreateAndSetupItem<BattlePassItem>(itemPrefab[2], parentSpawnUnlock[3], valueIndex.battlePassLengend.rewardType,i,valueIndex.battlePassLengend.amount);
         }

         if (i > 0)
         {
            var obj = Instantiate(itemPrefab[4], parentSpawnUnlock[1]);
            if (i < 2)
            {
               obj.transform.Find("Bg").gameObject.SetActive(false);
            }
            obj.transform.Find("Bg/TxtLevel").gameObject.GetComponent<TextMeshProUGUI>().text = countLevel++.ToString();
         }

      }
      for (int i = level + 2; i < countValues; i++)
      {
         var valueIndex = FacilityManager.Instance.BattlePassDataCollection[i];
         if (valueIndex.battlePassFree.rewardType == RewardBattlePassType.Skin)
            CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnAreaLock[0], valueIndex.battlePassFree.rewardType,i,valueIndex.battlePassFree.amount);
         else
            CreateAndSetupItem<BattlePassItem>(itemPrefab[0], parentSpawnAreaLock[0], valueIndex.battlePassFree.rewardType,i,valueIndex.battlePassFree.amount);  
         
         if (valueIndex.battlePassHero.rewardType == RewardBattlePassType.Skin)
            CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnAreaLock[2], valueIndex.battlePassHero.rewardType,i,valueIndex.battlePassHero.amount);
         else
            CreateAndSetupItem<BattlePassItem>(itemPrefab[1], parentSpawnAreaLock[2], valueIndex.battlePassHero.rewardType,i,valueIndex.battlePassHero.amount);  
         
         if (valueIndex.battlePassLengend.rewardType == RewardBattlePassType.Skin)
            CreateAndSetupItem<BattlePassItem>(itemPrefab[3], parentSpawnAreaLock[3], valueIndex.battlePassLengend.rewardType,i,valueIndex.battlePassLengend.amount);
         else
            CreateAndSetupItem<BattlePassItem>(itemPrefab[2], parentSpawnAreaLock[3], valueIndex.battlePassLengend.rewardType,i,valueIndex.battlePassLengend.amount);
         
         var obj = Instantiate(itemPrefab[5], parentSpawnAreaLock[1]);
         obj.transform.Find("Bg/TxtLevel").gameObject.GetComponent<TextMeshProUGUI>().text = countLevel++.ToString();
      }

      if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassHero)
         StateButtonBuyHeroPackage();
      if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassLegend)
         StateButtonBuyLegendPackage();

      callback.Invoke(); 
   }
   
   private void OnInitComplete()  
   {  
      DOVirtual.DelayedCall(0.5f,()=>
      {
         _setHeightUI.OnInit();
         _setHeightContentUI.OnInit();
      });  
   } 
   
   private bool mapOne => PlayerPrefs.GetInt("mapIndex", 1) == 1; 

   private void CreateAndSetupItem<T>(GameObject prefab, Transform parent, RewardBattlePassType rewardType, int index, int amount) where T : BattlePassItem  
   {
      var obj = Instantiate(prefab, parent);  
      var item = obj.GetComponent<T>();
      item.SetData(index, this);
      if (rewardType != null) 
      {  
         item.RewardType = rewardType;  
         item.SetIconItem();  
      }
      item.Amount = amount;

      item.SetAmount(amount);
      
      if (item is BattlePassHeroItem hero)  
      {
         heroItemList.Add(hero);
         if (FacilityManager.Instance.GameSaveLoad.GameData.listIDBattlePassHero.Contains(index))
         {
            hero.Claimed = true;
            hero.SetBackGround();
         }
         if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassHero)  
         {  
            hero.HideLock(); 
         }  
      }
      
      if (item is BattlePassLegendItem legend)  
      {
         legendItemList.Add(legend);
         if (FacilityManager.Instance.GameSaveLoad.GameData.listIDBattlePassLegend.Contains(index))
         {
            legend.Claimed = true;
            if (legend.Amount > 0)
            {
               legend.SetBackGround();
            }
         }
         if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassLegend)  
         {  
            legend.HideLock(); 
         }  
      }
      
      if (item is BattlePassFreeItem free)  
      {
         
         if (FacilityManager.Instance.GameSaveLoad.GameData.listIDBattlePassFree.Contains(index))
         {
            free.Claimed = true;
            free.SetBackGround();
         }
         
      }
      
   }  
   
   private int level;  
   public void UpdateLevel()
   {
      level = FacilityManager.Instance.GameSaveLoad.GameData.battlePassLevel;
      float levelPercentage = FacilityManager.Instance.GameSaveLoad.GameData.levelPercentageBattlePass;
      
      UpdateLevelProgress(level, levelPercentage);
   }

   private void UpdateLevelProgress(int level, float levelPercentage)
   {
      levelText.text = level.ToString();
      progressBarLevel.DOValue(levelPercentage, 0.5f)
         .SetEase(Ease.OutQuad);
   }

   public void BoughtHeroPackage()
   {
      Close();
      FacilityManager.Instance.ShowBattleFund();
      FacilityManager.Instance.isShowBattleFundFromSkin = false;
      // StateButtonBuyHeroPackage();
      // //int countValues = FacilityManager.Instance.BattlePassDataCollection.Value.Count;
      // for (var i = 0; i < heroItemList.Count; i++) heroItemList[i].HideLock();
   }

   public void BoughtLegendPackage()
   {
      Close();
      FacilityManager.Instance.ShowBattleFund();
      FacilityManager.Instance.isShowBattleFundFromSkin = false;
      // StateButtonBuyLegendPackage();
      // for (var i = 0; i < legendItemList.Count; i++) legendItemList[i].HideLock();
   }

   public void StateButtonBuyHeroPackage()
   {
      heroPackState[0].SetActive(false);
      heroPackState[1].SetActive(true);
      for (var i = 0; i < heroItemList.Count; i++) heroItemList[i].HideLock();
   }

   public void StateButtonBuyLegendPackage()
   {
      legendPackState[0].SetActive(false);
      legendPackState[1].SetActive(true);
      for (var i = 0; i < legendItemList.Count; i++) legendItemList[i].HideLock();
   }
   public override void Close()
   {
      CanvasManager.Instance.HidePopup(UIName, TypeCloseEffect.FadeToBottom);
      AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Exit_MGB);
      DOVirtual.DelayedCall(0.1f, () =>
      {
         FacilityManager.Instance.PlayerInfoUI.HideBattlePassPopupBg();
      });

      var gameData = FacilityManager.Instance.GameSaveLoad.GameData;  
      //var stableGameData = FacilityManager.Instance.GameSaveLoad.StableGameData;  
        
      int levelCurrent = gameData.battlePassLevel;
      bool isFreePassAvailable = false;  
      bool isHeroPassAvailable = false;  
      bool isLegendPassAvailable = false;  

      if (gameData.listIDBattlePassFree.Count > 0)  
      {  
         isFreePassAvailable = gameData.listIDBattlePassFree.Max() - 1 < levelCurrent;   
      }  

      if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassHero)  
      {
         if (gameData.listIDBattlePassHero.Count > 0)
         {
            isHeroPassAvailable = gameData.listIDBattlePassHero.Max() - 1 < levelCurrent; 
         }

         if (gameData.listIDBattlePassHero.Count < 2 )
         {
            isHeroPassAvailable = true;
         }
      }  

      if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtBattlePassLegend)  
      {
         if (gameData.listIDBattlePassLegend.Count > 0)
         {
            isLegendPassAvailable = gameData.listIDBattlePassLegend.Max() - 1 < levelCurrent; 
         }

         if (gameData.listIDBattlePassLegend.Count < 2 )
         {
            isLegendPassAvailable = true;
         }
      }  

      var isAnyPassAvailable = isFreePassAvailable || isHeroPassAvailable || isLegendPassAvailable;  

      if (isAnyPassAvailable)  
      {  
         FacilityManager.Instance.PlayerInfoUI.ShowBattlePassRequestAnimation();  
      }  
      else  
      {  
         FacilityManager.Instance.PlayerInfoUI.HideBattlePassRequestAnimation();  
      }

      if (!FacilityManager.Instance.GameSaveLoad.GameData.isShowedRateUs)
      {
         FacilityManager.Instance.GameSaveLoad.GameData.isShowedRateUs = true;
         RateUs.Instance.ShowRatePopup();
      }
   }

   public void ActiveOverLayHeader(bool active)
   {
      overlayMask.SetActive(active);
   }
   
}
