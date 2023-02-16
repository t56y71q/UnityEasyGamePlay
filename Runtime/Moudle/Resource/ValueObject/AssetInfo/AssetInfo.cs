using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    [Serializable]
    public struct AssetInfo
    {
        public string path;
        public string bundleName;
        public string assetPath;
        public string type;

        public AssetInfo(string path, string bundleName, string assetPath,Type type)
        {
            this.path = path;
            this.bundleName = bundleName;
            this.assetPath = assetPath;
            this.type = type.FullName+","+type.Assembly.GetName().Name;
        }
    }
}
