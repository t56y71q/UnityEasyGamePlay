using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    [Serializable]
    public struct ResourceSetting
    {
        public string folder;
        public string postFixed;

        public string defaultAssetInfoKey;
        public UnityEngine.TextAsset defaultAssetInfoFile;
    }
}
