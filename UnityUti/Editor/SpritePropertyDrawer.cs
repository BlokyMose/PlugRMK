using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.EditorUti
{ 
    [CustomPropertyDrawer(typeof(Sprite))]
    public class SpritePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            property.objectReferenceValue = EditorGUI.ObjectField(
                new(position.x, position.y + EditorGUIUtility.singleLineHeight * .5f, position.width - EditorGUIUtility.singleLineHeight * 2, EditorGUIUtility.singleLineHeight),
                label,
                property.objectReferenceValue,
                typeof(Sprite),
                false);

            property.objectReferenceValue = EditorGUI.ObjectField(
                new(position.x, position.y, position.width, position.height),
                " ",
                property.objectReferenceValue,
                typeof(Sprite),
                false);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }
    }
}
