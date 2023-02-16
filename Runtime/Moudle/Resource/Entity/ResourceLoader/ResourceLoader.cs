using System;
using System.Collections.Generic;


namespace EasyGamePlay
{ 
    public abstract class ResourceLoader
    {
        private string bundleName;
        private int count = 0;
        protected ResourceLoaderStatus resourceLoaderStatus;

        internal static Action<string> remove;

        public ResourceLoader(string bundleName,string path)
        {
            this.bundleName = bundleName;
        }

        internal void LoadObjectAsync(string assetPath, Type type, SmartObject smartObject)
        {
            count++;
            OnLoadObjectAsync(assetPath, type,  smartObject);
        }

        internal void LoadObjectAsync<T>(string assetPath,  SmartObject smartObject) where T : UnityEngine.Object
        {
            count++;
            OnLoadObjectAsync<T>(assetPath,  smartObject); 
        }

        internal void UnloadObject(UnityEngine.Object @object)
        {
            if(@object!=null)
            {
                OnUnloadObject(@object);
                if(--count==0)
                {
                    OnUnload();
                    remove(bundleName);
                }
            }
        }

        protected abstract void OnLoadObjectAsync(string assetPath, Type type,  ILoadObject loadObject);
        protected abstract void OnLoadObjectAsync<T>(string assetPath, ILoadObject loadObject) where T : UnityEngine.Object;

        protected abstract void OnUnloadObject(UnityEngine.Object @object);
        protected abstract void OnUnload();
    }

    public enum ResourceLoaderStatus
    {
        loading,
        loaded
    }
}
