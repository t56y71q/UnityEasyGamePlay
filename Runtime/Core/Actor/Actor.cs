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

        internal void Init(ActorProperty actorProperty)
        {
            actorProperty.actor = this;
            SetProperty(actorProperty);
            @object = actorProperty.gameObject;
            mTransform = @object.transform;
            Awake();
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
            actorProperty.actor = null;
            GameObject.Destroy(gameObject);
        }

        protected abstract void OnDestroy();
        protected abstract void SetProperty(ActorProperty actorProperty);

        public T BindActor<T>(ActorProperty actorProperty) where T:Actor,new()
        {
            T actor = new T();
            actor.Init(actorProperty);
            return actor;
        }

        public static T CreateActor<T>(GameObject prefab,ETransform transform) where T : Actor, new()
        {
            if (prefab.TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
            {
                T actor = new T();
                ActorProperty mActorProperty = GameObject.Instantiate<ActorProperty>(actorProperty);
                transform.SetUnityTransform(mActorProperty.transform);
                actor.Init(mActorProperty);
                return actor;
            }

            return null;
        }

        public static Actor CreateActorFromPrefab(GameObject prefab ,ETransform transform)
        {
            if(prefab.TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
            {
                Actor actor = Activator.CreateInstance(actorProperty.type) as Actor;
                ActorProperty mActorProperty = GameObject.Instantiate<ActorProperty>(actorProperty);
                transform.SetUnityTransform(mActorProperty.transform);
                actor.Init(mActorProperty);
                return actor;
            }
            return null;
        }

        public static Actor CreateActorFromInstance(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
            {
                Actor actor = Activator.CreateInstance(actorProperty.type) as Actor;
                actor.Init(actorProperty);
                return actor;
            }
            return null;
        }
    }
}
