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

            FrameWorkEditor.resourceEditor.editorResourceSetting.isEditor = false;

            string[] files = Directory.GetFiles(bundleFolder, "*.asset");
            string editorPath;
            EditorBundle editorBundle;
            string group;

            Dictionary<string, BuildContent> buildContents = new Dictionary<string, BuildContent>();
            List<EditorBundle> editorBundles = new List<EditorBundle>(files.Length);

            AssetBundleBuild bundleBuild;
            for (int i = 0; i < files.Length; i++)
            {
                editorPath = files[i].Substring(files[i].IndexOf("Assets"));
                editorBundle = AssetDatabase.LoadAssetAtPath<EditorBundle>(editorPath);

                if (editorBundle != null)
                {
                    editorBundles.Add(editorBundle);
                    //创建group文件夹
                    group = buildFolder + "/" + editorBundle.group;
                    if (!Directory.Exists(group))
                    {
                        Directory.CreateDirectory(group);
                    }
                }
            }

          
            for (int i=0;i<editorBundles.Count;i++)
            {
                editorBundle = editorBundles[i];
                //添加AssetInfo
                if (!buildContents.TryGetValue(editorBundle.group, out BuildContent buildContent))
                {
                    buildContent = new BuildContent(new List<AssetInfo>(), new List<AssetBundleBuild>());
                    buildContents.Add(editorBundle.group, buildContent);
                }

                List<AssetInfo> assetInfos = new List<AssetInfo>();
                AssetInfo assetInfo;
                //判断是否包含scene
                //如果包含scene，创建新的bundle，以scene命名
                //枚举依赖，需要从已知的bundle中查找是否有依赖
                //没有查找到，记录在新bundle中
                //不包含则直接枚举依赖，需要从已知的bundle中查找是否有依赖
                //没有查找到，记录在当前bundle中
                if (editorBundle.HasScene())
                {
                    List<string> dependencyAssetInfos = new List<string>();
                    List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

                    for (int j = 0; j < editorBundle.editorAssetInfos.Count; j++)
                    {
                        string path = AssetDatabase.GetAssetPath(editorBundle.editorAssetInfos[j].@object);
                        assetInfo = editorBundle.GetScene(j, scenes);
                        assetInfos.Add(assetInfo);
                    }
                    EditorBuildSettings.scenes = scenes.ToArray();
                }
                else
                {
                    for (int j = 0; j < editorBundle.editorAssetInfos.Count; j++)
                    {
                        assetInfos.Add(editorBundle.GetAssetInfo(j));
                    }
                }
                bundleBuild = new AssetBundleBuild();
                bundleBuild.assetBundleName = editorBundle.name;
                bundleBuild.assetBundleVariant = "ab";
                bundleBuild.assetNames = editorBundle.GetAssetPathes();
                buildContent.assetInfos.AddRange(assetInfos);
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

            FrameWorkEditor.resourceEditor.editorResourceSetting.isEditor = true;

            string[] files = Directory.GetFiles(bundleFolder, "*.asset");
            string editorPath;
            EditorBundle editorBundle;
            string group;

            Dictionary<string, BuildContent> buildContents = new Dictionary<string, BuildContent>();

            for (int i = 0; i < files.Length; i++)
            {
                editorPath = files[i].Substring(files[i].IndexOf("Assets"));
                editorBundle = AssetDatabase.LoadAssetAtPath<EditorBundle>(editorPath);

                if (editorBundle == null)
                    continue;

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

                List<AssetInfo> assetInfos = new List<AssetInfo>();
                List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                if (editorBundle.HasScene())
                {
                    for(int j=0;j< editorBundle.editorAssetInfos.Count;j++)
                    {
                        assetInfos.Add( editorBundle.GetSimulateScene(j, scenes));
                    }
                }
                else
                {
                    for (int j = 0; j < editorBundle.editorAssetInfos.Count; j++)
                    {
                        assetInfos.Add(editorBundle.GetSimulateAssetInfo(j));
                    }
                }
                buildContent.assetInfos.AddRange(assetInfos);
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

        private static void LoadDependencies(string bundleName,string[] dependencies,List<EditorBundle> bundles, List<string> sceneAssetInfos)
        {
            string dependency;
            for(int i=0;i<dependencies.Length;i++)
            {
                dependency = dependencies[i];
                if (dependency.Contains(".cs"))
                    continue;

                bool isExist = false;
                for(int j=0;j<bundles.Count;j++)
                {
                    if(bundles[j].HasPath(dependency))
                    {
                        isExist = true;
                        break;
                    }
                }

                if(!isExist)
                {
                    sceneAssetInfos.Add(dependency);
                }
            }
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
                string data = FrameWork.frameWork.serialize.Serialize(assetInfoFile, SerializeType.json);

                TextWriter textWriter = new TextWriter(folder + "/" + Path.GetFileNameWithoutExtension(group) + ".json");
                textWriter.Write(data);
                textWriter.CloseFile();

                if (bundleBuilds.Count > 0)
                    BuildPipeline.BuildAssetBundles(folder, bundleBuilds.ToArray(), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
                else
                    Debug.LogWarning(group+" bundles count is 0;");
            }

            public void Simulate(string buildFolder, string group)
            {
                string folder = buildFolder + "/" + group;

                AssetInfoFile assetInfoFile = new AssetInfoFile(assetInfos);
                string data = FrameWork.frameWork.serialize.Serialize(assetInfoFile, SerializeType.json);

                TextWriter textWriter = new TextWriter(folder + "/" + Path.GetFileNameWithoutExtension(group) + ".json");
                textWriter.Write(data);
                textWriter.CloseFile();
            }
        }
    }
}
