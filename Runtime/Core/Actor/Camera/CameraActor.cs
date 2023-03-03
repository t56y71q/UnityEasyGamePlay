using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class CameraActor : Actor
    {
        public override ActorProperty actorProperty => cameraProperty;

        private CameraProperty cameraProperty;

        protected override void OnAwake()
        {
           
        }

        protected override void SetProperty(ActorProperty actorProperty)
        {
            cameraProperty = actorProperty as CameraProperty;
        }

        protected override void OnDestroy()
        {
            
        }
    }
}
