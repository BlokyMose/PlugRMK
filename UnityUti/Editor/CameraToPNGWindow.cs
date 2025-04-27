using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SwingHero
{
    public class CameraToPNGWindow : EditorWindow
    {
        enum ShootType { Single, Batch };
        ShootType shootType = ShootType.Single;
        string fileName = "ImageName";
        const string OBJECT_NAME = "{OBJECT_NAME}";
        string batchFileNameFormat = $"{OBJECT_NAME}";
        Transform batchParent;
        Vector2Int imageSize = new(256, 256);
        Camera camera;

        [MenuItem("Tools/Camera To PNG")]
        public static void ShowWindow()
        {
            var window = GetWindow<CameraToPNGWindow>();
            window.titleContent = new GUIContent("Camera to PNG");
            window.Show();
        }

        void OnEnable()
        {
            if (camera == null)
                camera = Camera.main;
        }

        void OnGUI()
        {
            DrawShootTypeField();
            DrawFileNameFields();
            DrawImageSizeField();
            DrawCameraField();
            DrawExportButton();
        }

        #region [Methods: Field Drawers]

        void DrawShootTypeField()
        {
            shootType = (ShootType)EditorGUILayout.EnumPopup("Shoot Type", shootType);
        }

        void DrawFileNameFields()
        {
            switch (shootType)
            {
                case ShootType.Single:
                    fileName = EditorGUILayout.TextField("File Name", fileName);
                    break;
                case ShootType.Batch:
                    batchFileNameFormat = EditorGUILayout.TextField("File Name Format", batchFileNameFormat);
                    batchParent = (Transform)EditorGUILayout.ObjectField("Batch Parent", batchParent, typeof(Transform), true);
                    break;
            }
        }

        void DrawImageSizeField()
        {
            imageSize = EditorGUILayout.Vector2IntField("Size", imageSize);
        }

        void DrawCameraField()
        {
            camera = (Camera)EditorGUILayout.ObjectField("Camera", camera, typeof(Camera), true);
            if (camera == null)
                camera = Camera.main;
        }

        void DrawExportButton()
        {
            if (GUILayout.Button("Export", GUILayout.Height(32)))
            {
                switch (shootType)
                {
                    case ShootType.Single:
                        ExportSingle();
                        break;
                    case ShootType.Batch:
                        ExportBatch();
                        break;
                }
            }
        }

        #endregion

        #region [Methods: Export]

        void ExportSingle()
        {
            var path = EditorUtility.SaveFilePanel("Save PNG", "", $"{fileName}.png", "png");
            if (string.IsNullOrEmpty(path))
                return;

            Export(path);
            OpenFolder(path[..path.LastIndexOf('/')]);
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
                var fileName = batchFileNameFormat.Replace(OBJECT_NAME, child.name);
                Export($"{folderPath}/{fileName}.png");
                child.gameObject.SetActive(false);
            }
            RestoreInitialActiveStates(initialActiveState);
            OpenFolder(folderPath);
        }

        void Export(string path)
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

            folderPath = folderPath.Replace("/", "\\"); // Ensure compatibility with Windows paths

            if (System.IO.Directory.Exists(folderPath))
            {
#if UNITY_EDITOR_WIN
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
        
        #endregion
    }
}