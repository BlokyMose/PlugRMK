using System.Collections;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    public static class CoroutineUtility
    {
        public static Coroutine RestartCoroutine(this MonoBehaviour go, IEnumerator routine, Coroutine stopCoroutine)
        {
            if (stopCoroutine != null) go.StopCoroutine(stopCoroutine);
            return go.StartCoroutine(routine);
        }

        public static void StopCoroutineIfExists(this MonoBehaviour go, Coroutine routine)
        {
            if (routine != null) go.StopCoroutine(routine);
        }
    }
}