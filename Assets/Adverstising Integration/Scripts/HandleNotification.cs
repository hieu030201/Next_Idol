using System;
using Advertising;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandleNotification : Singleton<HandleNotification>
{
    [SerializeField] private bool notificationEnable = true;

    #region Unity Methods

    private void Start()
    {
        if (notificationEnable)
        {
            Gley.Notifications.API.Initialize();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
        }
        else
        {
            Gley.Notifications.API.CancelAllNotifications();
        }
    }

    #endregion

    private void SendNotification(string title, string description)
    {
        // Gley.Notifications.API.SendRepeatNotification(title, description, new TimeSpan(0, 0, 10),
        //     new TimeSpan(0, 0, 0, 30));
        Gley.Notifications.API.SendNotification(title, description, new TimeSpan(0, 0, 10));
    }
}