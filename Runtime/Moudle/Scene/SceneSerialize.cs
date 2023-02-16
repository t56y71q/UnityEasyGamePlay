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
        public string type;
        public Vector3 pos;
        public Vector3 rotation;
        public Vector3 scale;
        public bool active;
    }
}
