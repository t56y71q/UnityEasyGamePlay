using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public abstract class AGameState
    {
        public bool isPlaying { get => playing; set { playing = value; FrameWork.frameWork.stateMachine.AddStateMachine(gameStateMachine); } }
        internal bool isUnloaded { get => unloaded; set { unloaded = value; FrameWork.frameWork.stateMachine.AddStateMachine(gameStateMachine); } }

        private bool playing;
        private bool unloaded;

        protected StateMachine gameStateMachine;
        protected AGame game;

        private GamePlayingState gamePlayingState;

        internal int firstGameStateId;
        public const int playingId = 1;
        public const int unloadId = -2;
        public const int exitId = -1;

        public AGameState(AGame game)
        {
            playing = true;
            firstGameStateId = unloadId;
            this.game = game;
        }

        internal void Init()
        {
            gameStateMachine = new StateMachine();
            gameStateMachine.AddState(unloadId, new GameUnloadState(game, this));
            gameStateMachine.AddState(exitId, new GameExitState(game, this));
            gameStateMachine.AddState(0, new GameInitState(game, this));
            gamePlayingState = new GamePlayingState(game, this);
            gameStateMachine.AddState(playingId, gamePlayingState);

            gameStateMachine.AddTransistion(playingId, unloadId, delegate () { return !isPlaying; });

            gameStateMachine.AddTransistion(0, exitId, delegate () { return !isPlaying; });
            gameStateMachine.AddTransistion(0, playingId, delegate () { return isPlaying; });

            gameStateMachine.AddTransistion(unloadId, exitId, delegate () { return isUnloaded; });

            gameStateMachine.SetDefault(0);
            AddGameSubStates();
        }

        internal void Destroy()
        {
            game = null;
        }

        protected void SetFirstGameStateId(int id)
        {
            firstGameStateId = id;
        }

        protected void AddSubState(int id, AGameSubState GameSubState)
        {
            gamePlayingState.AddState(id, GameSubState);
        }

        protected void AddStateTransition(int id, int nextId, Func<bool> func)
        {
            gamePlayingState.AddTransistion(id, nextId, func);
        }

        protected abstract void AddGameSubStates();
    }
}
