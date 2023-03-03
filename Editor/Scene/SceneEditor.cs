using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace EasyGamePlay.Editor
{
    public class SceneEditor
    {
        public void OpenScene(string name,SceneSerialize sceneSerialize)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            scene.name = name;
            EditorSceneManager.SetActiveScene(scene);

            SceneActor sceneActor;
            for (int i = 0; i < sceneSerialize.sceneActors.Length; i++)
            {
                sceneActor = sceneSerialize.sceneActors[i];
                GameObject gameObject=GameObject.Instantiate(FrameWorkEditor.resourceEditor.LoadAsset<GameObject>(sceneActor.prefab));
                gameObject.name = sceneActor.name;
                gameObject.SetActive(sceneActor.active);
            }
        }

        public SceneSerialize Save()
        {
            SceneSerialize sceneSerialize = new SceneSerialize();
            List<SceneActor> sceneActors = new List<SceneActor>();

            GameObject[] gameObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                ChangeGameObjectToActor(gameObjects[i].transform, sceneActors);
            }

            sceneSerialize.sceneActors = sceneActors.ToArray();
            return sceneSerialize;
        }

        private void ChangeGameObjectToActor(Transform transform, List<SceneActor> sceneActors)
        {
            Transform[] transforms = transform.GetComponentsInChildren<Transform>();
            if (transforms != null)
            {
                GameObject prefab;
                SceneActor sceneActor;
                Transform child;
                for (int i = 0; i < transforms.Length; i++)
                {
                    child = transforms[i];
                    if (child.TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
                    {
                        prefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                        sceneActor = new SceneActor();
                        sceneActor.name = child.name;
                        sceneActor.prefab = FrameWorkEditor.resourceEditor.GetPath(prefab);
                        sceneActor.transform = new ETransform(child);
                        sceneActor.active = child.gameObject.activeSelf;

                        sceneActors.Add(sceneActor);
                    }
                }
            }
        }

        public void CloseScene()
        {
            EditorSceneManager.CloseScene(EditorSceneManager.GetActiveScene(), true);
        }

        [MenuItem("EasyGamePlay/Scene/CreateFromScene",false)]
        private static void SaveCurrentScene()
        {
            LevelAsset levelAsset = FrameWorkEditor.levelEditor.LoadLevel();
            levelAsset.sceneSerialize= FrameWorkEditor.sceneEditor.Save();
            FrameWorkEditor.levelEditor.Save(levelAsset);
        }

        [MenuItem("EasyGamePlay/Scene/CreateFromScene", true)]
        private static bool IsVaildSave()
        {
            return FrameWorkEditor.levelEditor.IsOpen();
        }

        [MenuItem("EasyGamePlay/Scene/Open",false)]
        private static void Open()
        {
            LevelAsset levelAsset= FrameWorkEditor.levelEditor.LoadLevel();
            FrameWorkEditor.sceneEditor.OpenScene(FrameWorkEditor.levelEditor.GetName(), levelAsset.sceneSerialize);
        }

        [MenuItem("EasyGamePlay/Scene/Open", true)]
        private static bool IsVaildOpen()
        {
            return FrameWorkEditor.levelEditor.IsOpen();
        }
    }
}
