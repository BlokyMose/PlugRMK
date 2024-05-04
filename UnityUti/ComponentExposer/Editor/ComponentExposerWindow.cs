using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static PlugRMK.UnityUti.UIToolkitUtility;
using static PlugRMK.UnityUti.Editor.UnityEditorUtility;
using static PlugRMK.GenericUti.StringUtility;

namespace PlugRMK.UnityUti.Editor
{
    public class ComponentExposerWindow : EditorWindow
    {
        public VisualTreeAsset mainUXML;
        VisualElement mainVE;
        const string SCRIPT_TARGET_FIELD = "scriptTarget_field";
        const string GENERATE_EXPOSER_FILE_BUTTON = "generateExposerFile_button";
        const string EXPOSER_FOLDER_NAME = "ExposerFiles";
        const string SO_FOLDER_NAME = "SO";

        string exposerFolderPath;
        string soFolderPath;

        [MenuItem("Tools/" + nameof(ComponentExposerWindow))]
        public static void OpenWindow()
        {
            GetWindow <ComponentExposerWindow>(nameof(ComponentExposerWindow)).Show();
        }

        void OnEnable()
        {
            var thisAsset = AssetDatabase.FindAssets(nameof(ComponentExposerWindow))[0];
            var thisFilePath = AssetDatabase.GUIDToAssetPath(thisAsset);
            var thisFolderPath = Directory.GetParent(thisFilePath).Parent.FullName;
            exposerFolderPath = $"{thisFolderPath}/{EXPOSER_FOLDER_NAME}";
            soFolderPath = $"{thisFolderPath}/{SO_FOLDER_NAME}";
            Directory.CreateDirectory(exposerFolderPath);
            Directory.CreateDirectory(soFolderPath);
        }

        void CreateGUI()
        {
            mainVE = mainUXML.Instantiate();
            rootVisualElement.Add(mainVE);

            SetButton(mainVE, GENERATE_EXPOSER_FILE_BUTTON, GenerateExposerFileButton_OnClick);
            void GenerateExposerFileButton_OnClick()
            {
                var field = mainVE.Q<ObjectField>(SCRIPT_TARGET_FIELD);
                CreateExposerFile(field.value as TextAsset, exposerFolderPath);
            }
        }

        void OnFocus()
        {
            ValidateScriptTargetField();
        }

        void ValidateScriptTargetField()
        {
            var field = mainVE.Q<ObjectField>(SCRIPT_TARGET_FIELD);
            var button = mainVE.Q<Button>(GENERATE_EXPOSER_FILE_BUTTON);

            if (field.value == null || field.value is not TextAsset)
                button.SetEnabled(false);
            else
                button.SetEnabled(true);
        }

        public static void CreateExposerFile(TextAsset targetComponent, string folderPath)
        {
            const string FILE_NAME = "[FileName]";
            const string USINGS = "[Usings]";
            const string USING_NAMESPACE = "[UsingNamespace]";
            const string EXPOSED_VARIABLES = "[Exposed Variables]";
            const string TARGET_COMPONENT = "[TargetComponent]";

            var template = LoadAsset<TextAsset>("ExposerFileTemplate");
            var fileName = targetComponent.name + "_Exposer";
            var fileText = template.text;

            fileText = WriteUsings(targetComponent.text, fileText);
            fileText = WriteNamespace(targetComponent.text, fileText);
            fileText = fileText.Replace(FILE_NAME, fileName);
            fileText = WriteExposedVariables(targetComponent.text, fileText);
            fileText = fileText.Replace(TARGET_COMPONENT, targetComponent.name);
            
            Debug.Log(fileText);

            var filePath = $"{folderPath}/{fileName}.cs";
            File.WriteAllText(filePath, fileText);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            static string WriteUsings(string source, string currentText)
            {
                var usingsText = "";
                var usingLibraryNames = source.ExtractAll("using", ";");
                for (int i = 0; i < usingLibraryNames.Count; i++)
                    usingsText += "using " + usingLibraryNames[i] + ";\n";
                
                currentText = currentText.Replace(USINGS, usingsText);
                return currentText;
            }

            static string WriteNamespace(string source, string currentText)
            {
                var namespaceName = source.Extract("namespace", "{");
                if (!string.IsNullOrEmpty(namespaceName))
                    currentText = currentText.Replace(USING_NAMESPACE, "using " + namespaceName + ";");
                else
                    currentText = currentText.Replace(USING_NAMESPACE, "");
                return currentText;
            }

            static string WriteExposedVariables(string source, string currentText)
            {
                var exposedVariablesTextRaw = source.Extract("#region " + EXPOSED_VARIABLES, "#endregion " + EXPOSED_VARIABLES, suppressWarning: true);
                var exposedVariablesTexts = exposedVariablesTextRaw.SplitBeforeToken(";");
                var exposedVariablesText = "";
                foreach (var variable in exposedVariablesTexts)
                    exposedVariablesText += "\n\t\t" + variable;
                currentText = currentText.Replace(EXPOSED_VARIABLES, exposedVariablesText);
                
                return currentText;
            }
        }
    }
}
