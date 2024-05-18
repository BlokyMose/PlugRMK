using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.Hext
{
    [InitializeOnLoad]
    public class HierarchyExt
    {
        static bool _hierarchyHasFocus = false;
        static EditorWindow _hierarchyEditorWindow;

        static string[] dataArray;
        static string path;
        static HierarchyExtStyleList extStyleList;

        static HierarchyExt()
        {
            dataArray = AssetDatabase.FindAssets("t:"+nameof(HierarchyExtStyleList));

            if (dataArray != null && dataArray.Length > 0)
            {
                path = AssetDatabase.GUIDToAssetPath(dataArray[0]);
                extStyleList = AssetDatabase.LoadAssetAtPath<HierarchyExtStyleList>(path);
                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindow;
                EditorApplication.update += OnEditorUpdate;
            }
        }

        static void OnEditorUpdate()
        {
            if (_hierarchyEditorWindow == null)
                _hierarchyEditorWindow = EditorWindow.GetWindow(Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor"));
            _hierarchyHasFocus = EditorWindow.focusedWindow != null &&
                EditorWindow.focusedWindow == _hierarchyEditorWindow;
        }

        static void OnHierarchyWindow(int instanceID, Rect selectionRect)
        {
            var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (go == null)
                return;

            if (go.TryGetComponent<HierarchyExtGO>(out var hextGO))
                Stylize(hextGO.style);
            else
                StylizeByExtensionStyle();

            void Stylize(HierarchyExtStyle style)
            {
                // Draw a rectangle as a background, and set the color
                GetSelectionState(out var isSelected, out var isHovered, out var isWindowFocused);
                var bgColor = style.isCustomBG
                    ? style.GetBGColor(isSelected, isHovered, isWindowFocused)
                    : GetEditorBGColor(isSelected, isHovered, isWindowFocused);
                var bgOffsetX = style.isCustomBGOffset
                    ? style.bgOffset.x
                    : style.icon != null ? 0 : HierarchyExtStyle.bgOffsetDefault.x;
                var bgOffsetY = style.isCustomBGOffset
                    ? style.bgOffset.y
                    : HierarchyExtStyle.bgOffsetDefault.y;
                var bgRect = new Rect(
                    selectionRect.x + bgOffsetX,
                    selectionRect.y + bgOffsetY,
                    selectionRect.width + 60,
                    selectionRect.height);
                EditorGUI.DrawRect(bgRect, bgColor);

                // Create a new GUIStyle based on the colorDesign
                var newStyle = new GUIStyle
                {
                    alignment = style.textAlignment,
                    fontStyle = style.fontStyle,
                    normal = new GUIStyleState()
                    {
                        textColor = style.textColor,
                    },
                    padding = new RectOffset(style.textOffset.x, style.textOffset.y, 0, 0)
                };

                // Draw a label to show the name in newStyle
                var newName = string.IsNullOrWhiteSpace(style.labelName) ? go.name : style.labelName;
                EditorGUI.LabelField(selectionRect, newName, newStyle);

                // Draw icon
                if (style.icon != null)
                    EditorGUI.LabelField(selectionRect, new GUIContent(style.icon.texture));
            }

            void StylizeByExtensionStyle()
            {
                for (int i = 0; i < extStyleList.styles.Count; i++)
                {
                    var style = extStyleList.styles[i];
                    if (go.name.StartsWith(style.token))
                    {
                        Stylize(style);
                        break;
                    }
                }
            }

            void GetSelectionState(out bool isSelected, out bool isHovered, out bool isWindowFocused)
            {
                isSelected = Selection.instanceIDs.Contains(instanceID);
                isHovered = selectionRect.Contains(Event.current.mousePosition);
                isWindowFocused = _hierarchyHasFocus;
            }

            Color GetEditorBGColor(bool isSelected, bool isHovered, bool isWindowFocused)
            {
                if (isSelected)
                {
                    if (isWindowFocused)
                    {
                        return EditorGUIUtility.isProSkin ? UnityEditorColorUtility.k_selectedProColor : UnityEditorColorUtility.k_selectedColor;
                    }
                    else
                    {
                        return EditorGUIUtility.isProSkin ? UnityEditorColorUtility.k_selectedUnFocusedProColor : UnityEditorColorUtility.k_selectedUnFocusedColor;
                    }
                }
                else if (isHovered)
                {
                    return EditorGUIUtility.isProSkin ? UnityEditorColorUtility.k_hoveredProColor : UnityEditorColorUtility.k_hoveredColor;
                }
                else
                {
                    return EditorGUIUtility.isProSkin ? UnityEditorColorUtility.k_defaultProColor : UnityEditorColorUtility.k_defaultcolor;
                }
            }
        }

        [MenuItem("GameObject/PlugRMK/Instantiate All Hierarchy Ext List")]
        public static void CreateAllHierarchyExtList()
        {
            if (extStyleList == null)
            {
                Debug.LogWarning("Hierarchy Extension List is not fonud or loaded yet");
                return;
            }

            extStyleList.InstantiateToScene();
        }
    }
}
