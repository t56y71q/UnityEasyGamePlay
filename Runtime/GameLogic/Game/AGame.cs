using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public abstract class AGame 
    {
        public AGameState gameState { get=>aGameState; }
        public GameObject gameObject { get => mGameObject; }

        protected AGameState aGameState;
        protected GameObject mGameObject;

        public void Init(GameConfig gameConfig)
        {
            mGameObject = new GameObject();
            mGameObject.name = GetType().Name;

            OnInit(gameConfig);

            aGameState = CreateGameState();
            aGameState.Init();
        }

        public void Destroy()
        {
            OnDestroy();
            GameObject.Destroy(gameObject);
        }

        protected abstract void OnInit(GameConfig gameConfig);
        protected abstract AGameState CreateGameState();
        public abstract void OnDestroy();

    }
}

