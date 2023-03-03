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
        private string postFixed;
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
                    Load(smartObject);
                    smartObject.Add();
                    smartObject.completed += delegate (UnityEngine.Object @object) { completed?.Invoke(new EAsset(smartObject.@object, path)); };
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

        public void LoadSetting(ResourceSetting resourceSetting)
        {
            folder = resourceSetting.folder + "/";

            if (string.IsNullOrEmpty(postFixed))
                postFixed = "." + resourceSetting.postFixed;
            else
                postFixed = string.Empty;

            AssetInfoFile infoFile = eSerialize.DeSerialize<AssetInfoFile>(resourceSetting.defaultAssetInfoFile.text,SerializeType.json);
            if(infoFile.assetInfos!=null)
                LoadInfo(resourceSetting.defaultAssetInfoKey, infoFile);

            AddLoaderCreator(new ResourcesLoaderCreator());
            AddLoaderCreator(new ABLoaderCreator());
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
                resourceLoader = CreateLoader(smartObject.bundleName,folder,postFixed);
                resourceLoaders.Add(smartObject.bundleName, resourceLoader);
            }
            resourceLoader.LoadObjectAsync(smartObject.assetPath, smartObject.type, smartObject);
        }

        private ResourceLoader CreateLoader(string bundleName, string folder, string postFixed)
        {
            ResourceLoader resourceLoader = null;
            for (int i=0;i<loaderCreators.Count;i++)
            {
                resourceLoader = loaderCreators[i].CreateLoader(bundleName,folder, postFixed);
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
