using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.EditorUti
{
    [CustomEditor(typeof(BoxCollider))]
    public class BoxColliderExtendedEditor : Editor
    {
        Editor _internalEditor;

        void OnEnable()
        {
            var editorType = Type.GetType("UnityEditor.BoxColliderEditor, UnityEditor");
            if (editorType != null)
                _internalEditor = CreateEditor(targets, editorType);
        }

        void OnDisable()
        {
            if (_internalEditor != null)
                DestroyImmediate(_internalEditor);
        }

        public override void OnInspectorGUI()
        {
            if (_internalEditor != null)
                _internalEditor.OnInspectorGUI();

            GUILayout.Space(25f);
            GUILayout.Label("Tools", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Recenter", GUILayout.Width(100f)))
            {
                var collider = (BoxCollider)target;

                var undoGroupIndex = Undo.GetCurrentGroup();
                Undo.RecordObject(collider.transform, "Recentering collider offset");
                Undo.RecordObject(collider, "Zeroed collider offset");

                collider.transform.position = collider.bounds.center;
                collider.center = Vector3.zero;
                
                Undo.CollapseUndoOperations(undoGroupIndex);
            }
        }
    }
}
