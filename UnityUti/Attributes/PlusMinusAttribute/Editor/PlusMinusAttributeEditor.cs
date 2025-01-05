using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.EditorUti
{
    [CustomPropertyDrawer(typeof(PlusMinusAttribute))]
    public class PlusMinusAttributeEditor : PropertyDrawer
    {
        const float FIELD_WIDTH_RATIO = .8f;
        const float BUTTON_WIDTH_RATIO = .1f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUILayout.BeginHorizontal();
            var fieldRect = CreateField(position, property, label);
            var plusButtonRect = CreatePlusButton(position, fieldRect, property, fieldInfo);
            var minusButtonRect = CreateMinusButton(position, plusButtonRect, property, fieldInfo);
            GUILayout.EndHorizontal();
        }

        static Rect CreateField(Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldRect = GetFieldRect(position);
            EditorGUI.PropertyField(fieldRect, property, label);
            return fieldRect;
        }

        static Rect CreatePlusButton(Rect position, Rect previousGUI, SerializedProperty property, FieldInfo fieldInfo)
        {
            var plusButtonRect = GetButtonRect(position, previousGUI);
            if (GUI.Button(plusButtonRect, "+"))
            {
                if (TryGetPlusMinusIncrement(fieldInfo, out var increment))
                {
                    if (fieldInfo.FieldType == typeof(int))
                        property.intValue += (int)increment;
                    else if (fieldInfo.FieldType == typeof(float))
                        property.floatValue += increment;
                }
            }

            return plusButtonRect;
        }

        static Rect CreateMinusButton(Rect position, Rect previousGUI, SerializedProperty property, FieldInfo fieldInfo)
        {
            var minusButtonRect = GetButtonRect(position, previousGUI);
            if (GUI.Button(minusButtonRect, "-"))
            {
                if (TryGetPlusMinusIncrement(fieldInfo, out var increment))
                {
                    if (fieldInfo.FieldType == typeof(int))
                        property.intValue -= (int)increment;
                    else if (fieldInfo.FieldType == typeof(float))
                        property.floatValue -= increment;
                }
            }

            return minusButtonRect;
        }

        static bool TryGetPlusMinusIncrement(FieldInfo field, out float increment)
        {
            var plusMinus = field.GetCustomAttribute<PlusMinusAttribute>();
            if (plusMinus != null)
            {
                increment =  plusMinus.Increment;
                return true;
            }
            else
            {
                increment = default;
                return false;
            }
        }

        static Rect GetFieldRect(Rect position)
        {
            return new Rect(
                position: position.position,
                size: new(position.width * FIELD_WIDTH_RATIO, position.height)
            );
        }

        static Rect GetButtonRect(Rect position, Rect previousGUI)
        {
            return new Rect(
                position: new(previousGUI.position.x + previousGUI.size.x, position.y),
                size: new(position.width * BUTTON_WIDTH_RATIO, position.height)
            );
        }
    }
}
