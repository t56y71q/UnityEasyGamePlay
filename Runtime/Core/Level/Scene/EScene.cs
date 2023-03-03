using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyGamePlay
{
    public class EScene
    {
        public event Loaded loaded;
        public event UnLoaded unloaded;

        internal List<Actor> actors=new List<Actor>();
        

        internal void Load()
        {
            loaded?.Invoke(this);
        }

        internal void Unload()
        {
            unloaded?.Invoke(this);
        }

        public delegate void Loaded(EScene scene);
        public delegate void UnLoaded(EScene scene);

    }
}
