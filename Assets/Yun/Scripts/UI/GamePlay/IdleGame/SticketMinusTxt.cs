using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yun.Scripts.GamePlay.IdleGame.Managers;

public class SticketMinusTxt : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sticketTxt;
    [SerializeField] private CanvasGroup ticketImgCanvasGroup;
    public float flyUp = 50f;
    public void OnInit()
    {
        if (FacilityManager.Instance.GameSaveLoad.StableGameData.isBoughtNoAdsVip && FacilityManager.Instance.GameSaveLoad.StableGameData.amountFreeRewardAds > 0)
        {
            ShowTicketMinusNumber(sticketTxt,ticketImgCanvasGroup);
        }
    }
    
    public void ShowTicketMinusNumber(TextMeshProUGUI sticketTxt, CanvasGroup imgTicket)
    {
        if (sticketTxt == null || imgTicket == null) return;

        sticketTxt.rectTransform.localPosition = new Vector3(sticketTxt.rectTransform.localPosition.x, flyUp,
            sticketTxt.rectTransform.localPosition.z);

        sticketTxt.gameObject.SetActive(true);

        sticketTxt.rectTransform.localPosition = new Vector3(sticketTxt.rectTransform.localPosition.x,
            sticketTxt.rectTransform.localPosition.y - 40,
            sticketTxt.rectTransform.localPosition.z);

        sticketTxt.DOFade(1, 0.8f)
            .OnStart(() =>
            {
                sticketTxt.rectTransform.DOLocalMoveY(sticketTxt.rectTransform.localPosition.y + flyUp, 0.8f)
                    .SetEase(Ease.OutCubic);
                imgTicket.DOFade(1, 0.8f);
            })
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.6f, () =>
                {
                    sticketTxt.rectTransform.DOKill();

                    sticketTxt.DOFade(0, 1.0f).OnStart(() =>
                    {
                        sticketTxt.rectTransform.DOLocalMoveY(sticketTxt.rectTransform.localPosition.y + 30, 0.8f)
                            .SetEase(Ease.InCubic);
                        imgTicket.DOFade(0, 0.8f);
                    }).OnComplete(() => { sticketTxt.gameObject.SetActive(false); });
                });
            });
    }
}
