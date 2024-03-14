using PlugRMK.GenericUti;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlugRMK.UnityUti
{
    public static class UIToolkitUtility
    {
        public enum AnchorVertical { Top, Bottom, Center }
        public enum AnchorHorizontal { Left, Right, Center }

        #region [Size and Position]

        public static StyleLength GetPixelLength(float value)
        {
            return new StyleLength(new Length(value, LengthUnit.Pixel));
        }

        public static void ConstraintPositionInsideScreen(VisualElement ve)
        {
            if (ve.layout.x > Screen.width - ve.layout.width)
                ve.style.left = GetPixelLength(Screen.width - ve.layout.width);
            else if (ve.layout.x < 0)
                ve.style.left = GetPixelLength(0);

            if (ve.layout.y > Screen.height - ve.layout.height)
                ve.style.top = GetPixelLength(Screen.height - ve.layout.height);
            else if (ve.layout.y < 0)
                ve.style.top = GetPixelLength(0);
        }

        public static bool IsOnLeftScreenSide(this VisualElement ve)
        {
            return GetX(ve) + ve.layout.width / 2 < Screen.width / 2;
        }

        public static bool IsOnTopScreenSide(this VisualElement ve)
        {
            return GetY(ve) + ve.layout.height / 2 < Screen.height / 2;
        }

        /// <summary>
        /// Returns horizontal positon by anchor fo the width<br></br>
        /// Only works if either the left or right has StyleKeyword.Auto
        /// </summary>
        /// <param name="anchor">Left anchor: 0;  Right anchor: 1</param>
        public static float GetX(this VisualElement ve, float anchor = 0)
        {
            if (ve.style.left.keyword == StyleKeyword.Auto)
                return Screen.width - ve.style.right.value.value - ve.layout.width * (1-anchor);

            return ve.style.left.value.value + ve.layout.width * anchor;
        }

        /// <summary>
        /// Returns vertical position by anchor of the height<br></br>
        /// Only works if either the top or bottom has StyleKeyword.Auto
        /// </summary>
        /// <param name="anchor">Top anchor: 0;  Bottom anchor: 1</param>
        public static float GetY(this VisualElement ve, float anchor = 0)
        {
            if (ve.style.top.keyword == StyleKeyword.Auto)
                return Screen.height - ve.style.bottom.value.value - ve.layout.height * (1 - anchor);

            return ve.style.top.value.value + ve.layout.height * anchor;
        }

        public static Vector2 GetPos(this VisualElement ve, float xAnchor = 0, float yAnchor = 0)
        {
            return new(ve.GetX(xAnchor), ve.GetY(yAnchor));
        }

        public static Vector2 GetPos(this VisualElement ve, float anchor = 0)
        {
            return new(ve.GetX(anchor), ve.GetY(anchor));
        }

        #endregion

        public static void SwitchClass(this VisualElement ve, string classNameToRemove, string classNameToAdd)
        {
            ve.RemoveFromClassList(classNameToRemove);
            ve.AddToClassList(classNameToAdd);
        }
    
        public static string GetIndexChoice(this DropdownField dropdown)
        {
            return dropdown.choices.GetAt(dropdown.index, "");
        }

        public static Button SetButton(VisualElement root, string buttonName, Action onClick)
        {
            var button = root.Q<Button>(buttonName);
            button.clicked += onClick;
            return button;
        }

        public static VisualElement MakeSpace(float space)
        {
            var ve = new VisualElement();
            ve.style.height = new StyleLength(space);
            ve.style.width = new StyleLength(space);
            return ve;
        }
    }
}
