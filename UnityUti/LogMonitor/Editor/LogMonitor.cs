using PlugRMK.GenericUti;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlugRMK.UnityUti.LogMonitorRelay;

namespace PlugRMK.UnityUti.Editor
{
    public class LogMonitor : EditorWindow
    {
        #region [Vars: Appearance Properties]

        Vector2 scrollViewPos;
        EditorColors guiColors = new();
        readonly Padding padding = new(5, 5, 10, 10);
        readonly EditorPos defaultFieldSize = new EditorPos(100, 25);
        readonly EditorPos iconSize = new EditorPos(25, 25);

        #endregion

        #region [Methods: Editor]

        [MenuItem("Tools/"+nameof(LogMonitor))]
        public static void OpenWindow()
        {
            GetWindow<LogMonitor>(nameof(LogMonitor)).Show();
        }

        #endregion

        #region [Methods: Utilty]

        void MakeScrollView(Rect rect, Rect viewRect, Action onMakeContent)
        {
            scrollViewPos = GUI.BeginScrollView(rect, scrollViewPos, viewRect);
            onMakeContent();
            GUI.EndScrollView();
        }

        Vector2 GetPaddedWindowSize()
        {
            return new Vector2(position.width - padding.Horizontal, position.height - padding.Vertical);
        }

        float MakeLabel(Vector2 pos, string labelName, string separator = ":", string tooltip = "")
        {
            var rect = new Rect(pos, new Vector2(defaultFieldSize.Width / 2, defaultFieldSize.Height));
            EditorGUI.LabelField(rect, new GUIContent(labelName + separator, tooltip));
            return rect.width;
        }

        float MakeToggleButton(Vector2 pos, Texture2D icon, ref bool boolValue, string boolName)
        {
            var rect = new Rect(pos, iconSize.Vector2);
            if (boolValue)
            {
                if (GUI.Button(rect, new GUIContent(icon, boolName + ": true")))
                    boolValue = false;
            }
            else
            {
                GUI.color = Color.white.ChangeAlpha(0.5f);
                if (GUI.Button(rect, new GUIContent(icon, boolName + ": false")))
                    boolValue = true;
            }

            GUI.color = guiColors.Color;

            return rect.width;
        }

        float MakeButton(Vector2 pos, string text, Action onClicked, Color? color = null)
        {
            var rect = new Rect(pos, iconSize.Vector2);

            if (color != null)
                GUI.color = (Color)color;

            if (GUI.Button(rect, text))
                onClicked();

            GUI.color = guiColors.Color;

            return rect.width;
        }

        float MakeFloatInput(Vector2 pos, ref float floatVar)
        {
            var rect = new Rect(pos, new Vector2(defaultFieldSize.Width / 2, defaultFieldSize.Height));
            floatVar = EditorGUI.FloatField(rect, floatVar);
            return rect.width;
        }


        #endregion

        void OnEnable()
        {
            guiColors = new EditorColors(GUI.color, GUI.contentColor, GUI.backgroundColor);
        }

        void OnGUI()
        {
           MakeDataList();
        }

        private void Update()
        {
            Repaint();
        }

        void MakeDataList()
        {
            var pos = new EditorPos(padding.left, padding.top);

            MakeScrollView(
                new(pos.Vector2, GetPaddedWindowSize()),
                new(Vector2.zero, GetContentSize()),
                OnMakeContent);

            void OnMakeContent()
            {
                var contentPos = new EditorPos(pos.Vector2);
                var dataList = LogMonitorRelay.Instance.dataList;
                foreach (var data in dataList)
                {
                    contentPos.AddRow(MakeDataField(data, contentPos.Vector2));
                }
            }

            float MakeDataField(LogData data, Vector2 pos)
            {
                var fieldPos = new EditorPos(pos);
                fieldPos.AddColumn(MakeLabel(fieldPos.Vector2, data.name));
                fieldPos.AddColumn(MakeLabel(fieldPos.Vector2, data.data.ToString(), ""));
                return defaultFieldSize.Height;
            }
        }

        Vector2 GetContentSize()
        {
            Vector2 size = default;
            var dataList = LogMonitorRelay.Instance.dataList;
            foreach (var data in dataList)
            {
                size = new Vector2(size.x, size.y + defaultFieldSize.Height);
            }
            return size;
        }

    }

}

