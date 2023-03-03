using System;
using System.Collections;


namespace EasyGamePlay
{
    class SceneLoader
    {
        private const int asyncCreateCount = 16;
        private ECoroutine eCoroutine;
        private EResource resource;
        private ESerialize serialize;

        public SceneLoader(ECoroutine eCoroutine, EResource resource, ESerialize serialize)
        {
            this.eCoroutine = eCoroutine;
            this.resource = resource;
            this.serialize = serialize;
        }

        public EScene LoadScene(SceneSerialize sceneSerialize)
        {
            EScene scene = new EScene();
            eCoroutine.StartCoroutine(LoadActor(sceneSerialize, scene));
            return scene;
        }

        public void UnloadScene(EScene eScene)
        {
            eCoroutine.StartCoroutine(UnloadActor(eScene));
        }

        private IEnumerator LoadActor(SceneSerialize sceneSerialize,EScene scene)
        {
            int count = 0;
            for (int i = 0; i < sceneSerialize.sceneActors.Length; i++)
            {
                SpawnActor(sceneSerialize.sceneActors[i], scene);

                if (++count == asyncCreateCount)
                {
                    count = 0;
                    yield return null;
                }
            }
            yield return null;
            scene.Load();
        }

        private IEnumerator UnloadActor(EScene eScene)
        {
            int count = 0;
            for (int i = 0; i < eScene.actors.Count; i++)
            {
                Entity.Destroy(eScene.actors[i]);
                if (++count == asyncCreateCount)
                {
                    count = 0;
                    yield return null;
                }
            }
            eScene.actors.Clear();

            yield return null;
            eScene.Unload();
        }

        private void SpawnActor(SceneActor sceneActor,EScene scene)
        {
            if (string.IsNullOrEmpty(sceneActor.prefab))
            {
                UnityEngine.Debug.LogError("Not Find Prefab file");
                return;
            }

            resource.LoadAssetAsync(sceneActor.prefab, delegate (EAsset asset)
            {
                UnityEngine.GameObject prefab = asset.@object as UnityEngine.GameObject;
                if (prefab.TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
                {
                    Actor actor = Actor.CreateActor(actorProperty.type, actorProperty) as Actor;
                    actor.SetPrefab(sceneActor.prefab);
                    sceneActor.transform.SetUnityTransform(actor.transform);
                    actor.eScene = scene;
                    scene.actors.Add(actor);
                    actor.SetActive(sceneActor.active);
                }
            });
        }
    }
}
