using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SMonkey
{
    public static class SaveAsDirtySO
    {
        const string MENU_PATH = "CONTEXT/ScriptableObject/Save as Dirty";

        [MenuItem(MENU_PATH)]
        public static void SaveAsDirty(MenuCommand command)
        {
            var so = command.context as ScriptableObject;
            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
