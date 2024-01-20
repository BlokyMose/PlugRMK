using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlugRMK.UnityUti
{
    public static class UnityEditorColorUtility
    {
        public static readonly Color k_defaultcolor = new Color(0.7843f, 0.7843f, 0.7843f);
        public static readonly Color k_defaultProColor = new Color(0.2196f, 0.2196f, 0.2196f);

        public static readonly Color k_selectedColor = new Color(0.22745f, 0.447f, 0.6902f);
        public static readonly Color k_selectedProColor = new Color(0.1725f, 0.3647f, 0.5294f);

        public static readonly Color k_selectedUnFocusedColor = new Color(0.68f, 0.68f, 0.68f);
        public static readonly Color k_selectedUnFocusedProColor = new Color(0.3f, 0.3f, 0.3f);

        public static readonly Color k_hoveredColor = new Color(0.698f, 0.698f, 0.698f);
        public static readonly Color k_hoveredProColor = new Color(0.2706f, 0.2706f, 0.2706f);
    }

}
