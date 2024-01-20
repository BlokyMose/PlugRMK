using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti.Hext
{
    [System.Serializable]
    public class HierarchyExtStyle
    {
        public string token;
        public string labelName;

        [FoldoutGroup("Text Style")]
        public Color textColor = new(0.9f, 0.9f, 0.9f);
        [FoldoutGroup("Text Style")]
        public TextAnchor textAlignment = TextAnchor.UpperLeft;
        [FoldoutGroup("Text Style")]
        public FontStyle fontStyle = FontStyle.Normal;
        [FoldoutGroup("Text Style")]
        public Vector2Int textOffset = new (18, 0);

        [FoldoutGroup("BG Style")]
        public bool isCustomBG = false;
        [FoldoutGroup("BG Style"), ShowIf(nameof(isCustomBG))]
        public Color bgColor = UnityEditorColorUtility.k_defaultProColor;
        [FoldoutGroup("BG Style"), ShowIf(nameof(isCustomBG))]
        public Color bgColorHover = UnityEditorColorUtility.k_hoveredProColor;
        [FoldoutGroup("BG Style"), ShowIf(nameof(isCustomBG))]
        public Color bgColorSelect = UnityEditorColorUtility.k_selectedProColor;

        [FoldoutGroup("BG Style")]
        public bool isCustomBGOffset = false;
        [FoldoutGroup("BG Style"), ShowIf(nameof(isCustomBGOffset))]
        public Vector2 bgOffset = new (0, 0);
        public static readonly Vector2 bgOffsetDefault = new(16, 0);

        [FoldoutGroup("Icon")]
        public Sprite icon;

        public Color GetBGColor(bool isSelected, bool isHovered, bool isWindowFocused)
        {
            if (isSelected)
            {
                if (isWindowFocused)
                {
                    return bgColorSelect;
                }
                else
                {
                    return bgColorHover;
                }
            }
            else if (isHovered)
            {
                return bgColorHover;
            }
            else
            {
                return bgColor;
            }
        }
    }

}
