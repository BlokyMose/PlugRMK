using PlugRMK.GenericUti;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti.BasicFunction
{
    [AddComponentMenu("Unity Utility/Basic/Set Random Sprite (IBasicFuntion)")]
    [Icon(GameComponentsIcon.UNI_SCRIPT)]

    public class SetRandomSprite : MonoBehaviour, IBasicFunction
    {
        [SerializeField]
        SpriteRenderer sr;

        [SerializeField]
        List<Sprite> sprites = new();

        void Awake()
        {
            if (sr == null)
                sr = this.GetComponentInFamily<SpriteRenderer>();

            if (sr.sprite != null && !sprites.Contains(sr.sprite)) 
                sprites.Add(sr.sprite);
        }

        public void Invoke()
        {
            sr.sprite = sprites.GetRandom();
        }
    }
}
