using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public abstract class LevelLoader
    {
        internal static SceneLoader sceneLoader;

        public Level Load(LevelAsset levelAsset)
        {
            Level level = new Level();
            level.systemTypes = new List<Type>(levelAsset.levelComponents.Length);
            SortedList<string, string> systemDatas = new SortedList<string, string>(levelAsset.levelComponents.Length);
            LevelComponent levelComponent;
            for (int i = 0; i < levelAsset.levelComponents.Length; i++)
            {
                levelComponent = levelAsset.levelComponents[i];
                systemDatas.Add(levelAsset.levelComponents[i].key, levelAsset.levelComponents[i].data);
            }

            OnLoad(systemDatas, level.systemTypes);
            level.SetScene(sceneLoader.LoadScene(levelAsset.sceneSerialize));
            return level;
        }

        protected abstract void OnLoad(SortedList<string,string> systemDatas, List<Type> systemTypes);
    }
}
