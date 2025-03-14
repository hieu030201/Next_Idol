using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using Gley.RateGame;
using UnityEngine;
using Yun.Scripts.Core;

public class RateUs : MonoSingleton<RateUs>
{
    public void ShowRatePopup()
    {
        Gley.RateGame.API.ShowRatePopupWithCallback(PopupClosed);
    }

    public void IncreaseCustomEvents()
    {
        Debug.Log("Run In IncreaseCustomEvents");
        Gley.RateGame.API.IncreaseCustomEvents();
    }
    
    private void PopupClosed(PopupOptions result, string message)
    {
        switch (result)
        {
            case PopupOptions.Never:
                Debug.Log("Never button was press from in-game popup");
                break;
            case PopupOptions.Rated:
                FirebaseAnalytics.LogEvent("rate_us");
                Debug.Log("Send/Yes button was press from in-game popup");
                break;
            case PopupOptions.NotNow:
                Debug.Log("Later button was press from in-game popup");
                break;
            case PopupOptions.NativeFailed:
                FirebaseAnalytics.LogEvent("rate_us_display_failed_" + message);
                Debug.Log($"Native popup failed to display. Reason {message}");
                break;
            case PopupOptions.NativeSuccess:
                Debug.Log("Native popup displayed");
                break;
        }
    }
}
