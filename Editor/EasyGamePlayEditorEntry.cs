using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EasyGamePlay.Editor
{
    public static class EasyGamePlayEditorEntry
    {

        [InitializeOnLoadMethod]
        private static void Entry()
        {
            FrameWork.CreateFrameWork();
            FrameWorkEditor.Init();
        }
    }
}

