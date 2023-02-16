using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    class GamePlayingState : SubStateMachine, IStateEnter,IStateExit
    {
        private AGame aGame;
        private AGameState gameState;

        public GamePlayingState(AGame game, AGameState gameState)
        {
            this.aGame = game;
            this.gameState = gameState;
        }

        public override int id => AGameState.playingId;

        public void Enter()
        {
            if(gameState.firstGameStateId != AGameState.unloadId)
                SetDefault(gameState.firstGameStateId);
            else
            {
                AddState(0, new GameInitState(aGame, gameState));
                SetDefault(0);
            }
               
        }

        public void Exit()
        {
            if (gameState.firstGameStateId != AGameState.unloadId )
            {
                IStateExit stateExit = GetCurrentState() as IStateExit;
                stateExit?.Exit();
            }
        }
    }
}
