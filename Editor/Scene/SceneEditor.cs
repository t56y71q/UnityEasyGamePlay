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

            GameObject gameObject;
            SceneActor sceneActor;
            for (int i = 0; i < sceneSerialize.sceneActors.Length; i++)
            {
                sceneActor = sceneSerialize.sceneActors[i];
                gameObject = GameObject.Instantiate(FrameWorkEditor.resourceEditor.LoadAsset<GameObject>(sceneActor.type));
                gameObject.transform.SetPositionAndRotation(sceneActor.pos, Quaternion.Euler(sceneActor.rotation));
                gameObject.transform.localScale = sceneActor.scale;
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
            if(transform.TryGetComponent<ActorCreator>(out ActorCreator actorCreator))
            {
                Type type = Type.GetType(actorCreator.type);
                if (type!=null)
                {
                    SceneActor sceneActor = new SceneActor();
                    sceneActor.active = transform.gameObject.activeSelf;
                    sceneActor.type = type.Name;
                    sceneActor.pos = transform.position;
                    sceneActor.rotation = transform.rotation.eulerAngles;
                    sceneActor.scale = transform.localScale;
                    sceneActors.Add(sceneActor);
                }
                else
                {
                    Debug.LogError(actorCreator.name + "'s Actor Type is Wrong");
                }
                return;
            }

            Transform[] transforms = transform.GetComponentsInChildren<Transform>();
            if (transforms != null)
            {
                for(int i=0;i< transforms.Length;i++)
                {
                    if(transforms[i]!=transform)
                        ChangeGameObjectToActor(transforms[i], sceneActors);
                }
            }
        }

        public void CloseScene()
        {
            EditorSceneManager.CloseScene(EditorSceneManager.GetActiveScene(), true);
        }

        [MenuItem("EasyGamePlay/Scene/Save",false)]
        private static void SaveCurrentScene()
        {
            LevelAsset levelAsset = FrameWorkEditor.levelEditor.LoadLevel();
            levelAsset.sceneSerialize= FrameWorkEditor.sceneEditor.Save();
            FrameWorkEditor.levelEditor.Save(levelAsset);
        }

        [MenuItem("EasyGamePlay/Scene/Save", true)]
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
