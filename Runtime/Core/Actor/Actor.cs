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

        private GameObject @object;
        private Transform mTransform;

        internal void SetGameObject(GameObject gameObject)
        {
            this.@object = gameObject;
            mTransform = gameObject.transform;
            if (gameObject.TryGetComponent<ActorProperty>(out ActorProperty actorProperty))
            {
                OnDeserialize(actorProperty);
                actorProperty.actor = this;
            }   
        }

        protected override void OnEnable()
        {
            gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            gameObject.SetActive(false);
        }

        protected override void OnDestroy()
        {
            GameObject.Destroy(gameObject);
        }

        protected abstract void OnDeserialize(ActorProperty actorProperty);
    }
}
