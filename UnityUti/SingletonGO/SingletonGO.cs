using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    public class SingletonGO<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                { 
                    instance = FindFirstObjectByType<T>();
                    if (instance == null)
                    {
                        var go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        void Awake()
        {
            if (instance != null && instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)(object)this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void OnDestroy()
        {
            if (instance != null)
                instance = null;
        }
    }
}
