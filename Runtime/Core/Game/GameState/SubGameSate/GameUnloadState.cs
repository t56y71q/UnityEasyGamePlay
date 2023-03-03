using System;
using System.Collections.Generic;

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

            Level level = FrameWork.frameWork.world.level;
            if (level != null)
            {
                level.unloaded += delegate (Level mLevel)
                  {
                      gameState.isUnloaded = true;
                  };
                LevelLoader.sceneLoader.UnloadScene(level.scene);
            }
            else
            {
                gameState.isUnloaded = true;
            }
        }
    }
}
