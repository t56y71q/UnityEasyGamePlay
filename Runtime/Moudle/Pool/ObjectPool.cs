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
                destroy(poolAble);
            else
                frees.Push(poolAble);
        }
    }
}
