using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

namespace Yun.Scripts.Notifications
{
    public class BasicNotifications : MonoBehaviour
    {
        public static BasicNotifications _instant;

        public static BasicNotifications Instant
        {
            get
            {
                if (_instant == null)
                {
                    _instant = FindObjectOfType<BasicNotifications>();
                }

                return _instant;
            }
        }
        private void Awake()
        {
            if (_instant != null && _instant.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
                Destroy(this.gameObject);
            else
                _instant = this.GetComponent<BasicNotifications>();

            DontDestroyOnLoad(this.gameObject);
        }

        public void onTouchSendNativeNotification()
        {
            try
            {
                if (UnityEngine.Android.Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
                {
                    Debug.Log("permission granted!!");
                }
                else
                {
                    // if (common.request_permission_notify == 0)
                    //     UnityEngine.Android.Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");

                    return;
                }

                // create new channel to show notification
                var channel = new AndroidNotificationChannel()
                {
                    Id = "channel_id",
                    Name = "Default Chanel",
                    Importance = Importance.Default,
                    Description = "Generic notification",
                };

                List<string> lmsg = new List<string>()
                {
                    "Hello my friend, today you haven't played Some News yet, play it now!"
                };

                AndroidNotificationCenter.RegisterNotificationChannel(channel);

                var notification = new AndroidNotification();
                notification.Title = "Bus Jam";
                notification.Text = lmsg[0];

                /// Timed notification
                // notification.FireTime = System.DateTime.Now.AddHours(common.noti_capping_time);
                notification.FireTime = System.DateTime.Now.AddHours(12);

                // Send notification
                var identifier = AndroidNotificationCenter.SendNotification(notification, "channel_id");

                if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
                {
                    //MessageBox.instance.showMessage("SendNotification Scheduled");
                    AndroidNotificationCenter.CancelAllNotifications();
                    AndroidNotificationCenter.SendNotification(notification, "channel_id");
                }
            }
            catch
            {

            }
        }
    }
}
