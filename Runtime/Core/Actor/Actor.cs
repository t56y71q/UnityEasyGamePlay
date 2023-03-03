using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public abstract class Actor:Entity
    {
        public GameObject gameObject { get=> @object; }
        public Transform transform { get=> mTransform; }
        public override string name { get => base.name; set { base.name = value; gameObject.name = value; } }
        public EScene scene { get => eScene; }
        public abstract ActorProperty actorProperty { get; }

        private GameObject @object;
        private Transform mTransform;
        internal EScene eScene;
        private string prefab;
      
        internal void ActorAwake(ActorProperty actorProperty)
        {
            if (actorProperty != null)
            {
                ActorProperty mActorProperty = GameObject.Instantiate<ActorProperty>(actorProperty);
                SetProperty(mActorProperty);
                @object = mActorProperty.gameObject;
                mTransform = @object.transform;
                Awake();
            }
            else
                Debug.LogError(GetType()+"ActorProperty is null");
        }

        protected override void OnEnable()
        {
            gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            gameObject.SetActive(false);
        }

        protected override void Destroy()
        {
            OnDestroy();
            GameObject.Destroy(gameObject);
            if(!string.IsNullOrEmpty(prefab))
                FrameWork.frameWork.resource.UnloadAsset(prefab);
        }

        protected abstract void OnDestroy();
        protected abstract void SetProperty(ActorProperty actorProperty);

        internal void SetPrefab(string prefab)
        {
            this.prefab = prefab;
        }

        public static T CreateActor<T>(ActorProperty actorProperty) where T : Actor, new()
        {
            T actor = new T();
            actor.ActorAwake(actorProperty);
            return actor;
        }

        public static Actor CreateActor(Type type, ActorProperty actorProperty)
        {
            Actor actor = Activator.CreateInstance(type) as Actor;
            actor.ActorAwake(actorProperty);
            return actor;
        }
    }
}
