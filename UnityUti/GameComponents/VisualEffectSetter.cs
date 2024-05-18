using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Visual Effect Setter")]
    [Icon(GameComponentsIcon.FX)]

    public class VisualEffectSetter : MonoBehaviour
    {
        [Serializable]
        public class SetterProperty
        {
            public enum VarType { Bool, Float, Int}

            public string setterName;
            public string varName;
            public VarType varType;

            public bool boolValue;
            
            public float floatValue;

            public int intValue;


            public void Invoke(VisualEffect vfx)
            {
                switch (varType)
                {
                    case VarType.Bool:
                        vfx.SetBool(varName, boolValue);
                        break;
                    case VarType.Float:
                        vfx.SetFloat(varName, floatValue);
                        break;
                    case VarType.Int:
                        vfx.SetInt(varName, intValue);
                        break;
                }
            }
        }

        [SerializeField]
        VisualEffect vfx;

        [SerializeField]
        List<SetterProperty> setters = new();

        void Awake()
        {
            if (TryGetComponent<VisualEffect>(out var visualEffect))
                vfx = visualEffect;
        }

        public void InvokeSetter(string setterName)
        {
            var foundSetter = setters.Find(s => s.setterName == setterName);
            if (foundSetter!=null)
            {
                foundSetter.Invoke(vfx);
            }
        }

    }
}
