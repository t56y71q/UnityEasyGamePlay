using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public abstract class AGame 
    {
        public AGameState gameState { get=>aGameState; }

        protected AGameState aGameState;

        public void Init(GameConfig gameConfig)
        {
            OnInit(gameConfig);

            aGameState = CreateGameState();
            aGameState.Init();
        }

        protected abstract void OnInit(GameConfig gameConfig);
        protected abstract AGameState CreateGameState();
        public abstract void Destroy();

    }
}

