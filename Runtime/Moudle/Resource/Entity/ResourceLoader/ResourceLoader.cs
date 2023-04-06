using System;
using System.Collections.Generic;


namespace EasyGamePlay
{ 
    public abstract class ResourceLoader
    {
        protected string bundleName;
        private int assetCount = 0;
        private int count = 0;
        protected ResourceLoaderStatus resourceLoaderStatus;

        internal static Action<string> remove;

        public ResourceLoader(string bundleName,string path)
        {
            this.bundleName = bundleName;
        }

        internal void LoadObjectAsync(string assetPath, Type type, SmartObject smartObject)
        {
            if(assetCount==0)
            {
                count++;
            }
            assetCount++;
            OnLoadObjectAsync(assetPath, type,  smartObject);
        }

        internal void LoadObjectAsync<T>(string assetPath,  SmartObject smartObject) where T : UnityEngine.Object
        {
            if (assetCount == 0)
            {
                count++;
            }
            assetCount++;
            OnLoadObjectAsync<T>(assetPath,  smartObject); 
        }

        internal void UnloadObject(UnityEngine.Object @object)
        {
            if(@object!=null)
            {
                OnUnloadObject(@object);
                if(--assetCount == 0 && --count == 0)
                {
                    OnUnload();
                    remove(bundleName);
                }
            }
        }

        internal void LoadScene(Action completed)
        {
            assetCount++;
            OnLoadScene(completed);
        }

        public void Add()
        {
            count++;
        }

        public void Release()
        {
            if(--count==0)
            {
                OnUnload();
                remove(bundleName);
            }
        }
      

        protected abstract void OnLoadScene(Action completed);

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
