using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace EasyGamePlay.Editor
{
    class EditorBundle:ScriptableObject
    {
        public string group="default";
        public List<EditorAssetInfo> editorAssetInfos;

        public AssetInfo GetAssetInfo(int index)
        {
            EditorAssetInfo editorAssetInfo = editorAssetInfos[index];
            string path = AssetDatabase.GetAssetPath(editorAssetInfo.@object);
            return new AssetInfo(editorAssetInfo.path, name, Path.GetFileNameWithoutExtension(path), editorAssetInfo.@object.GetType());
        }

        public AssetInfo GetSimulateAssetInfo(int index)
        {
            EditorAssetInfo editorAssetInfo = editorAssetInfos[index];
            return new AssetInfo(editorAssetInfo.path, name, AssetDatabase.GetAssetPath(editorAssetInfo.@object), editorAssetInfo.@object.GetType());
        }

        public AssetInfo GetScene(int index, List<EditorBuildSettingsScene> scenes)
        {
            EditorAssetInfo editorAssetInfo = editorAssetInfos[index];
            string path = AssetDatabase.GetAssetPath(editorAssetInfo.@object);
            for (int j = 0; j < scenes.Count; j++)
            {
                if (scenes[j].path == path)
                {
                    scenes.RemoveAt(j);
                    break;
                }
            }
            return new AssetInfo(editorAssetInfo.path, name, Path.GetFileNameWithoutExtension(path), typeof(UnityEngine.SceneManagement.Scene));
        }

        public AssetInfo GetSimulateScene(int index, List<EditorBuildSettingsScene> scenes)
        {
            EditorAssetInfo editorAssetInfo = editorAssetInfos[index];
            string path = AssetDatabase.GetAssetPath(editorAssetInfo.@object);
            bool isExist = false;
            for (int j = 0; j < scenes.Count; j++)
            {
                if (scenes[j].path == path)
                {
                    isExist = true;
                }
            }

            if (!isExist)
            {
                scenes.Add(new EditorBuildSettingsScene(path, true));
            }
            return new AssetInfo(editorAssetInfo.path, name, path, typeof(UnityEngine.SceneManagement.Scene));
        }

        public string[] GetPathes()
        {
            string[] pathes = new string[editorAssetInfos.Count];
            for (int i = 0; i < editorAssetInfos.Count; i++)
            {
                pathes[i] = editorAssetInfos[i].path;
            }
            return pathes;
        }

        public string[] GetAssetPathes()
        {
            string[] assetPathes = new string[editorAssetInfos.Count];
            int index;
            string path;
            string start = "Assets/";
            for (int i = 0; i < editorAssetInfos.Count; i++)
            {
                path = AssetDatabase.GetAssetPath(editorAssetInfos[i].@object);
                index = path.IndexOf(start);
                assetPathes[i] = path.Substring(index);
            }
            return assetPathes;
        }

        public UnityEngine.Object GetObject(string path)
        {
            for (int i = 0; i < editorAssetInfos.Count; i++)
            {
                if(path== editorAssetInfos[i].path)
                {
                    return editorAssetInfos[i].@object;
                }
            }
            return null;
        }

        public string GetPath(UnityEngine.Object @object)
        {
            for (int i = 0; i < editorAssetInfos.Count; i++)
            {
                if (@object == editorAssetInfos[i].@object)
                {
                    return editorAssetInfos[i].path;
                }
            }
            return null;
        }

        public bool HasScene()
        {
            return editorAssetInfos.Count > 0 && editorAssetInfos[0].@object.GetType() == typeof(SceneAsset);
        }

        public bool HasPath(string path)
        {
            for (int i = 0; i < editorAssetInfos.Count; i++)
            {
                if (path == AssetDatabase.GetAssetPath(editorAssetInfos[i].@object))
                    return true;
            }
            return false;
        }
    }
}
