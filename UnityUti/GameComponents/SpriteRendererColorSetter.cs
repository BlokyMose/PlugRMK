using PlugRMK.GenericUti;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Sprite Renderer Color Setter")]
    [Icon(GameComponentsIcon.UNI_SPRITE_RENDERER)]
    public class SpriteRendererColorSetter : MonoBehaviour
    {
        [System.Serializable]
        public class CustomAlpha
        {
            [SerializeField]
            SpriteRenderer sr;

            [SerializeField]
            float maxAlpha = 1f;

            [SerializeField]
            float minAlpha = 0f;

            public SpriteRenderer SR { get => sr; }
            public float MaxAlpha { get => maxAlpha; }
            public float MinAlpha { get => minAlpha; }
        }

        public List<SpriteRenderer> srs = new();

        public List<SpriteRenderer> excludeSRs = new();

        public List<CustomAlpha> customAlphas = new();

        Dictionary<SpriteRenderer, Color> initialColors = new();

        public void Awake()
        {
            for (int i = srs.Count - 1; i >= 0; i--)
            {
                var sr = srs[i];
                if (sr != null)
                    initialColors.AddIfHasnt(sr, sr.color);
                else
                    srs.Remove(sr);
            }
        }

        [ContextMenu("Auto-Set SRs")]
        public void AutoSetSRs()
        {
            srs.AddIfHasnt(gameObject.GetComponentsInFamily<SpriteRenderer>());

            foreach (var sr in excludeSRs)
            {
                var foundSR = srs.Find(x => x == sr);
                if (foundSR != null) 
                    srs.Remove(foundSR);
            }

            srs.RemoveNulls();
        }

        #region [Methods: Color]

        public void ChangeColor(Color color)
        {
            foreach (var sr in srs)
                sr.color = color;
        }

        public void ChangeColorExceptAlpha(Color color)
        {
            foreach (var sr in srs)
            {
                var currentAlpha = sr.color.a;
                sr.color = color.ChangeAlpha(currentAlpha);
            }
        }

        public void ResetColor()
        {
            foreach (var sr in initialColors)
                sr.Key.color = sr.Value;
        }

        public void ResetColorExceptAlpha()
        {
            foreach (var sr in initialColors)
            {
                var currentAlpha = sr.Key.color.a;
                sr.Key.color = sr.Value.ChangeAlpha(currentAlpha);
            }
        }

        #endregion

        public void RemoveSR(SpriteRenderer sr)
        {
            srs.RemoveIfHas(sr);
            excludeSRs.Add(sr);
        }

        #region [Methods: Alpha]

        public void BeTransparent(float duration)
        {
            ChangeAlpha(duration, 0f);
        }

        public void BeTransparentFromOpaque(float duration)
        {
            ChangeAlpha(duration, 0f, 1f);
        }

        public void BeOpaque(float duration)
        {
            ChangeAlpha(duration, 1f);
        }

        public void BeOpaqueFromTransparent(float duration)
        {
            ChangeAlpha(duration, 1f, 0f);
        }

        /// <summary>
        /// The parameters are duration, followed by a semi-colon, then the alpha value<br></br>
        /// Example: "0.15;1"
        /// </summary>
        public void ChangeAlpha(string parameters)
        {
            var parametersSplitted = parameters.Split(';');
            if (float.TryParse(parametersSplitted[0], out float duration))
            {
                if (float.TryParse(parametersSplitted[1], out float alpha))
                {
                    ChangeAlpha(duration, alpha);
                }
            }

            Debug.LogWarning("Failed to change alpha because the string format is wrong; it should be 'duration;alpha'");
        }

        public void ChangeAlpha(float duration, float alpha, float? alphaOrigin = null)
        {
            StopAllCoroutines();
            StartCoroutine(Delay());
            IEnumerator Delay()
            {
                var curve = AnimationCurve.EaseInOut(0, alphaOrigin == null ? srs[0].color.a : (float)alphaOrigin, duration, alpha);
                var time = 0f;
                while (time < duration)
                {
                    ChangeAlpha(curve.Evaluate(time));
                    time += Time.deltaTime;
                    yield return null;
                }

                ChangeAlpha(alpha);
            }
        }

        public void ChangeAlpha(float alpha)
        {
            foreach (var sr in srs)
            {
                sr.color = sr.color.ChangeAlpha(alpha);
            }

            foreach (var sr in customAlphas)
            {
                if (sr.SR.color.a > sr.MaxAlpha)
                {
                    sr.SR.color = sr.SR.color.ChangeAlpha(sr.MaxAlpha);
                }
                else if (sr.SR.color.a < sr.MinAlpha)
                {
                    sr.SR.color = sr.SR.color.ChangeAlpha(sr.MinAlpha);
                }
            }
        }

        public void BeTransparent()
        {
            ChangeAlpha(0);
        }

        #endregion
    }
}
