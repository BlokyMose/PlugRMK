using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Event Controller (Basic Function)")]

    public class EventController_BasicFunction : EventController
    {
        public override void InvokeAll()
        {
            base.InvokeAll();
            var allBasicFunctions = GetComponents<IBasicFunction>();
            foreach (var basic in allBasicFunctions)
                basic.Invoke();
        }
    }
}

