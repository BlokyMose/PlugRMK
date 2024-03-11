using PlugRMK.GenericUti;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    [AddComponentMenu("Unity Utility/Transform Controller")]
    [Icon(GameComponentsIcon.UNI_POS)]

    public class TransformController : MonoBehaviour
    {
        [Serializable]
        public class InitTransform
        {
            public Vector3 pos;
            public bool isWorldPos;
            public Vector3 rot;
            public bool isWorldRot;
        }

        [Serializable]
        public class ModifyTransform
        {
            public enum Modifier { Setter, Add, Subtract, Multiply, Divide };
            public Modifier modifier;
            public float period = -1;
            public bool isWorldPos;
            public Vector3 pos;
            public bool isWorldRot;
            public Vector3 rot;
        }

        public UnityInitialMethod initMethod;
        public InitTransform initTransform = new();
        public List<ModifyTransform> modifyTransforms = new();

        void Awake()
        {
            if (initMethod == UnityInitialMethod.Awake) Init();
        }

        void Start()
        {
            if (initMethod == UnityInitialMethod.Start) Init();
        }

    
        void OnEnable()
        {
            if (initMethod == UnityInitialMethod.OnEnable) Init();
        }


        void Init()
        {
            if (initTransform.isWorldPos)
                transform.position = initTransform.pos;
            else
                transform.localPosition = initTransform.pos;
        
            if (initTransform.isWorldRot)
                transform.eulerAngles = initTransform.rot;
            else
                transform.localEulerAngles = initTransform.rot;

            StopAllCoroutines();

            foreach (var modifier in modifyTransforms)
            {
                if (modifier.period > 0)
                    StartCoroutine(ModifyWithPeriod(modifier.period, modifier));
                else
                    StartCoroutine(ModifyWithNoPeriod(modifier));
            }

            IEnumerator ModifyWithPeriod(float period, ModifyTransform modifyTransform)
            {
                while (true)
                {
                    Modify(modifyTransform);
                    yield return new WaitForSeconds(period);
                }
            }

            IEnumerator ModifyWithNoPeriod(ModifyTransform modifyTransform)
            {
                while (true)
                {
                    Modify(modifyTransform);
                    yield return null;
                }
            }
        }

        void Modify(ModifyTransform modifyTransform)
        {
            switch (modifyTransform.modifier)
            {
                case ModifyTransform.Modifier.Setter:
                    if (modifyTransform.isWorldPos)
                        transform.position = modifyTransform.pos;
                    else
                        transform.localPosition = modifyTransform.pos;

                    if (modifyTransform.isWorldRot)
                        transform.eulerAngles = modifyTransform.rot;
                    else
                        transform.localEulerAngles = modifyTransform.rot;
                    break;

                case ModifyTransform.Modifier.Add:
                    transform.localPosition += modifyTransform.pos;
                    transform.localEulerAngles += modifyTransform.rot;
                    break;

                case ModifyTransform.Modifier.Subtract:
                    transform.localPosition -= modifyTransform.pos;
                    transform.localEulerAngles -= modifyTransform.rot;
                    break;

                case ModifyTransform.Modifier.Multiply:
                    transform.localPosition = new(
                        transform.localPosition.x * modifyTransform.pos.x,
                        transform.localPosition.y * modifyTransform.pos.y,
                        transform.localPosition.z * modifyTransform.pos.z);

                    transform.localEulerAngles = new(
                        transform.localPosition.x * modifyTransform.pos.x,
                        transform.localPosition.y * modifyTransform.pos.y,
                        transform.localPosition.z * modifyTransform.pos.z);
                    break;

                case ModifyTransform.Modifier.Divide:
                    transform.localPosition = new(
                        transform.localPosition.x / modifyTransform.pos.x,
                        transform.localPosition.y / modifyTransform.pos.y,
                        transform.localPosition.z / modifyTransform.pos.z);

                    transform.localEulerAngles = new(
                        transform.localPosition.x / modifyTransform.pos.x,
                        transform.localPosition.y / modifyTransform.pos.y,
                        transform.localPosition.z / modifyTransform.pos.z);
                    break;
            }
        }
    }

}
