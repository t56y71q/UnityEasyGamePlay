using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public class Level
    {
        public event Loaded loaded;
        public event UnLoaded unloaded;
        public EScene scene { get => eScene; }

        internal List<Type> systemTypes;
        private EScene eScene;

        internal void SetScene(EScene eScene)
        {
            this.eScene = eScene;
            
            eScene.loaded += delegate (EScene scene)
            {
                loaded?.Invoke(this);
                loaded = null;
            };
        }

        internal void Unload()
        {
            eScene.unloaded += Unload;
            eScene.Unload();
        }

        private void Unload(EScene scene)
        {
            for(int i=0;i< systemTypes.Count;i++)
            {
                FrameWork.frameWork.systemManager.UnregistSystem(systemTypes[i]);
            }
            systemTypes.Clear();
            unloaded?.Invoke(this);
            unloaded = null;
        }

        public delegate void Loaded(Level level);
        public delegate void UnLoaded(Level level);
    }
}
