using System;
using System.Collections.Generic;


namespace EasyGamePlay.Editor
{
    public static class FrameWorkEditor 
    {
        public static ResourceEditor resourceEditor=new ResourceEditor();
        public static GameColloction gameColloction = new GameColloction();
        public static SystemManager systemManager = new SystemManager();

        public static void Init()
        {
            resourceEditor.Init();
            FrameWork.frameWork.quit += Quit;
            FrameWork.frameWork.preload += delegate ()
            {
                if (resourceEditor.editorResourceSetting.isEditor)
                {
                    UnityEngine.Debug.Log("isEditor");
                    FrameWork.frameWork.resource.AddLoaderCreator(new EditorResourceLoaderCreator());
                }
            };
            gameColloction.Init();
        }

        private static void Quit()
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
