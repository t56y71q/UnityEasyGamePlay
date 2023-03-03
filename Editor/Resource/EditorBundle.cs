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

        public AssetInfo[] GetAssetInfos()
        {
            AssetInfo[] assetInfos = new AssetInfo[editorAssetInfos.Count];

            AssetInfo assetInfo;
            for (int i=0;i< editorAssetInfos.Count;i++)
            {
                assetInfo = new AssetInfo(editorAssetInfos[i].path,name, Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(editorAssetInfos[i].@object)), editorAssetInfos[i].@object.GetType());
                assetInfos[i] = assetInfo;
            }
            return assetInfos;
        }

        public AssetInfo[] GetEditorAssetInfos()
        {
            AssetInfo[] assetInfos = new AssetInfo[editorAssetInfos.Count];

            AssetInfo assetInfo;
            for (int i = 0; i < editorAssetInfos.Count; i++)
            {
                assetInfo = new AssetInfo(editorAssetInfos[i].path, name, AssetDatabase.GetAssetPath(editorAssetInfos[i].@object), editorAssetInfos[i].@object.GetType());
                assetInfos[i] = assetInfo;
            }
            return assetInfos;
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
    }
}
