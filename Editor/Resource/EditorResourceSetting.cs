using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyGamePlay.Editor
{
    public class EditorResourceSetting:ScriptableObject
    {
        public bool isEditor;
        public GameConfig gameConfig;
    }
}
