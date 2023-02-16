using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    public class EAsset
    {
        public UnityEngine.Object @object { get=> mObject; }
        public bool isValued { get=> valued; }
        public event Complete completed;

        private UnityEngine.Object mObject;
        private bool valued;
        internal string path;

        internal static Action<string> unload;

        internal EAsset(string path)
        {
            this.mObject = null;
            this.path = path;
            valued = false;
            completed = null;
        }

        public void Unload()
        {
            if(valued)
                unload.Invoke(path);
        }

        internal void SetObject(UnityEngine.Object @object)
        {
            mObject = @object;
            valued = true;

            completed?.Invoke(mObject);
            completed = null;
        }

        public delegate void Complete(UnityEngine.Object @object);
    }
}
