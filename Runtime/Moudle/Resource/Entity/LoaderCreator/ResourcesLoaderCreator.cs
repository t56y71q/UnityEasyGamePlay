using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    class ResourcesLoaderCreator : ResourceLoaderCreator
    {
        public ResourceLoader CreateLoader(string bundleName, string folder)
        {
            if(bundleName=="Resources")
                return new ResourcesResourceLoader(bundleName,null);
            return null;
        }
    }
}
