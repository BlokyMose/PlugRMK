using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.Editor
{
    public static class EditorUtility
    {
        public static T LoadAsset<T>(string assetName) where T:Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name} {assetName}");
            if (guids.Length == 0)
                return null;
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public static List<T> LoadAssets<T>(string assetName) where T : Object
        {
            var list = new List<T>();
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name} {assetName}");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(LoadAsset<T>(path));
            }
            return list;
        }
    }
}
