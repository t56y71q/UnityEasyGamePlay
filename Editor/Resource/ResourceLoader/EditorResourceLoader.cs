using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace EasyGamePlay.Editor
{
    class EditorResourceLoader : ResourceLoader
    {
        public EditorResourceLoader(string bundleName, string path) : base(bundleName, path)
        {
        }

        protected override void OnLoadObjectAsync(string assetPath, Type type, ILoadObject loadObject)
        {
            FrameWork.frameWork.NextFrame(delegate () { loadObject.@object=AssetDatabase.LoadAssetAtPath(assetPath, type); });
        }

        protected override void OnLoadObjectAsync<T>(string assetPath, ILoadObject loadObject)
        {
            FrameWork.frameWork.NextFrame(delegate () { loadObject.@object=AssetDatabase.LoadAssetAtPath<T>(assetPath); });
        }

        protected override void OnUnload()
        {
            
        }

        protected override void OnUnloadObject(UnityEngine.Object @object)
        {
            
        }
    }
}
