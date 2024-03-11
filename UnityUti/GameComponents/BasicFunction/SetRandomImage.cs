using PlugRMK.GenericUti;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlugRMK.UnityUti.BasicFunction
{
    [AddComponentMenu("Unity Utility/Basic/Set Random Image (IBasicFuntion)")]
    [Icon(GameComponentsIcon.UNI_SCRIPT)]

    public class SetRandomImage : MonoBehaviour, IBasicFunction
    {
        [SerializeField]
        Image image;

        [SerializeField]
        List<Sprite> sprites = new();

        void Awake()
        {
            if (image == null)
                image = this.GetComponentInFamily<Image>();

            if (image.sprite != null && !sprites.Contains(image.sprite))
                sprites.Add(image.sprite);
        }

        public void Invoke()
        {
            image.sprite = sprites.GetRandom();
        }
    }
}
