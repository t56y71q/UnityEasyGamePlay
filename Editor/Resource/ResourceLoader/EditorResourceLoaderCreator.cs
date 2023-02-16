using System;
using System.Collections.Generic;


namespace EasyGamePlay.Editor
{
    class EditorResourceLoaderCreator : ResourceLoaderCreator
    {
        public ResourceLoader CreateLoader(string bundleName, string folder, string postFixed)
        {
            return new EditorResourceLoader(bundleName, null);
        }
    }
}
