using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyGamePlay.Editor
{
    class OpenLevelWindow:EditorWindow
    {
        private string levelPath;

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    levelPath = EditorGUILayout.TextField(levelPath);
                    if (GUILayout.Button("..."))
                    {
                        levelPath = EditorUtility.OpenFilePanel("OpenLevel", "Assets/", "json");
                        Repaint();
                    }
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Open"))
                    {
                        FrameWorkEditor.levelEditor.Open(levelPath);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        [MenuItem("EasyGamePlay/Level/Open")]
        private static void ShowWindow()
        {
            OpenLevelWindow.CreateWindow<OpenLevelWindow>().Show();
        }
    }
}
