/// [Note]
/// Adapted from https://easings.net/
///

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    public static class EaseUtility
    {
        public struct EaseParameter
        {
            public float timeStart;
            public float valueStart;
            public float timeEnd;
            public float valueEnd;
            public float timeNow;

            public EaseParameter(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                this.timeStart = timeStart;
                this.valueStart = valueStart;
                this.timeEnd = timeEnd;
                this.valueEnd = valueEnd;
                this.timeNow = timeNow;
            }
        }

        public class Ease
        {
            float timeStart;
            float valueStart;
            float timeEnd; 
            float valueEnd;
            Func<EaseParameter, float> OnEvaluate;

            public Ease(float timeStart, float valueStart, float timeEnd, float valueEnd, Func<EaseParameter, float> OnEvaluate)
            {
                this.timeStart = timeStart;
                this.valueStart = valueStart;
                this.timeEnd = timeEnd;
                this.valueEnd = valueEnd;
                this.OnEvaluate = OnEvaluate;
            }

            public float Evaluate(float time)
            {
                return OnEvaluate(new(timeStart, valueStart, timeEnd, valueEnd, time));
            }

            #region [Out Quad]

            public static Ease OutQuad(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, OutQuadEvaluate);
            }

            static float OutQuadEvaluate(EaseParameter param)
            {
                return OutQuadEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float OutQuadEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : (1 - (1 - timeRatio) * (1 - timeRatio)) * (valueEnd - valueStart) + valueStart;
            }

            #endregion

            #region [In Quad]

            public static Ease InQuad(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, InQuadEvaluate);
            }

            static float InQuadEvaluate(EaseParameter param)
            {
                return InQuadEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float InQuadEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : (timeRatio * timeRatio) * (valueEnd - valueStart) + valueStart;
            }


            #endregion

            #region [InOut Quad]

            public static Ease InOutQuad(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, InOutQuadEvaluate);
            }

            static float InOutQuadEvaluate(EaseParameter param)
            {
                return InOutQuadEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float InOutQuadEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : (timeRatio < 0.5f 
                        ? 2 * timeRatio * timeRatio 
                        : 1 - Mathf.Pow(-2 * timeRatio + 2, 2) / 2
                    ) * (valueEnd - valueStart) + valueStart;
            }

            #endregion

            // ===

            #region [Out Sine]

            public static Ease OutSine(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, OutSineEvaluate);
            }

            static float OutSineEvaluate(EaseParameter param)
            {
                return OutSineEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float OutSineEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : Mathf.Sin((timeRatio * Mathf.PI) / 2) * (valueEnd - valueStart) + valueStart;
            }

            #endregion

            #region [In Sine]

            public static Ease InSine(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, InSineEvaluate);
            }

            static float InSineEvaluate(EaseParameter param)
            {
                return InSineEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float InSineEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : (1 - Mathf.Cos((timeRatio * Mathf.PI) / 2)) * (valueEnd - valueStart) + valueStart;
            }


            #endregion

            #region [InOut Sine]

            public static Ease InOutSine(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, InOutSineEvaluate);
            }

            static float InOutSineEvaluate(EaseParameter param)
            {
                return InOutSineEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float InOutSineEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : (-(Mathf.Cos(Mathf.PI * timeRatio) - 1) / 2) * (valueEnd - valueStart) + valueStart;
            }

            #endregion

            // ===

            #region [Out Elastic]

            public static Ease OutElastic(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, OutElasticEvaluate);
            }

            static float OutElasticEvaluate(EaseParameter param)
            {
                return OutElasticEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float OutElasticEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var c = (2 * Mathf.PI) / 3;
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : (Mathf.Pow(2, -10 * timeRatio) * Mathf.Sin((timeRatio * 10 - 0.75f) * c) + 1) * (valueEnd - valueStart) + valueStart;
            }

            #endregion

            #region [In Elastic]

            public static Ease InElastic(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, InElasticEvaluate);
            }

            static float InElasticEvaluate(EaseParameter param)
            {
                return InElasticEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float InElasticEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var c = (2 * Mathf.PI) / 3;
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : (-Mathf.Pow(2, 10 * timeRatio - 10) * Mathf.Sin((timeRatio * 10 - 10.75f) * c)) * (valueEnd - valueStart) + valueStart;
            }

            #endregion
            
            #region [InOut Elastic]

            public static Ease InOutElastic(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, InOutElasticEvaluate);
            }

            static float InOutElasticEvaluate(EaseParameter param)
            {
                return InOutElasticEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float InOutElasticEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var c = (2 * Mathf.PI) / 4.5f;
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : timeRatio < .5f
                  ? (-(Mathf.Pow(2, 20 * timeRatio - 10) * Mathf.Sin((timeRatio * 20 - 11.125f) * c))/2) * (valueEnd - valueStart) + valueStart
                  : ((Mathf.Pow(2, -20 * timeRatio + 10) * Mathf.Sin((timeRatio * 20 - 11.125f) * c))/2 + 1) * (valueEnd - valueStart) + valueStart;
            }

            #endregion

            // ===

            #region [Out Bounce]

            public static Ease OutBounce(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, OutBounceEvaluate);
            }

            static float OutBounceEvaluate(EaseParameter param)
            {
                return OutBounceEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float OutBounceEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var n1 = 7.5625f;
                var d1 = 2.75f;
                var x = (timeNow - timeStart) / (timeEnd - timeStart);

                if (x <= 0f)
                {
                    return valueStart;
                }
                else if (x >= 1f)
                {
                    return valueEnd;
                }
                else if (x < 1 / d1)
                {
                    return (n1 * x * x) * (valueEnd - valueStart) + valueStart;
                }
                else if (x < 2 / d1)
                {
                    return (n1 * (x -= 1.5f / d1) * x + 0.75f) * (valueEnd - valueStart) + valueStart;
                }
                else if (x < 2.5 / d1)
                {
                    return (n1 * (x -= 2.25f / d1) * x + 0.9375f) * (valueEnd - valueStart) + valueStart;
                }
                else
                {
                    return (n1 * (x -= 2.625f / d1) * x + 0.984375f) * (valueEnd - valueStart) + valueStart;
                }
            }

            #endregion

            #region [In Bounce]

            public static Ease InBounce(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, InBounceEvaluate);
            }

            static float InBounceEvaluate(EaseParameter param)
            {
                return InBounceEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float InBounceEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                return valueEnd - OutBounceEvaluate(timeStart, valueStart, timeEnd, valueEnd, timeEnd - timeNow) + valueStart;
            }

            #endregion
            
            #region [InOut Bounce]

            public static Ease InOutBounce(float timeStart, float valueStart, float timeEnd, float valueEnd)
            {
                return new(timeStart, valueStart, timeEnd, valueEnd, InOutBounceEvaluate);
            }

            static float InOutBounceEvaluate(EaseParameter param)
            {
                return InOutBounceEvaluate(param.timeStart, param.valueStart, param.timeEnd, param.valueEnd, param.timeNow);
            }

            static float InOutBounceEvaluate(float timeStart, float valueStart, float timeEnd, float valueEnd, float timeNow)
            {
                var timeRatio = (timeNow - timeStart) / (timeEnd - timeStart);

                return timeNow <= timeStart
                  ? valueStart
                  : timeNow >= timeEnd
                  ? valueEnd
                  : timeRatio < .5f
                  ? (valueEnd - OutBounceEvaluate(timeStart, valueStart, timeEnd, valueEnd, timeEnd - 2 * timeNow)) / 2 + valueStart
                  : (valueEnd + OutBounceEvaluate(timeStart, valueStart, timeEnd, valueEnd, 2 * timeNow - timeEnd)) / 2;
            }

            #endregion

        }
    }
}
