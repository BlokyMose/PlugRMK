using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.EditorUti
{
    public class BoxColToolboxWindow : EditorWindow
    {
        public enum PosPivot { Center, TopRight, BottomRight, BottomLeft, TopLeft, Custom }
    
        int tabIndex;
        bool isActive = true;
        GameObject boxPrefab;
        PosPivot handlePosPivot;
        Vector3 handlePosOffset;
        Vector3 startHandlePos, endHandlePos;
        Quaternion startHandleRot, endHandleRot;
        BoxCollider currentBox;
        Vector3 currentBoxOriginalSize;
        Vector3 currentBoxOriginalCenter;

        bool chain_drawGizmos = true;
        GameObject chain_parent;
        bool chain_closeLoop = true;
        bool chain_overridePivot = false;
        PosPivot chain_posPivot = PosPivot.BottomRight;
        bool chain_overrideSize = false;
        Vector2 chain_size = new(1, 1);
        bool chain_liveUpdate = false;

        static readonly Color COLOR_GREEN = new(0, 1, 0, .5f);
        static readonly Color COLOR_YELLOW = new(1, 1, 0, .5f);

        [MenuItem("Tools/Box Col Toolbox")]
        public static void OpenWindow()
        {
            var window = GetWindow<BoxColToolboxWindow>("Box Col");
            var title = EditorGUIUtility.IconContent("d_BoxCollider Icon");
            title.text = "Box Col";
            window.titleContent = title;
            window.Show();
        }

        #region [Methods: Setup Events]

        void OnEnable()
        {
            ResetCurrentBoxAndHandles();
            SceneView.RepaintAll();
            SceneView.duringSceneGui += OnSceneGUI;
        }

        void OnDisable()
        {
            ResetCurrentBoxAndHandles();
            SceneView.RepaintAll();
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        void OnSelectionChange()
        {
            if (isActive)
            {
                ResetCurrentBoxAndHandles();
                SceneView.RepaintAll();
            }
        }

        void OnSceneGUI(SceneView view)
        {
            if (isActive && currentBox != null)
            {
                CreatePosHandles();
                ModifyBoxByHandles();
                UpdateHandlesRot();
            }

            if (chain_parent != null)
            {
                if (chain_drawGizmos)
                    DrawGizmosBoxColliders(chain_parent.transform);
                if (chain_liveUpdate)
                    CreateBoxesChain();
            }
        }

        #endregion

        #region [Methods: UI]

        void OnGUI()
        {
            CreateTabs(ref tabIndex, new() { 
                ("Editor", CreateBoxColEditorUI), 
                ("Chain", CreateBoxesChainEditorUI)
            });

            if (tabIndex == 0)
                CreateBoxColEditorUI();
            else if (tabIndex == 1)
                CreateBoxesChainEditorUI();
        }

        void CreateBoxColEditorUI()
        {
            CreateToggle(ref isActive, "Active", OnEnable, OnDisable);
            CreateObjectField(ref boxPrefab, "Prefab");
            CreateButton(InstantiateBox, boxPrefab == null ? "New Default Box" : "New Prefab Box", 32);
            EditorGUILayout.Separator();
            CreateEnumField(ref handlePosPivot, "Handle Pos Pivot", OnHandlePivotChanged);
            CreateVector3FieldDisabled(ref handlePosOffset, "Handle Pos Offset", handlePosPivot != PosPivot.Custom);
            CreateCurrentBoxSizeField();

            void CreateCurrentBoxSizeField()
            {
                if (currentBox != null)
                {
                    EditorGUI.BeginDisabledGroup(currentBox == null || isActive);
                    var size = (Vector2)currentBoxOriginalSize;
                    CreateVector2Field(ref size, "Size");
                    currentBoxOriginalSize = new(size.x, size.y, currentBoxOriginalSize.z);
                    currentBox.size = new(size.x, size.y, currentBox.size.z);
                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        void CreateBoxesChainEditorUI()
        {
            CreateObjectField(ref chain_parent, "Chain Parent");
            CreateObjectField(ref boxPrefab, "Prefab");
            if (boxPrefab != null)
                CreateToggle(ref chain_overridePivot, "Override Pivot");
            if (boxPrefab == null || (boxPrefab != null && chain_overridePivot))
                CreateEnumField(ref chain_posPivot, "Pos Pivot", Chain_OnPivotChanged);
            if (boxPrefab != null)
                CreateToggle(ref chain_overrideSize, "Override Size");
            if (boxPrefab == null || (boxPrefab != null && chain_overrideSize))
                CreateVector2Field(ref chain_size, "Size");
            CreateToggle(ref chain_closeLoop, "Close Loop");
            CreateToggle(ref chain_drawGizmos, "Draw Yellow Gizmos");
            CreateToggle(ref chain_liveUpdate, "Live Update");
            if (!chain_liveUpdate)
                CreateButton(CreateBoxesChain, "Create Chain", 32);
        }

        static void CreateTabs(ref int tabIndex, List<(string, Action)> parameters)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < parameters.Count; i++)
            {
                var (label, action) = parameters[i];
                var guiColor = GUI.color;
                if (i == tabIndex)
                    GUI.color = Color.green;
                if (GUILayout.Button(label, GUILayout.Height(16)))
                {
                    tabIndex = i;
                    action?.Invoke();
                }
                GUI.color = guiColor;
            }
            GUILayout.EndHorizontal();
        }

        static void CreateToggle(ref bool targetBool, string label, Action onTrue = null, Action onFalse = null)
        {
            var GUIColor = GUI.color;
            GUI.color = targetBool ? Color.green : Color.gray;
            var newBool = EditorGUILayout.Toggle(label, targetBool);
            GUI.color = GUIColor;

            if (newBool != targetBool)
            {
                targetBool = newBool;
                if (targetBool)
                    onTrue?.Invoke();
                else
                    onFalse?.Invoke();
            }
        }

        static void CreateObjectField<T>(ref T targetObject, string label) where T : UnityEngine.Object
        {
            targetObject = (T)EditorGUILayout.ObjectField(label, targetObject, typeof(T), true);
        }

        static void CreateButton(Action action, string label, int height = 16)
        {
            if (GUILayout.Button(label, GUILayout.Height(height)))
                action?.Invoke();
        }

        static void CreateEnumField<T>(ref T targetEnum, string label, Action<T> onChanged) where T : Enum
        {
            var newEnum = (T)EditorGUILayout.EnumPopup(label, targetEnum);
            if (!Equals(newEnum, targetEnum))
            {
                targetEnum = newEnum;
                onChanged(targetEnum);
            }
        }

        static void CreateVector3Field(ref Vector3 targetVector, string label)
        {
            targetVector = EditorGUILayout.Vector3Field(label, targetVector);
        }

        static void CreateVector3FieldDisabled(ref Vector3 targetVector, string label, bool isDisabled)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            CreateVector3Field(ref targetVector, label);
            EditorGUI.EndDisabledGroup();
        }

        static void CreateVector2Field(ref Vector2 targetVector, string label)
        {
            targetVector = EditorGUILayout.Vector2Field(label, targetVector);
        }

        #endregion

        #region [Methods: Handles]

        void CreatePosHandles()
        {
            var offset = GetCalibratedHandleOffset(currentBox, handlePosOffset);
            startHandlePos = CreateHandle(startHandlePos, startHandleRot, offset, COLOR_GREEN);
            endHandlePos = CreateHandle(endHandlePos, endHandleRot, offset, COLOR_YELLOW);
        }

        static Vector3 GetCalibratedHandleOffset(BoxCollider box, Vector3 offset)
        {
            var sizeScaled = Multiply(box.size, box.transform.localScale);
            var handleOffsetScaled = GetOffsetByLocalRotation(box.transform, Multiply(sizeScaled, offset));
            var handleOffsetScaledHalf = handleOffsetScaled / 2;
            return handleOffsetScaledHalf;
        }

        static Vector3 CreateHandle(Vector3 pos, Quaternion rot, Vector3 offset, Color? color = null)
        {
            var currentPos = pos + offset;
            var newPos = currentPos;
            color ??= new(0,0,0,0);
            using (new Handles.DrawingScope((Color)color))
            {
                newPos = Handles.PositionHandle(currentPos, rot);
                Handles.SphereHandleCap(0, newPos, Quaternion.identity, .33f, EventType.Repaint);
            }
            if (currentPos != newPos)
                return newPos - offset;
            return pos;
        }

        void UpdateHandlesRot()
        {
            if (Tools.pivotRotation == PivotRotation.Local)
            {
                startHandleRot = currentBox.transform.rotation;
                endHandleRot = currentBox.transform.rotation;
            }
            else if (Tools.pivotRotation == PivotRotation.Global)
            {
                startHandleRot = Quaternion.identity;
                endHandleRot = Quaternion.identity;
            }
        }

        void OnHandlePivotChanged(PosPivot pivot)
        {
            switch (pivot)
            {
                case PosPivot.Center:
                    handlePosOffset = new(0, 0, 0);
                    break;
                case PosPivot.TopRight:
                    handlePosOffset = new(1, 1, 0);
                    break;
                case PosPivot.BottomRight:
                    handlePosOffset = new(1, -1, 0);
                    break;
                case PosPivot.BottomLeft:
                    handlePosOffset = new(-1, -1, 0);
                    break;
                case PosPivot.TopLeft:
                    handlePosOffset = new(-1, 1, 0);
                    break;
                case PosPivot.Custom:
                    break;
            }
        }

        #endregion

        #region [Methods: Modify Box]

        void ModifyBoxByHandles()
        {
            currentBox.gameObject.hideFlags = HideFlags.NotEditable;

            var distance = Vector3.Distance(startHandlePos, endHandlePos) / currentBox.transform.localScale.z;
            currentBox.transform.position = GetCurrentBoxPos(currentBox, startHandlePos);
            currentBox.transform.rotation = LookAt(startHandlePos, endHandlePos);
            currentBox.center = new(currentBoxOriginalCenter.x, currentBoxOriginalCenter.y, (distance - 1) / 2);
            currentBox.size = new(currentBoxOriginalSize.x, currentBoxOriginalSize.y, distance);

            static Vector3 GetCurrentBoxPos(BoxCollider box, Vector3 startHandlePos)
            {
                var centerScaled = Multiply(box.center, box.transform.localScale);
                return startHandlePos
                    - GetOffsetByLocalRotation(box.transform, centerScaled)
                    + GetForwardHalfSize(box);
            }
        }

        void ResetCurrentBoxAndHandles()
        {
            if (currentBox != null)
            {
                currentBox.gameObject.hideFlags = HideFlags.None;
                currentBox = null;
            }

            if (Selection.activeGameObject != null &&
                Selection.activeGameObject.TryGetComponent<BoxCollider>(out var col))
            {
                currentBox = col;
                currentBoxOriginalCenter = currentBox.center;
                currentBoxOriginalSize = currentBox.size;

                var center = Multiply(currentBox.center, currentBox.transform.localScale);
                var pos = currentBox.transform.position + GetOffsetByLocalRotation(currentBox.transform, center);
                var forwardRadius = currentBox.transform.forward * currentBoxOriginalSize.z / 2 * currentBox.transform.localScale.z;

                startHandlePos = pos - forwardRadius;
                startHandleRot = Quaternion.identity;
                endHandlePos = pos + forwardRadius;
                endHandleRot = Quaternion.identity;
            }
            else
            {
                currentBox = null;
                startHandlePos = Vector3.zero;
                startHandleRot = Quaternion.identity;
                endHandlePos = Vector3.zero;
                endHandleRot = Quaternion.identity;
            }
        }

        #endregion

        #region [Methods: Chain]

        void Chain_OnPivotChanged(PosPivot pivot)
        {
            chain_posPivot = pivot;
        }

        void CreateBoxesChain()
        {
            if (chain_parent == null || chain_parent.transform.childCount < 2)
                return;

            for (int i = 0; i < chain_parent.transform.childCount; i++)
            {
                if (chain_closeLoop && chain_parent.transform.childCount < 3)
                    break;
                else if (!chain_closeLoop && i == chain_parent.transform.childCount - 1)
                    break;

                var child = chain_parent.transform.GetChild(i);
                if (!child.TryGetComponent<BoxCollider>(out var box))
                    box = child.gameObject.AddComponent<BoxCollider>();
                var nextChild = chain_parent.transform.GetChild((i + 1) % chain_parent.transform.childCount);
                var distance = Vector3.Distance(child.position, nextChild.position);
                child.LookAt(nextChild);
                var sizeXY = GetSizeXY();
                box.size = new(sizeXY.x, sizeXY.y, distance);
                var centerXY = GetCenterXY(sizeXY);
                box.center = new(centerXY.x, centerXY.y, distance / 2);
            }

            Vector2 GetSizeXY()
            {
                if (boxPrefab != null && chain_overrideSize && boxPrefab.TryGetComponent<BoxCollider>(out var box))
                    return new(box.size.x, box.size.y);
                else
                    return chain_size;
            }

            Vector2 GetCenterXY(Vector2 sizeXY)
            {
                if (boxPrefab != null && chain_overrideSize && boxPrefab.TryGetComponent<BoxCollider>(out var box))
                {
                    return new(box.center.x, box.center.y);
                }
                else
                {
                    switch (chain_posPivot)
                    {
                        case PosPivot.Center:
                            return new(0, 0);
                        case PosPivot.TopRight:
                            return new(sizeXY.x / 2, -sizeXY.y / 2);
                        case PosPivot.BottomRight:
                            return new(sizeXY.x / 2, sizeXY.y / 2);
                        case PosPivot.BottomLeft:
                            return new(-sizeXY.x / 2, sizeXY.y / 2);
                        case PosPivot.TopLeft:
                            return new(-sizeXY.x / 2, -sizeXY.y / 2);
                        case PosPivot.Custom:
                            return new(0, 0);
                        default:
                            return new(0, 0);
                    }
                }
            }
        }

        static void DrawGizmosBoxColliders(Transform parent)
        {
            var currentSelectedBox = 
                Selection.activeGameObject != null &&
                Selection.activeGameObject.TryGetComponent<BoxCollider>(out var selectedBox) ? selectedBox : null;
            var originalMatrix = Handles.matrix;
            var originalColor = Handles.color;
            foreach (Transform child in parent.transform)
            {
                if (child.TryGetComponent<BoxCollider>(out var box) && box != currentSelectedBox)
                {
                    Handles.matrix = child.localToWorldMatrix;
                    Handles.color = Color.yellow;
                    Handles.DrawWireCube(box.center, box.size);
                    Handles.matrix = originalMatrix;
                    Handles.color = originalColor;
                }
            }
        }

        #endregion

        #region [Methods: Utility]

        void InstantiateBox()
        {
            if (!TryGetCamera(out var camera))
                return;

            var box = boxPrefab != null
                ? PrefabUtility.InstantiatePrefab(boxPrefab) as GameObject
                : GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.position = camera.transform.position + camera.transform.forward * 10;
            box.transform.localScale = Vector3.one * 2;
            Selection.activeGameObject = box;
        }

        static bool TryGetCamera(out Camera camera)
        {
            var lastSceneView = SceneView.lastActiveSceneView;
            if (lastSceneView != null)
            {
                camera = lastSceneView.camera;
                return true;
            }
            camera = null;
            return false;
        }

        static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        static Vector3 GetOffsetByLocalRotation(Transform transform, Vector3 offset)
        {
            return 
                (transform.forward * offset.z) +
                (transform.right * offset.x) + 
                (transform.up * offset.y);
        }

        static Vector3 GetForwardHalfSize(BoxCollider box)
        {
            return box.transform.forward * box.size.z / 2 * box.transform.localScale.z;
        }

        static Quaternion LookAt(Vector3 from, Vector3 to)
        {
            Vector3 forwardDirection = (to - from).normalized;
            if (forwardDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(forwardDirection, Vector3.up);
                return targetRotation;
            }
            return Quaternion.identity;
        }

        #endregion
    }
}
