using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    [Serializable]
    public struct AssetInfoFile
    {
        public AssetInfo[] assetInfos;

        public AssetInfoFile(List<AssetInfo> assetInfos)
        {
            this.assetInfos = assetInfos.ToArray();
        }
    }
}
