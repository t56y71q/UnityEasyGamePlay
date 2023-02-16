using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    class ABLoaderCreator : ResourceLoaderCreator
    {
        public ResourceLoader CreateLoader(string bundleName, string folder, string postFixed)
        {
            return new ABResourceLoader(bundleName,folder + bundleName+ postFixed);
        }
    }
}
