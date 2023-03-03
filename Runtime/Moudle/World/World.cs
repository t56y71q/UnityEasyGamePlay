using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyGamePlay
{
    public class World
    {
        public Level level { get => mLevel; }

        private Level mLevel;
        private Scene scene;
        private EResource eResource;
        private ESerialize eSerialize;

        private GameObject panels;
        private List<AiController> aiControllers=new List<AiController>();

        internal void Init(EResource eResource,ESerialize eSerialize,ECoroutine coroutine, GameObject gameObject)
        {
            this.eResource = eResource;
            this.eSerialize = eSerialize;
            scene = SceneManager.GetActiveScene();

            LevelLoader.sceneLoader = new SceneLoader(coroutine, eResource, eSerialize);

            panels = new GameObject();
            panels.name = "Panels";
            panels.transform.SetParent(gameObject.transform);
        }

        internal void Destroy()
        {
            GameObject.Destroy(panels);
        }

        public void SpawnActor<T>(string prefab, ETransform transform, Action<T> completed) where T:Actor,new()
        {
            if (string.IsNullOrEmpty(prefab))
            {
                if(typeof(T)!=typeof(EmptyActor))
                {
                    Debug.LogError("Not Find Prefab file");
                    return;
                }
            }

            eResource.LoadAssetAsync(prefab, delegate (EAsset asset)
            {
                GameObject mPrefab = asset.@object as GameObject;
                if (mPrefab.TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
                {
                    T actor = Actor.CreateActor<T>(actorProperty);
                    actor.SetPrefab(prefab);
                    transform.SetUnityTransform(actor.transform);
                    actor.eScene = level.scene;
                    level.scene.actors.Add(actor);
                    completed?.Invoke(actor);
                }
            });
        }

        public void DestroyActor(Actor actor)
        {
            level.scene.actors.Remove(actor);
            Entity.Destroy(actor);
        }

        public void OpenLevel<T>(string levelPath) where T:LevelLoader,new()
        {
            if (string.IsNullOrEmpty(levelPath))
                return;
            LoadLevelAsset<T>(levelPath);
        }

        public void MoveToPresistent(Actor actor)
        {
            if(level.scene== actor.scene)
            {
                level.scene.actors.Remove(actor);
                actor.eScene = null;
                SceneManager.MoveGameObjectToScene(actor.gameObject, scene);
            }
        }

        public void CreateHud<T>(string prefab, Action<T> completed) where T : Hud, new()
        {
            SpawnActor<T>(prefab, ETransform.GetOrigin(), delegate (T hud) { level.mHud = hud; completed?.Invoke(hud); });
        }

        public void CreatePanel<T>(string prefab, ETransform transform,Action<T> completed) where T:Panel,new()
        {
            SpawnActor<T>(prefab, transform, delegate (T panel) {panel.transform.SetParent(panels.transform) ; completed?.Invoke(panel); });
        }

        public void CreatePlayerController<T>(string prefab, Action<T> completed) where T : PlayerController, new()
        {
            SpawnActor<T>(prefab, ETransform.GetOrigin(), delegate (T controller) { level.mPlayerController = controller; completed?.Invoke(controller); });
        }

        public void CreateAiController<T>(string prefab, Action<T> completed) where T : AiController, new()
        {
            SpawnActor<T>(prefab, ETransform.GetOrigin(), delegate (T controller) { aiControllers.Add(controller); completed?.Invoke(controller); });
        }

        public AiController[] GetAiControllers()
        {
            return aiControllers.ToArray();
        }

        private void LoadLevelAsset<T>(string path) where T : LevelLoader, new()
        {
            eResource.LoadAssetAsync(path,delegate(EAsset asset)
            {
                LevelAsset levelAsset = eSerialize.DeSerialize<LevelAsset>((asset.@object as TextAsset).text, SerializeType.json);
                asset.Unload();

                T levelLoader = new T();
                if (mLevel != null)
                {
                    mLevel.unloaded += delegate (Level level)
                    {
                        mLevel = levelLoader.Load(levelAsset);
                    };
                    LevelLoader.sceneLoader.UnloadScene(mLevel.scene);
                }
                else
                {
                    mLevel = levelLoader.Load(levelAsset);
                }
            });
        }
    }
}
