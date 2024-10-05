using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestNameSpace
{
    public class TestComponent : MonoBehaviour
    {
        #region [Exposed Variables]

        public int a = 10;
        public int b;
        public float c = .1f;

        #endregion [Exposed Variables]


        #region [Exposer Methods]

        public void SetExposedVariables(ExposedFiles.TestComponent_Exposer exposer)
        {
            a = exposer.a;
            b = exposer.b;
        }

        #endregion

    }
}

