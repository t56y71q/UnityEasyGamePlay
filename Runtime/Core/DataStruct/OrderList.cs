using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public class OrderList<T> where T: IComparable<T>
    {
        private List<T> list;
        public int Count { get => list.Count; }

        public OrderList()
        {
            list = new List<T>();
        }

        public OrderList(int capacity)
        {
            list = new List<T>(capacity);
        }

        public void Insert(T @object)
        {
            if(list.Count==0)
                list.Add(@object);
            else 
            {
                int index = list.BinarySearch(@object);
                if(index < 0)
                    list.Insert(~index, @object);
            }
        }

        public void AddRange(T[] values)
        {
            for(int i=0;i< values.Length;i++)
            {
                Insert(values[i]);
            }
        }

        public T At(int id)
        {
            return list[id];
        }

        public T this[int index]
        {
            get => list [index];
            set
            {
                if ((uint)index < list.Count)
                    ReOrder(index, value);
                else
                    Insert(value);
            }
        }

        public void Remove(T value)
        {
            list.Remove(value);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }
        /// <summary>
        /// 如果键值不改变，更新用此函数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Update(int index, T value)
        {
            list[index] = value;
        }

        public int Find(T value)
        {
            return list.BinarySearch(value);
        }

        public void Clear()
        {
            list.Clear();
        }
        /// <summary>
        /// 如果键值改变，更新用此函数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void ReOrder(int index,T value)
        {
            list.RemoveAt(index);
            Insert(value);
        }

        /// <summary>
        /// 如果键值改变，更新用此函数
        /// </summary>
        /// <param name="preValue"></param>
        /// <param name="value"></param>
        public void ReOrder(T preValue, T value)
        {
            int index = list.BinarySearch(preValue);
            list.RemoveAt(index);

            Insert(value);
        }

        public T[] GetList()
        {
            return list.ToArray();
        }
    }
}
