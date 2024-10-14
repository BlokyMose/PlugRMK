using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static PlugRMK.UnityUti.GOPoolUtility;

namespace PlugRMK.UnityUti
{
    public class GOPoolManager : SingletonGO<GOPoolManager>
    {
        Dictionary<GameObject, ObjectPool<GameObject>> pools = new();

        public void AddPools(List<GameObject> prefabs)
        {
            foreach (var prefab in prefabs)
                if (!pools.ContainsKey(prefab))
                    AddPool(prefab);
        }

        public void AddPool(GameObject prefab)
        {
            pools.Add(prefab, CreatePool(prefab));
        }

        public GameObject GetGO(GameObject prefab)
        {
            if (!pools.ContainsKey(prefab))
                AddPool(prefab);

            return pools[prefab].Get();
        }

        public bool TryGetGO(GameObject prefab, out GameObject go)
        {
            if (pools.ContainsKey(prefab))
            {
                go = pools[prefab].Get();
                return true;
            }
            else
            {
                go = null;
                return false;
            }
        }
    }
}
