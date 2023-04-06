using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyGamePlay
{
    public class EScene
    {
        public Hud hud { get=> mHud; }
        public PlayerController playerController { get=> mPlayerController;  }

        internal Scene scene;
        private string path;
        private Hud mHud;
        private PlayerController mPlayerController;
        internal List<Actor> actors=new List<Actor>();

        public EScene(Scene scene)
        {
            this.scene = scene;
            path = null;
        }

        public EScene(string path, Scene scene)
        {
            this.path = path;
            this.scene = scene;
            SceneManager.SetActiveScene(scene);
        }

        internal void SetHud(Hud hud)
        {
            mHud = hud;
        }

        internal void SetPlayerController(PlayerController playerController)
        {
            mPlayerController = playerController;
        }

        internal void Unload()
        {
            Actor actor;
            for(int i= actors.Count-1; i >= 0;i--)
            {
                actor = actors[i];
                if(actor.gameObject!=null)
                {
                    actor.transform.SetParent(null);
                    Actor.Destroy(actor);
                }
                else
                {
                    Debug.LogWarning(actor);
                }
            }

            FrameWork.frameWork.NextFrame(delegate () 
            {
                var async=SceneManager.UnloadSceneAsync(scene);
                if(!string.IsNullOrEmpty(path))
                    async.completed += delegate (AsyncOperation asyncOperation) { FrameWork.frameWork.resource.UnloadAsset(path); };
            });
        }
    }
}
