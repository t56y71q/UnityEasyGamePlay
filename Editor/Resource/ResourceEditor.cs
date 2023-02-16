using System;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

namespace EasyGamePlay.Editor
{
    public class ResourceEditor
    {
        public EditorResourceSetting editorResourceSetting;
        private ResourceData resourceData;
        private List<EditorBundle> editorBundles=new List<EditorBundle>();
        private static string settingPath = "Resource/Editor/EditorResourceSetting.asset";
        private static string dataPath = "Resource/Editor/ResourceData.asset";

        internal void Init()
        {
            string path = UnityEngine.Application.dataPath + "/" + settingPath;
            if(!File.Exists(path))
            {
                string dir = Path.GetDirectoryName(path);
                if(!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                editorResourceSetting = EditorResourceSetting.CreateInstance<EditorResourceSetting>();
                AssetDatabase.CreateAsset(editorResourceSetting, "Assets/"+ settingPath);
                AssetDatabase.Refresh();
            }
            else
                editorResourceSetting = AssetDatabase.LoadAssetAtPath<EditorResourceSetting>("Assets/" + settingPath);

            path = UnityEngine.Application.dataPath + "/" + dataPath;
            if (!File.Exists(path))
            {
                string dir = Path.GetDirectoryName(path);

                resourceData = ResourceData.CreateInstance<ResourceData>();
                AssetDatabase.CreateAsset(resourceData, "Assets/" + dataPath);
                AssetDatabase.Refresh();
            }
            else
                resourceData = AssetDatabase.LoadAssetAtPath<ResourceData>("Assets/" + dataPath);

            for(int i= resourceData.folders.Count-1; i>=0;i--)
            {
                LoadBundles(resourceData.folders[i],i);
            }
        }

        internal void CreateBundle(string bundleName,string bundlefolder)
        {
            EditorBundle editorBundle = EditorBundle.CreateInstance<EditorBundle>();
            editorBundle.name = bundleName;
            editorBundles.Add(editorBundle);

            if(!resourceData.folders.Contains(bundlefolder))
            {
                resourceData.folders.Add(bundlefolder);
            }

            AssetDatabase.CreateAsset(editorBundle, bundlefolder + "/" + bundleName + ".asset");
            AssetDatabase.Refresh();
        }

        public UnityEngine.Object LoadAsset(string path)
        {
            UnityEngine.Object @object;
            for (int i=0;i< editorBundles.Count;i++)
            {
                @object = editorBundles[i]?.GetObject(path);
                if (@object != null)
                    return @object;
            }
            return null;
        }

        public T LoadAsset<T>(string path) where T:UnityEngine.Object
        {
            T @object;
            for (int i = 0; i < editorBundles.Count; i++)
            {
                @object = editorBundles[i]?.GetObject(path) as T;
                if (@object != null)
                    return @object;
            }
            return null;
        }

        private void LoadBundles(string folder,int index)
        {
            if (Directory.Exists(folder))
            {
                string[] files = Directory.GetFiles(folder, "*.asset");
                string editorPath;
                EditorBundle editorBundle;

                for (int i = 0; i < files.Length; i++)
                {
                    editorPath = files[i].Substring(files[i].IndexOf("Assets"));
                    editorBundle = AssetDatabase.LoadAssetAtPath<EditorBundle>(editorPath);
                    editorBundles.Add(editorBundle);
                }
            }
            else
            {
                resourceData.folders.RemoveAt(index);
            }
        }
    }
}
