using System;


namespace EasyGamePlay
{
    public abstract class AGameSubState : AState
    {
        protected AGame game;
        protected AGameState gameState;

        public AGameSubState( AGame game, AGameState gameState) 
        {
            this.game = game;
            this.gameState = gameState;
        }
    }
}
