using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlugRMK.UnityUti.EditorUti
{
    public class CameraToPNGWindow : EditorWindow
    {
        #region [Data Structures]

        enum ShootType { Single, Batch };
        enum SizeMode { Custom, Screen };

        class DelayedAction
        {
            public double Time { get; private set; }
            public Action Action { get; private set; }
            public bool IsInvoked { get; set; }

            public DelayedAction(double time, Action action)
            {
                this.Time = time;
                this.Action = action;
            }
        }

        #endregion

        #region [Variables]

        ShootType shootType = ShootType.Single;
        const string OBJECT_NAME = "{OBJECT_NAME}";
        readonly static string defaultObjectName = $"ImageFile_{DateTime.Now:yyyyMMdd_HH_mm_ss}";
        string fileNameFormat = $"{OBJECT_NAME}";
        Transform batchParent;
        SizeMode sizeMode = SizeMode.Custom;
        Vector2Int imageSize = new(512, 512);
        Camera camera;
        bool isDelayShoot;
        float shootDelay = .1f;
        readonly List<DelayedAction> delayedActions = new();

        #endregion

        #region [Methods: Setups]

        [MenuItem("Tools/Camera To PNG")]
        public static void ShowWindow()
        {
            var window = GetWindow<CameraToPNGWindow>("Camera to PNG");
            var title = EditorGUIUtility.IconContent("d_RawImage Icon");
            title.text = "Camera to PNG";
            window.titleContent = title;
            window.Show();
        }

        void OnEnable()
        {
            if (camera == null)
                camera = Camera.main;
            delayedActions.Clear();
        }

        void OnGUI()
        {
            DrawShootTypeField();
            DrawFileNameFields();
            DrawSizeModeField();
            DrawImageSizeField();
            DrawCameraField();
            DrawDelayShootFields();
            DrawExportButton();
        }

        void Update()
        {
            if (isDelayShoot)
            {
                foreach (var delayedAction in delayedActions)
                {
                    if (!delayedAction.IsInvoked && delayedAction.Time < EditorApplication.timeSinceStartup)
                    {
                        delayedAction.Action();
                        delayedAction.IsInvoked = true;
                    }
                }
            }
        }

        #endregion

        #region [Methods: Field Drawers]

        void DrawShootTypeField()
        {
            shootType = (ShootType)EditorGUILayout.EnumPopup("Shoot Type", shootType);
        }

        void DrawFileNameFields()
        {
            fileNameFormat = EditorGUILayout.TextField(
                new GUIContent("File Name", "{OBJECT_NAME} is the selected GameObject's name.\nIn Batch mode, it's the name of each children of Batch Parent"),
                fileNameFormat);

            if (shootType == ShootType.Single)
            {
                var fileName = GetSingleFileName(fileNameFormat);
                var labelStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleRight,
                    normal = { textColor = new Color(.7f, .7f, .7f) }
                };
                EditorGUILayout.LabelField($"{fileName}.png ", labelStyle);
            }
            else if (shootType == ShootType.Batch)
            {
                batchParent = (Transform)EditorGUILayout.ObjectField("Batch Parent", batchParent, typeof(Transform), true);
            }
        }

        void DrawSizeModeField()
        {
            sizeMode = (SizeMode)EditorGUILayout.EnumPopup("Size Mode", sizeMode);
        }

        void DrawImageSizeField()
        {
            if (sizeMode == SizeMode.Custom)
            {
                imageSize = EditorGUILayout.Vector2IntField("", imageSize);
            }
            else if (sizeMode == SizeMode.Screen)
            {
                var gameViewSize = Handles.GetMainGameViewSize();
                imageSize = new Vector2Int((int)gameViewSize.x, (int)gameViewSize.y);
            }
        }

        void DrawCameraField()
        {
            camera = (Camera)EditorGUILayout.ObjectField("Camera", camera, typeof(Camera), true);
            if (camera == null)
                camera = Camera.main;
        }

        void DrawDelayShootFields()
        {
            isDelayShoot = EditorGUILayout.Toggle("Delay Shoot", isDelayShoot);
            if (isDelayShoot)
            {
                shootDelay = EditorGUILayout.FloatField("Delay", shootDelay);
                if (shootDelay < 0)
                    shootDelay = 0.1f;
            }
        }

        void DrawExportButton()
        {
            if (GUILayout.Button("Export", GUILayout.Height(32)))
            {
                switch (shootType)
                {
                    case ShootType.Single:
                        if (isDelayShoot)
                            ExportSingleWithDelay();
                        else
                            ExportSingle();
                        break;
                    case ShootType.Batch:
                        if (isDelayShoot)
                            ExportBatchWithDelay();
                        else
                            ExportBatch();
                        break;
                }
            }
        }

        #endregion

        #region [Methods: Export]

        void ExportSingle()
        {
            var fileName = GetSingleFileName(fileNameFormat);
            var path = EditorUtility.SaveFilePanel("Save PNG", "", $"{fileName}.png", "png");
            if (string.IsNullOrEmpty(path))
                return;

            ExportCameraTo(path);
            OpenFolder(path[..path.LastIndexOf('/')]);
        }

        void ExportSingleWithDelay()
        {
            delayedActions.Clear();
            delayedActions.Add(new(EditorApplication.timeSinceStartup + shootDelay, ExportSingle));
        }

        void ExportBatch()
        {
            if (batchParent == null)
                return;

            var folderPath = EditorUtility.SaveFolderPanel("Save PNGs", "Folder", "");
            if (string.IsNullOrEmpty(folderPath))
                return;

            var initialActiveState = CreateInitialActiveStates(batchParent);
            foreach (Transform child in batchParent)
            {
                child.gameObject.SetActive(true);
                var fileName = fileNameFormat.Replace(OBJECT_NAME, child.name);
                ExportCameraTo($"{folderPath}/{fileName}.png");
                child.gameObject.SetActive(false);
            }
            RestoreInitialActiveStates(initialActiveState);
            OpenFolder(folderPath);
        }

        void ExportBatchWithDelay()
        {
            if (batchParent == null)
                return;

            var folderPath = EditorUtility.SaveFolderPanel("Save PNGs", "Folder", "");
            if (string.IsNullOrEmpty(folderPath))
                return;

            delayedActions.Clear();
            var index = 0;
            var initialActiveState = CreateInitialActiveStates(batchParent);
            foreach (Transform child in batchParent)
            {
                DelayActivation(index, child);
                DelayExport(index, child, folderPath);
                index++;
            }
            DelayReset(folderPath, initialActiveState);

            void DelayActivation(int index, Transform child)
            {
                delayedActions.Add(new(
                    EditorApplication.timeSinceStartup + index * shootDelay,
                    () => child.gameObject.SetActive(true)
                ));
            }

            void DelayExport(int index, Transform child, string folderPath)
            {
                delayedActions.Add(new(
                    EditorApplication.timeSinceStartup + index * shootDelay + .1,
                    () =>
                    {
                        var fileName = fileNameFormat.Replace(OBJECT_NAME, child.name);
                        ExportCameraTo($"{folderPath}/{fileName}.png");
                        child.gameObject.SetActive(false);
                    }
                ));
            }

            void DelayReset(string folderPath, List<(Transform, bool)> initialActiveState)
            {
                delayedActions.Add(new(
                    EditorApplication.timeSinceStartup + batchParent.childCount * shootDelay,
                    () =>
                    {
                        RestoreInitialActiveStates(initialActiveState);
                        OpenFolder(folderPath);
                    }
                ));
            }
        }

        void ExportCameraTo(string path)
        {
            camera.depthTextureMode = DepthTextureMode.Depth;
            var cacheCurrentTexture = camera.targetTexture;
            camera.targetTexture = new RenderTexture(imageSize.x, imageSize.y, 32, RenderTextureFormat.ARGBFloat)
            {
                antiAliasing = 4,
                filterMode = FilterMode.Bilinear,
            };
            camera.Render();

            var exportTexture = new Texture2D(imageSize.x, imageSize.y, TextureFormat.RGBA32, false, true);
            RenderTexture.active = camera.targetTexture;
            exportTexture.ReadPixels(new Rect(0, 0, imageSize.x, imageSize.y), 0, 0);
            exportTexture.Apply();
            RenderTexture.active = null;

            var pngData = exportTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, pngData);
            camera.targetTexture = cacheCurrentTexture;
        }

        #endregion

        #region [Methods: Utility]

        static List<(Transform, bool)> CreateInitialActiveStates(Transform parent)
        {
            var initialActiveState = new List<(Transform, bool)>();
            foreach (Transform child in parent)
            {
                initialActiveState.Add((child, child.gameObject.activeSelf));
                child.gameObject.SetActive(false);
            }

            return initialActiveState;
        }

        static void RestoreInitialActiveStates(List<(Transform, bool)> initialActiveState)
        {
            foreach (var (child, active) in initialActiveState)
                child.gameObject.SetActive(active);
        }

        static void OpenFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                Debug.LogError("Folder path is null or empty.");
                return;
            }
            if (System.IO.Directory.Exists(folderPath))
            {
#if UNITY_EDITOR_WIN
                folderPath = folderPath.Replace("/", "\\");
                System.Diagnostics.Process.Start("explorer.exe", folderPath);
#elif UNITY_EDITOR_OSX
                System.Diagnostics.Process.Start("open", folderPath);
#else
                Debug.LogError("Platform not supported for opening folders.");
#endif
            }
            else
            {
                Debug.LogError($"Folder does not exist: {folderPath}");
            }
        }

        static string GetSingleFileName(string fileNameFormat)
        {
            if (fileNameFormat.Contains(OBJECT_NAME))
            {
                var objectName = Selection.activeGameObject != null ? Selection.activeGameObject.name : defaultObjectName;
                return fileNameFormat.Replace(OBJECT_NAME, objectName);
            }
            else if (fileNameFormat.Length > 0)
            {
                return fileNameFormat;
            }
            else
            {
                return defaultObjectName;
            }
        }

        #endregion
    }
}