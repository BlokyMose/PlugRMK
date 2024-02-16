using System.Collections;
using System.Collections.Generic;
using static PlugRMK.UnityUti.EaseUtility;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    public static class CorAnimUtility2D
    {
        public static Coroutine ScalingTo(this MonoBehaviour go, Vector2 targetScale, float duration, Coroutine stopCoroutine, EaseType ease = EaseType.Sine, Transform transform = null)
        {
            if (transform == null)
                transform = go.transform;

            return go.RestartCoroutine(ScalingTo(transform, targetScale, duration, ease), stopCoroutine);
        }

        public static Coroutine MovingLocallyTo(this MonoBehaviour go, Vector2 targetPos, float duration, Coroutine stopCoroutine, EaseType ease = EaseType.Sine, Transform transform = null)
        {
            if (transform == null)
                transform = go.transform;

            return go.RestartCoroutine(MovingLocallyTo(transform, targetPos, duration, ease), stopCoroutine);
        }

        public static Coroutine MovingTo(this MonoBehaviour go, Vector2 targetPos, float duration, Coroutine stopCoroutine, EaseType ease = EaseType.Sine, Transform transform = null)
        {
            if (transform == null)
                transform = go.transform;

            return go.RestartCoroutine(MovingTo(transform, targetPos, duration, ease), stopCoroutine);
        }

        static IEnumerator ScalingTo(Transform transform, Vector2 targetScale, float duration, EaseType ease = EaseType.Sine)
        {
            var curveX = new Ease (0, transform.localScale.x, duration, targetScale.x, ease);
            var curveY = new Ease (0, transform.localScale.y, duration, targetScale.y, ease);
            var time = 0f;
            while (time < duration)
            {
                transform.localScale = new(curveX.Evaluate(time), curveY.Evaluate(time), transform.localScale.z);
                time += Time.deltaTime;
                yield return null;
            }
        }

        static IEnumerator MovingLocallyTo(Transform transform, Vector2 targetPos, float duration, EaseType ease = EaseType.Sine)
        {
            var curveX = new Ease (0, transform.localPosition.x, duration, targetPos.x, ease);
            var curveY = new Ease (0, transform.localPosition.y, duration, targetPos.y, ease);
            var time = 0f;
            while (time < duration)
            {
                transform.localPosition = new(curveX.Evaluate(time), curveY.Evaluate(time), transform.localPosition.z);
                time += Time.deltaTime;
                yield return null;
            }
        }

        static IEnumerator MovingTo(Transform transform, Vector2 targetPos, float duration, EaseType ease = EaseType.Sine)
        {
            var curveX = new Ease (0, transform.position.x, duration, targetPos.x, ease);
            var curveY = new Ease (0, transform.position.y, duration, targetPos.y, ease);
            var time = 0f;
            while (time < duration)
            {
                transform.position = new(curveX.Evaluate(time), curveY.Evaluate(time), transform.position.z);
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
