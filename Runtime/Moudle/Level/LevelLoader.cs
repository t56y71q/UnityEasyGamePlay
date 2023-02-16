using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    class LevelLoader
    {
        private SceneLoader sceneLoader=new SceneLoader();

        public Level Load(LevelAsset levelAsset)
        {
            Level level = new Level();
            level.systemTypes = new List<Type>(levelAsset.levelComponents.Length);

            Type type;
            LevelComponent levelComponent;
            for (int i = 0; i < levelAsset.levelComponents.Length; i++)
            {
                levelComponent = levelAsset.levelComponents[i];
                type = Type.GetType(levelComponent.systemType);
                level.systemTypes.Add(type);

                ISystemData system = Activator.CreateInstance(type) as ISystemData;
                system.LoadData(levelComponent.data);

                type = Type.GetType(levelComponent.baseSystemType);
                FrameWork.frameWork.systemManager.RegistSystem(system, type);
            }

            level.SetScene( sceneLoader.LoadScene(levelAsset.sceneSerialize));
            return level;
        }
    }
}
