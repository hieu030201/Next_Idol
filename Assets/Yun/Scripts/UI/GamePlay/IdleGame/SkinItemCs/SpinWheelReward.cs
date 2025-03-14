using System;
using System.Collections.Generic;
using Adverstising_Integration.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yun.Scripts.Audios;
using Yun.Scripts.GamePlay.IdleGame.Configs;
using Yun.Scripts.GamePlay.IdleGame.Managers;
using Yun.Scripts.UI.GamePlay.IdleGame;
using Image = UnityEngine.UI.Image;

public class SpinWheelReward : MonoBehaviour
{
    private int id;
    public TextMeshProUGUI nameReward;
    public Image iconReward;
    public TextMeshProUGUI amount;
    public Action<SpinWheelData> OnCloseAction;
    [SerializeField] private GameObject normalButtons;
    [SerializeField] private GameObject cashButtons;
    [SerializeField] private CanvasGroup noThanksButton;
    [SerializeField] private GameObject ContentReward;
    [SerializeField] private GameObject containerResult;
    [SerializeField] private SpinWheelPopup spinWheelPopup;
    [SerializeField] private Image iconBtnRewardAds;
    [SerializeField] [PreviewField(Alignment = ObjectFieldAlignment.Center)] private List<Sprite> listIconBtnRewardAds;
    [SerializeField] private SticketMinusTxt sticketMinusTxt;
    [SerializeField] private GameObject btnClaimReward;
    [SerializeField] private GameObject btnClaimRewardCash;
    [SerializeField] private GameObject btnClaimX2Reward;
    public void Awake()
    {
        normalButtons.SetActive(false);
        cashButtons.SetActive(false);

        if(FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
        {
            iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
        }
    }

    public void SetData(SpinWheelData itemObj)
    {
        id = itemObj.id;
        nameReward.text = itemObj.name;
        iconReward.sprite = itemObj.icon;
        iconReward.SetNativeSize();
        amount.text = itemObj.amount.ToString();
        if (itemObj.spinRewardType == SpinRewardType.Token)
        {
            iconReward.transform.localScale = new Vector3(2f, 2f, 2);  
        }else if (itemObj.spinRewardType == SpinRewardType.Gem)
        {
            iconReward.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);  
        }
        else
        {
            iconReward.transform.localScale = new Vector3(1f, 1f, 1);  
        }
        if (itemObj.spinRewardType == SpinRewardType.Cash)
        {
            cashButtons.SetActive(true);
            noThanksButton.alpha = 0f;
            noThanksButton.interactable = false; 
            noThanksButton.blocksRaycasts = false; 
            DOVirtual.DelayedCall(2.0f, ActiveBtn);
        }
        else
            normalButtons.SetActive(true);
    }

    private void ActiveBtn()
    {
      
        DOVirtual.DelayedCall(1.0f, () =>  
        {  
            noThanksButton.DOFade(1f, 1f) 
                .OnStart(() =>  
                {  
                    noThanksButton.alpha = 0f;
                    noThanksButton.interactable = true; 
                    noThanksButton.blocksRaycasts = true;
                });  
        });
    }
    
    private void ClaimRewardSpinWheel(SpinWheelData itemReward, int multip)
    {
        SetInnactiveBtnReward(btnClaimReward);
        SetInnactiveBtnReward(btnClaimRewardCash);
        SetInnactiveBtnReward(btnClaimX2Reward);
        switch (itemReward.spinRewardType)
        {
            case SpinRewardType.Cash:
                FacilityManager.Instance.AddMoney(itemReward.amount * multip, Vector3.zero, true,spinWheelPopup.transform);
                spinWheelPopup.ShowGemBtn();
                break;
            case SpinRewardType.Token:
                FacilityManager.Instance.AddToken(itemReward.amount * multip, Vector3.zero, true,true,spinWheelPopup.transform);
                spinWheelPopup.ShowGemBtn();
                break;
            case SpinRewardType.Gem:
                FacilityManager.Instance.AddGem(itemReward.amount * multip, Vector3.zero, true, spinWheelPopup.transform);
                spinWheelPopup.ShowGemBtn();
                break;
            case SpinRewardType.Skin:
                break;
            case SpinRewardType.Booster:
                break;
        }
        
        FacilityManager.Instance.GameSaveLoad.OrderToSaveData(true);
    }

    public void GetReward()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
        ClaimRewardSpinWheel(FacilityManager.Instance.SpinWheelDataCollection[id], 1);
        Close();
    }

    public void GetDoubleRewardByAds()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
        sticketMinusTxt.OnInit();
        AdsManager.Instance.ShowReward(GetDoubleReward, AdsManager.RewardType.SPIN_WHEEL);
    }

    public void GetDoubleReward()
    {
        if (AdsManager.Instance.isCancelRewardedAd)
        {
            AdsManager.Instance.isCancelRewardedAd = false;
            ClaimRewardSpinWheel(FacilityManager.Instance.SpinWheelDataCollection[id], 1);
            Close();
            return;
        }
        ClaimRewardSpinWheel(FacilityManager.Instance.SpinWheelDataCollection[id], 2);
        
        Close();
    }
    public void GetDoubleRewardByGem()
    {
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.Claim_Button_Click);
        if (FacilityManager.Instance.GameSaveLoad.GameData.gem >= 3)
        {
            GetDoubleReward();
        }
    }
    
    public void OnOutOfTurnTicket()
    {
        if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds <= 0)
        {
            iconBtnRewardAds.sprite = listIconBtnRewardAds[0];
        }else if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
        {
            iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
        }
    }
    
    public void Close()
    {
        DOVirtual.DelayedCall(1.5f, () =>
        {
            containerResult.GetComponent<RectTransform>().DOScale(0.1f, 0.6f).SetEase(Ease.InBack);
            ContentReward.GetComponent<CanvasGroup>().DOFade(0, 0.3f);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                gameObject.SetActive(false);
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    cashButtons.SetActive(false);
                    normalButtons.SetActive(false);
                });
            });
            if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip &&
                FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds <= 0)
            {
                iconBtnRewardAds.sprite = listIconBtnRewardAds[0];
                spinWheelPopup.OnOutOfTurnTicket();
            }
            else if(FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
            {
                iconBtnRewardAds.sprite = listIconBtnRewardAds[1];
            }
            spinWheelPopup.GetStateButtonSpin();
        });
    }
    public void SetInnactiveBtnReward(GameObject obj)
    {
        var button = obj.GetComponent<Button>();
        button.interactable = false;
        DOVirtual.DelayedCall(3.0f, ()=>
        {
            button.interactable = true;
        });
    }
}
