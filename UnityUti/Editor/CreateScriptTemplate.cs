using PlasticGui.WorkspaceWindow;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.EditorUti
{
    public static class CreateScriptTemplate
    {
        const int PRIORITY_INDEX = -100;
        const string TEMPLATES = "ScriptTemplates";

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
        [MenuItem("Assets/Create/New Code/UI Toolkit Window", priority = PRIORITY_INDEX)]
        public static void CreateUIToolkitWindow() => CreateScriptFromTemplateName("UIToolkit_Window");

        public static void CreateScriptFromTemplateName(string templateName)
        {
            var parentPath = GetParentPath(nameof(CreateScriptTemplate), templateName);
            var templatePath = parentPath + "/" + TEMPLATES + "/" + templateName + ".cs.txt";

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "New"+templateName+".cs");
        }

        public static string GetParentPath(string assetName, string childFileName)
        {
            var guids = AssetDatabase.FindAssets(assetName);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var folderPath = path[..(path.Length - "/".Length - assetName.Length - ".cs".Length )];
                var templatePath = folderPath + "/" + TEMPLATES + "/" + childFileName + ".cs.txt";
                if (File.Exists(templatePath))
                    return folderPath;
            }

            Debug.LogError($"Cannot find parent path of {childFileName}\nGUID count: {guids.Length}");
            return "";
        }
    }
}
