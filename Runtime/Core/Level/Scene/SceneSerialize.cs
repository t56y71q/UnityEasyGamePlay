using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    [Serializable]
    public struct SceneSerialize
    {
        public SceneActor[] sceneActors;
    }

    [Serializable]
    public struct SceneActor
    {
        public string prefab;
        public string name;
        public ETransform transform;
        public bool active;
    }
}
