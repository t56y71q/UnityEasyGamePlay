using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public  class EPool
    {
        private Dictionary<Type, ObjectPool> pools = new Dictionary<Type, ObjectPool>();

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

    }
}
