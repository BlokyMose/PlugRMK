using System;
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

        public static IEnumerator Delay(float delay, Action onFinished)
        {
            yield return new WaitForSeconds(delay);
            onFinished();
        }

        public static void StartDelay(this MonoBehaviour go, float delay, Action onFinished)
        {
            go.StartCoroutine(Delay(delay, onFinished));
        }

        public static IEnumerator Update(Func<float, float> onUpdate)
        {
            var time = 0f;
            while (true)
            {
                time = onUpdate(time);
                time += Time.deltaTime;
                yield return null;
            }
        }

        public static void StartUpdate(this MonoBehaviour go, Func<float, float> onUpdate)
        {
            go.StartCoroutine(Update(onUpdate));
        }
    }
}