using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public static class Entry
    {
        public static T CreateGame<T,U>(string resourcesPath) where T:AGame,new() where U: GameConfig
        {
            if (FrameWork.frameWork == null)
            {
                FrameWork.CreateFrameWork();
            }
            U gameConfig = Resources.Load<U>(resourcesPath);
           return FrameWork.frameWork.CreateGame<T>(gameConfig);
        }
    }

}
