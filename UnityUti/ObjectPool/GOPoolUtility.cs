using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PlugRMK.UnityUti
{
    public static class GOPoolUtility
    {
        public static Dictionary<GameObject, ObjectPool<GameObject>> CreatePools(
            List<GameObject> prefabs,
            bool collectionCheck = true,
            int defaultCapacity = 10,
            int maxSize = 100)
        {
            var pools = new Dictionary<GameObject, ObjectPool<GameObject>>();
            foreach (var prefab in prefabs)
                pools.Add(prefab, CreatePool(prefab, collectionCheck, defaultCapacity, maxSize));
            return pools;
        }

        public static ObjectPool<GameObject> CreatePool(
            GameObject prefab, 
            bool collectionCheck = true, 
            int defaultCapacity = 10, 
            int maxSize = 100)
        {
            ObjectPool<GameObject> pool = null;
            pool = new ObjectPool<GameObject>(
                InstantiateGO,
                OnGetFromPool,
                OnReleaseToPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxSize);

            return pool;

            GameObject InstantiateGO()
            {
                var newGO = GameObject.Instantiate(prefab);
                newGO.name = prefab.name + "_" + pool.CountAll;
                newGO.SetActive(false);

                if (newGO.TryGetComponent<GOPoolTarget>(out var target))
                    target.SetObjectPool(pool);
                else
                    Debug.LogWarning($"{prefab.name} doesn't have {nameof(GOPoolTarget)}");

                return newGO;
            }

            static void OnGetFromPool(GameObject go)
            {
            }

            static void OnReleaseToPool(GameObject go)
            {
            }

            static void OnDestroyPoolObject(GameObject go)
            {
            }
        }
    }
}
