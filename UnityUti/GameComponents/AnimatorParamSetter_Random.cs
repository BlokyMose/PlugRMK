using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Basic/Animator Param Setter (Random)")]
    public class AnimatorParamSetter_Random : MonoBehaviour, IBasicFunction
    {
        [SerializeField]
        List<GameplayUtilityClass.AnimatorParameterRandom> parameters = new List<GameplayUtilityClass.AnimatorParameterRandom>();

        Animator animator;

        protected void Awake()
        {
            animator = GetComponent<Animator>();
            foreach (var param in parameters)
                param.Init();
        }

        public void Invoke()
        {
            SetAllParams();
        }

        public void SetParam(string setterName)
        {
            foreach (var param in parameters)
                if (param.SetterName == setterName)
                    param.SetParam(animator);
        }

        public void SetAllParams()
        {
            foreach (var param in parameters)
                param.SetParam(animator);
        }
    }
}
