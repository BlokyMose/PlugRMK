using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PlugRMK.UnityUti
{
    public static class GameplayUtilityClass
    {
        [Serializable]
        public abstract class AnimatorParameter
        {
            public enum DataType { Float, Int, Bool, Trigger }

            [SerializeField]
            protected string paramName;
            public string ParamName => paramName;

            [SerializeField]
            protected DataType dataType;
            public DataType Type => dataType;

            [SerializeField]
            public string setterName;

            public abstract int IntValue { get; }

            public abstract float FloatValue { get; }

            public abstract bool BoolValue { get; }

            public string SetterName => setterName;

            protected int hash;

            public void Init()
            {
                hash = Animator.StringToHash(paramName);
            }


            public virtual void SetParam(Animator animator)
            {
                switch (dataType)
                {
                    case DataType.Float:
                        animator.SetFloat(hash, FloatValue);
                        break;
                    case DataType.Int:
                        animator.SetInteger(hash, IntValue);
                        break;
                    case DataType.Bool:
                        animator.SetBool(hash, BoolValue);
                        break;
                    case DataType.Trigger:
                        animator.SetTrigger(hash);
                        break;
                    default:
                        break;
                }
            }

            public virtual bool TrySetParam(Animator animator, string setterName)
            {
                if (setterName != this.setterName)
                    return false;

                SetParam(animator);
                return true;
            }
        }

        [Serializable]
        public class AnimatorParameterStatic : AnimatorParameter
        {
            [SerializeField]
            protected int intValue;
            public override int IntValue => intValue;

            [SerializeField]
            protected float floatValue;
            public override float FloatValue => floatValue;

            [SerializeField]
            protected bool boolValue;
            public override bool BoolValue => boolValue;

            public AnimatorParameterStatic(string paramName, int intValue)
            {
                this.paramName = paramName;
                this.dataType = DataType.Int;
                this.intValue = intValue;
            }

            public AnimatorParameterStatic(string paramName, float floatValue)
            {
                this.paramName = paramName;
                this.dataType = DataType.Float;
                this.floatValue = floatValue;
            }

            public AnimatorParameterStatic(string paramName, bool boolValue)
            {
                this.paramName = paramName;
                this.dataType = DataType.Bool;
                this.boolValue = boolValue;
            }

            public AnimatorParameterStatic(string paramName)
            {
                this.paramName = paramName;
                this.dataType = DataType.Trigger;
            }

        }

        [Serializable]
        public class AnimatorParameterRandom : AnimatorParameter
        {
            public enum NumberRandomMode { Between, List }

            [SerializeField]
            protected NumberRandomMode numberRandomMode;

            [SerializeField]
            protected List<int> intRandomList;
            public List<int> IntRandomList => intRandomList;


            [SerializeField]
            protected Vector2Int intRandomBetween;
            public Vector2Int IntRandomBetween => intRandomBetween;


            [SerializeField]
            protected List<float> floatRandomList;
            public List<float> FloatRandomList => floatRandomList;


            [SerializeField]
            protected Vector2 floatRandomBetween;
            public Vector2 FloatRandomBetween => floatRandomBetween;

            [SerializeField]
            protected float boolTrueChance;
            public float BoolTrueChance => boolTrueChance;

            public override float FloatValue
            {
                get
                {
                    switch (numberRandomMode)
                    {
                        case NumberRandomMode.Between:
                            return Random.Range(floatRandomBetween.x, floatRandomBetween.y);
                        case NumberRandomMode.List:
                            var random = Random.Range(0, floatRandomList.Count);
                            return intRandomList[random];
                        default:
                            return 0f;
                    }
                }
            }

            public override int IntValue
            {
                get
                {
                    switch (numberRandomMode)
                    {
                        case NumberRandomMode.Between:
                            return Random.Range(intRandomBetween.x, intRandomBetween.y);
                        case NumberRandomMode.List:
                            var random = Random.Range(0, intRandomList.Count);
                            return intRandomList[random];
                        default:
                            return 0;
                    }
                }
            }

            public override bool BoolValue
            {
                get
                {
                    var random = Random.Range(0, 1f);
                    return random < boolTrueChance;
                }
            }

            public AnimatorParameterRandom(string paramName, List<int> intRandomValueList)
            {
                this.paramName = paramName;
                this.intRandomList = intRandomValueList;
            }

            public AnimatorParameterRandom(string paramName, float floatValue, List<float> floatRandomValueList)
            {
                this.paramName = paramName;
                this.floatRandomList = floatRandomValueList;
            }

            public AnimatorParameterRandom(string paramName, bool boolValue, float boolTrueChance)
            {
                this.paramName = paramName;
                this.boolTrueChance = boolTrueChance;
            }
        }
    }
}
