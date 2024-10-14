using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PlugRMK.UnityUti
{
    public class GOPoolTarget : MonoBehaviour
    {
        IObjectPool<GameObject> pool;
        public void SetObjectPool(IObjectPool<GameObject> pool)
        {
            this.pool = pool;
        }
        
        public IObjectPool<GameObject> GetObjectPool()
        {
            return pool;
        }

        public void ReleaseToPool()
        {
            pool.Release(gameObject);
        }
    }
}
