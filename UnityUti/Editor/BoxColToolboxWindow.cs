using System;
using UnityEditor;
using UnityEngine;

public class BoxColToolboxWindow : EditorWindow
{
    public enum PosPivot { Center, TopRight, BottomRight, BottomLeft, TopLeft, Custom }
    public enum ScaleDirection { All, Corner }

    bool isActive = true;
    GameObject boxPrefab;
    PosPivot handlePosPivot;
    bool isUsingScale;
    ScaleDirection scaleDirection;
    Vector3 handlePosOffset, handleScaleOffset;
    Vector3 startHandlePos, endHandlePos, scaleHandlePos;
    Quaternion startHandleRot, endHandleRot, scaleHandleRot;
    BoxCollider currentBox;
    Vector3 currentBoxOriginalSize;
    Vector3 currentBoxOriginalCenter;
    static readonly Color COLOR_GREEN = new(0, 1, 0, .5f);
    static readonly Color COLOR_BLUE = new(0, 0, 1, .5f);

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
    
    #endregion

    #region [Methods: UI]

    void OnGUI()
    {
        CreateToggle(ref isActive, "Active", OnEnable, OnDisable);
        CreateObjectField(ref boxPrefab, "Prefab"); ;
        CreateButton(InstantiateBox, boxPrefab == null ? "New Default Box" : "New Prefab Box", 32);
        EditorGUILayout.Separator();
        CreateEnumField(ref handlePosPivot, "Handle Pos Pivot", OnHandlePivotChanged);
        CreateVector3FieldDisabled(ref handlePosOffset, "Handle Pos Offset", handlePosPivot != PosPivot.Custom);
        CreateToggle(ref isUsingScale, "Use Scale Handle");
        CreateEnumField(ref scaleDirection, "Scale Direction", OnScaleDirectionChanged);
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

    #endregion

    #region [Methods: Handles]

    void OnSceneGUI(SceneView view)
    {
        if (isActive && currentBox != null)
        {
            CreatePosHandles();
            CreateScaleHandle();
            ModifyBoxByHandles();
            UpdateHandlesRot();
        }
    }

    void CreatePosHandles()
    {
        var offset = GetCalibratedHandleOffset(currentBox, handlePosOffset);
        startHandlePos = CreateHandle(startHandlePos, startHandleRot, offset, COLOR_GREEN);
        endHandlePos = CreateHandle(endHandlePos, endHandleRot, offset, COLOR_GREEN);
    }

    void CreateScaleHandle()
    {
        if (!isUsingScale)
            return;
        var offset = GetCalibratedHandleOffset(currentBox, handleScaleOffset);
        var center = Multiply(currentBox.center, currentBox.transform.localScale);
        var currentBoxCenterPos = currentBox.transform.position + GetOffsetByLocalRotation(currentBox.transform, center);
        var forwardRadius = currentBox.transform.forward * currentBox.size.z / 2 * currentBox.transform.localScale.z;
        var pos = currentBoxCenterPos - forwardRadius;
        scaleHandlePos = CreateHandle(pos, scaleHandleRot, offset, COLOR_BLUE);
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
        scaleHandleRot = currentBox.transform.rotation;
    }

    void OnHandlePivotChanged(PosPivot pivot)
    {
        switch (pivot)
        {
            case PosPivot.Center:
                handlePosOffset = new(0, 0, 0);
                handleScaleOffset = new(1, 1, 0);
                break;
            case PosPivot.TopRight:
                handlePosOffset = new(1, 1, 0);
                handleScaleOffset = -handlePosOffset;
                break;
            case PosPivot.BottomRight:
                handlePosOffset = new(1, -1, 0);
                handleScaleOffset = -handlePosOffset;
                break;
            case PosPivot.BottomLeft:
                handlePosOffset = new(-1, -1, 0);
                handleScaleOffset = -handlePosOffset;
                break;
            case PosPivot.TopLeft:
                handlePosOffset = new(-1, 1, 0);
                handleScaleOffset = -handlePosOffset;
                break;
            case PosPivot.Custom:
                handleScaleOffset = new(1, 1, 0);
                break;
        }
    }

    void OnScaleDirectionChanged(ScaleDirection direction)
    {
        switch (direction)
        {
            case ScaleDirection.All:
                break;
            case ScaleDirection.Corner:
                break;
        }
    }

    #endregion

    #region [Methods: Modify Box]

    void ModifyBoxByHandles()
    {
        currentBox.gameObject.hideFlags = HideFlags.NotEditable;

        var distance = Vector3.Distance(startHandlePos, endHandlePos) / currentBox.transform.localScale.z;
        if (isUsingScale)
        {
            var centerToScaleHandle = currentBox.transform.InverseTransformPoint(scaleHandlePos) + currentBox.size;
            if (scaleDirection == ScaleDirection.All)
            {
                currentBox.center = new(currentBoxOriginalCenter.x,currentBoxOriginalCenter.y, (distance - 1) / 2);
                currentBox.size = new(centerToScaleHandle.x, centerToScaleHandle.y, distance);
            }
            else if (scaleDirection == ScaleDirection.Corner)
            {
                currentBox.center = new(currentBoxOriginalCenter.x, currentBoxOriginalCenter.y, (distance - 1) / 2);
                currentBox.size = new(centerToScaleHandle.x, centerToScaleHandle.y, distance);
            }
        }
        else
        {
            currentBox.center = new(currentBoxOriginalCenter.x, currentBoxOriginalCenter.y, (distance - 1) / 2);
            currentBox.size = new(currentBoxOriginalSize.x, currentBoxOriginalSize.y, distance);
        }
        currentBox.transform.position = GetCurrentBoxPos(currentBox, startHandlePos);
        currentBox.transform.rotation = LookAt(startHandlePos, endHandlePos);

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
            scaleHandlePos = pos - forwardRadius;
            scaleHandleRot = Quaternion.identity;
        }
        else
        {
            currentBox = null;
            startHandlePos = Vector3.zero;
            startHandleRot = Quaternion.identity;
            endHandlePos = Vector3.zero;
            endHandleRot = Quaternion.identity;
            scaleHandlePos = Vector3.zero;
            scaleHandleRot = Quaternion.identity;
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

/*
TODO:
- Implement ScaleDirection.Rotation feature
- Spawn At Pos field
- Apply Scale button
- Chain Box Cols feature
- Handle multiple selections
*/