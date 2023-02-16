using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace EasyGamePlay.Editor
{
    class CreateBundleWindow:EditorWindow
    {
        string bundleFolder;
        string bundleName;

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Bundle文件夹");
                    GUILayout.Space(2f);
                    bundleFolder = EditorGUILayout.TextField(bundleFolder);
                    if (GUILayout.Button("..."))
                    {
                        bundleFolder = EditorUtility.OpenFolderPanel("Bundle", "Assets", null);
                        bundleFolder = bundleFolder.Substring(bundleFolder.IndexOf("Assets"));
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.EndHorizontal();
                {
                    GUILayout.Label("Bundle:");
                    GUILayout.Space(2f);
                    bundleName = EditorGUILayout.TextField(bundleName);
                }
                GUILayout.BeginHorizontal();

                GUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(200f);
                   
                    if (GUILayout.Button("创建") && !string.IsNullOrEmpty(bundleFolder) && Directory.Exists(bundleFolder))
                    {
                        FrameWorkEditor.resourceEditor.CreateBundle(bundleName, bundleFolder);
                        Close();
                    }

                    if (GUILayout.Button("取消"))
                    {
                        Close();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginVertical();
        }

        [MenuItem("EasyGamePlay/Resource/CreateBundle")]
        private static void ShowWindow()
        {
            CreateBundleWindow.CreateWindow<CreateBundleWindow>().Show();
        }
    }
}
