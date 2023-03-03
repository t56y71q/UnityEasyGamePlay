using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyGamePlay.Editor
{
    class CreateLevelWindow:EditorWindow
    {
        private string folderPath;
        private string levelName;

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("folder:");
                    folderPath = EditorGUILayout.TextField(folderPath);
                    if (GUILayout.Button("..."))
                    {
                        folderPath = EditorUtility.OpenFolderPanel("Folder", "Assets/", folderPath);
                        Repaint();
                    }
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("LevelName:");
                    levelName = EditorGUILayout.TextField(levelName);
                   
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Create"))
                    {
                        FrameWorkEditor.levelEditor.Create(folderPath + "/" + levelName + ".json");
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        [MenuItem("EasyGamePlay/Level/Create",priority = 10)]
        private static void ShowWindow()
        {
            CreateLevelWindow.CreateWindow<CreateLevelWindow>().Show();
        }
    }
}
