using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public  class EPool
    {
        private Dictionary<Type, ObjectPool> pools = new Dictionary<Type, ObjectPool>();
        private Dictionary<GameObject, ObjectPool> actorPools = new Dictionary<GameObject, ObjectPool>();

        public void BuildPool<T>(Func<T> create, Action<T> destroy, int count) where T : IPoolAble
        {
            Type type = typeof(T);
            if (!pools.ContainsKey(type))
            {
                ObjectPool<T> pool = new ObjectPool<T>(create, destroy, count);
                pools.Add(type, pool);
            }
        }

        public void DestroyPool<T>() where T : IPoolAble
        {
            pools.Remove(typeof(T));
        }

        public  T Get<T>() where T : IPoolAble
        {
            ObjectPool pool;
            if (pools.TryGetValue(typeof(T), out pool))
            {
                return (pool as ObjectPool<T>).Get();
            }
            return default;
        }

        public void Release<T>(T @object) where T : IPoolAble
        {
            ObjectPool pool;
            if (pools.TryGetValue(typeof(T), out pool))
            {
                (pool as ObjectPool<T>).Release(@object);
            }
        }

        public void BuildActorPool<T>(GameObject prefab, int count) where T : Actor,IPoolAble,new()
        {
            if(!actorPools.ContainsKey(prefab))
            {
                ActorPool<T> pool = new ActorPool<T>(prefab,count);
                actorPools.Add(prefab, pool);
            }
        }

        public void DestroyActorPool<T>(GameObject prefab) where T : Actor, IPoolAble, new()
        {
            if (actorPools.TryGetValue(prefab, out ObjectPool pool))
            {
                (pool as ActorPool<T>).Destroy();
                actorPools.Remove(prefab);
            }
        }

        public T Get<T>(GameObject prefab) where T : Actor, IPoolAble, new()
        {
            ObjectPool pool;
            if (actorPools.TryGetValue(prefab, out pool))
            {
                return (pool as ActorPool<T>).Get();
            }
            return default;
        }

        public void Release<T>(GameObject prefab, T @object) where T : Actor, IPoolAble, new()
        {
            ObjectPool pool;
            if (actorPools.TryGetValue(prefab, out pool))
            {
                (pool as ActorPool<T>).Release(@object);
            }
        }
    }
}
