using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    
    public abstract class GameConfig:ScriptableObject
    {
        [Header("Resource")]
        public ResourceSetting resourceSetting;

        [Header("Input")]
        public UnityEngine.InputSystem.InputActionAsset InputAsset;
    }
}
