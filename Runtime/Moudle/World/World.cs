using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class World
    {
        public Level level { get => mLevel; }

        private Level mLevel;
        private LevelLoader levelLoader=new LevelLoader();

        public ActorAsync<T> SpawnActor<T>(Vector3 pos, Vector3 rotation, Vector3 scale) where T:Actor,new()
        {
            return mLevel.scene.CreateActor<T>(pos, rotation, scale);
        }

        public ActorAsync SpawnActor(Type type, Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            return mLevel.scene.CreateActor(type, pos, rotation, scale);
        }

        public void DestroyActor(Actor actor)
        {
            mLevel.scene.DestroyActor(actor);
        }

        public void OpenLevel(string levelPath)
        {
            if (string.IsNullOrEmpty(levelPath))
                return;
            LoadLevelAsset(levelPath);
        }

        private void LoadLevelAsset(string path)
        {
            EAsset asset = FrameWork.frameWork.LoadAssetAsync(path);
            asset.completed += delegate (UnityEngine.Object @object)
            {
                UnityEngine.TextAsset textAsset = @object as UnityEngine.TextAsset;
                LevelAsset levelAsset= FrameWork.frameWork.DeSerialize<LevelAsset>(textAsset.text, SerializeType.json);
                asset.Unload();

                if (mLevel != null)
                {
                    mLevel.Unload();
                    mLevel.unloaded += delegate (Level level)
                    {
                        mLevel = levelLoader.Load(levelAsset);
                    };
                }
                else
                {
                    mLevel= levelLoader.Load(levelAsset);
                }
            };
        }
    }
}
