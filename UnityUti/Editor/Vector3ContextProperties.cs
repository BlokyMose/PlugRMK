using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.EditorUti
{
    public static class Vector3ContextProperties
    {
        [InitializeOnLoadMethod]
        public static void Init()
        {
            EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
        }

        static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.Vector3)
                return;

            menu.AddItem(new("Zero"), false, () =>
            {
                property.vector3Value = Vector3.zero;
                property.serializedObject.ApplyModifiedProperties();
            });


            menu.AddItem(new("One"), false, () =>
            {
                property.vector3Value = Vector3.one;
                property.serializedObject.ApplyModifiedProperties();
            });
        }
    }
}
