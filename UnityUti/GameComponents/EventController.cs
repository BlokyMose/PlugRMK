using PlugRMK.GenericUti;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Event Controller")]
    [Icon(GameComponentsIcon.EVENT)]

    public class EventController : MonoBehaviour
    {
        [Serializable]
        public class EventProperty
        {
            public string eventName;
            public bool isAutoInvoke = true;
            public float delay = -1;
            public UnityEvent onInvoke;

            public void InvokeWithDelay(MonoBehaviour parent)
            {
                if (delay < 0)
                    onInvoke.Invoke();
                else
                    parent.StartCoroutine(Delay(delay));

                IEnumerator Delay(float delay)
                {
                    yield return new WaitForSeconds(delay);
                    onInvoke.Invoke();
                }
            }

            public void InvokeWithoutDelay()
            {
                onInvoke.Invoke();
            }
        }

        public bool autoInvoke = true;

        public UnityInitialMethod invokeWhen;

        public List<EventProperty> events = new();

        void Awake()
        {
            if (autoInvoke && invokeWhen == UnityInitialMethod.Awake)
                InvokeAll();
        }

        void Start()
        {
            if (autoInvoke && invokeWhen == UnityInitialMethod.Start)
                InvokeAll();
        }

        void OnEnable()
        {
            if (autoInvoke && invokeWhen == UnityInitialMethod.OnEnable)
                InvokeAll();
        }

        public virtual void InvokeAll()
        {
            foreach (var eventProp in events)
                eventProp.InvokeWithDelay(this);
        }

        public void Invoke(string eventName)
        {
            var foundEvent = events.Find(x => x.eventName == eventName);
            if (foundEvent != null)
                foundEvent.InvokeWithDelay(this);
        }

        public void InvokeWithoutDelay(string eventName)
        {
            var foundEvent = events.Find(x => x.eventName == eventName);
            if (foundEvent != null)
                foundEvent.InvokeWithoutDelay();
        }
    }
}
