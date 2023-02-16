using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class ABResourceLoader : ResourceLoader
    {
        private AssetBundleCreateRequest request;

        public ABResourceLoader(string bundleName, string path) : base(bundleName,path)
        {
            request=AssetBundle.LoadFromFileAsync(path);
            resourceLoaderStatus = ResourceLoaderStatus.loading;
            request.completed += delegate (AsyncOperation asyncOperation) {
                resourceLoaderStatus = ResourceLoaderStatus.loaded; };
        }

        protected override void OnUnload()
        {
            request.assetBundle.UnloadAsync(true);
        }

        protected override void OnUnloadObject(UnityEngine.Object @object)
        {
            
        }

        protected override void OnLoadObjectAsync(string assetPath, Type type, ILoadObject loadObject)
        {
            switch (resourceLoaderStatus)
            {
                case ResourceLoaderStatus.loading:
                    {
                        request.completed += delegate (AsyncOperation asyncOperation)
                        {
                            var assetBundleRequest = (asyncOperation as AssetBundleCreateRequest).assetBundle.LoadAssetAsync(assetPath, type);
                            assetBundleRequest.completed += delegate (AsyncOperation async)
                            {
                                loadObject.LoadObject((async as AssetBundleRequest).asset);
                            };
                        };
                        break;
                    }
                case ResourceLoaderStatus.loaded:
                    {
                        var assetBundleRequest = request.assetBundle.LoadAssetAsync(assetPath, type);
                        assetBundleRequest.completed += delegate (AsyncOperation async) {
                            loadObject.LoadObject((async as AssetBundleRequest).asset);
                        };
                        break;
                    }
                default:
                    break;
            }
        }

        protected override void OnLoadObjectAsync<T>(string assetPath, ILoadObject loadObject)
        {
            switch (resourceLoaderStatus)
            {
                case ResourceLoaderStatus.loading:
                    {
                        request.completed += delegate (AsyncOperation asyncOperation)
                        {
                            var assetBundleRequest = request.assetBundle.LoadAssetAsync<T>(assetPath);
                            assetBundleRequest.completed += delegate (AsyncOperation async)
                            {
                                loadObject?.LoadObject((async as AssetBundleRequest).asset as T);
                            };
                        };
                        break;
                    }
                case ResourceLoaderStatus.loaded:
                    {
                        var assetBundleRequest = request.assetBundle.LoadAssetAsync<T>(assetPath);
                        assetBundleRequest.completed += delegate (AsyncOperation async) {
                            loadObject?.LoadObject((async as AssetBundleRequest).asset as T);
                        };
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
