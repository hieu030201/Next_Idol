#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
using UnityEngine;

namespace Advertising
{
    public class AskAppTracking : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_IOS
            // Check the user's consent status.
            // If the status is undetermined, display the request request:
            print("Ask App Tracking Test");
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
#endif
        }
    }
}