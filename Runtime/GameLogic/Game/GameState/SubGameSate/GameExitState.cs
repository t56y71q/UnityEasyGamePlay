using System;

namespace EasyGamePlay
{
    class GameExitState : AGameSubState,IStateEnter
    {
        public GameExitState(AGame game, AGameState gameState) : base(game, gameState)
        {
          
        }

        public override int id => AGameState.exitId;

        public void Enter()
        {
            UnityEngine.Debug.Log("GameExitState");
            FrameWork.frameWork.DestroyGame();
        }
    }
}
