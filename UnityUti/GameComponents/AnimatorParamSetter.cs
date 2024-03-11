using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Basic/Animator Param Setter (IBasicFunction)")]
    [Icon(GameComponentsIcon.UNI_ANIMATION)]

    public class AnimatorParamSetter : MonoBehaviour, IBasicFunction
    {
        [SerializeField]
        Animator animator;

        [SerializeField]
        List<GameplayUtilityClass.AnimatorParameterStatic> parameters = new List<GameplayUtilityClass.AnimatorParameterStatic>();

         void Awake()
        {
            if (animator == null)
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
