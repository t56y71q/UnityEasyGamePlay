using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    abstract class ObjectPool
    {
      
    }

    class ObjectPool<T> : ObjectPool where T: IPoolAble
    {
        private Stack<T> frees;
        private int count;
        private Func<T> create;
        private Action<T> destroy;

        public ObjectPool(Func<T> create, Action<T> destroy, int count)
        {
            this.count = count;
            this.create = create;
            this.destroy = destroy;
            
            frees = new Stack<T>(count);
            for (int i=0;i< count;i++)
            {
                frees.Push(create());
            }
        }

        public T Get()
        {
            T poolAble;
            if (frees.Count > 0)
                poolAble = frees.Pop();
            else
                poolAble = create();

            return poolAble;
        }

        public void Release(T poolAble)
        {

            if (frees.Count > count)
            {
                destroy(poolAble);
            }   
            else
                frees.Push(poolAble);
        }
    }

    class ActorPool<T> : ObjectPool where T : Actor,IPoolAble,new()
    {
        private UnityEngine.GameObject prefab;
        private UnityEngine.Transform manager;

        private Stack<T> frees;
        private int count;

        public ActorPool(UnityEngine.GameObject prefab, int count)
        {
            manager = new UnityEngine.GameObject().transform;
            manager.name = prefab.name;
            this.prefab = prefab;
            this.count = count;

            frees = new Stack<T>(count);
            T actor;
            for (int i = 0; i < count; i++)
            {
                actor = FrameWork.frameWork.world.SpawnActor(prefab, ETransform.GetOrigin()) as T;
                actor.SetActive(false);
                actor.gameObject.SetActive(false);
                actor.transform.SetParent(manager);
                frees.Push(actor);
            }
        }

        public T Get()
        {
            T poolAble;
            if (frees.Count > 0)
                poolAble = frees.Pop();
            else
                poolAble = FrameWork.frameWork.world.SpawnActor<T>(prefab, ETransform.GetOrigin());

            return poolAble;
        }

        public void Release(T poolAble)
        {
            if (frees.Count > count)
            {
                FrameWork.frameWork.world.DestroyActor(poolAble);
            }
            else
            {
                frees.Push(poolAble);
                poolAble.transform.SetParent(manager);
            }
                
        }

        public void Destroy()
        {
            while(frees.Count>0)
            {
                FrameWork.frameWork.world.DestroyActor(frees.Pop());
            }
            UnityEngine.GameObject.Destroy(manager.gameObject);
        }
    }
}
