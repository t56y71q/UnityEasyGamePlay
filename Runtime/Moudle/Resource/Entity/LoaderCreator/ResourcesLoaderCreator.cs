using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    class ResourcesLoaderCreator : ResourceLoaderCreator
    {
        public ResourceLoader CreateLoader(string bundleName, string folder, string postFixed)
        {
            if(bundleName=="Resources")
                return new ResourcesResourceLoader(bundleName,null);
            return null;
        }
    }
}
