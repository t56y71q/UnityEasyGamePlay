using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class ControllerComponent:MonoBehaviour
    {
        public string controllerPrefab;
        public bool isPlayer;

        private void Awake()
        {
            FrameWork.frameWork.resource.LoadAssetAsync(controllerPrefab, delegate (EAsset asset) 
            {
                Controller controller;
                if (isPlayer)
                {
                    controller = FrameWork.frameWork.world.CreatePlayerController(asset.@object as GameObject);
                }
                else
                {
                    controller = FrameWork.frameWork.world.SpawnActor(asset.@object as GameObject,ETransform.GetOrigin()) as Controller;
                }

                if (TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
                {
                    controller.pawn = actorProperty.actor as Pawn;
                    (actorProperty.actor as Pawn).owner = controller;
                }
            });
            
        }
    }
}
