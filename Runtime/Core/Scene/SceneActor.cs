using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class SceneActor:MonoBehaviour
    {
        private void Start()
        {
            if(TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
            {
                Actor actor = Activator.CreateInstance(actorProperty.type) as Actor;
                actor.Init(actorProperty);
                actor.eScene = FrameWork.frameWork.world.scene;
                FrameWork.frameWork.world.scene.actors.Add(actor);
                actor.SetActive(gameObject.activeSelf);
            }
        }
    }
}
