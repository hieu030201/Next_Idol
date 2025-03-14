using UnityEngine;

namespace Yun.Scripts.Utilities
{
    public class HideWhenPlay : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
