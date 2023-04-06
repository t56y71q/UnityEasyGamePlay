using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public interface ResourceLoaderCreator
    {
        ResourceLoader CreateLoader(string bundleName,string folder);
    }
}
