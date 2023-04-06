using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class ABResourceLoader : ResourceLoader
    {
        private AssetBundleCreateRequest request;
        

        private static AssetBundleManifest manifest;
        private static AssetBundle mainfestBundle;

        internal static string postfixed;
        internal static Action<string> loadLoader;
        internal static Action<string> unloadLoader;

        public ABResourceLoader(string bundleName, string path) : base(bundleName,path)
        {
            request=AssetBundle.LoadFromFileAsync(path + postfixed);
            resourceLoaderStatus = ResourceLoaderStatus.loading;
            request.completed += delegate (AsyncOperation asyncOperation) {
                resourceLoaderStatus = ResourceLoaderStatus.loaded; };

            string[] bundles = manifest.GetAllDependencies(bundleName.ToLower() + postfixed);
            string bundle;
            string depedency;
            for (int i=0;i< bundles.Length;i++)
            {
                bundle = bundles[i];
                depedency = bundle.Substring(0, bundle.IndexOf(postfixed));
                loadLoader(depedency);
            }
        }

        protected override void OnUnload()
        {
            request.assetBundle.UnloadAsync(true);

            string[] bundles = manifest.GetAllDependencies(bundleName.ToLower() + postfixed);
            for (int i = 0; i < bundles.Length; i++)
            {
                unloadLoader(bundles[i]);
            }
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
                                loadObject.@object = (async as AssetBundleRequest).asset;
                            };
                        };
                        break;
                    }
                case ResourceLoaderStatus.loaded:
                    {
                        var assetBundleRequest = request.assetBundle.LoadAssetAsync(assetPath, type);
                        assetBundleRequest.completed += delegate (AsyncOperation async) {
                            loadObject.@object = (async as AssetBundleRequest).asset;
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
                                loadObject.@object = (async as AssetBundleRequest).asset as T;
                            };
                        };
                        break;
                    }
                case ResourceLoaderStatus.loaded:
                    {
                        var assetBundleRequest = request.assetBundle.LoadAssetAsync<T>(assetPath);
                        assetBundleRequest.completed += delegate (AsyncOperation async) {
                            loadObject.@object = (async as AssetBundleRequest).asset as T;
                        };
                        break;
                    }
                default:
                    break;
            }
        }

        protected override void OnLoadScene(Action completed)
        {
            switch (resourceLoaderStatus)
            {
                case ResourceLoaderStatus.loading:
                    {
                        request.completed += delegate (AsyncOperation asyncOperation)
                        {
                            Debug.Log("completed");
                            completed?.Invoke();
                        };
                        break;
                    }
                case ResourceLoaderStatus.loaded:
                    {
                        FrameWork.frameWork.NextFrame(completed);
                        Debug.Log("completed");
                        break;
                    }
                default:
                    break;
                  
            }
        }

        public static void LoadMainfest(string folder)
        {
            mainfestBundle= AssetBundle.LoadFromFile(folder);
            manifest= mainfestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        public static void UnloadMainfest()
        {
            manifest = null;
            mainfestBundle.Unload(true);
        }
    }
}
