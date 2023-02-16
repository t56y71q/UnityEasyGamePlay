using System;
using System.Collections;


namespace EasyGamePlay
{
    class SceneLoader
    {
        private const int asyncCreateCount = 16;

        public EScene LoadScene(SceneSerialize sceneSerialize)
        {
            EScene scene = new EScene();
            FrameWork.frameWork.StartCoroutine(new CoroutineTask(LoadActor(sceneSerialize, scene)));
            return scene;
        }

        public void UnloadScene(EScene eScene)
        {
            FrameWork.frameWork.StartCoroutine(new CoroutineTask(UnloadActor(eScene)));
        }

        private IEnumerator LoadActor(SceneSerialize sceneSerialize,EScene scene)
        {
            SceneActor sceneActor;
            EAsset asset;
            int count = 0;
            for (int i = 0; i < sceneSerialize.sceneActors.Length; i++)
            {
                sceneActor = sceneSerialize.sceneActors[i];
                asset = FrameWork.frameWork.LoadAssetAsync(sceneActor.type);
                if(asset.isValued)
                {
                    asset.completed += delegate (UnityEngine.Object @object) 
                    {
                        UnityEngine.GameObject.Instantiate(@object as UnityEngine.GameObject);
                    };

                    if (++count == asyncCreateCount)
                    {
                        count = 0;
                        yield return null;
                    }
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
    }
}
