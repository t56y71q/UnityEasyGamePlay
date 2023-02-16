using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace EasyGamePlay
{
   
    public class EScene
    {
        public event Loaded loaded;
        public event UnLoaded unloaded;
        internal List<Actor> actors=new List<Actor>();

        internal ActorAsync CreateActor(Type type, Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            ActorAsync actorAsync = new ActorAsync();
            EAsset asset= FrameWork.frameWork.LoadAssetAsync(type.Name);
            asset.completed += delegate (UnityEngine.Object @object)
            {
                Actor actor;
                actor = Entity.CreateEntity(type) as Actor;
                actor.SetGameObject(GameObject.Instantiate(@object as GameObject));
                actor.transform.SetPositionAndRotation(pos, Quaternion.Euler(rotation));
                actor.transform.localScale = scale;
                actors.Add(actor);
                actorAsync.SetActor(actor);
            };
            return actorAsync;
        }

        internal ActorAsync<T> CreateActor<T>(Vector3 pos, Vector3 rotation, Vector3 scale) where T:Actor,new()
        {
            ActorAsync<T> actorAsync = new ActorAsync<T>();
            EAsset asset = FrameWork.frameWork.LoadAssetAsync(typeof(T).Name);
            asset.completed += delegate (UnityEngine.Object @object)
            {
                T actor = Entity.CreateEntity<T>();
                actor.SetGameObject(GameObject.Instantiate(@object as GameObject));
                actor.transform.SetPositionAndRotation(pos, Quaternion.Euler(rotation));
                actor.transform.localScale = scale;
                actors.Add(actor);
                actorAsync.SetActor(actor);
            };
            return actorAsync;
        }

        internal void DestroyActor(Actor actor)
        {
            actors.Remove(actor);
            Entity.Destroy(actor);
        }

        internal void Load()
        {
            loaded?.Invoke(this);
        }

        internal void Unload()
        {
            unloaded?.Invoke(this);
        }

        public delegate void Loaded(EScene scene);
        public delegate void UnLoaded(EScene scene);

    }
}
