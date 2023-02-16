using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace EasyGamePlay.Editor
{
    class ResourceBuild
    {
        public static void Build(string buildFolder,string bundleFolder)
        {
            GameConfig gameConfig = FrameWorkEditor.resourceEditor.editorResourceSetting.gameConfig;
            if (gameConfig == null)
            {
                UnityEngine.Debug.LogError("Not set GameConfig");
                return;
            }

            if (!Directory.Exists(buildFolder))
            {
                if (string.IsNullOrEmpty(buildFolder))
                {
                    buildFolder = "./Assets/Build";
                }
                Directory.CreateDirectory(buildFolder);
            }

            string[] files = Directory.GetFiles(bundleFolder, "*.asset");
            string editorPath;
            EditorBundle editorBundle;
            string group;

            Dictionary<string, BuildContent> buildContents = new Dictionary<string, BuildContent>();

            AssetBundleBuild bundleBuild;
            for (int i = 0; i < files.Length; i++)
            {
                editorPath = files[i].Substring(files[i].IndexOf("Assets"));
                editorBundle = AssetDatabase.LoadAssetAtPath<EditorBundle>(editorPath);

                //创建group文件夹
                group = buildFolder + "/" + editorBundle.group;
                if (!Directory.Exists(group))
                {
                    Directory.CreateDirectory(group);
                }
                //添加AssetInfo
                if (!buildContents.TryGetValue(editorBundle.group, out BuildContent buildContent))
                {
                    buildContent = new BuildContent(new List<AssetInfo>(), new List<AssetBundleBuild>());
                    buildContents.Add(editorBundle.group, buildContent);
                }

                buildContent.assetInfos.AddRange(editorBundle.GetAssetInfos());

                bundleBuild = new AssetBundleBuild();
                bundleBuild.assetBundleName = editorBundle.name;
                bundleBuild.assetBundleVariant = "ab";
                bundleBuild.assetNames = editorBundle.GetAssetPathes();

                buildContent.bundleBuilds.Add(bundleBuild);
            }
            //创建文件
            List<string> groups = new List<string>(buildContents.Keys);
            for (int i = 0; i < groups.Count; i++)
            {
                buildContents[groups[i]].Build(buildFolder, groups[i]);
            }
            if (string.IsNullOrEmpty(gameConfig.resourceSetting.defaultAssetInfoKey))
                gameConfig.resourceSetting.defaultAssetInfoKey = "default";
            string unityPath = buildFolder + "/" + gameConfig.resourceSetting.defaultAssetInfoKey + "/" + gameConfig.resourceSetting.defaultAssetInfoKey + ".json";
            unityPath = unityPath.Substring(unityPath.IndexOf("Assets/"));
            gameConfig.resourceSetting.defaultAssetInfoFile = AssetDatabase.LoadAssetAtPath<TextAsset>(unityPath);
            AssetDatabase.Refresh();
        }

        public static void Simulate(string buildFolder, string bundleFolder)
        {
            GameConfig gameConfig = FrameWorkEditor.resourceEditor.editorResourceSetting.gameConfig;
            if (gameConfig == null)
            {
                UnityEngine.Debug.LogError("Not set GameConfig");
                return;
            }

            if (!Directory.Exists(buildFolder))
            {
                if (string.IsNullOrEmpty(buildFolder))
                {
                    buildFolder = "./Assets/Build";
                }
                Directory.CreateDirectory(buildFolder);
            }

            string[] files = Directory.GetFiles(bundleFolder, "*.asset");
            string editorPath;
            EditorBundle editorBundle;
            string group;

            Dictionary<string, BuildContent> buildContents = new Dictionary<string, BuildContent>();


            for (int i = 0; i < files.Length; i++)
            {
                editorPath = files[i].Substring(files[i].IndexOf("Assets"));
                editorBundle = AssetDatabase.LoadAssetAtPath<EditorBundle>(editorPath);

                //创建group文件夹
                group = buildFolder + "/" + editorBundle.group;
                if (!Directory.Exists(group))
                {
                    Directory.CreateDirectory(group);
                }
                //添加AssetInfo
                if (!buildContents.TryGetValue(editorBundle.group, out BuildContent buildContent))
                {
                    buildContent = new BuildContent(new List<AssetInfo>(), new List<AssetBundleBuild>());
                    buildContents.Add(editorBundle.group, buildContent);
                }

                buildContent.assetInfos.AddRange(editorBundle.GetEditorAssetInfos());
            }

            if (string.IsNullOrEmpty(gameConfig.resourceSetting.defaultAssetInfoKey))
                gameConfig.resourceSetting.defaultAssetInfoKey = "default";
            string unityPath = buildFolder + "/" + gameConfig.resourceSetting.defaultAssetInfoKey + "/" + gameConfig.resourceSetting.defaultAssetInfoKey + ".json";
            unityPath = unityPath.Substring(unityPath.IndexOf("Assets/"));
            gameConfig.resourceSetting.defaultAssetInfoFile = AssetDatabase.LoadAssetAtPath<TextAsset>(unityPath);

            List<string> groups = new List<string>(buildContents.Keys);
            for (int i = 0; i < groups.Count; i++)
            {
                buildContents[groups[i]].Simulate(buildFolder, groups[i]);
            }
            AssetDatabase.Refresh();
        }

        private struct BuildContent
        {
            public List<AssetInfo> assetInfos;
            public List<AssetBundleBuild> bundleBuilds;

            public BuildContent(List<AssetInfo> assetInfos, List<AssetBundleBuild> bundleBuilds)
            {
                this.assetInfos = assetInfos;
                this.bundleBuilds = bundleBuilds;
            }

            public void Build(string buildFolder, string group)
            {
                string folder = buildFolder + "/" + group;

                AssetInfoFile assetInfoFile = new AssetInfoFile(assetInfos);
                string data = FrameWork.frameWork.Serialize(assetInfoFile, SerializeType.json);

                TextWriter textWriter = new TextWriter(folder + "/" + Path.GetFileNameWithoutExtension(group) + ".json");
                textWriter.Write(data);
                textWriter.CloseFile();

                BuildPipeline.BuildAssetBundles(folder, bundleBuilds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            }

            public void Simulate(string buildFolder, string group)
            {
                string folder = buildFolder + "/" + group;

                AssetInfoFile assetInfoFile = new AssetInfoFile(assetInfos);
                string data = FrameWork.frameWork.Serialize(assetInfoFile, SerializeType.json);

                TextWriter textWriter = new TextWriter(folder + "/" + Path.GetFileNameWithoutExtension(group) + ".json");
                textWriter.Write(data);
                textWriter.CloseFile();
            }
        }
    }
}
