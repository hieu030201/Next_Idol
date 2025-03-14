using UnityEngine;

namespace Yun.Scripts.Core
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    Debug.Log(typeof(T) + " is NULL.");
                return _instance;
            }
        }

        [SerializeField] private bool dontDestroyOnload;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
                if (dontDestroyOnload)
                {
                    DontDestroyOnLoad(this);
                }
            }
        }

        public static bool IsInstanceExisted() => _instance != null;
    }
}
