using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public class EResource
    {
        private Dictionary<string, SmartObject> assets=new Dictionary<string, SmartObject>();
        private Dictionary<string, List<string>> pathes = new Dictionary<string, List<string>>();
        private Dictionary<string, ResourceLoader> resourceLoaders = new Dictionary<string, ResourceLoader>();
        private List<ResourceLoaderCreator> loaderCreators = new List<ResourceLoaderCreator>();

        private string folder;
        private ESerialize eSerialize;

        public EResource(ESerialize eSerialize)
        {
            this.eSerialize = eSerialize;
            EAsset.unload = UnloadAsset;
            SmartObject.release = ReleaseAsset;
            ResourceLoader.remove = RemoveResourceLoader;

            EAsset.unload = UnloadAsset;
        }

        public void LoadAssetAsync(string path,Action<EAsset> completed)
        {
            if (assets.TryGetValue(path,out SmartObject smartObject) )
            {
                if (smartObject.IsZeroCount())
                {
                    smartObject.completed += completed;
                    Load(smartObject);
                    smartObject.Add();
                } 
                else
                {
                    smartObject.Add();
                    completed?.Invoke(new EAsset(smartObject.@object, path));
                }             
            }
            else
                UnityEngine.Debug.LogWarning("has no asset in this path:"+path);
        }

        internal void LoadSceneAsync(string scene,Action complete )
        {
            if (assets.TryGetValue(scene, out SmartObject smartObject)&&smartObject.type==typeof(UnityEngine.SceneManagement.Scene))
            {
                if (!resourceLoaders.TryGetValue(smartObject.bundleName,out ResourceLoader resourceLoader))
                {
                    resourceLoader = CreateLoader(smartObject.bundleName);
                    resourceLoaders.Add(smartObject.bundleName, resourceLoader);
                    UnityEngine.Debug.Log("ResourceLoader Count:" + resourceLoaders.Count);
                }
                smartObject.Add();
                resourceLoader.LoadScene(complete);
            }
            else
                UnityEngine.Debug.LogWarning("has no Scene in this path:" + scene);
        }

        public void LoadSetting(ResourceSetting resourceSetting)
        {
            folder = resourceSetting.folder + "/";
           
            if (!string.IsNullOrEmpty(resourceSetting.postFixed))
                ABResourceLoader.postfixed = "." + resourceSetting.postFixed;
            else
                ABResourceLoader.postfixed = string.Empty;

            AssetInfoFile infoFile = eSerialize.DeSerialize<AssetInfoFile>(resourceSetting.defaultAssetInfoFile.text,SerializeType.json);
            if(infoFile.assetInfos!=null)
                LoadInfo(resourceSetting.defaultAssetInfoKey, infoFile);

            AddLoaderCreator(new ResourcesLoaderCreator());
            AddLoaderCreator(new ABLoaderCreator());

            ABResourceLoader.loadLoader =delegate(string bundleName) 
            {
                if(!resourceLoaders.TryGetValue(bundleName,out ResourceLoader resourceLoader))
                {
                    resourceLoader = CreateLoader(bundleName);
                    resourceLoaders.Add(bundleName, resourceLoader);
                }
                resourceLoader.Add();
            };
            ABResourceLoader.unloadLoader =delegate(string bundleName) 
            {
                if (resourceLoaders.TryGetValue(bundleName, out ResourceLoader resourceLoader))
                {
                    resourceLoader.Release();
                }
            };
            string mainfestPath = folder + System.IO.Path.GetFileNameWithoutExtension(resourceSetting.folder.ToLower());
            if (System.IO.File.Exists(mainfestPath))
                ABResourceLoader.LoadMainfest(mainfestPath);
        }

        public void LoadInfo(string key, AssetInfoFile infoFile)
        {
            if(!pathes.ContainsKey(key))
            {
                List<string> mPathes = new List<string>(infoFile.assetInfos.Length);

                AssetInfo assetInfo;
                for(int i=0;i< infoFile.assetInfos.Length;i++)
                {
                    assetInfo = infoFile.assetInfos[i];
                    mPathes.Add(assetInfo.path);
                    if (!assets.ContainsKey(assetInfo.path))
                        assets.Add(assetInfo.path, new SmartObject(assetInfo));
                }
            }
        }

        public void UnloadInfo(string key)
        {
            if(pathes.TryGetValue(key,out List<string> mPathes))
            {
                for (int i = 0; i < mPathes.Count; i++)
                {
                    assets.Remove(mPathes[i]);
                }
            }
        }

        public void AddLoaderCreator(ResourceLoaderCreator resourceLoaderCreator)
        {
            loaderCreators.Add(resourceLoaderCreator);
        }

        public void UnloadAsset(string path)
        {
            if (assets.TryGetValue(path, out SmartObject smartObject) && smartObject.IsRelease())
            {
                smartObject.Release();
            }
        }

        private void ReleaseAsset(string bundleName,UnityEngine.Object @object)
        {
            if(resourceLoaders.TryGetValue(bundleName, out ResourceLoader resourceLoader))
            {
                resourceLoader.UnloadObject(@object);
            }
        }

        private void Load(SmartObject smartObject)
        {
            if(!resourceLoaders.TryGetValue(smartObject.bundleName, out ResourceLoader resourceLoader))
            {
                resourceLoader = CreateLoader(smartObject.bundleName);
                resourceLoaders.Add(smartObject.bundleName, resourceLoader);
            }
            resourceLoader.LoadObjectAsync(smartObject.assetPath, smartObject.type, smartObject);
        }

        private ResourceLoader CreateLoader(string bundleName)
        {
            ResourceLoader resourceLoader = null;
            for (int i=0;i<loaderCreators.Count;i++)
            {
                resourceLoader = loaderCreators[i].CreateLoader(bundleName,folder);
                if (resourceLoader != null)
                    break; 
            }
            return resourceLoader;
        }

        private void RemoveResourceLoader(string bundleName)
        {
            resourceLoaders.Remove(bundleName);
        }
    }
}
