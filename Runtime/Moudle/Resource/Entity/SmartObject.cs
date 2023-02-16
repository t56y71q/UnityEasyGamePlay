using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class SmartObject: ILoadObject
    {
        public EAsset asset { get;}

        public string bundleName { get;  }
        public string assetPath { get; }
        public Type type { get; }

        private int count;

        internal static Action<string,UnityEngine.Object> release;

        public SmartObject(AssetInfo assetInfo)
        {
            asset = new EAsset(assetInfo.path);
           
            this.bundleName = assetInfo.bundleName;
            this.assetPath = assetInfo.assetPath;
            type = Type.GetType(assetInfo.type);
            count = 0;
        }

        public void Add()
        {
            count++;
        }

        public void Release()
        {
            release(bundleName, asset.@object);
        }

        public bool IsRelease()
        {
            return --count == 0;
        }

        public bool IsZeroCount()
        {
            return count == 0;
        }

        public void LoadObject(UnityEngine.Object @object)
        {
            asset.SetObject(@object);
        }
    }
}
