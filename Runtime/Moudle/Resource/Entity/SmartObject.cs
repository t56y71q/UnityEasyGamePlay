﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class SmartObject: ILoadObject
    {
        public UnityEngine.Object @object { get=> mObject; set=>SetObject(value); }
        public Action<EAsset> completed { get; set; }
        public string path;
        public string bundleName { get;  }
        public string assetPath { get; }
        public Type type { get; }

        private int count;
        private UnityEngine.Object mObject;

        internal static Action<string,UnityEngine.Object> release;

        public SmartObject(AssetInfo assetInfo)
        {
            this.path = assetInfo.path;
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
            release(bundleName, @object);
        }

        public bool IsRelease()
        {
            return --count == 0;
        }

        public bool IsZeroCount()
        {
            return count == 0;
        }

        private void SetObject(UnityEngine.Object @object)
        {
            mObject = @object;
            completed?.Invoke(new EAsset(@object, path));
            completed = null;
        }
    }
}
