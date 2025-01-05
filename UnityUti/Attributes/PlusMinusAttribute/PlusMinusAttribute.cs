using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PlusMinusAttribute : PropertyAttribute
    {
        public float Increment { get; private set; } = 1;

        public PlusMinusAttribute(float increment = 1)
        {
            Increment = increment;
        }
    }
}
