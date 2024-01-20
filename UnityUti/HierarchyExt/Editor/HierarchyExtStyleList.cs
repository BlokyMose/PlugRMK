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
            foreach (var item in styles)
                new GameObject(item.token);
        }
    }
}
