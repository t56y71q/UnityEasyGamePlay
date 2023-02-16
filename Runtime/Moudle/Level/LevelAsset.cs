using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    [Serializable]
    public struct LevelAsset
    {
        public SceneSerialize sceneSerialize;
        public LevelComponent[] levelComponents;
    }

    [Serializable]
    public struct LevelComponent
    {
        public string baseSystemType;
        public string systemType;
        public string data;
    }
}
