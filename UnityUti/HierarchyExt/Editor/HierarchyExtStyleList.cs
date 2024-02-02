using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti.Hext
{
    [CreateAssetMenu(fileName ="HierarchyExtStyleList", menuName ="SO/Editor/Hierarchy Ext Style List")]
    public class HierarchyExtStyleList : ScriptableObject
    {
        public List<HierarchyExtStyle> styles = new();

        [Button, PropertyOrder(-1)]
        public void InstantiateToScene()
        {
#if UNITY_EDITOR
            int undoGroupID = UnityEditor.Undo.GetCurrentGroup();
            foreach (var item in styles)
            {
                var go = new GameObject(item.token);
                UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Hierarchy Ext: Instantiated GO");
            }
            UnityEditor.Undo.CollapseUndoOperations(undoGroupID);
#endif
        }
    }
}
