using UnityEngine;

namespace PlugRMK.UnityUti.BasicFunction
{
    [AddComponentMenu("Unity Utility/Basic/Destroy GO (IBasicFuntion)")]
    [Icon(GameComponentsIcon.UNI_SCRIPT)]

    public class DestroyGO : MonoBehaviour, IBasicFunction
    {
        public bool isDestroySelf = false;
        public GameObject target;

        public void Invoke()
        {
            if (isDestroySelf || target == null)
                Destroy(gameObject);
        }
    }

}
