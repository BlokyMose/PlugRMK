using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.SceneManagement;

[Overlay(typeof(SceneView), "Scene Selection")]
[Icon(k_icon) ]
public class SceneSelectionOverlay : ToolbarOverlay
{
    public const string k_icon = "Assets/PlugRMK/UnityUti/Editor/Editor Default Resources/Icons/edicon_scene.png";
    SceneSelectionOverlay() : base(SceneDropdown.k_id)
    {

    }

    [EditorToolbarElement(k_id, typeof(SceneView))]
    class SceneDropdown:EditorToolbarDropdown, IAccessContainerWindow
    {
        public const string k_id = "SceneSelectionOverlay/SceneDropdownToggle";

        public EditorWindow containerWindow { get; set; }
        
        SceneDropdown()
        {
            tooltip = "Select a scene to load";
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(SceneSelectionOverlay.k_icon);
            clicked += ShowSceneMenu;
        }

        private void ShowSceneMenu()
        {
            var menu = new GenericMenu();
            var currentScene = EditorSceneManager.GetActiveScene();
            var sceneGuids = AssetDatabase.FindAssets("t:scene", null);
            for (int i = 0; i < sceneGuids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
                var name = Path.GetFileNameWithoutExtension(path);
                menu.AddItem(new GUIContent(name), string.Compare(currentScene.name,name)==0, () => OpenScene(currentScene, path));
            }
            menu.ShowAsContext();
        }

        void OpenScene(Scene currentScene, string path)
        {
            if (currentScene.isDirty)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(path);
            }
            else
            {
                    EditorSceneManager.OpenScene(path);
            }
        }
    }
}
