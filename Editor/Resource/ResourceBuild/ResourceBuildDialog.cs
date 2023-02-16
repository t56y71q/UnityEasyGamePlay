using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace EasyGamePlay.Editor
{
    class ResourceBuildDialog:EditorWindow
    {
        private string bundleFolder;
        private string buildFolder="./Assets/Build";

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("文件夹");
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Bundle文件夹");
                    GUILayout.Space(2f);
                    bundleFolder = EditorGUILayout.TextField(bundleFolder);
                    if(GUILayout.Button("..."))
                    {
                        bundleFolder= EditorUtility.OpenFolderPanel("Bundle", "Assets", null);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Build文件夹");
                    GUILayout.Space(2f);
                    buildFolder = EditorGUILayout.TextField(buildFolder);
                    if (GUILayout.Button("..."))
                    {
                        buildFolder=EditorUtility.OpenFolderPanel("Build", "Assets", null);
                    }
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.Separator();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(200f);
                    if (GUILayout.Button("模拟打包") && !string.IsNullOrEmpty(bundleFolder) && Directory.Exists(bundleFolder))
                    {
                        ResourceBuild.Simulate(buildFolder, bundleFolder);
                        Close();
                    }

                    if (GUILayout.Button("打包") && !string.IsNullOrEmpty(bundleFolder) && Directory.Exists(bundleFolder))
                    {
                        ResourceBuild.Build(buildFolder, bundleFolder);
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

        [MenuItem("EasyGamePlay/Resource/Build")]
        private static void showWindow()
        {
            ResourceBuildDialog.CreateWindow<ResourceBuildDialog>().Show();
        }
    }
}
