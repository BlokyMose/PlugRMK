using PlasticGui.WorkspaceWindow;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.Editor
{
    public static class CreateScriptTemplate
    {
        const int PRIORITY_INDEX = -100;

        [MenuItem("Assets/Create/New Code/MonoBehaviour", priority = PRIORITY_INDEX)]
        public static void CreateMonoBehaviour() => CreateScriptFromTemplateName("MonoBehaviour");

        [MenuItem("Assets/Create/New Code/ScriptableObject", priority = PRIORITY_INDEX)]
        public static void CreateScriptableObject() => CreateScriptFromTemplateName("ScriptableObject");

        [MenuItem("Assets/Create/New Code/Static Class", priority = PRIORITY_INDEX)]
        public static void CreateStaticClass() => CreateScriptFromTemplateName("StaticClass");

        [MenuItem("Assets/Create/New Code/Interface", priority = PRIORITY_INDEX)]
        public static void CreateInterface() => CreateScriptFromTemplateName("Interface");

        [MenuItem("Assets/Create/New Code/Editor Window", priority = PRIORITY_INDEX)]
        public static void CreateEditorWindow() => CreateScriptFromTemplateName("EditorWindow");
        
        [MenuItem("Assets/Create/New Code/UI Toolkit Inspector", priority = PRIORITY_INDEX)]
        public static void CreateUIToolkitInspector() => CreateScriptFromTemplateName("UIToolkit_Inspector");
        
        public static void CreateScriptFromTemplateName(string templateName)
        {
            var TEMPLATES = "ScriptTemplates";
            var parentPath = GetParentPath(nameof(CreateScriptTemplate));
            var templatePath = parentPath + "/" + TEMPLATES + "/" + templateName + ".cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "New"+templateName+".cs");
        }

        public static string GetParentPath(string assetName)
        {
            var guids = AssetDatabase.FindAssets(assetName);
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var folderPath = path[..(path.Length - "/".Length - assetName.Length - ".cs".Length )];
            return folderPath;
        }
    }
}
