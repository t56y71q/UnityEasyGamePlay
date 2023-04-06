using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace EasyGamePlay
{
    class GameUnloadState : AGameSubState,IStateEnter
    {
        public GameUnloadState(AGame game, AGameState gameState) : base(game, gameState)
        {
           
        }

        public override int id => AGameState.unloadId;

        public void Enter()
        {
            UnityEngine.Debug.Log("UnloadGameState");

            EScene scene = FrameWork.frameWork.world.scene;
            if (scene != null)
            {
                var async=SceneManager.UnloadSceneAsync(scene.scene);
                async.completed += delegate (UnityEngine.AsyncOperation asyncOperation) { gameState.isUnloaded = true; };
            }
            else
            {
                gameState.isUnloaded = true;
            }
        }
    }
}
