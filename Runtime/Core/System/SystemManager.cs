using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    public class SystemManager
    {
        public SortedList<Type, ISystem> systems=new SortedList<Type, ISystem>();

        public void RegistSystem<T>(T system) where T:class,ISystem,new()
        {
            Type type = typeof(T);
            if (!systems.ContainsKey(type))
            {
                systems.Add(type, system);
            }
        }

        public void RegistSystem(ISystem system,Type type)
        {
            if(!systems.ContainsKey(type))
            {
                systems.Add(type, system);
            }
        }

        public void UnregistSystem<T>() where T : class,ISystem
        {
            systems.Remove(typeof(T));
        }

        public void UnregistSystem(Type type)
        {
            systems.Remove(type);
        }

        public T GetSystem<T>() where T : class,ISystem
        {
            systems.TryGetValue(typeof(T), out ISystem system);
            return system as T;
        }

        public ISystem GetSystem(Type type)
        {
            systems.TryGetValue(type, out ISystem system);
            return system;
        }
    }
}
