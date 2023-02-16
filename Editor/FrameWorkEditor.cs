using System;
using System.Collections.Generic;


namespace EasyGamePlay.Editor
{
    public static class FrameWorkEditor 
    {
        public static ResourceEditor resourceEditor=new ResourceEditor();
        public static SceneEditor sceneEditor = new SceneEditor();
        public static LevelEditor levelEditor = new LevelEditor();

        public static void Init()
        {
            resourceEditor.Init();
            FrameWork.frameWork.quit += Quit;
            if (resourceEditor.editorResourceSetting.isEditor)
            {
                UnityEngine.Debug.Log("isEditor");
                FrameWork.frameWork.AddLoaderCreator(new EditorResourceLoaderCreator());
            }
            levelEditor.colseLevel += sceneEditor.CloseScene;
        }

        private static void Quit()
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
