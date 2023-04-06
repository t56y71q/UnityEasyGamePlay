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

        private bool isBuild;
        private bool isSimulate;

        private void OnGUI()
        {
            bool isClose = false;
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
                        isBuild = true;
                        isSimulate = true;
                        isClose = true;
                    }

                    if (GUILayout.Button("打包") && !string.IsNullOrEmpty(bundleFolder) && Directory.Exists(bundleFolder))
                    {
                        isBuild = true;
                        isSimulate = false;
                        isClose = true;
                    }

                    if (GUILayout.Button("取消"))
                    {
                        isClose = true;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            if(isClose)
            {
                Close();
            }
        }

        private void OnDestroy()
        {
            if (isBuild)
            {
                if (isSimulate)
                {
                    ResourceBuild.Simulate(buildFolder, bundleFolder);
                }
                else
                {
                    ResourceBuild.Build(buildFolder, bundleFolder);
                }
            }
        }

        [MenuItem("EasyGamePlay/Resource/Build")]
        private static void showWindow()
        {
            ResourceBuildDialog.CreateWindow<ResourceBuildDialog>().Show();
        }
    }
}
