using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    public struct EAsset
    {
        public UnityEngine.Object @object { get=> mObject; }
        public bool isValued { get=> valued; }
        
        private UnityEngine.Object mObject;
        private bool valued;
        internal string path;

        internal static Action<string> unload;

        internal EAsset(UnityEngine.Object @object, string path)
        {
            this.mObject = @object;
            this.path = path;
            valued = true;
        }

        public void Unload()
        {
            if(valued)
                unload.Invoke(path);
        }
    }

   
}
