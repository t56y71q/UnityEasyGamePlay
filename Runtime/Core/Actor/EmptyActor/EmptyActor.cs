using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public class EmptyActor : Actor
    {
        public override ActorProperty actorProperty => emptyActorProperty;

        private EmptyActorProperty emptyActorProperty;

        protected override void OnAwake()
        {
            
        }

        protected override void SetProperty(ActorProperty actorProperty)
        {
            emptyActorProperty = actorProperty as EmptyActorProperty;
        }

        protected override void OnDestroy()
        {
            
        }
    }
}
