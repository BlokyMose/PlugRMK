using UnityEngine;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Basic/Instantiate GO (IBasicFuntion)")]
    [Icon(GameComponentsIcon.UNI_SCRIPT)]

    public class InstantiateGO : MonoBehaviour, IBasicFunction
    {
        [SerializeField]
        GameObject prefab;

        [SerializeField]
        bool isOverrideScale = false;

        [SerializeField]
        Vector2 scaleOverride = Vector2.one;

        [SerializeField]
        bool isOverrideRotation = false;

        [SerializeField]
        Vector3 rotationOverride = Vector3.zero;

        public void Invoke()
        {
            var go = Instantiate(prefab, null);
            go.SetActive(true);
            go.transform.position = transform.position;

            if (isOverrideRotation)
                go.transform.localEulerAngles = rotationOverride;
            else
                go.transform.localEulerAngles = transform.localEulerAngles;

            if (isOverrideScale) 
                go.transform.localScale = scaleOverride;
        }
    }
}
