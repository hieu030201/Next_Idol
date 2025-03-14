using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yun.Scripts.Utilities
{
    public abstract class Singleton<T> where T : new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }

    public class ManualSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] bool dontDestroyOnLoad;
        private static T _instance;
        private static bool _applicationIsQuitting;
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting) return null;
                if (_instance == null)
                { Debug.LogError("Cannot find Object with type " + typeof(T)); }
                return _instance;
            }
        }
        public static bool IsInstanceValid()
        {
            return (_instance != null);
        }
        public virtual void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning("Already has instance of " + typeof(T));
                GameObject.Destroy(this);
                return;
            }

            if (_instance == null)
                _instance = (T)(MonoBehaviour)this;

            if (_instance == null)
            {
                Debug.LogError("Still null after Awake " + typeof(T));
            }
            if (dontDestroyOnLoad)
                DontDestroyOnLoad(this);
        }
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }
}