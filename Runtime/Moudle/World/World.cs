using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyGamePlay
{
    public class World
    {
        public EScene scene { get => mScene; }

        private EScene mScene;
        private EScene presistent;
        private EResource eResource;
        private ESerialize eSerialize;

        internal void Init(EResource eResource,ESerialize eSerialize,ECoroutine coroutine)
        {
            this.eResource = eResource;
            this.eSerialize = eSerialize;
            presistent=new EScene(SceneManager.GetActiveScene());
        }

        internal void Destroy()
        {
           
        }

        public T SpawnActor<T>(UnityEngine.GameObject prefab, ETransform transform) where T : Actor, new()
        {
            T actor = Actor.CreateActor<T>(prefab, transform);
            if (actor != null)
            {
                actor.eScene = scene;
                scene.actors.Add(actor);
                return actor;
            }
            Debug.LogError("Prefab is not adjust to " + typeof(T));
            return null;
        }

        public Actor SpawnActor(UnityEngine.GameObject prefab, ETransform transform)
        {
            Actor actor = Actor.CreateActorFromPrefab(prefab, transform);
            if (actor != null)
            {
                actor.eScene = scene;
                scene.actors.Add(actor);
                return actor;
            }
            return null;
        }

        public void DestroyActor(Actor actor)
        {
            actor.eScene.actors.Remove(actor);
            Entity.Destroy(actor);
        }

        public void OpenScene(string sceneName) 
        {
            if (string.IsNullOrEmpty(sceneName))
                return;

            eResource.LoadSceneAsync(sceneName, delegate () 
            {
                var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                async.completed += delegate (AsyncOperation asyncOperation) {
                    mScene?.Unload();
                    mScene = new EScene(sceneName,SceneManager.GetSceneByName(sceneName));
                };
            });
        }

        public void MoveToPresistent(Actor actor)
        {
            if(scene== actor.scene)
            {
                scene.actors.Remove(actor);
                actor.eScene = presistent;
                SceneManager.MoveGameObjectToScene(actor.gameObject, presistent.scene);
            }
        }

        public T CreateHud<T>(GameObject prefab) where T : Hud, new()
        {
            T hud= SpawnActor<T>(prefab, ETransform.GetOrigin());
            scene.SetHud(hud); 
            return hud;
        }

        public T CreatePlayerController<T>(GameObject prefab) where T : PlayerController, new()
        {
            T playerController= SpawnActor<T>(prefab, ETransform.GetOrigin());
            scene.SetPlayerController(playerController);
            return playerController;
        }

        public PlayerController CreatePlayerController(GameObject prefab)
        {
            PlayerController playerController= SpawnActor(prefab, ETransform.GetOrigin()) as PlayerController;
            scene.SetPlayerController(playerController);
            return playerController;
        }

        public void DestroyPlayerController()
        {
            if(scene.playerController!=null)
            {
                DestroyActor(scene.playerController);
                scene.SetPlayerController (null);
            }
        }
    }
}
