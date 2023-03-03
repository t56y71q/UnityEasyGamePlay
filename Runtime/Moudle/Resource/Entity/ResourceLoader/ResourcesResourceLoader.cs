using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class ResourcesResourceLoader : ResourceLoader
    {
        public ResourcesResourceLoader(string bundleName, string path) : base(bundleName,path)
        {
        }

        protected override void OnLoadObjectAsync(string assetPath, Type type, ILoadObject loadObject)
        {
            var request= Resources.LoadAsync(assetPath, type);
            request.completed += delegate (AsyncOperation async)
            {
               loadObject.@object=(async as ResourceRequest).asset;
            };
        }

        protected override void OnLoadObjectAsync<T>(string assetPath, ILoadObject loadObject)
        {
            var request = Resources.LoadAsync<T>(assetPath);
            request.completed += delegate (AsyncOperation async)
            {
                loadObject.@object = (async as ResourceRequest).asset;
            };
        }

        protected override void OnUnload()
        {
            Resources.UnloadUnusedAssets();
        }

        protected override void OnUnloadObject(UnityEngine.Object @object)
        {
            Resources.UnloadAsset(@object);
        }
    }
}
